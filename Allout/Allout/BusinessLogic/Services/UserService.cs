using Allout.BusinessLogic.Core.Interfaces;
using Allout.BusinessLogic.Core.Models;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Allout.DataAccess.Core.Interfaces.DBContext;
using Allout.DataAccess.Core.Models;
using Microsoft.EntityFrameworkCore;
using Share.Exceptions;
using System.Linq;
using Z.EntityFramework.Extensions.EFCore;
using BusinessLogic.Core.Models;
using Shared.Exceptions;

namespace Allout.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IContext _context;

        public UserService(IMapper mapper, IContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserInformationBlo> Registration(UserIdentityBlo userIdentityBlo)
        {
            if (await _context.Users.AnyAsync(x => x.Nickname.ToLower() == userIdentityBlo.Nickname!.ToLower()))
                throw new BadRequestException($"Пользователь с никнеймом {userIdentityBlo.Nickname} уже существует");

            UserRto userRto = new UserRto()
            {
                Email = userIdentityBlo.Email,
                Nickname = userIdentityBlo.Nickname!,
                Password = userIdentityBlo.Password!,
                IsDeleted = false
            };

            _context.Users.Add(userRto);
            await _context.SaveChangesAsync();
            return ConvertToUserInformation(userRto);
        }

        public async Task<UserInformationBlo> Authenticate(UserIdentityBlo userIdentityBlo)
        {
            UserRto? user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Nickname.ToLower() == userIdentityBlo.Nickname!.ToLower() && x.Password == userIdentityBlo.Password!);

            if (user == null)
                throw new BadRequestException("Неверное имя пользователя или пароль");

            return ConvertToUserInformation(user);
        }

        public async Task<UserInformationBlo> GetUser(int userId)
        {
            UserRto? user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new NotFoundException($"Пользователь с id {userId} не найден");

            return ConvertToUserInformation(user);
        }

        public async Task<UserInformationBlo> UpdateUser(string nickname, UserUpdateBlo userUpdateBlo)
        {
            UserRto? user = await _context.Users
                .FirstOrDefaultAsync(x => x.Nickname.ToLower() == nickname.ToLower());

            if (user == null)
                throw new NotFoundException("Пользователь не найден");

            if (userUpdateBlo.CurrentPassword != null)
            {
                bool passwordIsGoog = await _context.Users.AnyAsync(x => x.Nickname.ToLower() == nickname.ToLower() && x.Password == userUpdateBlo.CurrentPassword);
                if (passwordIsGoog == false) throw new BadRequestException("Пользователь ввел не верный пароль");

                user.Email = userUpdateBlo.Email == null ? user.Email : userUpdateBlo.Email;
                user.Nickname = userUpdateBlo.Nickname == null ? user.Nickname : userUpdateBlo.Nickname;
                user.Password = userUpdateBlo.NewPassword == null ? user.Password : userUpdateBlo.NewPassword;

                await _context.SaveChangesAsync();
            }
            else 
            {
                throw new BadRequestException("Пользователь ввел неверный пароль");
            }

            return ConvertToUserInformation(user);
        }

        public async Task<string> UpdateAvatarUser(string nickname, string avatarUrl)
        {
            UserRto? user = await _context.Users
                .FirstOrDefaultAsync(x => x.Nickname.ToLower() == nickname.ToLower());

            if (user == null)
                throw new NotFoundException("Пользователь не найден");

            user.AvatarUrl = avatarUrl;
            await _context.SaveChangesAsync();
            return avatarUrl;
        }

        public async Task<UserInformationBlo> MarkAsDeletedUser(int userId)
        {
            UserRto? user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new NotFoundException($"Пользователь с id {userId} не найден");

            user.IsDeleted = true;
            await _context.SaveChangesAsync();
            return ConvertToUserInformation(user);
        }

        public async Task<bool> DoesUserExist(string nickname)
        {
            return await _context.Users.AnyAsync(x => x.Nickname.ToLower() == nickname.ToLower());
        }


        public async Task<int> AddUserBalance(int userId, int money)
        {
            UserRto? user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new NotFoundException($"Пользователь с id {userId} не найден");

            UserBalanceRto balanceRto = new UserBalanceRto()
            {
                Balance = money,
                UserId = userId 
            };

            _context.UserBalances.Add(balanceRto);

            await _context.SaveChangesAsync();

            return money;
        }

        public async Task<int> UpdateUserBalance(int userId, int changeMoney)
        {
            UserRto? user = await _context.Users
                .Include(user => user.Balance)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new NotFoundException($"Пользователь с id {userId} не найден");

            int money = user.Balance.Balance;
            user.Balance.Balance += changeMoney;

            await _context.SaveChangesAsync();

            return money + changeMoney;
        }


        public async Task AddUserStar(int userWhoSendStarId, int userWhoGetStarId, int count)
        {
            var sendingUserRto = await _context.Users
                .AsNoTracking()
                .Include(e => e.UserWhoSendStars)
                .Include(e => e.UserWhoGetStars)
                .FirstOrDefaultAsync(e => e.Id == userWhoSendStarId);

            if (sendingUserRto == null)
                throw new ConflictException("Не удалось найти вас в нашей базе данных");

            if (!await _context.Users.AsNoTracking().AnyAsync(e => e.Id == userWhoGetStarId))
                throw new NotFoundException("Вы хотели добавить здезду несуществующему пользователю");

            var existingStarRto = await _context.UserStars
                .FirstOrDefaultAsync(e => e.UserWhoSendStarId == userWhoSendStarId && e.UserWhoGetStarId == userWhoGetStarId);

            if (existingStarRto != null)
                throw new ConflictException("Вы уже ставили звёзды этому пользователя");

            var starRto = new UserStarRto
            {
                UserWhoSendStarId = userWhoSendStarId,
                UserWhoGetStarId = userWhoGetStarId,
                CountStar = count
            };

            _context.UserStars.Add(starRto);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountStars(int userId)
        {
            UserRto? user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new NotFoundException($"Пользователь с id {userId} не найден");

            List<int> starBlos = await _context.UserStars
                .Where(e => e.UserWhoGetStarId == userId)
                .Select(e => e.CountStar)
                .ToListAsync();

            return starBlos.Sum() / starBlos.Count;
        }

        
        public async Task<UserCommentBlo> AddUserComment(int userWhoSendCommentId, int userWhoGetCommentId, string text)
        {
            var sendingUserRto = await _context.Users
                .AsNoTracking()
                .Include(e => e.UserWhoSendComments)
                .Include(e => e.UserWhoGetComments)
                .FirstOrDefaultAsync(e => e.Id == userWhoSendCommentId);

            if (sendingUserRto == null)
                throw new ConflictException("Не удалось найти вас в нашей базе данных");

            if (!await _context.Users.AsNoTracking().AnyAsync(e => e.Id == userWhoGetCommentId))
                throw new NotFoundException("Вы хотели добавить комментарий несуществующему пользователю");

            var existingCommentRto = await _context.UserComments
                .FirstOrDefaultAsync(e => e.UserWhoSendCommentId == userWhoSendCommentId && e.UserWhoGetCommentId == userWhoGetCommentId);

            if (existingCommentRto != null)
                throw new ConflictException("Вы уже оставляли комментарий этому пользователя");

            var commentRto = new UserCommentRto
            {
                UserWhoSendCommentId = userWhoSendCommentId,
                UserWhoGetCommentId = userWhoGetCommentId,
                Text = text
            };

            _context.UserComments.Add(commentRto);
            await _context.SaveChangesAsync();
            return ConvertToUserCommentBlo(commentRto);
        }

        public async Task<List<UserCommentBlo>> GetComments(int userId, int count, int skipCount)
        {
            bool doesExsist = await _context.Users
                .AnyAsync(x => x.Id == userId);

            if (doesExsist == false)
                throw new NotFoundException("Ой, у нас не нашлось такого пользователя");

            List<UserCommentRto> userCommentRtos = await _context.UserComments
                .Where(e => e.UserWhoGetCommentId == userId)
                .Skip(skipCount)
                .Take(count)
                .ToListAsync();

            if (userCommentRtos.Count == 0)
                throw new NotFoundException("У пользователя нет комментариев");

            return ConvertToUserCommentBloList(userCommentRtos);
        }
        
        public async Task<int> GetCommentAmount(int userId)
        {
            bool doesExsist = await _context.Users
                .AnyAsync(x => x.Id == userId);

            if (doesExsist == false)
                throw new NotFoundException("Ой, у нас не нашлось такого пользователя");

            int userCommentCount = await _context.UserComments
                .CountAsync(e => e.UserWhoGetCommentId == userId);

            return userCommentCount;
        }


        public async Task<AuctionBlo> AddAuction(int userId, AuctionCreateBlo auctionCreateBlo)
        {
            UserRto? user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new NotFoundException($"Пользователь с id {userId} не найден");

            var auctionRto = new AuctionRto
            {
                UserWhoUploadId = userId,
                LotName = auctionCreateBlo.LotName,
                ImageUrl = auctionCreateBlo.ImageUrl,
                StartCost = auctionCreateBlo.StartCost,
                NowCost = auctionCreateBlo.NowCost,
                Location = auctionCreateBlo.Location,
                IsDeleted = false,
                Description = auctionCreateBlo.Description,
                DateCreation = DateTime.Now,
                Duration = auctionCreateBlo.Duration,
                StatusId = 1
            };

            _context.Auctions.Add(auctionRto);
            await _context.SaveChangesAsync();
            return ConvertToAuctionBlo(auctionRto);
        }
        
        public async Task<AuctionBlo> UpdateInfoAuction(int auctionId, int NowCost)
        {
            AuctionRto? auction = await _context.Auctions
                .FirstOrDefaultAsync(x => x.Id == auctionId);

            if (auction == null)
                throw new NotFoundException($"Аукцион с id {auctionId} не найден");

            auction.NowCost = NowCost;
            await _context.SaveChangesAsync();

            return ConvertToAuctionBlo(auction);
        }

        public async Task<AuctionBlo> UpdateStatusAuction(int auctionId, int auctionStatusModerationId)
        {
            AuctionRto? auction = await _context.Auctions
                .FirstOrDefaultAsync(x => x.Id == auctionId);

            if (auction == null)
                throw new NotFoundException($"Аукцион с id {auctionId} не найден");

            auction.StatusId = auctionStatusModerationId;
            await _context.SaveChangesAsync();

            return ConvertToAuctionBlo(auction);
        }
        
        public async Task<AuctionBlo> GetAuction(int auctionId)
        {
            AuctionRto? auction = await _context.Auctions
                .FirstOrDefaultAsync(x => x.Id == auctionId);

            if (auction == null)
                throw new NotFoundException($"Аукцион с id {auctionId} не найден");

            return ConvertToAuctionBlo(auction);
        }

        public async Task<AuctionBlo> MarkAsDeletedAuction(int auctionId)
        {
            AuctionRto? auction = await _context.Auctions
                .FirstOrDefaultAsync(x => x.Id == auctionId);

            if (auction == null)
                throw new NotFoundException($"Аукцион с id {auctionId} не найден");

            auction.IsDeleted = true;
            await _context.SaveChangesAsync();
            return ConvertToAuctionBlo(auction);
        }

        public async Task<List<AuctionBlo>> AllAuctionsWhichCreatUser(int userId, int count, int skipCount)
        {
            bool doesExsist = await _context.Users
                .AnyAsync(x => x.Id == userId);

            if (doesExsist == false)
                throw new NotFoundException("Ой, у нас не нашлось такого пользователя");

            List<AuctionRto> auctionRtos = await _context.Auctions
                .Where(e => e.UserWhoUploadId == userId)
                .Skip(skipCount)
                .Take(count)
                .ToListAsync();

            if (auctionRtos.Count == 0)
                throw new NotFoundException("У пользователя нет аукционов");

            return ConvertToAuctionBloList(auctionRtos);
        }

        public async Task<BuyLotBlo> AddBuy(int userId, int auctionId)
        {
            UserRto? user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new NotFoundException($"Пользователь с id {userId} не найден");

            AuctionRto? auction = await _context.Auctions
                .FirstOrDefaultAsync(x => x.Id == auctionId);

            if (auction == null)
                throw new NotFoundException($"Аукцион с id {auctionId} не найден");

            return None;
        }

        private List<AuctionBlo> ConvertToAuctionBloList(List<AuctionRto> auctionRtos)
        {
            if (auctionRtos == null || auctionRtos.Count < 1)
                throw new ArgumentNullException(nameof(auctionRtos));

            List<AuctionBlo> auctionBlos = new();
            for (int i = 0; i < auctionRtos.Count; i++)
            {
                auctionBlos.Add(_mapper.Map<AuctionBlo>(auctionRtos[i]));
            }

            return auctionBlos;
        }

        private AuctionBlo ConvertToAuctionBlo(AuctionRto auctionRto)
        {
            if (auctionRto == null)
                throw new ArgumentNullException(nameof(auctionRto));

            AuctionBlo auctionBlo = _mapper.Map<AuctionBlo>(auctionRto);
            return auctionBlo;
        }

        private List<UserCommentBlo> ConvertToUserCommentBloList(List<UserCommentRto> userCommentRtos)
        {
            if (userCommentRtos == null || userCommentRtos.Count < 1)
                throw new ArgumentNullException(nameof(userCommentRtos));

            List<UserCommentBlo> userCommentBlos = new();
            for (int i = 0; i < userCommentRtos.Count; i++)
            {
                userCommentBlos.Add(_mapper.Map<UserCommentBlo>(userCommentRtos[i]));
            }

            return userCommentBlos;
        }

        private UserCommentBlo ConvertToUserCommentBlo(UserCommentRto commentRto)
        {
            if (commentRto == null)
                throw new ArgumentNullException(nameof(commentRto));

            UserCommentBlo userCommentBlo = _mapper.Map<UserCommentBlo>(commentRto);
            return userCommentBlo;
        }

        private UserInformationBlo ConvertToUserInformation(UserRto userRto)
        {
            if (userRto == null)
                throw new ArgumentNullException(nameof(userRto));

            UserInformationBlo userInformationBlo = _mapper.Map<UserInformationBlo>(userRto);
            return userInformationBlo;
        }
    }
}

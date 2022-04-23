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

            List<UserStarRto> starBlos = await _context.UserStars
                .Include(e => e.CountStar)
                .Where(e => e.UserWhoGetStarId == userId)
                .ToListAsync();

            List<int> AllStar = new();
            for (int i = 0; i < starBlos.Count; i++)
            {
                AllStar.Add(starBlos);
            }

            return medalCategoryInformationBlos;

            return 1;
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

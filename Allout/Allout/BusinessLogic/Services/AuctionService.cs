using Allout.BusinessLogic.Core.Interfaces;
using Allout.BusinessLogic.Core.Models;
using Allout.DataAccess.Core.Interfaces.DBContext;
using Allout.DataAccess.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Share.Exceptions;

namespace Allout.BusinessLogic.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IMapper _mapper;
        private readonly IContext _context;

        public AuctionService(IMapper mapper, IContext context)
        {
            _mapper = mapper;
            _context = context;
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
                NowCost = auctionCreateBlo.StartCost,
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

        public async Task<AuctionBlo> UpdateInfoAuction(int auctionId, float NowCost)
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

        public async Task<List<AuctionBlo>> AllAuctionsWhichMemberUser(int userId, int count, int skipCount)
        {
            bool doesExsist = await _context.Users
                .AnyAsync(x => x.Id == userId);

            if (doesExsist == false)
                throw new NotFoundException("Ой, у нас не нашлось такого пользователя");

            List<AuctionRto> auctionRtos = await _context.BuyLots
                .AsNoTracking()
                .Include(e => e.Auction)
                .Where(e => e.UserWhoBuyId == userId)
                .Select(x => x.Auction)
                .ToListAsync();

            if (auctionRtos.Count == 0)
                throw new NotFoundException("У пользователя нет аукционов");

            return ConvertToAuctionBloList(auctionRtos);
        }

        public async Task<List<AuctionBlo>> AllAvailableAuctions(int userId, int count, int skipCount)
        {
            bool doesExsist = await _context.Users
                .AnyAsync(x => x.Id == userId);

            if (doesExsist == false)
                throw new NotFoundException("Ой, у нас не нашлось такого пользователя");

            List<AuctionRto> auctionRtos = await _context.Auctions
                .AsNoTracking()
                .ToListAsync();

            List<AuctionRto> auctionRto = new();
            foreach (var item in auctionRtos)
            {
                TimeSpan interval = DateTime.Now - item.DateCreation;
                if (interval.Hours < item.Duration.Hour)
                {
                    auctionRto.Add(item);
                }
            }

            return ConvertToAuctionBloList(auctionRto);
        }


        private List<AuctionBlo> ConvertToAuctionBloList(List<AuctionRto> auctionRtos)
        {
            if (auctionRtos == null)
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
    }
}

using Allout.BusinessLogic.Core.Interfaces;
using Allout.BusinessLogic.Core.Models;
using Allout.DataAccess.Core.Interfaces.DBContext;
using Allout.DataAccess.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Share.Exceptions;
using Shared.Exceptions;

namespace Allout.BusinessLogic.Services
{
    public class BuyLotService : IBuyLotService
    {
        private readonly IMapper _mapper;
        private readonly IContext _context;

        public BuyLotService(IMapper mapper, IContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<BuyLotBlo> AddBuy(int userId, int auctionId, BuyLotCreateBlo buyLotCreateBlo)
        {
            UserRto? user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new NotFoundException($"Пользователь с id {userId} не найден");

            AuctionRto? auction = await _context.Auctions
                .FirstOrDefaultAsync(x => x.Id == auctionId);

            if (auction == null)
                throw new NotFoundException($"Аукцион с id {auctionId} не найден");

            var buyLosts = await _context.BuyLots
                .AsNoTracking()
                .Where(e => e.AuctionId == auctionId)
                .Select(e => e.Money)
                .ToListAsync();

            float MaxMoney = buyLosts.Max();

            if (MaxMoney >= buyLotCreateBlo.Money)
            {
                throw new ConflictException($"Новая цена {buyLotCreateBlo.Money} меньше старой цены");
            }
           
            BuyLotRto buyLotRto = new()
            { 
                UserWhoBuyId = userId,
                AuctionId = auctionId,
                PurchaseDate = buyLotCreateBlo.PurchaseDate,
                Money = buyLotCreateBlo.Money
            };

            _context.BuyLots.Add(buyLotRto);
            await _context.SaveChangesAsync();

            return ConvertToBuyLotBlo(buyLotRto);
        }

        public async Task<int> GetLastBuyUserId(int auctionId)
        {
            AuctionRto? auction = await _context.Auctions
                .FirstOrDefaultAsync(x => x.Id == auctionId);

            if (auction == null)
                throw new NotFoundException($"Аукцион с id {auctionId} не найден");

            var auctionRto = await _context.BuyLots
                .Where(e => e.AuctionId == auctionId)
                .OrderByDescending(e => e.Money)
                .ToListAsync();

            return auctionRto[0].UserWhoBuyId;
        }


        private BuyLotBlo ConvertToBuyLotBlo(BuyLotRto buyLotRto)
        {
            if (buyLotRto == null)
                throw new ArgumentNullException(nameof(buyLotRto));

            BuyLotBlo buyLotBlo = _mapper.Map<BuyLotBlo>(buyLotRto);
            return buyLotBlo;
        }
    }
}

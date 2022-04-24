using Allout.BusinessLogic.Core.Models;

namespace Allout.BusinessLogic.Core.Interfaces
{
    public interface IBuyLotService
    {
        Task<BuyLotBlo> AddBuy(int userId, int auctionId, BuyLotCreateBlo buyLotCreateBlo);
        Task<int> GetLastBuyUserId(int auctionId);
    }
}

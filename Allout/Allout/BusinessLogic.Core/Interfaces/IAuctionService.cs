using Allout.BusinessLogic.Core.Models;

namespace Allout.BusinessLogic.Core.Interfaces
{
    public interface IAuctionService
    {
        Task<AuctionBlo> AddAuction(int userId, AuctionCreateBlo auctionCreateBlo);
        Task<AuctionBlo> UpdateInfoAuction(int auctionId, float NowCost);
        Task<AuctionBlo> UpdateStatusAuction(int auctionId, int auctionStatusModerationId);
        Task<AuctionBlo> GetAuction(int auctionId);
        Task<AuctionBlo> MarkAsDeletedAuction(int auctionId);
        Task<List<AuctionBlo>> AllAuctionsWhichCreatUser(int userId, int count, int skipCount);
        Task<List<AuctionBlo>> AllAuctionsWhichMemberUser(int userId, int count, int skipCount);
        Task<List<AuctionBlo>> AllAvailableAuctions(int userId, int count, int skipCount);
    }
}

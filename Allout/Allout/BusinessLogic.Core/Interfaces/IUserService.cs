using Allout.BusinessLogic.Core.Models;
using BusinessLogic.Core.Models;

namespace Allout.BusinessLogic.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserInformationBlo> Registration(UserIdentityBlo userIdentityBlo);
        Task<UserInformationBlo> Authenticate(UserIdentityBlo userIdentityBlo);
        Task<UserInformationBlo> GetUser(int userId);
        Task<UserInformationBlo> UpdateUser(int nickname, UserUpdateBlo userUpdateDobleBlo);
        Task<string> UpdateAvatarUser(string nickname, string avatarUrl);
        Task<UserInformationBlo> MarkAsDeletedUser(int userId);
        Task<bool> DoesExistUser(int nickname);

        Task<int> AddUserBalance(int userId, int money);
        Task<int> UpdateUserBalance(int userId, int changeMoney);

        Task AddUserStar(int userWhoSendStarId, int userWhoGetStarId, int count);
        Task<int> GetCountStars(int userId);

        Task<UserCommentBlo> AddUserComment(int userWhoSendCommentId, int userWhoGetCommentId, string text);
        Task<List<UserCommentBlo>> GetComments(int userId, int count, int skipCount);
        Task<int> GetCommentAmount(int userId);

        Task<AuctionBlo> AddAuction(int userId, AuctionCreateBlo auctionCreateBlo);
        Task<AuctionBlo> UpdateInfoAuction(int auctionId, int NowCost);
        Task<AuctionBlo> UpdateStatusAuction(int auctionId, int auctionStatusModerationId);
        Task<AuctionBlo> GetAuction(int auctionId);
        Task<AuctionBlo> MarkAsDeletedAuction(int auctionId);
        Task<List<AuctionBlo>> AllAuctionsWhichCreatUser(int userId, int count, int skipCount); // все аукционы, которые создал юзер
        Task<List<AuctionBlo>> AllAuctionsWhichMemberUser(int userId, int count, int skipCount); // все аукционы, в которых учавствовал юзер
        Task<List<AuctionBlo>> AllAvailableAuctions(int userId, int count, int skipCount); // сделать все доступные аукционы

        Task<BuyLotBlo> AddBuy(int userId, int auctionId, DateTime PurchaseDate, int money); // сделать
        Task<int> GetLastBuyUserId(int auctionId); // сделать
    }
}

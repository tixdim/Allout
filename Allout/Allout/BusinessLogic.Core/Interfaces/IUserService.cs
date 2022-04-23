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

        //все аукционы, которые создал юзер
        //все аукционы, в которых учавствовал юзер
        //все доступные аукционы

        Task<BuyLotBlo> AddBuy(int userId, int auctionId); // сделать
        Task<int> GetLastBuyUserUd(int auctionId); // сделать
    }
}

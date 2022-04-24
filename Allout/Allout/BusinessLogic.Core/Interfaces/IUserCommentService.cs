using BusinessLogic.Core.Models;

namespace Allout.BusinessLogic.Core.Interfaces
{
    public interface IUserCommentService
    {
        Task<UserCommentBlo> AddUserComment(int userWhoSendCommentId, int userWhoGetCommentId, string text);
        Task<List<UserCommentBlo>> GetComments(int userId, int count, int skipCount);
        Task<int> GetCommentAmount(int userId);
    }
}

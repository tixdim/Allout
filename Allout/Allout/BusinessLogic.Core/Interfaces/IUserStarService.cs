namespace Allout.BusinessLogic.Core.Interfaces
{
    public interface IUserStarService
    {
        Task AddUserStar(int userWhoSendStarId, int userWhoGetStarId, int count);
        Task<int> GetCountStars(int userId);
    }
}

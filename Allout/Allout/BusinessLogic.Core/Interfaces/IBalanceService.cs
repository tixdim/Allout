namespace Allout.BusinessLogic.Core.Interfaces
{
    public interface IBalanceService
    {
        Task<float> AddUserBalance(int userId, float money);
        Task<float> UpdateUserBalance(int userId, float changeMoney);
    }
}

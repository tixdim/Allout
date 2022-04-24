using Allout.BusinessLogic.Core.Interfaces;
using Allout.DataAccess.Core.Interfaces.DBContext;
using Allout.DataAccess.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Share.Exceptions;

namespace Allout.BusinessLogic.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly IContext _context;

        public BalanceService(IContext context)
        {
            _context = context;
        }

        public async Task<float> AddUserBalance(int userId, float money)
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

        public async Task<float> UpdateUserBalance(int userId, float changeMoney)
        {
            UserRto? user = await _context.Users
                .Include(user => user.Balance)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new NotFoundException($"Пользователь с id {userId} не найден");

            float money = user.Balance.Balance;
            user.Balance.Balance += changeMoney;

            await _context.SaveChangesAsync();

            return money + changeMoney;
        }
    }
}

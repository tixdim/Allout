using Allout.BusinessLogic.Core.Interfaces;
using Allout.DataAccess.Core.Interfaces.DBContext;
using Allout.DataAccess.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Share.Exceptions;
using Shared.Exceptions;

namespace Allout.BusinessLogic.Services
{
    public class UserStarService : IUserStarService
    {
        private readonly IContext _context;

        public UserStarService(IContext context)
        {
            _context = context;
        }

        public async Task AddUserStar(int userWhoSendStarId, int userWhoGetStarId, int count)
        {
            var sendingUserRto = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == userWhoSendStarId);

            if (sendingUserRto == null)
                throw new NotFoundException("Не удалось найти вас в нашей базе данных");

            if (!await _context.Users.AsNoTracking().AnyAsync(e => e.Id == userWhoGetStarId))
                throw new NotFoundException("Вы хотели добавить здезду несуществующему пользователю");

            var existingStarRto = await _context.UserStars
                .FirstOrDefaultAsync(e => e.UserWhoSendStarId == userWhoSendStarId && e.UserWhoGetStarId == userWhoGetStarId);

            if (existingStarRto != null)
                throw new ConflictException("Вы уже ставили звёзды этому пользователя");

            var starRto = new UserStarRto
            {
                UserWhoSendStarId = userWhoSendStarId,
                UserWhoGetStarId = userWhoGetStarId,
                CountStar = count
            };

            _context.UserStars.Add(starRto);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountStars(int userId)
        {
            UserRto? user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new NotFoundException($"Пользователь с id {userId} не найден");

            List<int> starBlos = await _context.UserStars
                .Where(e => e.UserWhoGetStarId == userId)
                .Select(e => e.CountStar)
                .ToListAsync();

            return starBlos.Sum() / starBlos.Count;
        }
    }
}

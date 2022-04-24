using Allout.BusinessLogic.Core.Interfaces;
using Allout.DataAccess.Core.Interfaces.DBContext;
using Allout.DataAccess.Core.Models;
using AutoMapper;
using BusinessLogic.Core.Models;
using Microsoft.EntityFrameworkCore;
using Share.Exceptions;
using Shared.Exceptions;

namespace Allout.BusinessLogic.Services
{
    public class UserCommentService : IUserCommentService
    {
        private readonly IMapper _mapper;
        private readonly IContext _context;

        public UserCommentService(IMapper mapper, IContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserCommentBlo> AddUserComment(int userWhoSendCommentId, int userWhoGetCommentId, string text)
        {
            var sendingUserRto = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == userWhoSendCommentId);

            if (sendingUserRto == null)
                throw new NotFoundException("Не удалось найти вас в нашей базе данных");

            if (!await _context.Users.AsNoTracking().AnyAsync(e => e.Id == userWhoGetCommentId))
                throw new NotFoundException("Вы хотели добавить комментарий несуществующему пользователю");

            var existingCommentRto = await _context.UserComments
                .FirstOrDefaultAsync(e => e.UserWhoSendCommentId == userWhoSendCommentId && e.UserWhoGetCommentId == userWhoGetCommentId);

            if (existingCommentRto != null)
                throw new ConflictException("Вы уже оставляли комментарий этому пользователя");

            var commentRto = new UserCommentRto
            {
                UserWhoSendCommentId = userWhoSendCommentId,
                UserWhoGetCommentId = userWhoGetCommentId,
                Text = text
            };

            _context.UserComments.Add(commentRto);
            await _context.SaveChangesAsync();

            return ConvertToUserCommentBlo(commentRto);
        }

        public async Task<List<UserCommentBlo>> GetComments(int userId, int count, int skipCount)
        {
            bool doesExsist = await _context.Users
                .AnyAsync(x => x.Id == userId);

            if (doesExsist == false)
                throw new NotFoundException("Ой, у нас не нашлось такого пользователя");

            List<UserCommentRto> userCommentRtos = await _context.UserComments
                .Where(e => e.UserWhoGetCommentId == userId)
                .Skip(skipCount)
                .Take(count)
                .ToListAsync();

            if (userCommentRtos.Count == 0)
                throw new NotFoundException("У пользователя нет комментариев");

            return ConvertToUserCommentBloList(userCommentRtos);
        }

        public async Task<int> GetCommentAmount(int userId)
        {
            bool doesExsist = await _context.Users
                .AnyAsync(x => x.Id == userId);

            if (doesExsist == false)
                throw new NotFoundException("Ой, у нас не нашлось такого пользователя");

            int userCommentCount = await _context.UserComments
                .CountAsync(e => e.UserWhoGetCommentId == userId);

            return userCommentCount;
        }

        private List<UserCommentBlo> ConvertToUserCommentBloList(List<UserCommentRto> userCommentRtos)
        {
            if (userCommentRtos == null)
                throw new ArgumentNullException(nameof(userCommentRtos));

            List<UserCommentBlo> userCommentBlos = new();
            for (int i = 0; i < userCommentRtos.Count; i++)
            {
                userCommentBlos.Add(_mapper.Map<UserCommentBlo>(userCommentRtos[i]));
            }

            return userCommentBlos;
        }

        private UserCommentBlo ConvertToUserCommentBlo(UserCommentRto commentRto)
        {
            if (commentRto == null)
                throw new ArgumentNullException(nameof(commentRto));

            UserCommentBlo userCommentBlo = _mapper.Map<UserCommentBlo>(commentRto);
            return userCommentBlo;
        }
    }
}

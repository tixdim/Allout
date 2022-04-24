using Allout.BusinessLogic.Core.Interfaces;
using AutoMapper;
using Allout.DataAccess.Core.Interfaces.DBContext;
using Allout.DataAccess.Core.Models;
using Microsoft.EntityFrameworkCore;
using Share.Exceptions;
using BusinessLogic.Core.Models;

namespace Allout.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IContext _context;

        public UserService(IMapper mapper, IContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserInformationBlo> Registration(UserIdentityBlo userIdentityBlo)
        {
            if (await _context.Users.AnyAsync(x => x.Nickname.ToLower() == userIdentityBlo.Nickname!.ToLower()))
                throw new BadRequestException($"Пользователь с никнеймом {userIdentityBlo.Nickname} уже существует");

            UserRto userRto = new UserRto()
            {
                Email = userIdentityBlo.Email,
                Nickname = userIdentityBlo.Nickname!,
                Password = userIdentityBlo.Password!,
                IsDeleted = false
            };

            _context.Users.Add(userRto);
            await _context.SaveChangesAsync();
            return ConvertToUserInformation(userRto);
        }

        public async Task<UserInformationBlo> Authenticate(UserIdentityBlo userIdentityBlo)
        {
            UserRto? user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Nickname.ToLower() == userIdentityBlo.Nickname!.ToLower() && x.Password == userIdentityBlo.Password!);

            if (user == null)
                throw new BadRequestException("Неверное имя пользователя или пароль");

            return ConvertToUserInformation(user);
        }

        public async Task<UserInformationBlo> GetUser(int userId)
        {
            UserRto? user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new NotFoundException($"Пользователь с id {userId} не найден");

            return ConvertToUserInformation(user);
        }

        public async Task<UserInformationBlo> UpdateUser(string nickname, UserUpdateBlo userUpdateBlo)
        {
            UserRto? user = await _context.Users
                .FirstOrDefaultAsync(x => x.Nickname.ToLower() == nickname.ToLower());

            if (user == null)
                throw new NotFoundException("Пользователь не найден");

            if (userUpdateBlo.CurrentPassword != null)
            {
                bool passwordIsGoog = await _context.Users.AnyAsync(x => x.Nickname.ToLower() == nickname.ToLower() && x.Password == userUpdateBlo.CurrentPassword);
                if (passwordIsGoog == false) throw new BadRequestException("Пользователь ввел не верный пароль");

                user.Email = userUpdateBlo.Email == null ? user.Email : userUpdateBlo.Email;
                user.Nickname = userUpdateBlo.Nickname == null ? user.Nickname : userUpdateBlo.Nickname;
                user.Password = userUpdateBlo.NewPassword == null ? user.Password : userUpdateBlo.NewPassword;
                user.AvatarUrl = userUpdateBlo.AvatarUrl == null ? user.AvatarUrl : userUpdateBlo.AvatarUrl;

                await _context.SaveChangesAsync();
            }
            else 
            {
                throw new BadRequestException("Пользователь ввел неверный пароль");
            }

            return ConvertToUserInformation(user);
        }

        public async Task<string> UpdateAvatarUser(string nickname, string avatarUrl)
        {
            UserRto? user = await _context.Users
                .FirstOrDefaultAsync(x => x.Nickname.ToLower() == nickname.ToLower());

            if (user == null)
                throw new NotFoundException("Пользователь не найден");

            user.AvatarUrl = avatarUrl;
            await _context.SaveChangesAsync();
            return avatarUrl;
        }

        public async Task<UserInformationBlo> MarkAsDeletedUser(int userId)
        {
            UserRto? user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new NotFoundException($"Пользователь с id {userId} не найден");

            user.IsDeleted = true;
            await _context.SaveChangesAsync();
            return ConvertToUserInformation(user);
        }

        public async Task<bool> DoesExistUser(string nickname)
        {
            return await _context.Users.AnyAsync(x => x.Nickname.ToLower() == nickname.ToLower());
        }


        private UserInformationBlo ConvertToUserInformation(UserRto userRto)
        {
            if (userRto == null)
                throw new ArgumentNullException(nameof(userRto));

            UserInformationBlo userInformationBlo = _mapper.Map<UserInformationBlo>(userRto);
            return userInformationBlo;
        }
    }
}

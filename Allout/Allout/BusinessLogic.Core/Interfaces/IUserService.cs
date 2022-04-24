using BusinessLogic.Core.Models;

namespace Allout.BusinessLogic.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserInformationBlo> Registration(UserIdentityBlo userIdentityBlo);
        Task<UserInformationBlo> Authenticate(UserIdentityBlo userIdentityBlo);
        Task<UserInformationBlo> GetUser(int userId);
        Task<UserInformationBlo> UpdateUser(string nickname, UserUpdateBlo userUpdateBlo);
        Task<string> UpdateAvatarUser(string nickname, string avatarUrl);
        Task<UserInformationBlo> MarkAsDeletedUser(int userId);
        Task<bool> DoesExistUser(string nickname);
    }
}

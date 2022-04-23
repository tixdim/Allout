using AutoMapper;
using Allout.DataAccess.Core.Models;
using BusinessLogic.Core.Models;

namespace Allout.BusinessLogic.AutoMapperProfile
{
    public class BusinessLogicProfile: Profile
    {
        public BusinessLogicProfile()
        {
            CreateMap<UserRto, UserInformationBlo>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.Email, x => x.MapFrom(m => m.Email))
                .ForMember(x => x.Nickname, x => x.MapFrom(m => m.Nickname))
                .ForMember(x => x.AvatarUrl, x => x.MapFrom(m => m.AvatarUrl));
            CreateMap<UserCommentRto, UserCommentBlo>()
                .ForMember(x => x.UserWhoSendCommentId, x => x.MapFrom(m => m.UserWhoSendCommentId))
                .ForMember(x => x.UserWhoGetCommentId, x => x.MapFrom(m => m.UserWhoGetCommentId))
                .ForMember(x => x.Text, x => x.MapFrom(m => m.Text));
        }
    }
}

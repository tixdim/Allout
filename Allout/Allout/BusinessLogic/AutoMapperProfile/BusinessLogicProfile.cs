using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Allout.BusinessLogic.Core.Models;
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

        }
    }
}

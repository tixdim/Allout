using Allout.API.Core.Models;
using Allout.BusinessLogic.Core.Models;
using Allout.Core;
using Allout.DataAccess.Core.Models;
using AutoMapper;
using BusinessLogic.Core.Models;

namespace Allout.BusinessLogic.AutoMapperProfile
{
    public class BusinessLogicProfile : Profile
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

            CreateMap<BuyLotRto, BuyLotBlo>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.AuctionId, x => x.MapFrom(m => m.AuctionId))
                .ForMember(x => x.UserWhoBuyId, x => x.MapFrom(m => m.UserWhoBuyId))
                .ForMember(x => x.PurchaseDate, x => x.MapFrom(m => m.PurchaseDate))
                .ForMember(x => x.Money, x => x.MapFrom(m => m.Money));

            CreateMap<AuctionRto, AuctionBlo>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.UserWhoUploadId, x => x.MapFrom(m => m.UserWhoUploadId))
                .ForMember(x => x.LotName, x => x.MapFrom(m => m.LotName))
                .ForMember(x => x.ImageUrl, x => x.MapFrom(m => m.ImageUrl))
                .ForMember(x => x.StartCost, x => x.MapFrom(m => m.StartCost))
                .ForMember(x => x.NowCost, x => x.MapFrom(m => m.NowCost))
                .ForMember(x => x.Location, x => x.MapFrom(m => m.Location))
                .ForMember(x => x.Description, x => x.MapFrom(m => m.Description))
                .ForMember(x => x.DateCreation, x => x.MapFrom(m => m.DateCreation))
                .ForMember(x => x.Duration, x => x.MapFrom(m => m.Duration))
                .ForMember(x => x.IsDeleted, x => x.MapFrom(m => m.IsDeleted))
                .ForMember(x => x.StatusId, x => x.MapFrom(m => m.StatusId));

            CreateMap<UserInformationBlo, UserInformationDto>();

            CreateMap<UserUpdateDto, UserUpdateBlo>();

            CreateMap<UserIdentityDto, UserIdentityBlo>();

            CreateMap<AuctionDto, AuctionBlo>();

            CreateMap<AuctionCreateDto, AuctionCreateBlo>();

            CreateMap<UserCommentDto, UserCommentBlo>();

            CreateMap<BuyLotCreateBlo, BuyLotCreateDto>();

            CreateMap<BuyLotDto, BuyLotBlo>();
        }
    }
}

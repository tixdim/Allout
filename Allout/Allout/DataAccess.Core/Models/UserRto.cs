using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Allout.DataAccess.Core.Models
{
    [Table("Users")]
    public class UserRto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Nickname { get; set; }
        [Required, MinLength(6)] public string? Password { get; set; }
        public bool IsDeleted { get; set; }
        public string? AvatarUrl { get; set; }
        public UserBalanceRto Balance { get; set; }
        public List<UserCommentRto> UserWhoSendComments { get; set; }
        public List<UserCommentRto> UserWhoGetComments { get; set; }
        public List<UserStarRto> UserWhoSendStars { get; set; }
        public List<UserStarRto> UserWhoGetStars { get; set; }
        public List<AuctionRto> UserWhoUploads { get; set; }
        public List<BuyLotRto> UserWhoBuys { get; set; }
    }
}

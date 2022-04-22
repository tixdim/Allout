using System.ComponentModel.DataAnnotations.Schema;

namespace Allout.DataAccess.Core.Models
{
    [Table("UserStars")]
    public class UserStarRto
    {
        public int UserWhoSendStarId { get; set; }
        public UserRto UserWhoSendStar { get; set; }
        public int UserWhoGetStarId { get; set; }
        public UserRto UserWhoGetStar { get; set; }
        public int Count { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace Allout.DataAccess.Core.Models
{
    [Table("Auctions")]
    public class AuctionRto
    {
        public int Id { get; set; }
        public int UserWhoUploadId { get; set; }
        public UserRto UserWhoUpload { get; set; }
        public string LotName { get; set; }
        public string ImageUrl { get; set; }
        public float StartCost { get; set; }
        public float NowCost { get; set; }
        public string Location { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime Duration { get; set; }
        public int StatusId { get; set; }
        public AuctionStatusModerationRto Status { get; set; }
        public List<BuyLotRto> BuyLots { get; set; }
    }
}

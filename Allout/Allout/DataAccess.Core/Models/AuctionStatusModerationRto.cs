using System.ComponentModel.DataAnnotations.Schema;

namespace Allout.DataAccess.Core.Models
{
    [Table("AuctionStatusModerations")]
    public class AuctionStatusModerationRto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<AuctionRto> Auctions { get; set; }
    }
}

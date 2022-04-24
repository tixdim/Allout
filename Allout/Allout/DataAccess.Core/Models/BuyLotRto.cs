using System.ComponentModel.DataAnnotations.Schema;

namespace Allout.DataAccess.Core.Models
{
    [Table("BuyLots")]
    public class BuyLotRto
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public AuctionRto Auction { get; set; }
        public int UserWhoBuyId { get; set; }
        public UserRto UserWhoBuy { get; set; }
        public DateTime PurchaseDate { get; set; }
        public float Money { get; set; }
    }
}

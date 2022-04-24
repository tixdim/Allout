namespace Allout.API.Core.Models
{
    public class BuyLotDto
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public int UserWhoBuyId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public float Money { get; set; }
    }
}

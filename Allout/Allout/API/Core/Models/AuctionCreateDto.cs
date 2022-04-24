namespace Allout.API.Core.Models
{
    public class AuctionCreateDto
    {
        public string LotName { get; set; }
        public string ImageUrl { get; set; }
        public float StartCost { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime Duration { get; set; }
    }
}

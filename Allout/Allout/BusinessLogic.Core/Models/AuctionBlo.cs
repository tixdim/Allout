namespace Allout.BusinessLogic.Core.Models
{
    public class AuctionBlo
    {
        public int Id { get; set; }
        // public int UserWhoUploadId { get; set; }  спросить надо ли так 
        public string LotName { get; set; }
        public string ImageUrl { get; set; }
        public int StartCost { get; set; }
        public int NowCost { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime Duration { get; set; }
        // public int StatusId { get; set; } хз
    }
}

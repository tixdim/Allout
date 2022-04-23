namespace BusinessLogic.Core.Models
{
    public class UserCommentBlo
    {
        public int UserWhoSendCommentId { get; set; }
        public int UserWhoGetCommentId { get; set; }
        public string Text { get; set; }
    }
}

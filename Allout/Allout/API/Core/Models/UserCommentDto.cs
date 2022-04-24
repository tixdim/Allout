namespace Allout.API.Core.Models
{
    public class UserCommentDto
    {
        public int UserWhoSendCommentId { get; set; }
        public int UserWhoGetCommentId { get; set; }
        public string Text { get; set; }
    }
}

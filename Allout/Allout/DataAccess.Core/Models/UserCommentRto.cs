using System.ComponentModel.DataAnnotations.Schema;

namespace Allout.DataAccess.Core.Models
{
    [Table("UserComments")]
    public class UserCommentRto
    {
        public int UserWhoSendCommentId { get; set; }
        public UserRto UserWhoSendComment { get; set; }
        public int UserWhoGetCommentId { get; set; }
        public UserRto UserWhoGetComment { get; set; }
        public string Text { get; set; }
    }
}

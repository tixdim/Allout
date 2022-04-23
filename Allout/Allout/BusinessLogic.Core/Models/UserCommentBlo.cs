namespace BusinessLogic.Core.Models
{
    public class UserCommentBlo
    {
        public int Id { get; set; }
        public UserInformationBlo Author { get; set; }
        public string Text { get; set; }
    }
}

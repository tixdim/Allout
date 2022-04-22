using System.ComponentModel.DataAnnotations.Schema;

namespace Allout.DataAccess.Core.Models
{
    [Table("UserBalances")]
    public class UserBalanceRto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserRto User { get; set; }
        public int Balance { get; set; }
    }
}

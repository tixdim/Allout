using Microsoft.EntityFrameworkCore;
using Allout.DataAccess.Core.Models;

namespace Allout.DataAccess.Core.Interfaces.DBContext
{
    public interface IContext : IDisposable, IAsyncDisposable
    {
        DbSet<UserRto> Users { get; set; }
        DbSet<UserCommentRto> UserComments { get; set; }
        DbSet<UserStarRto> UserStars { get; set; }
        DbSet<UserBalanceRto> UserBalances { get; set; }
        DbSet<BuyLotRto> BuyLots { get; set; }
        DbSet<AuctionRto> Auctions { get; set; }
        DbSet<AuctionStatusModerationRto> AuctionStatusModerations { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

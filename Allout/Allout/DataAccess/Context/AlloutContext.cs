using Microsoft.EntityFrameworkCore;
using Allout.DataAccess.Core.Interfaces.DBContext;
using Allout.DataAccess.Core.Models;

namespace Allout.DataAccess.Context
{
    public class AlloutContext : DbContext, IContext
    {
        public AlloutContext(DbContextOptions<AlloutContext> options) : base(options)
        {
        }   

        public DbSet<UserRto> Users { get; set; }
        public DbSet<UserCommentRto> UserComments { get; set; }
        public DbSet<UserStarRto> UserStars { get; set; }
        public DbSet<UserBalanceRto> UserBalances { get; set; }
        public DbSet<BuyLotRto> BuyLots { get; set; }
        public DbSet<AuctionRto> Auctions { get; set; }
        public DbSet<AuctionStatusModerationRto> AuctionStatusModerations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserStarRto>(builder =>
            {
                builder.HasKey(e => new
                {
                    e.UserWhoSendStarId,
                    e.UserWhoGetStarId
                });

                builder.HasIndex(e => e.UserWhoSendStarId);
                builder.HasIndex(e => e.UserWhoGetStarId);

                builder.HasOne<UserRto>(e => e.UserWhoSendStar)
                    .WithMany(e => e.UserWhoSendStars)
                    .HasForeignKey(e => e.UserWhoSendStarId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();

                builder.HasOne<UserRto>(e => e.UserWhoGetStar)
                    .WithMany(e => e.UserWhoGetStars)
                    .HasForeignKey(e => e.UserWhoGetStarId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();
            });

            modelBuilder.Entity<UserCommentRto>(builder =>
            {
                builder.HasKey(e => new
                {
                    e.UserWhoSendCommentId,
                    e.UserWhoGetCommentId
                });

                builder.HasIndex(e => e.UserWhoSendCommentId);
                builder.HasIndex(e => e.UserWhoGetCommentId);

                builder.HasOne<UserRto>(e => e.UserWhoSendComment)
                    .WithMany(e => e.UserWhoSendComments)
                    .HasForeignKey(e => e.UserWhoSendCommentId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();

                builder.HasOne<UserRto>(e => e.UserWhoGetComment)
                    .WithMany(e => e.UserWhoGetComments)
                    .HasForeignKey(e => e.UserWhoGetCommentId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();
            });
        }
    }
}

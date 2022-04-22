using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Allout.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuctionStatusModerations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionStatusModerations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    NickName = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    AvatarUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserWhoUploadId = table.Column<int>(type: "INTEGER", nullable: false),
                    LotName = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    StartCost = table.Column<int>(type: "INTEGER", nullable: false),
                    NowCost = table.Column<int>(type: "INTEGER", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auctions_AuctionStatusModerations_StatusId",
                        column: x => x.StatusId,
                        principalTable: "AuctionStatusModerations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Auctions_Users_UserWhoUploadId",
                        column: x => x.UserWhoUploadId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBalances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Balance = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBalances_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserComments",
                columns: table => new
                {
                    UserWhoSendCommentId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserWhoGetCommentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserComments", x => new { x.UserWhoSendCommentId, x.UserWhoGetCommentId });
                    table.ForeignKey(
                        name: "FK_UserComments_Users_UserWhoGetCommentId",
                        column: x => x.UserWhoGetCommentId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserComments_Users_UserWhoSendCommentId",
                        column: x => x.UserWhoSendCommentId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserStars",
                columns: table => new
                {
                    UserWhoSendStarId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserWhoGetStarId = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStars", x => new { x.UserWhoSendStarId, x.UserWhoGetStarId });
                    table.ForeignKey(
                        name: "FK_UserStars_Users_UserWhoGetStarId",
                        column: x => x.UserWhoGetStarId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserStars_Users_UserWhoSendStarId",
                        column: x => x.UserWhoSendStarId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BuyLots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AuctionId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserWhoBuyId = table.Column<int>(type: "INTEGER", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyLots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyLots_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyLots_Users_UserWhoBuyId",
                        column: x => x.UserWhoBuyId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_StatusId",
                table: "Auctions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_UserWhoUploadId",
                table: "Auctions",
                column: "UserWhoUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyLots_AuctionId",
                table: "BuyLots",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyLots_UserWhoBuyId",
                table: "BuyLots",
                column: "UserWhoBuyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBalances_UserId",
                table: "UserBalances",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserComments_UserWhoGetCommentId",
                table: "UserComments",
                column: "UserWhoGetCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserComments_UserWhoSendCommentId",
                table: "UserComments",
                column: "UserWhoSendCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStars_UserWhoGetStarId",
                table: "UserStars",
                column: "UserWhoGetStarId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStars_UserWhoSendStarId",
                table: "UserStars",
                column: "UserWhoSendStarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyLots");

            migrationBuilder.DropTable(
                name: "UserBalances");

            migrationBuilder.DropTable(
                name: "UserComments");

            migrationBuilder.DropTable(
                name: "UserStars");

            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.DropTable(
                name: "AuctionStatusModerations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebshopPractice.Server.Migrations
{
    /// <inheritdoc />
    public partial class renamesellerinfotoseller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SellerInfo_SellerInfoId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SellerInfo");

            migrationBuilder.RenameColumn(
                name: "SellerInfoId",
                table: "AspNetUsers",
                newName: "SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_SellerInfoId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_SellerId");

            migrationBuilder.CreateTable(
                name: "Seller",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrganizationName = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Verified = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    VerifiedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seller", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Seller_SellerId",
                table: "AspNetUsers",
                column: "SellerId",
                principalTable: "Seller",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Seller_SellerId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Seller");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "AspNetUsers",
                newName: "SellerInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_SellerId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_SellerInfoId");

            migrationBuilder.CreateTable(
                name: "SellerInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    OrganizationName = table.Column<string>(type: "TEXT", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false),
                    Verified = table.Column<int>(type: "INTEGER", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerInfo", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SellerInfo_SellerInfoId",
                table: "AspNetUsers",
                column: "SellerInfoId",
                principalTable: "SellerInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

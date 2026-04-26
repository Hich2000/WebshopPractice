using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebshopPractice.Server.Migrations
{
    /// <inheritdoc />
    public partial class sellerinfodeletebehaviour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SellerInfo_SellerInfoId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SellerInfo_SellerInfoId",
                table: "AspNetUsers",
                column: "SellerInfoId",
                principalTable: "SellerInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SellerInfo_SellerInfoId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SellerInfo_SellerInfoId",
                table: "AspNetUsers",
                column: "SellerInfoId",
                principalTable: "SellerInfo",
                principalColumn: "Id");
        }
    }
}

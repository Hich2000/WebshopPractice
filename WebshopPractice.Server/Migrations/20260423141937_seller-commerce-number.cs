using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebshopPractice.Server.Migrations
{
    /// <inheritdoc />
    public partial class sellercommercenumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommerceNumber",
                table: "Seller",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommerceNumber",
                table: "Seller");
        }
    }
}

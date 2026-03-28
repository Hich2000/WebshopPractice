using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebshopPractice.Server.Migrations
{
    /// <inheritdoc />
    public partial class RootTestingUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO Users (Name, Username, Password, UserLevel) VALUES ('Root', 'root@admin.com', 'password', 1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM Users WHERE Username = 'root@admin.com'");
        }
    }
}

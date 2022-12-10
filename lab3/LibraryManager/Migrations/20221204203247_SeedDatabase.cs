using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManager.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Books",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Date", "Leased", "Publisher", "Reserved", "Title", "Username" },
                values: new object[,]
                {
                    { 1, "Jeremy Clarkson", 2020, null, "Penguin Random House UK", null, "Can You Make This Thing Go Faster", null },
                    { 2, "Jeremy Clarkson", 2020, null, "Penguin Random House UK", null, "Diddly Squat - a Year on the Farm", null },
                    { 3, "F. Scott Fitzgerald", 1925, null, "Charles Scribner's Sons", null, "The Great Gatsby", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Username", "IsAdmin", "Password" },
                values: new object[,]
                {
                    { "james", false, "123" },
                    { "jeremy", false, "123" },
                    { "librarian", true, "123" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_Username",
                table: "Books",
                column: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_Username",
                table: "Books",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_Username",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_Username",
                table: "Books");

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "james");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "jeremy");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "librarian");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "Books",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

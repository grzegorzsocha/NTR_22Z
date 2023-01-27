using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Author = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<int>(type: "integer", nullable: false),
                    Publisher = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Reserved = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Leased = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username");
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

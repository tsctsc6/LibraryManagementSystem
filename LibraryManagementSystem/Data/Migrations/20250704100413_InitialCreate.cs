using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    BookSortCallNumber = table.Column<string>(type: "nchar(15)", maxLength: 15, nullable: false),
                    BookFormCallNumber = table.Column<string>(type: "nchar(15)", maxLength: 15, nullable: false),
                    BookName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PublishingHouse = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PublicDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ManageBy = table.Column<string>(type: "char(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => new { x.BookSortCallNumber, x.BookFormCallNumber });
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    LocationLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    LocationParent = table.Column<int>(type: "int", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ManageBy = table.Column<string>(type: "char(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => new { x.LocationLevel, x.LocationId });
                });

            migrationBuilder.CreateTable(
                name: "BookLocation",
                columns: table => new
                {
                    BookSortCallNumber = table.Column<string>(type: "nchar(15)", nullable: false),
                    BooksBookFormCallNumber = table.Column<string>(type: "nchar(15)", nullable: false),
                    LocationsLocationLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    LocationsLocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLocation", x => new { x.BookSortCallNumber, x.BooksBookFormCallNumber, x.LocationsLocationLevel, x.LocationsLocationId });
                    table.ForeignKey(
                        name: "FK_BookLocation_Book_BookSortCallNumber_BooksBookFormCallNumber",
                        columns: x => new { x.BookSortCallNumber, x.BooksBookFormCallNumber },
                        principalTable: "Book",
                        principalColumns: new[] { "BookSortCallNumber", "BookFormCallNumber" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookLocation_Location_LocationsLocationLevel_LocationsLocationId",
                        columns: x => new { x.LocationsLocationLevel, x.LocationsLocationId },
                        principalTable: "Location",
                        principalColumns: new[] { "LocationLevel", "LocationId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    BookSortCallNumber = table.Column<string>(type: "nchar(15)", maxLength: 15, nullable: false),
                    BookFormCallNumber = table.Column<string>(type: "nchar(15)", maxLength: 15, nullable: false),
                    LocationLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    StoreDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StoreNum = table.Column<byte>(type: "tinyint", nullable: false),
                    RemainNum = table.Column<byte>(type: "tinyint", nullable: false),
                    ManageBy = table.Column<string>(type: "char(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => new { x.BookSortCallNumber, x.BookFormCallNumber, x.LocationLevel, x.LocationId });
                    table.ForeignKey(
                        name: "FK_Store_Book_BookSortCallNumber_BookFormCallNumber",
                        columns: x => new { x.BookSortCallNumber, x.BookFormCallNumber },
                        principalTable: "Book",
                        principalColumns: new[] { "BookSortCallNumber", "BookFormCallNumber" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Store_Location_LocationLevel_LocationId",
                        columns: x => new { x.LocationLevel, x.LocationId },
                        principalTable: "Location",
                        principalColumns: new[] { "LocationLevel", "LocationId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_BookName",
                table: "Book",
                column: "BookName");

            migrationBuilder.CreateIndex(
                name: "IX_BookLocation_LocationsLocationLevel_LocationsLocationId",
                table: "BookLocation",
                columns: new[] { "LocationsLocationLevel", "LocationsLocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Store_LocationLevel_LocationId",
                table: "Store",
                columns: new[] { "LocationLevel", "LocationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookLocation");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Location");
        }
    }
}

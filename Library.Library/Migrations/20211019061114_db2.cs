using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Library.Migrations
{
    public partial class db2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_IdLibraryCode",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_Books_IdLibraryCode",
                table: "Books",
                column: "IdLibraryCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_IdLibraryCode",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_Books_IdLibraryCode",
                table: "Books",
                column: "IdLibraryCode",
                unique: true);
        }
    }
}

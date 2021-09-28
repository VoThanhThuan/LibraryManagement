using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Library.Migrations
{
    public partial class db2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryCode_Books_Id",
                table: "LibraryCode");

            migrationBuilder.AlterColumn<string>(
                name: "IdLibraryCode",
                table: "Books",
                type: "nvarchar(8)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_IdLibraryCode",
                table: "Books",
                column: "IdLibraryCode",
                unique: true,
                filter: "[IdLibraryCode] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_LibraryCode_IdLibraryCode",
                table: "Books",
                column: "IdLibraryCode",
                principalTable: "LibraryCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_LibraryCode_IdLibraryCode",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_IdLibraryCode",
                table: "Books");

            migrationBuilder.AlterColumn<string>(
                name: "IdLibraryCode",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryCode_Books_Id",
                table: "LibraryCode",
                column: "Id",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

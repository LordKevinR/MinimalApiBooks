using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalApiBooks.Migrations
{
    /// <inheritdoc />
    public partial class BooksRepair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ISBM",
                table: "Books",
                newName: "ISBN");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImage",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ISBN",
                table: "Books",
                newName: "ISBM");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImage",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

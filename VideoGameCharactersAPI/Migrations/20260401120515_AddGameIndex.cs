using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoGameCharactersAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddGameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Game",
                table: "Characters",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Game",
                table: "Characters",
                column: "Game");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Characters_Game",
                table: "Characters");

            migrationBuilder.AlterColumn<string>(
                name: "Game",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}

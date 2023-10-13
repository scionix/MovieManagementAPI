using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Biographymodelupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Biographies",
                columns: new[] { "Id", "ActorId", "Description" },
                values: new object[] { 1, 1, "Early internet meme." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Biographies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}

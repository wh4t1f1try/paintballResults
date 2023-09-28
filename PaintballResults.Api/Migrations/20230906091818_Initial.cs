using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintballResults.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gameresults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Gameday = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamOne = table.Column<string>(type: "TEXT", nullable: true),
                    TeamTwo = table.Column<string>(type: "TEXT", nullable: true),
                    TeamOneMatchPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamTwoMatchPoints = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gameresults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gameresults");
        }
    }
}

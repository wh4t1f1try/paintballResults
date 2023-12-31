﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paintball.Database.Migrations
{
    using System.Diagnostics.CodeAnalysis;

   
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameDay = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamOne = table.Column<string>(type: "TEXT", nullable: false),
                    TeamTwo = table.Column<string>(type: "TEXT", nullable: false),
                    TeamOneMatchPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamTwoMatchPoints = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameResults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameResults");
        }
    }
}

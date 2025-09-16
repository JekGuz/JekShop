using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JekShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class FileToAppication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Spaceship",
                table: "Spaceship");

            migrationBuilder.RenameTable(
                name: "Spaceship",
                newName: "Spaceships");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spaceships",
                table: "Spaceships",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FileToApis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExistingFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpaceshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileToApis", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileToApis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Spaceships",
                table: "Spaceships");

            migrationBuilder.RenameTable(
                name: "Spaceships",
                newName: "Spaceship");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spaceship",
                table: "Spaceship",
                column: "Id");
        }
    }
}

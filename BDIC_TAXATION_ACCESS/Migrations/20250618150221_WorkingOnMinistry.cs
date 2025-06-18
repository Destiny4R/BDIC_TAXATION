using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDIC_TAXATION_ACCESS.Migrations
{
    /// <inheritdoc />
    public partial class WorkingOnMinistry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MDAsMinistries",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "EstablistedDate",
                table: "MDAsMinistries",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "MDAsMinistries");

            migrationBuilder.DropColumn(
                name: "EstablistedDate",
                table: "MDAsMinistries");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDIC_TAXATION_ACCESS.Migrations
{
    /// <inheritdoc />
    public partial class OptionalForeighKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileReturnIndividuals_BusinessTaxpayers_BusinessTaxId",
                table: "FileReturnIndividuals");

            migrationBuilder.DropForeignKey(
                name: "FK_FileReturnIndividuals_IndividualTaxpayers_IndividualId",
                table: "FileReturnIndividuals");

            migrationBuilder.AlterColumn<int>(
                name: "IndividualId",
                table: "FileReturnIndividuals",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessTaxId",
                table: "FileReturnIndividuals",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_FileReturnIndividuals_BusinessTaxpayers_BusinessTaxId",
                table: "FileReturnIndividuals",
                column: "BusinessTaxId",
                principalTable: "BusinessTaxpayers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileReturnIndividuals_IndividualTaxpayers_IndividualId",
                table: "FileReturnIndividuals",
                column: "IndividualId",
                principalTable: "IndividualTaxpayers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileReturnIndividuals_BusinessTaxpayers_BusinessTaxId",
                table: "FileReturnIndividuals");

            migrationBuilder.DropForeignKey(
                name: "FK_FileReturnIndividuals_IndividualTaxpayers_IndividualId",
                table: "FileReturnIndividuals");

            migrationBuilder.AlterColumn<int>(
                name: "IndividualId",
                table: "FileReturnIndividuals",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessTaxId",
                table: "FileReturnIndividuals",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FileReturnIndividuals_BusinessTaxpayers_BusinessTaxId",
                table: "FileReturnIndividuals",
                column: "BusinessTaxId",
                principalTable: "BusinessTaxpayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileReturnIndividuals_IndividualTaxpayers_IndividualId",
                table: "FileReturnIndividuals",
                column: "IndividualId",
                principalTable: "IndividualTaxpayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

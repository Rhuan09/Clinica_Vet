using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinica_Vet.Migrations
{
    /// <inheritdoc />
    public partial class attconsulta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnimalId",
                table: "Consultas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Consultas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Consultas");
        }
    }
}

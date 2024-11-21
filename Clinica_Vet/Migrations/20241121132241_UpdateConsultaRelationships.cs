using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinica_Vet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConsultaRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Consultas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VeterinarioId1",
                table: "Consultas",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_ClienteId",
                table: "Consultas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_VeterinarioId1",
                table: "Consultas",
                column: "VeterinarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Clientes_ClienteId",
                table: "Consultas",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Veterinarios_VeterinarioId1",
                table: "Consultas",
                column: "VeterinarioId1",
                principalTable: "Veterinarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Clientes_ClienteId",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Veterinarios_VeterinarioId1",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_ClienteId",
                table: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_VeterinarioId1",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "VeterinarioId1",
                table: "Consultas");
        }
    }
}

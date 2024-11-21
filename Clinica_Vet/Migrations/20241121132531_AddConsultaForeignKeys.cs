using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinica_Vet.Migrations
{
    public partial class AddConsultaForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF;");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Veterinarios_VeterinarioId",
                table: "Consultas",
                column: "VeterinarioId",
                principalTable: "Veterinarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Clientes_ClienteId",
                table: "Consultas",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql("PRAGMA foreign_keys = ON;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF;");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Veterinarios_VeterinarioId",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Clientes_ClienteId",
                table: "Consultas");

            migrationBuilder.Sql("PRAGMA foreign_keys = ON;");
        }
    }
}

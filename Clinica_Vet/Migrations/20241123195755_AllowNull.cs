using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinica_Vet.Migrations
{
    /// <inheritdoc />
    public partial class AllowNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tratamentos_Animais_AnimalId",
                table: "Tratamentos");

            migrationBuilder.AddForeignKey(
                name: "FK_Tratamentos_Animais_AnimalId",
                table: "Tratamentos",
                column: "AnimalId",
                principalTable: "Animais",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tratamentos_Animais_AnimalId",
                table: "Tratamentos");

            migrationBuilder.AddForeignKey(
                name: "FK_Tratamentos_Animais_AnimalId",
                table: "Tratamentos",
                column: "AnimalId",
                principalTable: "Animais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

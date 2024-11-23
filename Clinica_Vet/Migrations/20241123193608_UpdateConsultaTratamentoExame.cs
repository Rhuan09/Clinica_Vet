using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinica_Vet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConsultaTratamentoExame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Clientes_ClienteId",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Tratamentos_TratamentoId",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Veterinarios_VeterinarioId",
                table: "Consultas");

            migrationBuilder.AddColumn<int>(
                name: "AnimalId1",
                table: "Tratamentos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Tratamentos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Resultado",
                table: "Exames",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "TratamentoId",
                table: "Consultas",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamentos_AnimalId1",
                table: "Tratamentos",
                column: "AnimalId1");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_AnimalId",
                table: "Consultas",
                column: "AnimalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Animais_AnimalId",
                table: "Consultas",
                column: "AnimalId",
                principalTable: "Animais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Clientes_ClienteId",
                table: "Consultas",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Tratamentos_TratamentoId",
                table: "Consultas",
                column: "TratamentoId",
                principalTable: "Tratamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Veterinarios_VeterinarioId",
                table: "Consultas",
                column: "VeterinarioId",
                principalTable: "Veterinarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tratamentos_Animais_AnimalId1",
                table: "Tratamentos",
                column: "AnimalId1",
                principalTable: "Animais",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Animais_AnimalId",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Clientes_ClienteId",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Tratamentos_TratamentoId",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Veterinarios_VeterinarioId",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tratamentos_Animais_AnimalId1",
                table: "Tratamentos");

            migrationBuilder.DropIndex(
                name: "IX_Tratamentos_AnimalId1",
                table: "Tratamentos");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_AnimalId",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "AnimalId1",
                table: "Tratamentos");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Tratamentos");

            migrationBuilder.AlterColumn<string>(
                name: "Resultado",
                table: "Exames",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TratamentoId",
                table: "Consultas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Clientes_ClienteId",
                table: "Consultas",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Tratamentos_TratamentoId",
                table: "Consultas",
                column: "TratamentoId",
                principalTable: "Tratamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Veterinarios_VeterinarioId",
                table: "Consultas",
                column: "VeterinarioId",
                principalTable: "Veterinarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

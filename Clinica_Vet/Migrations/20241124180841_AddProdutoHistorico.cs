using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinica_Vet.Migrations
{
    /// <inheritdoc />
    public partial class AddProdutoHistorico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Animais_AnimalId",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Veterinarios_VeterinarioId1",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tratamentos_Animais_AnimalId",
                table: "Tratamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Tratamentos_Animais_AnimalId1",
                table: "Tratamentos");

            migrationBuilder.DropIndex(
                name: "IX_Tratamentos_AnimalId1",
                table: "Tratamentos");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_VeterinarioId1",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "AnimalId1",
                table: "Tratamentos");

            migrationBuilder.DropColumn(
                name: "DataSaida",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "VeterinarioId1",
                table: "Consultas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataEntrada",
                table: "Produtos",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Produtos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ProdutosHistorico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataValidade = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataSaida = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VeterinarioId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosHistorico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutosHistorico_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutosHistorico_Veterinarios_VeterinarioId",
                        column: x => x.VeterinarioId,
                        principalTable: "Veterinarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosHistorico_ProdutoId",
                table: "ProdutosHistorico",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosHistorico_VeterinarioId",
                table: "ProdutosHistorico",
                column: "VeterinarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Animais_AnimalId",
                table: "Consultas",
                column: "AnimalId",
                principalTable: "Animais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tratamentos_Animais_AnimalId",
                table: "Tratamentos",
                column: "AnimalId",
                principalTable: "Animais",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Animais_AnimalId",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tratamentos_Animais_AnimalId",
                table: "Tratamentos");

            migrationBuilder.DropTable(
                name: "ProdutosHistorico");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Produtos");

            migrationBuilder.AddColumn<int>(
                name: "AnimalId1",
                table: "Tratamentos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataEntrada",
                table: "Produtos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataSaida",
                table: "Produtos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VeterinarioId1",
                table: "Consultas",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tratamentos_AnimalId1",
                table: "Tratamentos",
                column: "AnimalId1");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_VeterinarioId1",
                table: "Consultas",
                column: "VeterinarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Animais_AnimalId",
                table: "Consultas",
                column: "AnimalId",
                principalTable: "Animais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Veterinarios_VeterinarioId1",
                table: "Consultas",
                column: "VeterinarioId1",
                principalTable: "Veterinarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tratamentos_Animais_AnimalId",
                table: "Tratamentos",
                column: "AnimalId",
                principalTable: "Animais",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tratamentos_Animais_AnimalId1",
                table: "Tratamentos",
                column: "AnimalId1",
                principalTable: "Animais",
                principalColumn: "Id");
        }
    }
}

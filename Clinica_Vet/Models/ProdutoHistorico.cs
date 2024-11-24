using Clinica_Vet.Models;
using System;

public class ProdutoHistorico
{
    public int Id { get; set; } // Chave primária
    public int ProdutoId { get; set; }
    public string Nome { get; set; }
    public DateTime DataEntrada { get; set; }
    public DateTime? DataValidade { get; set; }
    public DateTime DataSaida { get; set; }
    public int VeterinarioId { get; set; }

    // Propriedades de navegação
    public Produto Produto { get; set; }
    public Veterinario Veterinario { get; set; }
}

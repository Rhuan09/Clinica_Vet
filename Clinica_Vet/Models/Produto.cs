using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica_Vet.Models
{
    public class Produto
    {
        public int Id { get; set; } // Chave primária
        public string Nome { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataValidade { get; set; }
        // Remova DataSaida, pois agora usaremos uma tabela separada para o histórico
        public bool Ativo { get; set; } = true; // Indica se o produto está no estoque
    }


}

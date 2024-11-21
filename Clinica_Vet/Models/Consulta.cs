using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica_Vet.Models
{
    public class Consulta
    {
        public int Id { get; set; } // Chave primária
        public DateTime Data { get; set; }
        public string DataFormatada => Data.ToString("HH:mm");

        public string Relatorio { get; set; }

        public int VeterinarioId { get; set; } // Chave estrangeira para Veterinario
        public Veterinario Veterinario { get; set; }

        public int TratamentoId { get; set; } // Chave estrangeira para Tratamento
        public Tratamento Tratamento { get; set; }

        public List<Exame> Exames { get; set; } = new(); // Relação 1:N com Exame

        // Relação com Cliente
        public int ClienteId { get; set; } // Chave estrangeira
        public Cliente Cliente { get; set; }
    }

}

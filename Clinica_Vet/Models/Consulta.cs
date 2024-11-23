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
        public int AnimalId { get; set; } // Chave estrangeira para Animal
        public string Relatorio { get; set; }

        public string Descricao { get; set; } // Detalhes ou descrição da consulta

        public int VeterinarioId { get; set; } // Chave estrangeira para Veterinario
        public Veterinario Veterinario { get; set; }

        public int TratamentoId { get; set; } // Chave estrangeira para Tratamento
        public Tratamento Tratamento { get; set; }

        public ICollection<Exame> Exames { get; set; } = new List<Exame>(); // Relação 1:N com Exames
        // Relação com Cliente
        public int ClienteId { get; set; } // Chave estrangeira
        public Cliente Cliente { get; set; }


    }

}

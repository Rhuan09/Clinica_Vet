using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica_Vet.Models
{
    public class Tratamento
    {
        public int Id { get; set; } // Chave primária
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public int? AnimalId { get; set; } // Chave estrangeira para Animal
        public Animal Animal { get; set; }

        public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>(); // Relação 1:N com Consulta

        public string Descricao { get; set; } // Detalhes do tratamento
    }

}

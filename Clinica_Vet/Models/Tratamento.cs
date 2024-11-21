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

        public int AnimalId { get; set; } // Chave estrangeira para Animal
        public Animal Animal { get; set; }

        public List<Consulta> Consultas { get; set; } = new(); // Relação 1:N com Consulta
    }

}

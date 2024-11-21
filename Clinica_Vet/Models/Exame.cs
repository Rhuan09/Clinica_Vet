using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica_Vet.Models
{
    public class Exame
    {
        public int Id { get; set; } // Chave primária
        public string Nome { get; set; }
        public string Resultado { get; set; }

        public int ConsultaId { get; set; } // Chave estrangeira para Consulta
        public Consulta Consulta { get; set; }
    }

}

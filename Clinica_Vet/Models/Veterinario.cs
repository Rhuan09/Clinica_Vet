using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica_Vet.Models
{
    public class Veterinario : Usuario
    {
        public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica_Vet.Models
{
    public class Veterinario : Usuario
    {
        public List<Consulta> Consultas { get; set; } = new(); // Relação 1:N com Consulta
    }

}

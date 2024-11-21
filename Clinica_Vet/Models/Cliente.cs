using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica_Vet.Models
{
    public class Cliente : Usuario
    {
        public List<Animal> Animais { get; set; } = new(); // Relação 1:N com Animal
    }

}

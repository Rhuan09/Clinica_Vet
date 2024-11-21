using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica_Vet.Models
{
    public class Especie
    {
        public int Id { get; set; } // Chave primária
        public string Nome { get; set; }

        public List<Animal> Animais { get; set; } = new(); // Relação 0:N com Animal
    }

}

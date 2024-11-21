using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica_Vet.Models
{
    public class Animal
    {
        public int Id { get; set; } // Chave primária
        public string Nome { get; set; }
        public int Idade { get; set; }
        public int Sexo { get; set; }
        public double Peso { get; set; }

        public int ClienteId { get; set; } // Chave estrangeira para Cliente
        public Cliente Cliente { get; set; }

        public int? EspecieId { get; set; } // Chave estrangeira para Especie (opcional)
        public Especie Especie { get; set; }

        public List<Tratamento> Tratamentos { get; set; } = new(); // Relação 1:N com Tratamento
    }

}

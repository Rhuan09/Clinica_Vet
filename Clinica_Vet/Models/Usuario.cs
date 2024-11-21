using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica_Vet.Models
{
    public abstract class Usuario
    {
        public int Id { get; set; } // Chave primária
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }

}

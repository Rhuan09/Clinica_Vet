using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Clinica_Vet.Models
{
    public class Consulta : INotifyPropertyChanged
    {
        private int id;
        private DateTime data;
        private string descricao;
        private string relatorio;
        private int clienteId;
        private Cliente cliente;
        private int animalId;
        private Animal animal;
        private int veterinarioId;
        private Veterinario veterinario;
        private int? tratamentoId;
        private Tratamento? tratamento;
        private ICollection<Exame>? exames;

        // Propriedade: Id
        public int Id
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        // Propriedade: Data
        public DateTime Data
        {
            get => data;
            set
            {
                if (data != value)
                {
                    data = value;
                    OnPropertyChanged(nameof(Data));
                }
            }
        }

        // Propriedade: Descricao
        public string Descricao
        {
            get => descricao;
            set
            {
                if (descricao != value)
                {
                    descricao = value;
                    OnPropertyChanged(nameof(Descricao));
                }
            }
        }

        // Propriedade: Relatorio
        public string Relatorio
        {
            get => relatorio;
            set
            {
                if (relatorio != value)
                {
                    relatorio = value;
                    OnPropertyChanged(nameof(Relatorio));
                }
            }
        }

        // Propriedade: ClienteId
        public int ClienteId
        {
            get => clienteId;
            set
            {
                if (clienteId != value)
                {
                    clienteId = value;
                    OnPropertyChanged(nameof(ClienteId));
                }
            }
        }

        // Propriedade de Navegação: Cliente
        public Cliente Cliente
        {
            get => cliente;
            set
            {
                if (cliente != value)
                {
                    cliente = value;
                    OnPropertyChanged(nameof(Cliente));
                }
            }
        }

        // Propriedade: AnimalId
        public int AnimalId
        {
            get => animalId;
            set
            {
                if (animalId != value)
                {
                    animalId = value;
                    OnPropertyChanged(nameof(AnimalId));
                }
            }
        }

        // Propriedade de Navegação: Animal
        public Animal Animal
        {
            get => animal;
            set
            {
                if (animal != value)
                {
                    animal = value;
                    OnPropertyChanged(nameof(Animal));
                }
            }
        }

        // Propriedade: VeterinarioId
        public int VeterinarioId
        {
            get => veterinarioId;
            set
            {
                if (veterinarioId != value)
                {
                    veterinarioId = value;
                    OnPropertyChanged(nameof(VeterinarioId));
                }
            }
        }

        // Propriedade de Navegação: Veterinario
        public Veterinario Veterinario
        {
            get => veterinario;
            set
            {
                if (veterinario != value)
                {
                    veterinario = value;
                    OnPropertyChanged(nameof(Veterinario));
                }
            }
        }

        // Propriedade: TratamentoId
        public int? TratamentoId
        {
            get => tratamentoId;
            set
            {
                if (tratamentoId != value)
                {
                    tratamentoId = value;
                    OnPropertyChanged(nameof(TratamentoId));
                }
            }
        }

        // Propriedade de Navegação: Tratamento
        public Tratamento? Tratamento
        {
            get => tratamento;
            set
            {
                if (tratamento != value)
                {
                    tratamento = value;
                    OnPropertyChanged(nameof(Tratamento));
                }
            }
        }

        // Propriedade de Navegação: Exames
        public ICollection<Exame>? Exames
        {
            get => exames;
            set
            {
                if (exames != value)
                {
                    exames = value;
                    OnPropertyChanged(nameof(Exames));
                }
            }
        }

        // Construtor
        public Consulta()
        {
            Exames = new List<Exame>();
        }

        // Implementação de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

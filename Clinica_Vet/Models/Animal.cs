using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Clinica_Vet.Models
{
    public class Animal : INotifyPropertyChanged
    {
        private string _nome;
        private int _idade;
        private int _sexo;
        private double _peso;
        private int? _especieId;
        private Especie _especie;
        private ObservableCollection<Tratamento> _tratamentos;

        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public string Nome
        {
            get => _nome;
            set
            {
                if (_nome != value)
                {
                    _nome = value;
                    OnPropertyChanged(nameof(Nome));
                }
            }
        }

        public int Idade
        {
            get => _idade;
            set
            {
                if (_idade != value)
                {
                    _idade = value;
                    OnPropertyChanged(nameof(Idade));
                }
            }
        }

        public int Sexo
        {
            get => _sexo;
            set
            {
                if (_sexo != value)
                {
                    _sexo = value;
                    OnPropertyChanged(nameof(Sexo));
                }
            }
        }

        public double Peso
        {
            get => _peso;
            set
            {
                if (_peso != value)
                {
                    _peso = value;
                    OnPropertyChanged(nameof(Peso));
                }
            }
        }

        public int? EspecieId
        {
            get => _especieId;
            set
            {
                if (_especieId != value)
                {
                    _especieId = value;
                    OnPropertyChanged(nameof(EspecieId));
                }
            }
        }

        public Especie Especie
        {
            get => _especie;
            set
            {
                if (_especie != value)
                {
                    _especie = value;
                    OnPropertyChanged(nameof(Especie));
                }
            }
        }

        public ObservableCollection<Tratamento> Tratamentos
        {
            get => _tratamentos ??= new ObservableCollection<Tratamento>();
            set
            {
                if (_tratamentos != value)
                {
                    _tratamentos = value;
                    OnPropertyChanged(nameof(Tratamentos));
                }
            }
        }

        public ICollection<Consulta>? Consultas { get; set; } = new List<Consulta>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

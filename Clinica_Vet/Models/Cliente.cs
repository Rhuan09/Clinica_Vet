using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Clinica_Vet.Models
{
    public class Cliente : INotifyPropertyChanged
    {
        private string _nome;
        private string _telefone;
        private string _email;
        private string _endereco;
        private string _cep;
        private ObservableCollection<Animal> _animais;

        public int Id { get; set; }

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

        public string Telefone
        {
            get => _telefone;
            set
            {
                if (_telefone != value)
                {
                    _telefone = value;
                    OnPropertyChanged(nameof(Telefone));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string Endereco
        {
            get => _endereco;
            set
            {
                if (_endereco != value)
                {
                    _endereco = value;
                    OnPropertyChanged(nameof(Endereco));
                }
            }
        }

        public string Cep
        {
            get => _cep;
            set
            {
                if (_cep != value)
                {
                    _cep = value;
                    OnPropertyChanged(nameof(Cep));
                }
            }
        }

        public ObservableCollection<Animal> Animais
        {
            get => _animais ??= new ObservableCollection<Animal>();
            set
            {
                if (_animais != value)
                {
                    if (_animais != null)
                    {
                        _animais.CollectionChanged -= OnAnimaisCollectionChanged;
                    }

                    _animais = value;
                    OnPropertyChanged(nameof(Animais));

                    if (_animais != null)
                    {
                        _animais.CollectionChanged += OnAnimaisCollectionChanged;
                    }
                }
            }
        }

        private void OnAnimaisCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Animais));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

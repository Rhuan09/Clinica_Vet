using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Clinica_Vet.ViewModels
{
    public partial class AreaPetViewModel : ObservableObject
    {
        private readonly IDataAccess<Cliente> _clienteDao;
        private readonly IDataAccess<Animal> _animalDao;

        public AreaPetViewModel(IDataAccess<Cliente> clienteDao, IDataAccess<Animal> animalDao)
        {
            _clienteDao = clienteDao;
            _animalDao = animalDao;

            Clientes = new ObservableCollection<Cliente>();
            AnimaisDoCliente = new ObservableCollection<Animal>();
        }

        [ObservableProperty]
        private ObservableCollection<Cliente> clientes;

        [ObservableProperty]
        private Cliente clienteSelecionado;

        [ObservableProperty]
        private ObservableCollection<Animal> animaisDoCliente;

        [ObservableProperty]
        private Animal animalSelecionado;

        public async Task CarregarClientesAsync()
        {
            var lista = await _clienteDao.ConsultarAsync();
            Clientes.Clear();
            foreach (var cliente in lista)
            {
                Clientes.Add(cliente);
            }
        }

        public async Task CarregarAnimaisDoClienteAsync()
        {
            if (ClienteSelecionado != null)
            {
                var animais = await _animalDao.ConsultarAsync(a => a.ClienteId == ClienteSelecionado.Id);

                AnimaisDoCliente.Clear();
                foreach (var animal in animais)
                {
                    AnimaisDoCliente.Add(animal);
                }
            }
            else
            {
                AnimaisDoCliente.Clear();
            }
        }
    }
}

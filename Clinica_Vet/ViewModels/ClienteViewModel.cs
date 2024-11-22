using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using System;
using Clinica_Vet.Models;

namespace Clinica_Vet.ViewModels
{
    public partial class ClienteViewModel : ObservableObject
    {
        private readonly IDataAccess<Cliente> _clienteDao;
        private readonly IDataAccess<Animal> _animalDao;
        private readonly IDataAccess<Especie> _especieDao; // Para gerenciar espécies

        [ObservableProperty]
        private ObservableCollection<Cliente> clientes;

        [ObservableProperty]
        private Cliente clienteSelecionado;

        [ObservableProperty]
        private Animal animalSelecionado;

        [ObservableProperty]
        private ObservableCollection<Especie> especiesDisponiveis; // Lista de espécies disponíveis

        // Propriedades para capturar dados do cliente
        [ObservableProperty]
        private string nome;

        [ObservableProperty]
        private string telefone;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string endereco;

        [ObservableProperty]
        private string cep;

        public ClienteViewModel(IDataAccess<Cliente> clienteDao, IDataAccess<Animal> animalDao, IDataAccess<Especie> especieDao)
        {
            _clienteDao = clienteDao;
            _animalDao = animalDao;
            _especieDao = especieDao;

            _ = CarregarClientesAsync();
            _ = CarregarEspeciesAsync(); // Carregar espécies disponíveis
        }

        private async Task CarregarClientesAsync()
{
    var todosClientes = await _clienteDao.ConsultarAsync();

    foreach (var cliente in todosClientes)
    {
        // Carrega os animais relacionados para cada cliente
        cliente.Animais = new ObservableCollection<Animal>(
            await _animalDao.ConsultarAsync(a => a.ClienteId == cliente.Id));
    }

    Clientes = new ObservableCollection<Cliente>(todosClientes);
}


        private async Task CarregarEspeciesAsync()
        {
            var especies = await _especieDao.ConsultarAsync();
            EspeciesDisponiveis = new ObservableCollection<Especie>(especies);
        }

        [RelayCommand]
        public async Task AdicionarClienteAsync()
        {
            var novoCliente = new Cliente
            {
                Nome = Nome,
                Telefone = Telefone,
                Email = Email,
                Endereco = Endereco,
                Cep = Cep
            };

            // Salva no banco de dados
            await _clienteDao.RegistrarAsync(novoCliente);

            // Atualiza a lista de clientes
            Clientes.Add(novoCliente);

            // Limpa os campos do formulário
            Nome = Telefone = Email = Endereco = Cep = string.Empty;
        }

        [RelayCommand]
        public async Task EditarClienteAsync()
        {
            if (ClienteSelecionado == null) return;

            // Atualiza o cliente no banco de dados
            await _clienteDao.AtualizarAsync(ClienteSelecionado);

            // Atualiza os animais associados
            foreach (var animal in ClienteSelecionado.Animais)
            {
                await _animalDao.AtualizarAsync(animal);
            }
        }

        [RelayCommand]
        public async Task AdicionarAnimalAsync(Animal novoAnimal)
        {
            if (ClienteSelecionado == null)
                return;

            // Salva o animal no banco de dados
            novoAnimal.ClienteId = ClienteSelecionado.Id;
            await _animalDao.RegistrarAsync(novoAnimal);

            // Atualiza a lista de animais no cliente selecionado
            ClienteSelecionado.Animais ??= new ObservableCollection<Animal>();
            ClienteSelecionado.Animais.Add(novoAnimal);

            // Atualiza a interface
            OnPropertyChanged(nameof(ClienteSelecionado.Animais));
        }



        [RelayCommand]
        public async Task RemoverAnimalAsync()
        {
            if (AnimalSelecionado == null || ClienteSelecionado == null) return;

            try
            {
                await _animalDao.RemoverAsync(AnimalSelecionado); // Remove from database
                ClienteSelecionado.Animais.Remove(AnimalSelecionado); // Remove from collection
                OnPropertyChanged(nameof(ClienteSelecionado.Animais)); // Notify UI
            }
            catch (Exception ex)
            {
                // Handle error, log if needed
                Debug.WriteLine($"Error removing animal: {ex.Message}");
            }
        }



        [RelayCommand]
        public async Task RemoverClienteAsync()
        {
            if (ClienteSelecionado == null)
            {
                // Adicione um log ou mensagem de erro, se necessário
                return;
            }

            await _clienteDao.RemoverAsync(ClienteSelecionado);
            Clientes.Remove(ClienteSelecionado);

            // Atualiza a interface
            OnPropertyChanged(nameof(Clientes));
        }

    }
}

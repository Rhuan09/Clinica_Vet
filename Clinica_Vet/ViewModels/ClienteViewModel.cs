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
        private ObservableCollection<Cliente> clientesFiltrados;


        [ObservableProperty]
        private Cliente clienteSelecionado;

        [ObservableProperty]
        private Animal animalSelecionado;

        [ObservableProperty]
        private string searchQuery;


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

        [ObservableProperty]
        private bool isSalvarHabilitado;

        partial void OnNomeChanged(string value) => VerificarCamposObrigatorios();
        partial void OnTelefoneChanged(string value) => VerificarCamposObrigatorios();
        partial void OnEmailChanged(string value) => VerificarCamposObrigatorios();
        partial void OnEnderecoChanged(string value) => VerificarCamposObrigatorios();
        partial void OnCepChanged(string value) => VerificarCamposObrigatorios();


        public ClienteViewModel(IDataAccess<Cliente> clienteDao, IDataAccess<Animal> animalDao, IDataAccess<Especie> especieDao)
        {
            _clienteDao = clienteDao;
            _animalDao = animalDao;
            _especieDao = especieDao;

            _ = CarregarClientesAsync();
            _ = CarregarEspeciesAsync(); // Carregar espécies disponíveis
        }

        private void VerificarCamposObrigatorios()
        {
            // Valida se todos os campos obrigatórios estão preenchidos
            IsSalvarHabilitado = !string.IsNullOrWhiteSpace(Nome) &&
                                 !string.IsNullOrWhiteSpace(Telefone) &&
                                 !string.IsNullOrWhiteSpace(Email) &&
                                 !string.IsNullOrWhiteSpace(Endereco) &&
                                 !string.IsNullOrWhiteSpace(Cep);
        }

        private void FiltrarClientes()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                // Mostra todos os clientes se não houver texto na busca
                ClientesFiltrados = new ObservableCollection<Cliente>(Clientes);
            }
            else
            {
                // Filtra os clientes com base no nome
                var clientesFiltrados = Clientes
                    .Where(c => c.Nome.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                ClientesFiltrados = new ObservableCollection<Cliente>(clientesFiltrados);
            }
        }

        partial void OnSearchQueryChanged(string value)
        {
            FiltrarClientes();
        }

        private async Task CarregarClientesAsync()
        {
            var todosClientes = await _clienteDao.ConsultarAsync();

            foreach (var cliente in todosClientes)
            {
                // Carrega os animais relacionados somente do banco de dados
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
            if (ClienteSelecionado == null || novoAnimal == null) return;

            try
            {
                // Salva o animal no banco de dados
                novoAnimal.ClienteId = ClienteSelecionado.Id;
                await _animalDao.RegistrarAsync(novoAnimal);

                // Opcional: Atualize a lista local apenas do banco
                ClienteSelecionado.Animais = new ObservableCollection<Animal>(
                    await _animalDao.ConsultarAsync(a => a.ClienteId == ClienteSelecionado.Id));

                // Notifica a interface para atualização
                OnPropertyChanged(nameof(ClienteSelecionado.Animais));
            }
            catch (Exception ex)
            {
                // Log de erro
                System.Diagnostics.Debug.WriteLine($"Erro ao adicionar animal: {ex.Message}");
            }
        }




        [RelayCommand]
        public async Task RemoverAnimalAsync()
        {
            if (AnimalSelecionado == null || ClienteSelecionado == null) return;

            try
            {
                // Store the animal to be removed
                var animalToRemove = AnimalSelecionado;

                // Remove from the database
                await _animalDao.RemoverAsync(animalToRemove);

                // Remove from the local collection
                ClienteSelecionado.Animais.Remove(animalToRemove);

                // Clear the selected animal to prevent re-execution
                AnimalSelecionado = null;

                // Notify the UI
                OnPropertyChanged(nameof(ClienteSelecionado.Animais));
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine($"Erro ao remover animal: {ex.Message}");
            }
        }





        [RelayCommand]
        public async Task RemoverClienteAsync()
        {
            if (ClienteSelecionado == null)
                return;

            // Remove todos os animais associados ao cliente
            if (ClienteSelecionado.Animais != null)
            {
                foreach (var animal in ClienteSelecionado.Animais.ToList())
                {
                    await _animalDao.RemoverAsync(animal);
                }
            }

            // Remove o cliente
            await _clienteDao.RemoverAsync(ClienteSelecionado);
            Clientes.Remove(ClienteSelecionado);

            // Limpa a seleção
            ClienteSelecionado = null;

            // Atualiza a interface
            OnPropertyChanged(nameof(Clientes));
        }





    }
}

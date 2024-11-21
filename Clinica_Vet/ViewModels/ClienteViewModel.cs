using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace Clinica_Vet.ViewModels
{
    public partial class ClienteViewModel : ObservableObject
    {
        private readonly IDataAccess<Cliente> _clienteDao;

        [ObservableProperty]
        private ObservableCollection<Cliente> clientes;

        [ObservableProperty]
        private ObservableCollection<Cliente> clientesFiltrados;

        [ObservableProperty]
        private Cliente clienteSelecionado;

        [ObservableProperty]
        private string searchTerm;

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

        public ClienteViewModel(IDataAccess<Cliente> clienteDao)
        {
            _clienteDao = clienteDao;
            Clientes = new ObservableCollection<Cliente>();
            ClientesFiltrados = new ObservableCollection<Cliente>();
            _ = CarregarClientesAsync();
        }

        private async Task CarregarClientesAsync()
        {
            var todosClientes = await _clienteDao.ConsultarAsync();
            Clientes = new ObservableCollection<Cliente>(todosClientes);
            FiltrarClientes();
        }

        partial void OnSearchTermChanged(string value)
        {
            FiltrarClientes();
        }

        private void FiltrarClientes()
        {
            if (string.IsNullOrEmpty(SearchTerm))
            {
                ClientesFiltrados = new ObservableCollection<Cliente>(Clientes);
            }
            else
            {
                var filtro = Clientes.Where(c =>
                    c.Nome.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.Telefone.Contains(SearchTerm) ||
                    c.Email.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
                ClientesFiltrados = new ObservableCollection<Cliente>(filtro);
            }
        }

        [RelayCommand]
        public void AdicionarCliente()
        {
            var novoCliente = new Cliente
            {
                Nome = Nome,
                Telefone = Telefone,
                Email = Email,
                Endereco = Endereco,
                Cep = Cep
            };

            _clienteDao.RegistrarAsync(novoCliente);
            Clientes.Add(novoCliente);
            FiltrarClientes();
        }

        [RelayCommand]
        public void EditarCliente()
        {
            if (ClienteSelecionado == null) return;

            ClienteSelecionado.Nome = Nome;
            ClienteSelecionado.Telefone = Telefone;
            ClienteSelecionado.Email = Email;
            ClienteSelecionado.Endereco = Endereco;
            ClienteSelecionado.Cep = Cep;

            _clienteDao.AtualizarAsync(ClienteSelecionado);
            FiltrarClientes();
        }

        [RelayCommand]
        public void RemoverCliente()
        {
            if (ClienteSelecionado == null) return;

            _clienteDao.RemoverAsync(ClienteSelecionado);
            Clientes.Remove(ClienteSelecionado);
            FiltrarClientes();
        }

        [RelayCommand]
        private void AdicionarAnimal()
        {
            if (ClienteSelecionado == null) return;

            // Lógica para adicionar animal associado ao cliente selecionado
        }
    }
}

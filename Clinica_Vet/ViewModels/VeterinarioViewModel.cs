using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace Clinica_Vet.ViewModels
{
    public partial class VeterinarioViewModel : ObservableObject
    {
        private readonly IDataAccess<Veterinario> _dao;

        [ObservableProperty]
        private ObservableCollection<Veterinario> veterinarios;

        [ObservableProperty]
        private ObservableCollection<Veterinario> veterinariosFiltrados;

        [ObservableProperty]
        private string termoPesquisa;


        [ObservableProperty]
        private Veterinario veterinarioSelecionado;

        // Propriedades para o formulário
        [ObservableProperty]
        private string nome;

        [ObservableProperty]
        private string telefone;

        [ObservableProperty]
        private string endereco;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string cep;

        public VeterinarioViewModel(IDataAccess<Veterinario> dao)
        {
            _dao = dao;
            Veterinarios = new ObservableCollection<Veterinario>();
            VeterinariosFiltrados = new ObservableCollection<Veterinario>();
            _ = CarregarVeterinariosAsync();
        }



        [RelayCommand]
        public async Task EditarAsync()
        {
            if (VeterinarioSelecionado == null)
                return;

            // Atualiza o veterinário selecionado no banco de dados
            await _dao.AtualizarAsync(VeterinarioSelecionado);

            // Não é necessário atualizar manualmente a lista local,
            // pois a edição é feita diretamente no objeto vinculado.
        }

        [RelayCommand]
        public async Task AdicionarAsync()
        {
            if (string.IsNullOrWhiteSpace(Nome) || string.IsNullOrWhiteSpace(Telefone) ||
                string.IsNullOrWhiteSpace(Endereco) || string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Cep))
            {
                return; // Validação básica para evitar adicionar dados incompletos
            }

            var novoVeterinario = new Veterinario
            {
                Nome = Nome,
                Telefone = Telefone,
                Endereco = Endereco,
                Email = Email,
                Cep = Cep
            };

            await _dao.RegistrarAsync(novoVeterinario);

            // Atualiza a lista local
            Veterinarios.Add(novoVeterinario);

            // Limpa os campos após adicionar
            Nome = Telefone = Email = Endereco = Cep = string.Empty;
        }

        public async Task CarregarVeterinariosAsync()
        {
            var lista = await _dao.ConsultarAsync();
            Veterinarios.Clear();
            foreach (var veterinario in lista)
            {
                Veterinarios.Add(veterinario);
            }

            AplicarFiltro(); // Inicializa os filtrados
        }



        public void AplicarFiltro()
        {
            if (string.IsNullOrWhiteSpace(TermoPesquisa))
            {
                VeterinariosFiltrados = new ObservableCollection<Veterinario>(Veterinarios);
            }
            else
            {
                var filtro = TermoPesquisa.ToLower();
                var filtrados = Veterinarios.Where(v =>
                    (!string.IsNullOrEmpty(v.Nome) && v.Nome.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(v.Email) && v.Email.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(v.Telefone) && v.Telefone.ToLower().Contains(filtro))
                );

                VeterinariosFiltrados = new ObservableCollection<Veterinario>(filtrados);
            }
        }

        [RelayCommand]
        public async Task RemoverAsync()
        {
            if (VeterinarioSelecionado == null) return;

            await _dao.RemoverAsync(VeterinarioSelecionado);

            // Remove localmente
            Veterinarios.Remove(VeterinarioSelecionado);
        }
    }
}
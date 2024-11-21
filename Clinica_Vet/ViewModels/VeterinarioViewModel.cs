using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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
        private Veterinario veterinarioSelecionado;

        [ObservableProperty]
        private string nome;
        [ObservableProperty]
        private string endereco;
        [ObservableProperty]
        private string cep;
        [ObservableProperty]
        private string telefone;
        [ObservableProperty]
        private string email;

        public VeterinarioViewModel(IDataAccess<Veterinario> dao)
        {
            _dao = dao;
            Veterinarios = new ObservableCollection<Veterinario>();
            CarregarVeterinariosAsync();
        }

        public VeterinarioViewModel()
        {
            Veterinarios = new ObservableCollection<Veterinario>();
        }

        [RelayCommand]
        public async Task AdicionarAsync()
        {
            var novoVeterinario = new Veterinario { Nome = Nome, Endereco = Endereco, Cep = Cep, Telefone = Telefone, Email = Email };
            await _dao.RegistrarAsync(novoVeterinario);
            await CarregarVeterinariosAsync();
        }

        [RelayCommand]
        public async Task EditarAsync()
        {
            if (VeterinarioSelecionado == null) return;

            VeterinarioSelecionado.Nome = Nome;
            VeterinarioSelecionado.Endereco = Endereco;
            VeterinarioSelecionado.Cep = Cep;
            VeterinarioSelecionado.Telefone = Telefone;
            VeterinarioSelecionado.Email = Email;

            await _dao.AtualizarAsync(VeterinarioSelecionado);
            await CarregarVeterinariosAsync();
        }

        [RelayCommand]
        public async Task RemoverAsync()
        {
            if (VeterinarioSelecionado == null) return;

            await _dao.RemoverAsync(VeterinarioSelecionado);
            await CarregarVeterinariosAsync();
        }

        private async Task CarregarVeterinariosAsync()
        {
            var lista = await _dao.ConsultarAsync();
            Veterinarios = new ObservableCollection<Veterinario>(lista);
        }
    }
}
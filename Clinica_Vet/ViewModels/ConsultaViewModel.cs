using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Clinica_Vet.ViewModels
{
    public partial class ConsultaViewModel : ObservableObject
    {
        private readonly IDataAccess<Consulta> _consultaDao;
        private readonly IDataAccess<Cliente> _clienteDao;
        private readonly IDataAccess<Veterinario> _veterinarioDao;
        private readonly IDataAccess<Animal> _animalDao;

        [ObservableProperty]
        private ObservableCollection<Animal> animaisDisponiveis;

        public ObservableCollection<Consulta> Consultas { get; private set; }
        public ObservableCollection<Cliente> Clientes { get; private set; }
        public ObservableCollection<Veterinario> Veterinarios { get; private set; }
        public ObservableCollection<Animal> Animais { get; private set; }

        [ObservableProperty]
        private Consulta consultaSelecionada;

        [ObservableProperty]
        private Cliente clienteSelecionado;

        [ObservableProperty]
        private Veterinario veterinarioSelecionado;

        [ObservableProperty]
        private Animal animalSelecionado;

        [ObservableProperty]
        private TimeSpan horaSelecionada;

        public ConsultaViewModel(
            IDataAccess<Consulta> consultaDao,
            IDataAccess<Cliente> clienteDao,
            IDataAccess<Veterinario> veterinarioDao,
            IDataAccess<Animal> animalDao)
        {
            _consultaDao = consultaDao;
            _clienteDao = clienteDao;
            _veterinarioDao = veterinarioDao;
            _animalDao = animalDao;

            Consultas = new ObservableCollection<Consulta>();
            Clientes = new ObservableCollection<Cliente>();
            Veterinarios = new ObservableCollection<Veterinario>();
            Animais = new ObservableCollection<Animal>();
        }

        public async Task CarregarDadosAsync()
        {
            Consultas = new ObservableCollection<Consulta>(await _consultaDao.ConsultarAsync());
            Clientes = new ObservableCollection<Cliente>(await _clienteDao.ConsultarAsync());
            Veterinarios = new ObservableCollection<Veterinario>(await _veterinarioDao.ConsultarAsync());
        }

        public void CriarNovaConsulta()
        {
            ConsultaSelecionada = new Consulta
            {
                Data = DateTime.Now,
                Descricao = "Nova Consulta"
            };
        }

        public async Task CarregarAnimaisDoClienteAsync()
        {
            if (ClienteSelecionado == null)
            {
                AnimaisDisponiveis = new ObservableCollection<Animal>();
                return;
            }

            // Carregar animais do cliente selecionado
            var animais = await _animalDao.ConsultarAsync(a => a.ClienteId == ClienteSelecionado.Id);

            AnimaisDisponiveis = new ObservableCollection<Animal>(animais);
        }

        public async Task SalvarConsultaAsync()
        {
            if (ConsultaSelecionada.Id == 0)
            {
                await _consultaDao.RegistrarAsync(ConsultaSelecionada);
            }
            else
            {
                await _consultaDao.AtualizarAsync(ConsultaSelecionada);
            }

            await CarregarDadosAsync();
        }
    }
}

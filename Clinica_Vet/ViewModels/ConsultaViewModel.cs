using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Clinica_Vet.ViewModels
{
    public partial class ConsultaViewModel : ObservableObject
    {
        private readonly IDataAccess<Consulta> _consultaDao;
        private readonly IDataAccess<Veterinario> _veterinarioDao;
        private readonly IDataAccess<Cliente> _clienteDao;

        [ObservableProperty]
        private ObservableCollection<Consulta> consultas;

        [ObservableProperty]
        private ObservableCollection<Veterinario> veterinarios;

        [ObservableProperty]
        private ObservableCollection<Cliente> clientes;

        [ObservableProperty]
        private Consulta consultaSelecionada;

        [ObservableProperty]
        private DateTime dataHoraConsulta;

        [ObservableProperty]
        private string relatorio;

        [ObservableProperty]
        private Veterinario veterinarioSelecionado;

        [ObservableProperty]
        private Cliente clienteSelecionado;

        public ConsultaViewModel(IDataAccess<Consulta> consultaDao, IDataAccess<Veterinario> veterinarioDao, IDataAccess<Cliente> clienteDao)
        {
            _consultaDao = consultaDao;
            _veterinarioDao = veterinarioDao;
            _clienteDao = clienteDao;

            Consultas = new ObservableCollection<Consulta>();
            Veterinarios = new ObservableCollection<Veterinario>();
            Clientes = new ObservableCollection<Cliente>();

            CarregarDadosAsync();
        }

        //public ConsultaViewModel()
        //{
        //    Consultas = new ObservableCollection<Consulta>();
        //    Veterinarios = new ObservableCollection<Veterinario>();
        //    Clientes = new ObservableCollection<Cliente>();
        //}

        [RelayCommand]
        public async Task AdicionarConsultaAsync()
        {
            // Validação de conflitos
            if (ExisteConflitoDeHorario(DataHoraConsulta, VeterinarioSelecionado.Id, ClienteSelecionado.Id))
            {
                throw new InvalidOperationException("Já existe uma consulta marcada neste horário para o mesmo veterinário ou cliente.");
            }

            // Criar a nova consulta
            var novaConsulta = new Consulta
            {
                Data = DataHoraConsulta,
                Relatorio = Relatorio,
                VeterinarioId = VeterinarioSelecionado.Id,
                ClienteId = ClienteSelecionado.Id
            };

            await _consultaDao.RegistrarAsync(novaConsulta);
            await CarregarConsultasAsync();
            LimparCampos();
        }

        [RelayCommand]
        public async Task EditarConsultaAsync()
        {
            if (ConsultaSelecionada == null) return;

            // Validação de conflitos
            if (ExisteConflitoDeHorario(DataHoraConsulta, VeterinarioSelecionado.Id, ClienteSelecionado.Id, ConsultaSelecionada.Id))
            {
                throw new InvalidOperationException("Já existe uma consulta marcada neste horário para o mesmo veterinário ou cliente.");
            }

            ConsultaSelecionada.Data = DataHoraConsulta;
            ConsultaSelecionada.Relatorio = Relatorio;
            ConsultaSelecionada.VeterinarioId = VeterinarioSelecionado.Id;
            ConsultaSelecionada.ClienteId = ClienteSelecionado.Id;

            await _consultaDao.AtualizarAsync(ConsultaSelecionada);
            await CarregarConsultasAsync();
            LimparCampos();
        }

        [RelayCommand]
        public async Task RemoverConsultaAsync()
        {
            if (ConsultaSelecionada == null) return;

            await _consultaDao.RemoverAsync(ConsultaSelecionada);
            await CarregarConsultasAsync();
            LimparCampos();
        }

        private async Task CarregarDadosAsync()
        {
            await CarregarConsultasAsync();
            await CarregarVeterinariosAsync();
            await CarregarClientesAsync();
        }

        private async Task CarregarConsultasAsync()
        {
            var lista = await _consultaDao.ConsultarAsync() ?? new List<Consulta>();
            Consultas = new ObservableCollection<Consulta>(lista);
        }

        private async Task CarregarVeterinariosAsync()
        {
            var lista = await _veterinarioDao.ConsultarAsync() ?? new List<Veterinario>();
            Veterinarios = new ObservableCollection<Veterinario>(lista);
        }

        private async Task CarregarClientesAsync()
        {
            var lista = await _clienteDao.ConsultarAsync() ?? new List<Cliente>();
            Clientes = new ObservableCollection<Cliente>(lista);
        }

        private bool ExisteConflitoDeHorario(DateTime dataHora, int veterinarioId, int clienteId, int? consultaId = null)
        {
            return Consultas.Any(c =>
                c.Data == dataHora &&
                (c.VeterinarioId == veterinarioId || c.ClienteId == clienteId) &&
                c.Id != consultaId);
        }

        private void LimparCampos()
        {
            DataHoraConsulta = DateTime.Now;
            Relatorio = string.Empty;
            VeterinarioSelecionado = null;
            ClienteSelecionado = null;
            ConsultaSelecionada = null;
        }
    }
}

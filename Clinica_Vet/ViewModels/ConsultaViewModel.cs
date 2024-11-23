using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Consulta> consultas = new ObservableCollection<Consulta>();

        [ObservableProperty]
        private ObservableCollection<Cliente> clientes = new ObservableCollection<Cliente>();

        [ObservableProperty]
        private ObservableCollection<Veterinario> veterinarios = new ObservableCollection<Veterinario>();

        [ObservableProperty]
        private ObservableCollection<Animal> animaisDisponiveis = new ObservableCollection<Animal>();

        [ObservableProperty]
        private Consulta consultaSelecionada;

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
        }

        public async Task CarregarDadosAsync()
        {
            var consultasList = await _consultaDao.ConsultarAsync();
            Consultas.Clear();
            foreach (var consulta in consultasList)
            {
                Consultas.Add(consulta);
            }

            var clientesList = await _clienteDao.ConsultarAsync();
            Clientes.Clear();
            foreach (var cliente in clientesList)
            {
                Clientes.Add(cliente);
            }

            var veterinariosList = await _veterinarioDao.ConsultarAsync();
            Veterinarios.Clear();
            foreach (var veterinario in veterinariosList)
            {
                Veterinarios.Add(veterinario);
            }
        }

        public void CriarNovaConsulta()
        {
            ConsultaSelecionada = new Consulta
            {
                Data = DateTime.Now,
                Descricao = "Nova Consulta"
            };

            HoraSelecionada = TimeSpan.Zero;
        }

        public async Task CarregarAnimaisDoClienteAsync()
        {
            if (ConsultaSelecionada == null || ConsultaSelecionada.ClienteId == 0)
            {
                AnimaisDisponiveis.Clear();
                return;
            }

            int clienteId = ConsultaSelecionada.ClienteId;

            var animais = await _animalDao.ConsultarAsync(a => a.ClienteId == clienteId);

            AnimaisDisponiveis.Clear();
            foreach (var animal in animais)
            {
                AnimaisDisponiveis.Add(animal);
            }
        }

        public async Task ExcluirConsultaAsync()
        {
            if (ConsultaSelecionada != null)
            {
                await _consultaDao.RemoverAsync(ConsultaSelecionada);
                Consultas.Remove(ConsultaSelecionada);
                ConsultaSelecionada = null;
            }
        }

        public async Task SalvarConsultaAsync()
        {
            if (ConsultaSelecionada == null)
                return;

            // Combina a data e hora
            if (HoraSelecionada != null)
            {
                ConsultaSelecionada.Data = ConsultaSelecionada.Data.Date + HoraSelecionada;
            }

            // Verifica se os IDs estão setados
            if (ConsultaSelecionada.ClienteId == 0 ||
                ConsultaSelecionada.VeterinarioId == 0 ||
                ConsultaSelecionada.AnimalId == 0)
            {
                // Exiba uma mensagem de erro ou lance uma exceção
                throw new InvalidOperationException("Cliente, Veterinário e Animal são obrigatórios.");
            }

            if (string.IsNullOrEmpty(ConsultaSelecionada.Relatorio))
            {
                ConsultaSelecionada.Relatorio = "Relatório padrão";
            }

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

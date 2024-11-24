using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private ObservableCollection<Consulta> consultas = new ObservableCollection<Consulta>();

        [ObservableProperty]
        private ObservableCollection<Cliente> clientes = new ObservableCollection<Cliente>();

        [ObservableProperty]
        private ObservableCollection<Veterinario> veterinarios = new ObservableCollection<Veterinario>();

        [ObservableProperty]
        private ObservableCollection<Animal> animaisDisponiveis = new ObservableCollection<Animal>();

        [ObservableProperty]
        private ObservableCollection<TimeSpan> horariosDisponiveis = new ObservableCollection<TimeSpan>();

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

            if (Clientes.Count == 0)
            {
                var clientesList = await _clienteDao.ConsultarAsync();
                Clientes.Clear();
                foreach (var cliente in clientesList)
                {
                    Clientes.Add(cliente);
                }
            }

            if (Veterinarios.Count == 0)
            {
                var veterinariosList = await _veterinarioDao.ConsultarAsync();
                Veterinarios.Clear();
                foreach (var veterinario in veterinariosList)
                {
                    Veterinarios.Add(veterinario);
                }
            }
        }

        public void CriarNovaConsulta()
        {
            ConsultaSelecionada = new Consulta
            {
                Data = DateTime.Now.Date,
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

        partial void OnConsultaSelecionadaChanged(Consulta oldValue, Consulta newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= ConsultaSelecionada_PropertyChanged;
            }

            if (newValue != null)
            {
                newValue.PropertyChanged += ConsultaSelecionada_PropertyChanged;
            }

            AtualizarHorariosDisponiveisAsync();
        }

        private void ConsultaSelecionada_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Consulta.VeterinarioId) || e.PropertyName == nameof(Consulta.Data))
            {
                AtualizarHorariosDisponiveisAsync();
            }
        }

        public async Task AtualizarHorariosDisponiveisAsync()
        {
            HorariosDisponiveis.Clear();

            if (ConsultaSelecionada == null || ConsultaSelecionada.VeterinarioId == 0 || ConsultaSelecionada.Data == default)
            {
                return;
            }

            int veterinarioId = ConsultaSelecionada.VeterinarioId;
            DateTime dataConsulta = ConsultaSelecionada.Data.Date;

            // Gera todos os horários possíveis no dia
            var horariosPossiveis = GerarHorariosDoDia();

            // Busca as consultas do veterinário na data selecionada
            var consultasNoDia = await _consultaDao.ConsultarAsync(c =>
                c.VeterinarioId == veterinarioId &&
                c.Data.Date == dataConsulta &&
                c.Id != ConsultaSelecionada.Id); // Exclui a própria consulta se estiver editando

            // Obter os horários ocupados
            var horariosOcupados = consultasNoDia.Select(c => c.Data.TimeOfDay).ToList();

            // Filtrar os horários disponíveis
            var horariosDisponiveis = horariosPossiveis.Except(horariosOcupados).ToList();

            // Atualizar a coleção observável
            foreach (var horario in horariosDisponiveis)
            {
                HorariosDisponiveis.Add(horario);
            }
        }

        private List<TimeSpan> GerarHorariosDoDia()
        {
            var horarios = new List<TimeSpan>();
            for (int hora = 8; hora <= 17; hora++)
            {
                horarios.Add(new TimeSpan(hora, 0, 0));
                horarios.Add(new TimeSpan(hora, 30, 0));
            }
            return horarios;
        }

        public async Task SalvarConsultaAsync()
        {
            if (ConsultaSelecionada == null)
                return;

            // Validar se a data não é no passado
            if (ConsultaSelecionada.Data.Date < DateTime.Now.Date)
            {
                throw new InvalidOperationException("A data da consulta não pode ser no passado.");
            }

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
                throw new InvalidOperationException("Cliente, Veterinário e Animal são obrigatórios.");
            }

            if (string.IsNullOrEmpty(ConsultaSelecionada.Relatorio))
            {
                ConsultaSelecionada.Relatorio = "Relatório padrão";
            }

            // Verificar disponibilidade do veterinário
            bool isDisponivel = await VerificarDisponibilidadeVeterinarioAsync(ConsultaSelecionada);
            if (!isDisponivel)
            {
                throw new InvalidOperationException("O horário selecionado não está mais disponível.");
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

        private async Task<bool> VerificarDisponibilidadeVeterinarioAsync(Consulta consulta)
        {
            DateTime inicio = consulta.Data;
            DateTime fim = consulta.Data.AddMinutes(30);

            var consultasConflitantes = await _consultaDao.ConsultarAsync(c =>
                c.VeterinarioId == consulta.VeterinarioId &&
                c.Id != consulta.Id &&
                c.Data >= inicio && c.Data < fim
            );

            return !consultasConflitantes.Any();
        }

        public async Task ExcluirConsultaAsync()
        {
            if (ConsultaSelecionada == null)
                return;

            await _consultaDao.RemoverAsync(ConsultaSelecionada);
            Consultas.Remove(ConsultaSelecionada);
            ConsultaSelecionada = null;
        }
    }
}

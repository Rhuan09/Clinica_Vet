using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Clinica_Vet.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IDataAccess<Consulta> _consultaDao;

        [ObservableProperty]
        private DateTimeOffset dataSelecionada;

        [ObservableProperty]
        private ObservableCollection<Consulta> consultasDoDia;

        public HomeViewModel(IDataAccess<Consulta> consultaDao)
        {
            _consultaDao = consultaDao;
            DataSelecionada = DateTimeOffset.Now; // Inicializa com a data atual
            ConsultasDoDia = new ObservableCollection<Consulta>();
            _ = CarregarConsultasAsync();
        }


        private async Task CarregarConsultasAsync()
        {
            // Carrega todas as consultas do banco de dados
            var todasConsultas = await _consultaDao.ConsultarAsync();

            // Atualiza as consultas do dia com base na data selecionada
            AtualizarConsultasDoDia(todasConsultas);
        }

        private void AtualizarConsultasDoDia(IEnumerable<Consulta> todasConsultas)
        {
            // Filtra consultas com base na data selecionada
            var consultasFiltradas = todasConsultas.Where(c =>
                c.Data.Date == DataSelecionada.Date);

            ConsultasDoDia = new ObservableCollection<Consulta>(consultasFiltradas);
        }

        partial void OnDataSelecionadaChanged(DateTimeOffset value)
        {
            // Quando a data muda, recarrega as consultas
            _ = CarregarConsultasAsync();
        }
    }
}

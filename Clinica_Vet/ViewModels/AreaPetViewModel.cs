using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Clinica_Vet.ViewModels
{
    public partial class AreaPetViewModel : ObservableObject
    {
        private readonly IDataAccess<Cliente> _clienteDao;
        private readonly IDataAccess<Animal> _animalDao;

        public AreaPetViewModel(IDataAccess<Cliente> clienteDao, IDataAccess<Animal> animalDao)
        {
            _clienteDao = clienteDao;
            _animalDao = animalDao;

            Clientes = new ObservableCollection<Cliente>();
            AnimaisDoCliente = new ObservableCollection<Animal>();
        }

        [ObservableProperty]
        private ObservableCollection<Cliente> clientes;

        [ObservableProperty]
        private Cliente clienteSelecionado;

        [ObservableProperty]
        private ObservableCollection<Animal> animaisDoCliente;

        [ObservableProperty]
        private Animal animalSelecionado;

        public async Task CarregarClientesAsync()
        {
            var lista = await _clienteDao.ConsultarAsync();
            Clientes.Clear();
            foreach (var cliente in lista)
            {
                Clientes.Add(cliente);
            }
        }

        public async Task CarregarAnimaisDoClienteAsync()
        {
            if (ClienteSelecionado != null)
            {
                var animais = await _animalDao.ConsultarAsync(a => a.ClienteId == ClienteSelecionado.Id);

                AnimaisDoCliente.Clear();
                foreach (var animal in animais)
                {
                    // Carregar tratamentos do animal
                    var tratamentosDao = Ioc.Default.GetRequiredService<IDataAccess<Tratamento>>();
                    var tratamentos = await tratamentosDao.ConsultarAsync(t => t.AnimalId == animal.Id);
                    animal.Tratamentos = new ObservableCollection<Tratamento>(tratamentos);

                    // Carregar as consultas do banco de dados e incluir os exames relacionados
                    var consultasDao = Ioc.Default.GetRequiredService<IDataAccess<Consulta>>();
                    var consultas = await consultasDao.ConsultarAsync(c => c.AnimalId == animal.Id);

                    // Garantir que cada consulta tenha os exames carregados
                    foreach (var consulta in consultas)
                    {
                        var examesDao = Ioc.Default.GetRequiredService<IDataAccess<Exame>>();
                        consulta.Exames = await examesDao.ConsultarAsync(e => e.ConsultaId == consulta.Id);
                    }

                    // Filtrar os exames pendentes
                    var examesPendentes = consultas
                        .SelectMany(c => c.Exames)
                        .Where(e => string.IsNullOrEmpty(e.Resultado)) // Exames sem resultado
                        .DistinctBy(e => e.Nome) // Remover duplicatas por nome
                        .ToList();

                    // Atualizar indicadores do animal
                    animal.AtualizarIndicadores();

                    // Adicionar o animal à lista observável
                    AnimaisDoCliente.Add(animal);
                }
            }
            else
            {
                AnimaisDoCliente.Clear();
            }
        }



    }
}

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
    public partial class ConfiguracoesViewModel : ObservableObject
    {
        private readonly IDataAccess<Especie> _especieDao;
        private readonly IDataAccess<Animal> _animalDao;

        [ObservableProperty]
        private ObservableCollection<Especie> especies;

        [ObservableProperty]
        private ObservableCollection<Especie> especiesFiltradas = new ObservableCollection<Especie>();

        [ObservableProperty]
        private Especie especieSelecionada;

        [ObservableProperty]
        private string termoPesquisa;

        [ObservableProperty]
        private ObservableCollection<Animal> animais = new();

        public ConfiguracoesViewModel(IDataAccess<Especie> especieDao, IDataAccess<Animal> animalDao)
        {
            _especieDao = especieDao;
            _animalDao = animalDao;
            Especies = new ObservableCollection<Especie>();
        }

        public async Task CarregarEspeciesAsync()
        {
            var lista = await _especieDao.ConsultarAsync();
            Especies.Clear();
            foreach (var especie in lista)
            {
                Especies.Add(especie);
            }

            // Carregar animais associados
            var animaisLista = await _animalDao.ConsultarAsync();
            Animais = new ObservableCollection<Animal>(animaisLista);

            AplicarFiltro();
        }


        partial void OnTermoPesquisaChanged(string value)
        {
            AplicarFiltro();
        }


        public void AplicarFiltro()
        {
            if (string.IsNullOrWhiteSpace(TermoPesquisa))
            {
                EspeciesFiltradas = new ObservableCollection<Especie>(Especies);
            }
            else
            {
                var filtro = TermoPesquisa.ToLower();
                EspeciesFiltradas = new ObservableCollection<Especie>(
                    Especies.Where(e => e.Nome != null && e.Nome.ToLower().Contains(filtro))
                );
            }
        }


        public void CriarNovaEspecie(Especie especie = null)
        {
            if (especie == null)
            {
                especie = new Especie { Id = 0, Nome = string.Empty };
            }

            EspecieSelecionada = especie;
            if (especie.Id == 0)
            {
                Especies.Add(especie);
            }
        }


        public async Task SalvarEspecieAsync()
        {
            if (EspecieSelecionada == null)
                return;

            if (EspecieSelecionada.Id == 0)
            {
                await _especieDao.RegistrarAsync(EspecieSelecionada);
            }
            else
            {
                await _especieDao.AtualizarAsync(EspecieSelecionada);
            }

            await CarregarEspeciesAsync();
            EspecieSelecionada = null;
        }


        public async Task RemoverEspecieAsync()
        {
            if (EspecieSelecionada == null) return;

            // Verifica se existem animais associados à espécie
            var existeAnimalAssociado = Animais.Any(a => a.EspecieId == EspecieSelecionada.Id);

            if (existeAnimalAssociado)
            {
                throw new InvalidOperationException("Não é possível excluir a espécie. Existem animais associados a ela.");
            }

            // Remove a espécie do banco de dados
            await _especieDao.RemoverAsync(EspecieSelecionada);

            // Atualiza as listas
            await CarregarEspeciesAsync();
        }


        partial void OnTermoPesquisaChanged(string oldValue, string newValue)
        {
            AplicarFiltro();
        }
    }
}

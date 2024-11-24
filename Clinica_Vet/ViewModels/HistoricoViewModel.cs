using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Clinica_Vet.ViewModels
{
    public partial class HistoricoViewModel : ObservableObject
    {
        private readonly IDataAccess<ProdutoHistorico> _produtoHistoricoDao;

        [ObservableProperty]
        private ObservableCollection<ProdutoHistorico> produtosHistorico = new ObservableCollection<ProdutoHistorico>();

        public HistoricoViewModel(IDataAccess<ProdutoHistorico> produtoHistoricoDao)
        {
            _produtoHistoricoDao = produtoHistoricoDao;
        }

        public async Task CarregarHistoricoAsync()
        {
            var lista = await _produtoHistoricoDao.ConsultarAsync();
            ProdutosHistorico.Clear();
            foreach (var item in lista)
            {
                ProdutosHistorico.Add(item);
            }
        }
    }
}

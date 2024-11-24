using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Clinica_Vet.ViewModels
{
    public partial class EstoqueAtualViewModel : ObservableObject
    {
        private readonly IDataAccess<Produto> _produtoDao;

        [ObservableProperty]
        private ObservableCollection<Produto> produtos = new ObservableCollection<Produto>();

        [ObservableProperty]
        private ObservableCollection<Produto> produtosFiltrados = new ObservableCollection<Produto>();

        [ObservableProperty]
        private Produto produtoSelecionado;

        [ObservableProperty]
        private string termoPesquisa;

        public EstoqueAtualViewModel(IDataAccess<Produto> produtoDao)
        {
            _produtoDao = produtoDao;
        }

        public async Task CarregarProdutosAsync()
        {
            var lista = await _produtoDao.ConsultarAsync(p => p.Ativo) ?? new System.Collections.Generic.List<Produto>();
            Produtos.Clear();
            foreach (var produto in lista)
            {
                Produtos.Add(produto);
            }
            AplicarFiltro();
        }

        public void AplicarFiltro()
        {
            if (string.IsNullOrWhiteSpace(TermoPesquisa))
            {
                ProdutosFiltrados = new ObservableCollection<Produto>(Produtos);
            }
            else
            {
                var filtro = TermoPesquisa.ToLower();
                var filtrados = Produtos.Where(p => p.Nome.ToLower().Contains(filtro));
                ProdutosFiltrados = new ObservableCollection<Produto>(filtrados);
            }
        }

        public void CriarNovoProduto()
        {
            ProdutoSelecionado = new Produto
            {
                DataEntrada = DateTime.Now,
                Ativo = true
            };
        }

        public async Task SalvarProdutoAsync()
        {
            if (ProdutoSelecionado == null)
                return;

            if (ProdutoSelecionado.Id == 0)
            {
                await _produtoDao.RegistrarAsync(ProdutoSelecionado);
            }
            else
            {
                await _produtoDao.AtualizarAsync(ProdutoSelecionado);
            }

            await CarregarProdutosAsync();
            ProdutoSelecionado = null;
        }

        public async Task RemoverProdutoAsync(int veterinarioId)
        {
            if (ProdutoSelecionado == null)
                return;

            // Marcar o produto como inativo
            ProdutoSelecionado.Ativo = false;
            await _produtoDao.AtualizarAsync(ProdutoSelecionado);

            // Registrar no histórico
            var produtoHistorico = new ProdutoHistorico
            {
                ProdutoId = ProdutoSelecionado.Id,
                Nome = ProdutoSelecionado.Nome,
                DataEntrada = ProdutoSelecionado.DataEntrada,
                DataValidade = ProdutoSelecionado.DataValidade,
                DataSaida = DateTime.Now,
                VeterinarioId = veterinarioId
            };

            var produtoHistoricoDao = Ioc.Default.GetRequiredService<IDataAccess<ProdutoHistorico>>();
            await produtoHistoricoDao.RegistrarAsync(produtoHistorico);

            await CarregarProdutosAsync();
            ProdutoSelecionado = null;
        }

        partial void OnTermoPesquisaChanged(string oldValue, string newValue)
        {
            AplicarFiltro();
        }
    }
}

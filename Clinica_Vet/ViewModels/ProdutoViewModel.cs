using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Clinica_Vet.ViewModels
{
    public partial class ProdutoViewModel : ObservableObject
    {
        private readonly IDataAccess<Produto> _produtoDao;

        [ObservableProperty]
        private ObservableCollection<Produto> produtos;

        [ObservableProperty]
        private Produto produtoSelecionado;

        [ObservableProperty]
        private string nome;

        [ObservableProperty]
        private DateTime? dataEntrada;

        [ObservableProperty]
        private DateTime? dataValidade;

        [ObservableProperty]
        private DateTime? dataSaida;

        public ProdutoViewModel(IDataAccess<Produto> produtoDao)
        {
            _produtoDao = produtoDao;
            Produtos = new ObservableCollection<Produto>();
            DataEntrada = null; // Inicializa como vazio para exibir o placeholder
            CarregarProdutosAsync();
        }

        public ProdutoViewModel()
        {
            Produtos = new ObservableCollection<Produto>();
            DataEntrada = null; // Inicializa como vazio para exibir o placeholder
        }

        [RelayCommand]
        public async Task AdicionarProdutoAsync()
        {
            var novoProduto = new Produto
            {
                Nome = Nome,
                DataEntrada = DataEntrada,
                DataValidade = DataValidade,
                DataSaida = DataSaida
            };

            await _produtoDao.RegistrarAsync(novoProduto);
            await CarregarProdutosAsync();
            LimparCampos();
        }

        [RelayCommand]
        public async Task EditarProdutoAsync()
        {
            if (ProdutoSelecionado == null) return;

            ProdutoSelecionado.Nome = Nome;
            ProdutoSelecionado.DataEntrada = DataEntrada;
            ProdutoSelecionado.DataValidade = DataValidade;
            ProdutoSelecionado.DataSaida = DataSaida;

            await _produtoDao.AtualizarAsync(ProdutoSelecionado);
            await CarregarProdutosAsync();
            LimparCampos();
        }

        [RelayCommand]
        public async Task RemoverProdutoAsync()
        {
            if (ProdutoSelecionado == null) return;

            await _produtoDao.RemoverAsync(ProdutoSelecionado);
            await CarregarProdutosAsync();
            LimparCampos();
        }

        private async Task CarregarProdutosAsync()
        {
            var lista = await _produtoDao.ConsultarAsync() ?? new List<Produto>();
            Produtos = new ObservableCollection<Produto>(lista);
        }

        private void LimparCampos()
        {
            Nome = string.Empty;
            DataEntrada = DateTime.Now;
            DataValidade = null;
            DataSaida = null;
            ProdutoSelecionado = null;
        }
    }
}

using Clinica_Vet.DataAccess;
using Clinica_Vet.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Clinica_Vet.Views
{
    public sealed partial class HistoricoPage : Page
    {
        public HistoricoPage()
        {
            this.InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<HistoricoViewModel>();
            Loaded += OnPageLoaded;
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is HistoricoViewModel viewModel)
            {
                await viewModel.CarregarHistoricoAsync();
            }
        }

        private async void OnExcluirProdutoClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ProdutoHistorico produtoHistorico)
            {
                // Confirmação do usuário
                var dialog = new ContentDialog
                {
                    Title = "Confirmação de Exclusão",
                    Content = $"Tem certeza de que deseja excluir o histórico do produto \"{produtoHistorico.Nome}\"?",
                    PrimaryButtonText = "Sim",
                    CloseButtonText = "Cancelar",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = this.XamlRoot
                };

                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    if (DataContext is HistoricoViewModel viewModel)
                    {
                        // Exclui o item do banco de dados
                        var produtoHistoricoDao = Ioc.Default.GetRequiredService<IDataAccess<ProdutoHistorico>>();
                        await produtoHistoricoDao.RemoverAsync(produtoHistorico);

                        // Atualiza a lista
                        viewModel.ProdutosHistorico.Remove(produtoHistorico);
                    }
                }
            }
        }


    }
}

using Clinica_Vet.Models;
using Clinica_Vet.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace Clinica_Vet.Views
{
    public sealed partial class ConfiguracoesPage : Page
    {
        public ConfiguracoesPage()
        {
            this.InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<ConfiguracoesViewModel>();
            Loaded += ConfiguracoesPage_Loaded;
        }

        private async void ConfiguracoesPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConfiguracoesViewModel viewModel)
            {
                await viewModel.CarregarEspeciesAsync();
            }
        }

        private async void OnAdicionarEspecieClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConfiguracoesViewModel viewModel)
            {
                viewModel.CriarNovaEspecie();
                var dialog = CreateEspecieDialog();
                await dialog.ShowAsync();
            }
        }

        private async void OnEditarEspecieClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConfiguracoesViewModel viewModel && viewModel.EspecieSelecionada != null)
            {
                var dialog = CreateEspecieDialog();
                await dialog.ShowAsync();
            }
        }

        private async void OnRemoverEspecieClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConfiguracoesViewModel viewModel && viewModel.EspecieSelecionada != null)
            {
                var confirmDialog = new ContentDialog
                {
                    Title = "Confirmação",
                    Content = "Deseja realmente remover esta espécie?",
                    PrimaryButtonText = "Sim",
                    CloseButtonText = "Não",
                    XamlRoot = this.XamlRoot
                };

                if (await confirmDialog.ShowAsync() == ContentDialogResult.Primary)
                {
                    try
                    {
                        await viewModel.RemoverEspecieAsync();
                    }
                    catch (InvalidOperationException ex)
                    {
                        // Exibe uma mensagem de erro caso existam animais associados
                        var errorDialog = new ContentDialog
                        {
                            Title = "Erro",
                            Content = ex.Message,
                            CloseButtonText = "Ok",
                            XamlRoot = this.XamlRoot
                        };
                        await errorDialog.ShowAsync();
                    }
                }
            }
        }

        private ContentDialog CreateEspecieDialog()
        {
            var viewModel = DataContext as ConfiguracoesViewModel;

            // Cria uma cópia temporária para edição
            var especieTemp = new Especie
            {
                Id = viewModel?.EspecieSelecionada?.Id ?? 0,
                Nome = viewModel?.EspecieSelecionada?.Nome
            };

            var dialog = new ContentDialog
            {
                Title = especieTemp.Id == 0 ? "Nova Espécie" : "Editar Espécie",
                PrimaryButtonText = "Salvar",
                SecondaryButtonText = "Cancelar",
                XamlRoot = this.XamlRoot
            };

            // Cria um TextBox para o nome da espécie
            var especieTextBox = new TextBox
            {
                Header = "Nome da Espécie",
                Text = especieTemp.Nome,
                Margin = new Thickness(0, 16, 0, 0)
            };

            dialog.Content = especieTextBox;

            // Vincula o evento PrimaryButtonClick
            dialog.PrimaryButtonClick += async (sender, args) =>
            {
                if (viewModel != null)
                {
                    // Atualiza o nome no objeto temporário
                    especieTemp.Nome = especieTextBox.Text;

                    if (especieTemp.Id == 0)
                    {
                        // Adiciona a nova espécie
                        viewModel.CriarNovaEspecie(especieTemp);
                    }
                    else
                    {
                        // Atualiza os valores no objeto selecionado
                        viewModel.EspecieSelecionada.Nome = especieTemp.Nome;
                    }

                    await viewModel.SalvarEspecieAsync();
                }
            };

            return dialog;
        }


    }
}

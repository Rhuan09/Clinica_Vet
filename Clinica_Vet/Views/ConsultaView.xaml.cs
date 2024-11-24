using Clinica_Vet.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Clinica_Vet.Views
{
    public sealed partial class ConsultaView : Page
    {
        public ConsultaView()
        {
            this.InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<ConsultaViewModel>();
            Loaded += OnPageLoaded;
        }

        private async void OnPageLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (DataContext is ConsultaViewModel viewModel)
            {
                await viewModel.CarregarDadosAsync();
            }
        }

        private async void OnExcluirConsultaClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (DataContext is ConsultaViewModel viewModel && viewModel.ConsultaSelecionada != null)
            {
                // Fecha o diálogo atual antes de abrir outro
                ConsultaDialog.Hide();

                var confirmDialog = new ContentDialog
                {
                    Title = "Confirmar exclusão",
                    Content = "Tem certeza de que deseja excluir esta consulta?",
                    PrimaryButtonText = "Sim",
                    CloseButtonText = "Não",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = this.XamlRoot // Configura o XamlRoot corretamente
                };

                var result = await confirmDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    await viewModel.ExcluirConsultaAsync();
                }
                else
                {
                    // Reabre o `ConsultaDialog` se a exclusão for cancelada
                    ConsultaDialog.XamlRoot = this.XamlRoot; // Define o XamlRoot
                    ConsultaDialog.DataContext = viewModel;
                    await ConsultaDialog.ShowAsync();
                }
            }
        }

        private async void OnConsultaSelecionadaChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ConsultaViewModel viewModel && viewModel.ConsultaSelecionada != null)
            {
                ConsultaDialog.XamlRoot = this.XamlRoot; // Define o XamlRoot
                ConsultaDialog.DataContext = viewModel;
                await ConsultaDialog.ShowAsync();
            }
        }

        private void OnCancelarConsultaClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ConsultaDialog.Hide();
            if (DataContext is ConsultaViewModel viewModel)
            {
                viewModel.ConsultaSelecionada = null;
            }
        }

        private async void OnClienteSelecionadoChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ConsultaViewModel viewModel && viewModel.ConsultaSelecionada != null)
            {
                await viewModel.CarregarAnimaisDoClienteAsync();
            }
        }

        private async void OnSalvarConsultaClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (DataContext is ConsultaViewModel viewModel)
            {
                if (string.IsNullOrWhiteSpace(viewModel.ConsultaSelecionada.Descricao) ||
                    viewModel.ConsultaSelecionada.ClienteId == 0 ||
                    viewModel.ConsultaSelecionada.AnimalId == 0 ||
                    viewModel.ConsultaSelecionada.VeterinarioId == 0 ||
                    viewModel.ConsultaSelecionada.Data == default)
                {
                    var errorDialog = new ContentDialog
                    {
                        Title = "Erro",
                        Content = "Por favor, preencha todos os campos obrigatórios antes de salvar.",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot
                    };

                    await errorDialog.ShowAsync();
                    return;
                }

                try
                {
                    await viewModel.SalvarConsultaAsync();
                    ConsultaDialog.Hide();
                    viewModel.ConsultaSelecionada = null;
                }
                catch (Exception ex)
                {
                    var errorDialog = new ContentDialog
                    {
                        Title = "Erro ao salvar",
                        Content = $"Ocorreu um erro ao salvar a consulta: {ex.Message}",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }

        private void OnAdicionarConsultaClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (DataContext is ConsultaViewModel viewModel)
            {
                viewModel.CriarNovaConsulta();
                ConsultaDialog.XamlRoot = this.XamlRoot; // Define o XamlRoot
                ConsultaDialog.DataContext = viewModel;
                ConsultaDialog.ShowAsync();
            }
        }
    }
}

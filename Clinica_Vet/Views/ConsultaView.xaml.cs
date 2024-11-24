using Clinica_Vet.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Clinica_Vet.Views
{
    public sealed partial class ConsultaView : Page
    {
        private bool isDialogOpen = false;


        public ConsultaView()
        {
            this.InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<ConsultaViewModel>();
            Loaded += OnPageLoaded;
            Unloaded += OnPageUnloaded;
        }

        private void OnPageUnloaded(object sender, RoutedEventArgs e)
        {
            // Fecha qualquer diálogo aberto
            if (ConsultaDialog != null)
            {
                ConsultaDialog.Hide();
            }
        }

        private async void OnPageLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (DataContext is ConsultaViewModel viewModel)
            {
                await viewModel.CarregarDadosAsync();
            }
        }

        private async void OnExcluirConsultaClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConsultaViewModel viewModel && viewModel.ConsultaSelecionada != null)
            {
                // Fecha o diálogo principal antes de abrir o de confirmação
                if (ConsultaDialog.IsLoaded)
                {
                    ConsultaDialog.Hide();
                }

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
                    try
                    {
                        // Executa a exclusão
                        await viewModel.ExcluirConsultaAsync();

                        // Reseta a seleção para evitar referências obsoletas
                        viewModel.ConsultaSelecionada = null;
                    }
                    catch (Exception ex)
                    {
                        var errorDialog = new ContentDialog
                        {
                            Title = "Erro ao excluir",
                            Content = $"Ocorreu um erro ao excluir a consulta: {ex.Message}",
                            CloseButtonText = "Ok",
                            XamlRoot = this.XamlRoot
                        };

                        await errorDialog.ShowAsync();
                    }
                }

                // Certifique-se de que o diálogo principal permanece fechado
                if (ConsultaDialog.IsLoaded)
                {
                    ConsultaDialog.Hide();
                }
            }
        }



        private async void OnConsultaSelecionadaChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isDialogOpen || DataContext is not ConsultaViewModel viewModel || viewModel.ConsultaSelecionada == null)
                return;

            isDialogOpen = true;

            try
            {
                ConsultaDialog.XamlRoot = this.XamlRoot; // Define o XamlRoot
                ConsultaDialog.DataContext = viewModel;
                await ConsultaDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao abrir o diálogo: {ex.Message}");
            }
            finally
            {
                isDialogOpen = false;
            }
        }

        private void OnCancelarConsultaClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (DataContext is ConsultaViewModel viewModel)
            {
                ConsultaDialog.Hide();
                viewModel.ConsultaSelecionada = null; // Limpa a consulta selecionada
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
                    ConsultaDialog.Hide(); // Fecha o diálogo antes de exibir o erro

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
                    viewModel.ConsultaSelecionada = null; // Reseta a seleção
                }
                catch (Exception ex)
                {
                    ConsultaDialog.Hide();

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

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

        private void OnConsultaSelecionadaChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ConsultaViewModel viewModel && viewModel.ConsultaSelecionada != null)
            {
                ConsultaDialog.ShowAsync();
            }
        }

        private void OnCancelarConsultaClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ConsultaDialog.Hide();
        }

        private async void OnClienteSelecionadoChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ConsultaViewModel viewModel)
            {
                await viewModel.CarregarAnimaisDoClienteAsync();
            }
        }


        private async void OnSalvarConsultaClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (DataContext is ConsultaViewModel viewModel)
            {
                // Verifique se os campos obrigatórios estão preenchidos
                if (string.IsNullOrWhiteSpace(viewModel.ConsultaSelecionada.Descricao) ||
                    viewModel.ConsultaSelecionada.ClienteId == 0 ||
                    viewModel.ConsultaSelecionada.AnimalId == 0 ||
                    viewModel.ConsultaSelecionada.VeterinarioId == 0 ||
                    viewModel.ConsultaSelecionada.Data == default)
                {
                    // Fecha o dialog original antes de exibir a mensagem de erro
                    ConsultaDialog.Hide();

                    // Exiba uma mensagem de erro se algum campo obrigatório estiver vazio
                    var errorDialog = new ContentDialog
                    {
                        Title = "Erro",
                        Content = "Por favor, preencha todos os campos obrigatórios antes de salvar.",
                        CloseButtonText = "Ok",
                        XamlRoot = ConsultaDialog.XamlRoot // Certifique-se de configurar o XamlRoot
                    };

                    await errorDialog.ShowAsync();

                    // Reabra o diálogo principal após o erro
                    await ConsultaDialog.ShowAsync();

                    return; // Sai para evitar salvar uma consulta inválida
                }

                // Salva a consulta se todos os campos forem válidos
                await viewModel.SalvarConsultaAsync();
                ConsultaDialog.Hide();
            }
        }




        private void OnAdicionarConsultaClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (DataContext is ConsultaViewModel viewModel)
            {
                viewModel.CriarNovaConsulta();
                ConsultaDialog.ShowAsync();
            }
        }
    }
}

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

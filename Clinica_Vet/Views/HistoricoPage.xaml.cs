using Clinica_Vet.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
    }
}

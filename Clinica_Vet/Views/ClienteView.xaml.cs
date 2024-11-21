using Clinica_Vet.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clinica_Vet.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClienteView : Page
    {
        public ClienteView()
        {
            this.InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<ClienteViewModel>();
        }

        private void OnAdicionarClienteClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // Limpa os campos antes de abrir o diálogo
            if (DataContext is ViewModels.ClienteViewModel viewModel)
            {
                viewModel.Nome = string.Empty;
                viewModel.Telefone = string.Empty;
                viewModel.Email = string.Empty;
                viewModel.Endereco = string.Empty;
                viewModel.Cep = string.Empty;
            }

            // Abre o ContentDialog
            AdicionarClienteDialog.ShowAsync();
        }

        private void OnSalvarClienteClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (DataContext is ViewModels.ClienteViewModel viewModel)
            {
                viewModel.AdicionarCliente();
            }

            // Fecha o diálogo
            AdicionarClienteDialog.Hide();
        }

        private void OnCancelarClienteClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Fecha o diálogo sem salvar
            AdicionarClienteDialog.Hide();
        }

    }
}

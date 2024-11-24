using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clinica_Vet
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(Views.HomeView)); // Define a página inicial
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem item)
            {
                switch (item.Tag)
                {
                    case "HomeView":
                        ContentFrame.Navigate(typeof(Views.HomeView));
                        break;
                    case "ConsultaView":
                        ContentFrame.Navigate(typeof(Views.ConsultaView));
                        break;
                    case "ClienteView":
                        ContentFrame.Navigate(typeof(Views.ClienteView));
                        break;
                    case "VeterinarioView":
                        ContentFrame.Navigate(typeof(Views.VeterinarioView));
                        break;
                    case "EstoqueAtualPage":
                        ContentFrame.Navigate(typeof(Views.EstoqueAtualPage));
                        break;
                    case "HistoricoPage":
                        ContentFrame.Navigate(typeof(Views.HistoricoPage));
                        break;
                    default:
                        ContentFrame.Navigate(typeof(Views.HomeView));
                        break;
                }
            }
        }
    }
}

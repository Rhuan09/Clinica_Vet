using Clinica_Vet.Models;
using Clinica_Vet.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;

namespace Clinica_Vet.Views
{
    public sealed partial class VeterinarioView : Page
    {
        private bool isDialogOpen = false; // Controle global para evitar múltiplos diálogos

        public VeterinarioView()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<VeterinarioViewModel>();
            Unloaded += OnPageUnloaded; // Adicione o evento para fechar diálogos
        }

        // Evento para abrir o diálogo de edição do veterinário selecionado
        private async void OnVeterinarioSelecionadoChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is VeterinarioViewModel viewModel && viewModel.VeterinarioSelecionado != null)
            {
                // Verifica se já existe um diálogo ativo
                if (isDialogOpen)
                    return;

                isDialogOpen = true; // Marca o diálogo como ativo
                try
                {
                    await VeterinarioDialog.ShowAsync(); // Exibe o diálogo
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao abrir o diálogo: {ex.Message}");
                }
                finally
                {
                    isDialogOpen = false; // Libera o controle
                }
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is VeterinarioViewModel viewModel)
            {
                // Atualiza o termo de pesquisa no ViewModel
                viewModel.TermoPesquisa = ((TextBox)sender).Text;

                // Aplica o filtro
                viewModel.AplicarFiltro();
            }
        }


        // Evento para salvar as alterações do veterinário
        private async void OnSalvarVeterinarioClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (DataContext is VeterinarioViewModel viewModel)
            {
                await viewModel.EditarAsync();
                VeterinarioDialog.Hide();
            }
        }

        // Evento para cancelar as alterações
        private void OnCancelarVeterinarioClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            VeterinarioDialog.Hide();
        }

        // Evento para adicionar um novo veterinário
        private async void OnAdicionarVeterinarioClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is VeterinarioViewModel viewModel)
            {
                if (isDialogOpen) return; // Evita abertura de múltiplos diálogos
                isDialogOpen = true;

                var dialog = new ContentDialog
                {
                    Title = "Adicionar Veterinário",
                    PrimaryButtonText = "Salvar",
                    CloseButtonText = "Cancelar",
                    DefaultButton = ContentDialogButton.Primary,
                    Content = new StackPanel
                    {
                        Spacing = 8,
                        Children =
                        {
                            new TextBox { Header = "Nome", PlaceholderText = "Nome do Veterinário" },
                            new TextBox { Header = "Telefone", PlaceholderText = "Telefone" },
                            new TextBox { Header = "Email", PlaceholderText = "Email" },
                            new TextBox { Header = "Endereço", PlaceholderText = "Endereço" },
                            new TextBox { Header = "CEP", PlaceholderText = "CEP" }
                        }
                    },
                    XamlRoot = this.XamlRoot
                };

                var result = await dialog.ShowAsync();
                isDialogOpen = false; // Marca o diálogo como fechado

                if (result == ContentDialogResult.Primary)
                {
                    var stackPanel = (StackPanel)dialog.Content;

                    // Preencher as propriedades do ViewModel com os valores do diálogo
                    viewModel.Nome = ((TextBox)stackPanel.Children[0]).Text;
                    viewModel.Telefone = ((TextBox)stackPanel.Children[1]).Text;
                    viewModel.Email = ((TextBox)stackPanel.Children[2]).Text;
                    viewModel.Endereco = ((TextBox)stackPanel.Children[3]).Text;
                    viewModel.Cep = ((TextBox)stackPanel.Children[4]).Text;

                    // Chamar o comando para adicionar
                    await viewModel.AdicionarAsync();
                    await viewModel.CarregarVeterinariosAsync();
                }
            }
        }

        // Evento para excluir o veterinário selecionado
        private async void OnExcluirVeterinarioClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is VeterinarioViewModel viewModel && viewModel.VeterinarioSelecionado != null)
            {
                // Fecha o diálogo principal antes de abrir o de erro ou confirmação
                if (VeterinarioDialog.IsLoaded)
                {
                    VeterinarioDialog.Hide();
                }

                // Verificar se o veterinário possui consultas futuras
                if (viewModel.VeterinarioSelecionado.Consultas != null &&
                    viewModel.VeterinarioSelecionado.Consultas.Any(c => c.Data > DateTime.Now))
                {
                    // Exibe o erro informando que há consultas futuras
                    var errorDialog = new ContentDialog
                    {
                        Title = "Erro",
                        Content = "Este veterinário possui consultas futuras agendadas. Exclua ou remaneje as consultas antes de tentar novamente.",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot // Configura o XamlRoot corretamente
                    };

                    await errorDialog.ShowAsync();
                    return; // Sai do método sem tentar excluir o veterinário
                }

                // Diálogo de confirmação
                var confirmDialog = new ContentDialog
                {
                    Title = "Confirmar exclusão",
                    Content = "Tem certeza de que deseja excluir este veterinário?",
                    PrimaryButtonText = "Sim",
                    CloseButtonText = "Não",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = this.XamlRoot
                };

                var result = await confirmDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    try
                    {
                        // Executa a exclusão
                        await viewModel.RemoverAsync();
                        viewModel.CarregarVeterinariosAsync();

                        // Reseta a seleção para evitar referências obsoletas
                        viewModel.VeterinarioSelecionado = null;
                    }
                    catch (Exception ex)
                    {
                        // Exibe um diálogo de erro caso a exclusão falhe
                        var errorDialog = new ContentDialog
                        {
                            Title = "Erro ao excluir",
                            Content = $"Ocorreu um erro ao excluir o veterinário: {ex.Message}",
                            CloseButtonText = "Ok",
                            XamlRoot = this.XamlRoot
                        };

                        await errorDialog.ShowAsync();
                    }
                }

                // Certifique-se de que o diálogo principal permanece fechado
                if (VeterinarioDialog.IsLoaded)
                {
                    VeterinarioDialog.Hide();
                }
            }
        }


        private void OnPageUnloaded(object sender, RoutedEventArgs e)
        {

            if (DataContext is VeterinarioViewModel viewModel)
            {
                viewModel.VeterinarioSelecionado = null;
            }
                // Fecha qualquer diálogo aberto
                if (VeterinarioDialog != null && VeterinarioDialog.IsLoaded)
            {
                VeterinarioDialog.Hide();
            }

            if (ConfirmDeleteDialog != null && ConfirmDeleteDialog.IsLoaded)
            {
                ConfirmDeleteDialog.Hide();
            }

            if (ErrorDialog != null && ErrorDialog.IsLoaded)
            {
                ErrorDialog.Hide();
            }
        }


        // Manipulador para confirmar a exclusão
        private void OnConfirmDelete(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Ação já é tratada no método OnExcluirVeterinarioClick
            // Você pode deixar este método vazio ou implementar lógica adicional se necessário
        }

        // Manipulador para fechar o diálogo de erro
        private void OnCloseError(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Nenhuma ação necessária ao fechar o erro
        }


    }
}

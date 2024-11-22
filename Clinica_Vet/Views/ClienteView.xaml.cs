using Clinica_Vet.Models;
using Clinica_Vet.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Clinica_Vet.Views
{
    public sealed partial class ClienteView : Page
    {
        public ClienteView()
        {
            this.InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<ClienteViewModel>();

        }
        private bool isDialogOpen = false;

        /// <summary>
        /// Abre o diálogo para exibir as informações do cliente selecionado.
        /// </summary>

        private async void OnClienteSelecionadoChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            if (DataContext is ClienteViewModel viewModel && viewModel.ClienteSelecionado != null)
            {
                // Verifica se outro diálogo já está aberto
                if (isDialogOpen)
                    return;

                isDialogOpen = true; // Marca o diálogo como aberto
                try
                {
                    await ClienteDialog.ShowAsync(); // Exibe o diálogo
                }
                catch (Exception ex)
                {
                    // Opcional: Registrar erros
                    System.Diagnostics.Debug.WriteLine($"Erro ao abrir o diálogo: {ex.Message}");
                }
                finally
                {
                    isDialogOpen = false; // Libera o controle do diálogo
                }
            }
        }

        private async void OnExcluirClienteClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ClienteViewModel viewModel)
            {
                await viewModel.RemoverClienteAsync();

                // Fecha o diálogo após excluir o cliente
                ClienteDialog.Hide();
            }
        }


        private async void OnAdicionarAnimalClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (DataContext is ClienteViewModel viewModel && viewModel.ClienteSelecionado != null)
            {
                // Fecha o ClienteDialog para permitir a abertura de outro ContentDialog
                ClienteDialog.Hide();

                // Configura o ComboBox de espécies
                AnimalEspecieComboBox.SelectedIndex = -1;

                // Limpa os campos do diálogo
                AnimalNomeTextBox.Text = string.Empty;
                AnimalIdadeTextBox.Text = string.Empty;
                AnimalPesoTextBox.Text = string.Empty;

                // Abre o diálogo para adicionar animal
                var result = await AdicionarAnimalDialog.ShowAsync();

                // Reabre o ClienteDialog após fechar o AdicionarAnimalDialog
                if (result == ContentDialogResult.Secondary || result == ContentDialogResult.Primary)
                {
                    await ClienteDialog.ShowAsync();

                }
            }
            else
            {
                // Cliente não selecionado
                var noClientDialog = new ContentDialog
                {
                    Title = "Erro",
                    Content = "Por favor, selecione um cliente antes de adicionar um animal.",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot
                };

                await noClientDialog.ShowAsync();
            }
        }


        private async void OnSalvarAnimalClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (DataContext is ClienteViewModel viewModel)
            {
                // Fecha o diálogo antes de realizar a operação
                AdicionarAnimalDialog.Hide();

                string nome = AnimalNomeTextBox.Text;
                int.TryParse(AnimalIdadeTextBox.Text, out int idade);
                double.TryParse(AnimalPesoTextBox.Text, out double peso);
                int? especieId = AnimalEspecieComboBox.SelectedValue as int?;

                if (!string.IsNullOrEmpty(nome) && especieId.HasValue)
                {
                    // Adiciona o animal ao cliente selecionado
                    await viewModel.AdicionarAnimalAsync(new Animal
                    {
                        Nome = nome,
                        Idade = idade,
                        Peso = peso,
                        ClienteId = viewModel.ClienteSelecionado.Id,
                        EspecieId = especieId.Value
                    });
                }
                else
                {
                    // Exibe um erro se o formulário estiver incompleto
                    var errorDialog = new ContentDialog
                    {
                        Title = "Erro",
                        Content = "Preencha todos os campos antes de salvar.",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }


        private void OnCancelarAnimalClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            AdicionarAnimalDialog.Hide();
        }


        /// <summary>
        /// Lógica para adicionar um novo cliente.
        /// </summary>
        private async void OnAdicionarClienteClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (DataContext is ViewModels.ClienteViewModel viewModel)
            {
                // Cria um diálogo para adicionar cliente
                var dialog = new ContentDialog
                {
                    Title = "Adicionar Cliente",
                    PrimaryButtonText = "Salvar",
                    CloseButtonText = "Cancelar",
                    DefaultButton = ContentDialogButton.Primary,
                    Content = new StackPanel
                    {
                        Spacing = 8,
                        Children =
                        {
                            new TextBox { Header = "Nome", Text = viewModel.Nome },
                            new TextBox { Header = "Telefone", Text = viewModel.Telefone },
                            new TextBox { Header = "Email", Text = viewModel.Email },
                            new TextBox { Header = "Endereço", Text = viewModel.Endereco },
                            new TextBox { Header = "CEP", Text = viewModel.Cep }
                        }
                    },
                    XamlRoot = this.XamlRoot
                };

                var result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    var stackPanel = (StackPanel)dialog.Content;

                    // Preenche as propriedades do ViewModel com os dados do formulário
                    viewModel.Nome = ((TextBox)stackPanel.Children[0]).Text;
                    viewModel.Telefone = ((TextBox)stackPanel.Children[1]).Text;
                    viewModel.Email = ((TextBox)stackPanel.Children[2]).Text;
                    viewModel.Endereco = ((TextBox)stackPanel.Children[3]).Text;
                    viewModel.Cep = ((TextBox)stackPanel.Children[4]).Text;

                    // Chama o comando para adicionar cliente
                    await viewModel.AdicionarClienteAsync();
                }
            }
        }

        /// <summary>
        /// Salva as alterações feitas no cliente.
        /// </summary>
        private async void OnSalvarClienteClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (DataContext is ViewModels.ClienteViewModel viewModel)
            {
                await viewModel.EditarClienteAsync();
                // Fecha o ContentDialog
                
            }
        }

        /// <summary>
        /// Fecha o diálogo sem salvar.
        /// </summary>
        private void OnCancelarClienteClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ClienteDialog.Hide();
            ClientesDataGrid.SelectedItem = null;
        }
    }
}

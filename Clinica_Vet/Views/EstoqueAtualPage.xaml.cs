using Clinica_Vet.Converters;
using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using Clinica_Vet.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace Clinica_Vet.Views
{
    public sealed partial class EstoqueAtualPage : Page
    {
        public EstoqueAtualPage()
        {
            this.InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<EstoqueAtualViewModel>();
            Loaded += OnPageLoaded;
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is EstoqueAtualViewModel viewModel)
            {
                await viewModel.CarregarProdutosAsync();
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is EstoqueAtualViewModel viewModel)
            {
                viewModel.TermoPesquisa = ((TextBox)sender).Text;
            }
        }

        private async void OnAdicionarProdutoClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is EstoqueAtualViewModel viewModel)
            {
                viewModel.CriarNovoProduto();
                var dialog = CreateProdutoDialog();
                await dialog.ShowAsync();
            }
        }

        private async void OnProdutoSelecionadoChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is EstoqueAtualViewModel viewModel && viewModel.ProdutoSelecionado != null)
            {
                var dialog = CreateProdutoDialog();
                await dialog.ShowAsync();
            }
        }

        private ContentDialog CreateProdutoDialog()
        {
            var viewModel = DataContext as EstoqueAtualViewModel;

            // Criar um StackPanel para conter os controles
            var panel = new StackPanel { Spacing = 16 };

            // Mensagem de erro
            var errorTextBlock = new TextBlock
            {
                Visibility = Visibility.Collapsed,
                TextWrapping = TextWrapping.Wrap
            };

            // Campo Nome
            var nomeTextBox = new TextBox
            {
                Header = "Nome do Produto"
            };
            nomeTextBox.SetBinding(TextBox.TextProperty, new Binding
            {
                Path = new PropertyPath("ProdutoSelecionado.Nome"),
                Mode = BindingMode.TwoWay
            });

            // Campo Data de Entrada
            var dataEntradaPicker = new DatePicker
            {
                Header = "Data de Entrada"
            };
            dataEntradaPicker.SetBinding(DatePicker.DateProperty, new Binding
            {
                Path = new PropertyPath("ProdutoSelecionado.DataEntrada"),
                Mode = BindingMode.TwoWay
            });

            // Campo Data de Validade
            var dataValidadePicker = new DatePicker
            {
                Header = "Data de Validade"
            };
            dataValidadePicker.SetBinding(DatePicker.DateProperty, new Binding
            {
                Path = new PropertyPath("ProdutoSelecionado.DataValidade"),
                Mode = BindingMode.TwoWay
            });

            // Botão Remover
            var removerButton = new Button
            {
                Content = "Remover",
            };
            removerButton.Click += OnRemoverProdutoClick;
            removerButton.SetBinding(VisibilityProperty, new Binding
            {
                Path = new PropertyPath("ProdutoSelecionado.Id"),
                Converter = new IdToVisibilityConverter()
            });

            // Adicionar controles ao painel
            panel.Children.Add(errorTextBlock);
            panel.Children.Add(nomeTextBox);
            panel.Children.Add(dataEntradaPicker);
            panel.Children.Add(dataValidadePicker);
            panel.Children.Add(removerButton);

            // Criar o diálogo
            var dialog = new ContentDialog
            {
                Title = viewModel?.ProdutoSelecionado?.Nome ?? "Produto",
                PrimaryButtonText = "Salvar",
                SecondaryButtonText = "Cancelar",
                XamlRoot = this.XamlRoot,
                DataContext = this.DataContext,
                Content = panel
            };

            dialog.PrimaryButtonClick += (sender, args) => OnSalvarProdutoClick(sender, args, errorTextBlock);
            dialog.SecondaryButtonClick += OnCancelarProdutoClick;

            return dialog;
        }

        private async void OnSalvarProdutoClick(ContentDialog sender, ContentDialogButtonClickEventArgs args, TextBlock errorTextBlock)
        {
            if (DataContext is EstoqueAtualViewModel viewModel)
            {
                if (string.IsNullOrWhiteSpace(viewModel.ProdutoSelecionado.Nome))
                {
                    args.Cancel = true;
                    errorTextBlock.Text = "Por favor, preencha o nome do produto.";
                    errorTextBlock.Visibility = Visibility.Visible;
                    return;
                }

                try
                {
                    await viewModel.SalvarProdutoAsync();
                    sender.Hide();
                }
                catch (Exception ex)
                {
                    args.Cancel = true;
                    errorTextBlock.Text = $"Erro ao salvar o produto: {ex.Message}";
                    errorTextBlock.Visibility = Visibility.Visible;
                }
            }
        }

        private void OnCancelarProdutoClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
            if (DataContext is EstoqueAtualViewModel viewModel)
            {
                viewModel.ProdutoSelecionado = null;
            }
        }

        private async void OnRemoverProdutoClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is EstoqueAtualViewModel viewModel && viewModel.ProdutoSelecionado != null)
            {
                // Fecha o diálogo atual antes de abrir o de confirmação
                var parentDialog = FindParentContentDialog(sender as DependencyObject);
                if (parentDialog != null)
                {
                    parentDialog.Hide();
                }

                // Selecionar o Veterinário
                var veterinarioDialog = new ContentDialog
                {
                    Title = "Selecionar Veterinário",
                    PrimaryButtonText = "Confirmar",
                    SecondaryButtonText = "Cancelar",
                    XamlRoot = this.XamlRoot
                };

                // Obter a lista de veterinários
                var veterinarioDao = Ioc.Default.GetRequiredService<IDataAccess<Veterinario>>();
                var veterinarios = await veterinarioDao.ConsultarAsync();

                // Criar ComboBox para seleção
                var comboBox = new ComboBox
                {
                    ItemsSource = veterinarios,
                    DisplayMemberPath = "Nome",
                    SelectedValuePath = "Id",
                    PlaceholderText = "Selecione o veterinário"
                };

                veterinarioDialog.Content = comboBox;

                var result = await veterinarioDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    if (comboBox.SelectedItem is Veterinario veterinarioSelecionado)
                    {
                        try
                        {
                            // Registrar no histórico
                            await viewModel.RemoverProdutoAsync(veterinarioSelecionado.Id);
                        }
                        catch (Exception ex)
                        {
                            var errorDialog = new ContentDialog
                            {
                                Title = "Erro ao remover",
                                Content = $"Ocorreu um erro ao remover o produto: {ex.Message}",
                                CloseButtonText = "Ok",
                                XamlRoot = this.XamlRoot
                            };

                            await errorDialog.ShowAsync();
                        }
                    }
                    else
                    {
                        // Se nenhum veterinário foi selecionado, reabra o diálogo
                        var dialog = CreateProdutoDialog();
                        await dialog.ShowAsync();
                    }
                }
                else
                {
                    // Reabre o diálogo de edição se a remoção for cancelada
                    var dialog = CreateProdutoDialog();
                    await dialog.ShowAsync();
                }
            }
        }

        private ContentDialog FindParentContentDialog(DependencyObject child)
        {
            DependencyObject parent = child;
            while (parent != null && !(parent is ContentDialog))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as ContentDialog;
        }
    }
}

using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using Clinica_Vet.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Clinica_Vet.Views
{
    public sealed partial class AreaPetView : Page
    {
        public AreaPetView()
        {
            this.InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<AreaPetViewModel>();
            Loaded += AreaPetView_Loaded;
        }

        private async void AreaPetView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is AreaPetViewModel viewModel)
            {
                await viewModel.CarregarClientesAsync();
            }
        }

        private async void ClienteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is AreaPetViewModel viewModel)
            {
                await viewModel.CarregarAnimaisDoClienteAsync();
            }
        }

        private async void Animal_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Border border && border.DataContext is Animal animal)
            {
                await MostrarDialogoAnimalAsync(animal);
            }
        }

        private async Task MostrarDialogoAnimalAsync(Animal animal)
        {
            // Carregar tratamentos
            var tratamentosDao = Ioc.Default.GetRequiredService<IDataAccess<Tratamento>>();
            var tratamentos = await tratamentosDao.ConsultarAsync(t => t.AnimalId == animal.Id);
            animal.Tratamentos = new ObservableCollection<Tratamento>(tratamentos);

            // Carregar consultas e exames
            var consultasDao = Ioc.Default.GetRequiredService<IDataAccess<Consulta>>();
            var consultas = await consultasDao.ConsultarAsync(c => c.AnimalId == animal.Id);
            foreach (var consulta in consultas)
            {
                var examesDao = Ioc.Default.GetRequiredService<IDataAccess<Exame>>();
                consulta.Exames = await examesDao.ConsultarAsync(e => e.ConsultaId == consulta.Id);
            }
            var examesPendentes = consultas.SelectMany(c => c.Exames)
                                           .Where(e => string.IsNullOrEmpty(e.Resultado))
                                           .ToList();

            // Configurar diálogo
            var dialog = new ContentDialog
            {
                Title = $"Detalhes do Pet - {animal.Nome}",
                PrimaryButtonText = "Fechar",
                XamlRoot = this.XamlRoot
            };

            var panel = new StackPanel { Spacing = 16 };

            // Informações do Animal
            panel.Children.Add(new TextBlock { Text = $"Nome: {animal.Nome}", FontSize = 18 });
            panel.Children.Add(new TextBlock { Text = $"Idade: {animal.Idade} anos" });
            panel.Children.Add(new TextBlock { Text = $"Peso: {animal.Peso} kg" });

            // Botões de ação
            var actionPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 16 };

            var iniciarTratamentoButton = new Button { Content = "Iniciar Tratamento" };
            iniciarTratamentoButton.Click += async (s, e) =>
            {
                dialog.Hide();
                await MostrarDialogoIniciarTratamentoAsync(animal);
            };
            actionPanel.Children.Add(iniciarTratamentoButton);

            var adicionarExameButton = new Button { Content = "Adicionar Exame" };
            adicionarExameButton.Click += async (s, e) =>
            {
                dialog.Hide();
                await MostrarDialogoAdicionarExameAsync(animal);
            };
            actionPanel.Children.Add(adicionarExameButton);

            panel.Children.Add(actionPanel);

            // Tratamentos
            panel.Children.Add(new TextBlock
            {
                Text = "Tratamentos Atuais",
                FontSize = 16,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold
            });

            foreach (var tratamento in animal.Tratamentos)
            {
                var tratamentoPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
                tratamentoPanel.Children.Add(new TextBlock { Text = tratamento.Descricao });
                tratamentoPanel.Children.Add(new TextBlock
                {
                    Text = $"Tempo restante: {Math.Max((tratamento.DataFim - DateTime.Now).Days, 0)} dias"
                });
                var deleteButton = new Button { Content = "Excluir" };
                deleteButton.Click += async (s, e) => await ExcluirTratamentoAsync(tratamento);
                tratamentoPanel.Children.Add(deleteButton);
                panel.Children.Add(tratamentoPanel);
            }

            // Exames
            panel.Children.Add(new TextBlock
            {
                Text = "Exames Pendentes",
                FontSize = 16,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold
            });

            foreach (var exame in examesPendentes)
            {
                var examePanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
                examePanel.Children.Add(new TextBlock { Text = exame.Nome });
                var deleteButton = new Button { Content = "Excluir" };
                deleteButton.Click += async (s, e) => await ExcluirExameAsync(exame);
                examePanel.Children.Add(deleteButton);
                panel.Children.Add(examePanel);
            }

            dialog.Content = new ScrollViewer { Content = panel };

            await dialog.ShowAsync();
        }


        private async Task ExcluirTratamentoAsync(Tratamento tratamento)
        {
            var tratamentoDao = Ioc.Default.GetRequiredService<IDataAccess<Tratamento>>();
            await tratamentoDao.RemoverAsync(tratamento);
            await AtualizarCardsAnimaisAsync();
        }

        private async Task ExcluirExameAsync(Exame exame)
        {
            var exameDao = Ioc.Default.GetRequiredService<IDataAccess<Exame>>();
            await exameDao.RemoverAsync(exame);
            await AtualizarCardsAnimaisAsync();
        }







        private async Task MostrarDialogoIniciarTratamentoAsync(Animal animal)
        {
            var consultasDao = Ioc.Default.GetRequiredService<IDataAccess<Consulta>>();
            var consultas = await consultasDao.ConsultarAsync(c => c.AnimalId == animal.Id);

            var dialog = new ContentDialog
            {
                Title = "Iniciar Tratamento",
                PrimaryButtonText = "Iniciar",
                SecondaryButtonText = "Cancelar",
                XamlRoot = this.XamlRoot
            };

            var panel = new StackPanel { Spacing = 16 };

            // ComboBox para selecionar a consulta
            var consultaComboBox = new ComboBox
            {
                ItemsSource = consultas,
                DisplayMemberPath = "Descricao",
                PlaceholderText = "Selecione a consulta"
            };
            panel.Children.Add(consultaComboBox);

            // Campo para descrição do tratamento
            var descricaoTextBox = new TextBox
            {
                Header = "Descrição do Tratamento"
            };
            panel.Children.Add(descricaoTextBox);

            // DatePicker para data de início
            var dataInicioPicker = new DatePicker
            {
                Header = "Data de Início",
                Date = DateTimeOffset.Now // Ajustado para DateTimeOffset
            };
            panel.Children.Add(dataInicioPicker);

            // DatePicker para data de fim
            var dataFimPicker = new DatePicker
            {
                Header = "Data de Fim",
                Date = DateTimeOffset.Now.AddMonths(1) // Ajustado para DateTimeOffset
            };
            panel.Children.Add(dataFimPicker);

            dialog.Content = panel;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (consultaComboBox.SelectedItem is Consulta consultaSelecionada)
                {
                    if (dataInicioPicker.Date != null && dataFimPicker.Date != null)
                    {
                        var tratamentoDao = Ioc.Default.GetRequiredService<IDataAccess<Tratamento>>();

                        var novoTratamento = new Tratamento
                        {
                            AnimalId = animal.Id,
                            DataInicio = dataInicioPicker.Date.DateTime, // Converte DateTimeOffset para DateTime
                            DataFim = dataFimPicker.Date.DateTime,       // Converte DateTimeOffset para DateTime
                            Descricao = descricaoTextBox.Text
                        };

                        await tratamentoDao.RegistrarAsync(novoTratamento);

                        // Atualizar a lista de tratamentos do animal
                        animal.Tratamentos.Add(novoTratamento);
                        await AtualizarCardsAnimaisAsync();
                    }
                    else
                    {
                        var errorDialog = new ContentDialog
                        {
                            Title = "Erro",
                            Content = "Por favor, preencha as datas de início e fim corretamente.",
                            CloseButtonText = "Ok",
                            XamlRoot = this.XamlRoot
                        };

                        await errorDialog.ShowAsync();
                    }
                }
            }
        }


        private async Task AtualizarCardsAnimaisAsync()
        {
            if (DataContext is AreaPetViewModel viewModel && viewModel.ClienteSelecionado != null)
            {
                await viewModel.CarregarAnimaisDoClienteAsync();
            }
        }

        private async Task MostrarDialogoAdicionarExameAsync(Animal animal)
        {
            var consultasDao = Ioc.Default.GetRequiredService<IDataAccess<Consulta>>();
            var consultas = await consultasDao.ConsultarAsync(c => c.AnimalId == animal.Id);

            var dialog = new ContentDialog
            {
                Title = "Adicionar Exame",
                PrimaryButtonText = "Adicionar",
                SecondaryButtonText = "Cancelar",
                XamlRoot = this.XamlRoot
            };

            var panel = new StackPanel { Spacing = 16 };

            // ComboBox para selecionar a consulta
            var consultaComboBox = new ComboBox
            {
                ItemsSource = consultas,
                DisplayMemberPath = "Descricao",
                PlaceholderText = "Selecione a consulta"
            };
            panel.Children.Add(consultaComboBox);

            // Campo para nome do exame
            var nomeExameTextBox = new TextBox
            {
                Header = "Nome do Exame"
            };
            panel.Children.Add(nomeExameTextBox);

            dialog.Content = panel;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (consultaComboBox.SelectedItem is Consulta consultaSelecionada)
                {
                    var exameDao = Ioc.Default.GetRequiredService<IDataAccess<Exame>>();

                    var novoExame = new Exame
                    {
                        ConsultaId = consultaSelecionada.Id,
                        Nome = nomeExameTextBox.Text
                    };

                    await exameDao.RegistrarAsync(novoExame);

                    // Atualizar a lista de exames na consulta
                    if (consultaSelecionada.Exames == null)
                        consultaSelecionada.Exames = new System.Collections.Generic.List<Exame>();

                    consultaSelecionada.Exames.Add(novoExame);
                    AtualizarCardsAnimaisAsync();
                }
            }
        }
    }
}

using Clinica_Vet.DataAccess;
using Clinica_Vet.Models;
using Clinica_Vet.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
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
            // Carregar os tratamentos do banco de dados e garantir que não haja duplicatas
            var tratamentosDao = Ioc.Default.GetRequiredService<IDataAccess<Tratamento>>();
            var tratamentos = await tratamentosDao.ConsultarAsync(t => t.AnimalId == animal.Id);
            animal.Tratamentos = new System.Collections.ObjectModel.ObservableCollection<Tratamento>(
                tratamentos.DistinctBy(t => t.Descricao) // Remover duplicatas com base na descrição
            );

            // Carregar as consultas do banco de dados e incluir os exames relacionados
            var consultasDao = Ioc.Default.GetRequiredService<IDataAccess<Consulta>>();
            var consultas = await consultasDao.ConsultarAsync(c => c.AnimalId == animal.Id);

            // Garantir que cada consulta tenha os exames carregados
            foreach (var consulta in consultas)
            {
                var examesDao = Ioc.Default.GetRequiredService<IDataAccess<Exame>>();
                consulta.Exames = await examesDao.ConsultarAsync(e => e.ConsultaId == consulta.Id);
            }

            // Filtrar os exames pendentes
            var examesPendentes = consultas
                .SelectMany(c => c.Exames)
                .Where(e => string.IsNullOrEmpty(e.Resultado)) // Exames sem resultado
                .DistinctBy(e => e.Nome) // Remover duplicatas por nome
                .ToList();

            // Configurar o diálogo
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

            // Botão para iniciar tratamento
            var iniciarTratamentoButton = new Button { Content = "Iniciar Tratamento" };
            iniciarTratamentoButton.Click += async (s, e) =>
            {
                dialog.Hide();
                await MostrarDialogoIniciarTratamentoAsync(animal);
            };
            panel.Children.Add(iniciarTratamentoButton);

            // Botão para adicionar exame
            var adicionarExameButton = new Button { Content = "Adicionar Exame" };
            adicionarExameButton.Click += async (s, e) =>
            {
                dialog.Hide();
                await MostrarDialogoAdicionarExameAsync(animal);
            };
            panel.Children.Add(adicionarExameButton);

            // Lista de tratamentos
            var tratamentosHeader = new TextBlock
            {
                Text = "Tratamentos Atuais",
                FontSize = 16,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold
            };
            panel.Children.Add(tratamentosHeader);

            var tratamentosList = new ListView
            {
                ItemsSource = animal.Tratamentos,
                DisplayMemberPath = "Descricao"
            };
            panel.Children.Add(tratamentosList);

            // Lista de exames pendentes
            var examesHeader = new TextBlock
            {
                Text = "Exames Pendentes",
                FontSize = 16,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold
            };
            panel.Children.Add(examesHeader);

            var examesList = new ListView
            {
                ItemsSource = new System.Collections.ObjectModel.ObservableCollection<Exame>(examesPendentes),
                DisplayMemberPath = "Nome"
            };
            panel.Children.Add(examesList);

            dialog.Content = new ScrollViewer { Content = panel };

            await dialog.ShowAsync();
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

            dialog.Content = panel;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (consultaComboBox.SelectedItem is Consulta consultaSelecionada)
                {
                    var tratamentoDao = Ioc.Default.GetRequiredService<IDataAccess<Tratamento>>();

                    var novoTratamento = new Tratamento
                    {
                        AnimalId = animal.Id,
                        DataInicio = DateTime.Now,
                        DataFim = DateTime.Now.AddMonths(1), // Duração de 1 mês
                        Descricao = descricaoTextBox.Text
                    };

                    await tratamentoDao.RegistrarAsync(novoTratamento);

                    // Atualizar a lista de tratamentos do animal
                    animal.Tratamentos.Add(novoTratamento);
                    AtualizarCardsAnimaisAsync();


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

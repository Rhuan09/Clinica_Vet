using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Clinica_Vet.DataAccess;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Clinica_Vet.Models;
using Clinica_Vet.DbContexts;
using Clinica_Vet.ViewModels;
using System.Globalization;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clinica_Vet
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            var services = new ServiceCollection();

            // Registro do DbContext
            services.AddDbContext<AppDbContext>();
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");
            // Registro das DAOs
            services.AddScoped<IDataAccess<Veterinario>, VeterinarioDAO>();
            services.AddScoped<IDataAccess<Cliente>, ClienteDAO>();
            services.AddScoped<IDataAccess<Animal>, AnimalDAO>();
            services.AddScoped<IDataAccess<Produto>, ProdutoDAO>();
            services.AddScoped<IDataAccess<Tratamento>, TratamentoDAO>();
            services.AddScoped<IDataAccess<Consulta>, ConsultaDAO>();
            services.AddScoped<IDataAccess<Exame>, ExameDAO>();
            services.AddScoped<IDataAccess<ProdutoHistorico>, ProdutoHistoricoDAO>();
            services.AddScoped<IDataAccess<Especie>, EspecieDAO>();
            services.AddScoped<HomeViewModel>();
            services.AddScoped<AreaPetViewModel>();
            services.AddScoped<ConsultaViewModel>();
            services.AddScoped<ClienteViewModel>();
            services.AddScoped<EstoqueAtualViewModel>();
            services.AddScoped<HistoricoViewModel>();
            services.AddScoped<ConfiguracoesViewModel>();
            services.AddScoped<VeterinarioViewModel>();
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "pt-BR";

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window? m_window;
    }
}

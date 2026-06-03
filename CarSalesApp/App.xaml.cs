using CarSalesApp.Services;
using CarSalesApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CarSalesApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        protected override void OnStartup(
            StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);            
            serviceProvider = services.BuildServiceProvider();

            var mainWindow =  serviceProvider.GetRequiredService<MainWindow>();

            mainWindow.Show();

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Services
            services.AddSingleton<ICarLoader, XmlCarLoader>();
            services.AddSingleton<ISalesCalculator, SalesCalculator>();

            // ViewModels
            services.AddSingleton<MainWindowViewModel>();

            // Views
            services.AddSingleton<MainWindow>();
        }

        protected override void OnExit( ExitEventArgs e)
        {
            serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }

}

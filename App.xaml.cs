using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Dictionar.MVVM.ViewModel;
using Dictionar.Core;
using Dictionar.Services;

namespace Dictionar
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<AdminViewModel>();
            services.AddSingleton<GameViewModel>();
            services.AddSingleton<EditViewModel>();
            services.AddSingleton<DetailsViewModel>();
            services.AddSingleton<EditWordViewModel>();
            services.AddSingleton<IWordDataService, WordDataService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider => viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));


            _serviceProvider  = services.BuildServiceProvider();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                // Log or display the exception details
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PocketFlowSharpGallery.Services;
using PocketFlowSharpGallery.ViewModels.Pages;
using PocketFlowSharpGallery.ViewModels.Windows;
using PocketFlowSharpGallery.Views.Pages;
using PocketFlowSharpGallery.Views.Windows;
using PocketFlowSharpGallery.Data;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace PocketFlowSharpGallery
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(AppContext.BaseDirectory)); })
            .ConfigureServices((context, services) =>
            {

                // 添加数据库服务
                var projectRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..");
                projectRootPath = Path.GetFullPath(projectRootPath);
                var dbPath = Path.Combine(projectRootPath, "Data", "pocketflowcharp.db");

                services.AddDbContext<PFSDbContext>(options =>
                    options.UseSqlite($"Data Source={dbPath}"));

                services.AddNavigationViewPageProvider();

                services.AddHostedService<ApplicationHostService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window with navigation
                services.AddSingleton<INavigationWindow, MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton<DashboardPage>();
                services.AddSingleton<DashboardViewModel>();
                services.AddSingleton<DataPage>();
                services.AddSingleton<DataViewModel>();
                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsViewModel>();
                services.AddSingleton<LLMConfigPage>();
                services.AddSingleton<LLMConfigViewModel>();
                services.AddSingleton<SearchEngineConfigPage>();
                services.AddSingleton<SearchEngineConfigViewModel>();
                services.AddSingleton<WebSearchPage>();
                services.AddSingleton<WebSearchViewModel>();
                
                // Register LLMConfig repository
                services.AddScoped<ILLMConfigRepository, LLMConfigRepository>();
                services.AddScoped<ISearchEngineConfigRepository, SearchEngineConfigRepository>();
                
                // Register dialog service
                services.AddScoped<IDialogService, DialogService>();               
            }).Build();

        /// <summary>
        /// Gets services.
        /// </summary>
        public static IServiceProvider Services
        {
            get { return _host.Services; }
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}

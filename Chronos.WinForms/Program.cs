using Chronos.Core;
using Chronos.Views.TabPages;
using Microsoft.Extensions.DependencyInjection;

namespace Chronos.WinForms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var appDataDirectory = EnsureAppDataDirectoryExists();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddChronosCore(appDataDirectory);
            serviceCollection.AddTransient<MainView>();
            serviceCollection.AddTransient<DashboardPage>();
            serviceCollection.AddTransient<TrackingPage>();
            serviceCollection.AddTransient<RecordsPage>();
            serviceCollection.AddTransient<TimeSheetPage>();
            serviceCollection.AddTransient<MasterDataPage>();
            serviceCollection.AddTransient<StatisticsPage>();
            serviceCollection.AddTransient<TimeAccountPage>();
            serviceCollection.AddTransient<ActivityPage>();
            serviceCollection.AddTransient<ObjectivePage>();

            using var serviceProvider = serviceCollection.BuildServiceProvider();
            var chronosCore = serviceProvider.GetRequiredService<ChronosCore>();
            chronosCore.Initialize();
            Application.Run(serviceProvider.GetRequiredService<MainView>());
        }

        private static string EnsureAppDataDirectoryExists()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var directoryPath = Path.Combine(appData, "Chronos");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return directoryPath;
        }
    }
}
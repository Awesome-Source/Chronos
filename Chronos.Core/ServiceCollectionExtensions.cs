using Apollo.Core;
using Apollo.Core.Interfaces;
using Apollo.Sqlite;
using Chronos.Core.Contracts.Repositories;
using Chronos.Core.Contracts.Services;
using Chronos.Core.Implementations.Database;
using Chronos.Core.Implementations.Repositories;
using Chronos.Core.Implementations.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Chronos.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void AddChronosCore(this ServiceCollection serviceCollection, string appDataDirectory)
        {
            RegisterDatabaseServices(serviceCollection, appDataDirectory);
            RegisterRepositories(serviceCollection);
            RegisterServices(serviceCollection);            

            serviceCollection.AddSingleton<ChronosCore>();
        }

        private static void RegisterDatabaseServices(ServiceCollection serviceCollection, string appDataDirectory)
        {
            serviceCollection.AddTransient<IDatabaseAccessor, SqliteDataBaseAccessor>();
            serviceCollection.AddSingleton<IPatchInfoRepository, PatchInfoRepository>();
            serviceCollection.AddSingleton<IDatabaseConnectionConfiguration>(new DatabaseConnectionConfiguration(appDataDirectory));
            serviceCollection.AddSingleton<DatabaseInitializer>();
        }

        private static void RegisterServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ITimeAccountService, TimeAccountService>();
            serviceCollection.AddSingleton<IActivityService, ActivityService>();
            serviceCollection.AddSingleton<IObjectiveService, ObjectiveService>();
            serviceCollection.AddSingleton<ITrackingService, TrackingService>();
            serviceCollection.AddSingleton<IStatisticsService, StatisticsService>();
        }

        private static void RegisterRepositories(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ITimeAccountRepository, TimeAccountRepository>();
            serviceCollection.AddSingleton<IActivityRepository, ActivityRepository>();
            serviceCollection.AddSingleton<IObjectiveRepository, ObjectiveRepository>();
            serviceCollection.AddSingleton<ITrackingTargetRepository, TrackingTargetRepository>();
            serviceCollection.AddSingleton<ITrackingDayRepository, TrackingDayRepository>();
            serviceCollection.AddSingleton<ITrackingRecordRepository, TrackingRecordRepository>();
            serviceCollection.AddSingleton<IStatisticsRepository, StatisticsRepository>();
        }
    }
}

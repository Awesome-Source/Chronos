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
        public static void AddChronosCore(this IServiceCollection serviceCollection, string appDataDirectory)
        {
            RegisterPersistentDatabaseServices(serviceCollection, appDataDirectory);
            AddChronosCoreInternal(serviceCollection);
        }

        public static void AddInMemoryChronosCore(this IServiceCollection serviceCollection)
        {
            RegisterInMemoryDatabaseServices(serviceCollection);
            AddChronosCoreInternal(serviceCollection);
        }

        private static void AddChronosCoreInternal(IServiceCollection serviceCollection)
        {
            RegisterRepositories(serviceCollection);
            RegisterServices(serviceCollection);

            serviceCollection.AddSingleton<ChronosCore>();
        }        

        private static void RegisterPersistentDatabaseServices(IServiceCollection serviceCollection, string appDataDirectory)
        {
            serviceCollection.AddTransient<IDatabaseAccessor, SqliteDataBaseAccessor>();
            serviceCollection.AddSingleton<IPatchInfoRepository, PatchInfoRepository>();
            serviceCollection.AddSingleton<IDatabaseConnectionConfiguration>(new DatabaseConnectionConfiguration(appDataDirectory));
            serviceCollection.AddSingleton<DatabaseInitializer>();
        }

        private static void RegisterInMemoryDatabaseServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDatabaseAccessor, SqliteInMemoryDataBaseAccessor>();
            serviceCollection.AddSingleton<IPatchInfoRepository, PatchInfoRepository>();
            serviceCollection.AddSingleton<DatabaseInitializer>();
        }

        private static void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ITimeAccountService, TimeAccountService>();
            serviceCollection.AddSingleton<IActivityService, ActivityService>();
            serviceCollection.AddSingleton<IObjectiveService, ObjectiveService>();
            serviceCollection.AddSingleton<ICategoryService, CategoryService>();
            serviceCollection.AddSingleton<ITrackingService, TrackingService>();
            serviceCollection.AddSingleton<IStatisticsService, StatisticsService>();
        }

        private static void RegisterRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ITimeAccountRepository, TimeAccountRepository>();
            serviceCollection.AddSingleton<IActivityRepository, ActivityRepository>();
            serviceCollection.AddSingleton<IObjectiveRepository, ObjectiveRepository>();
            serviceCollection.AddSingleton<ICategoryRepository, CategoryRepository>();
            serviceCollection.AddSingleton<ITrackingTargetRepository, TrackingTargetRepository>();
            serviceCollection.AddSingleton<ITrackingDayRepository, TrackingDayRepository>();
            serviceCollection.AddSingleton<ITrackingRecordRepository, TrackingRecordRepository>();
            serviceCollection.AddSingleton<IStatisticsRepository, StatisticsRepository>();
        }
    }
}

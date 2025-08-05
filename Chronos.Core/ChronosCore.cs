using Apollo.Core;
using Apollo.Core.Interfaces;
using Chronos.Core.Contracts.Services;
using Chronos.Core.Implementations.Database.Patches;

namespace Chronos.Core
{
    public class ChronosCore
    {
        private readonly DatabaseInitializer _databaseInitializer;

        public ITimeAccountService TimeAccountService { get; }
        public IActivityService ActivityService { get; }
        public IObjectiveService ObjectiveService { get; }
        public ITrackingService TrackingService { get; }
        public IStatisticsService StatisticsService { get; }

        public ChronosCore(ITimeAccountService timeAccountService, IActivityService activityService, IObjectiveService objectiveService, ITrackingService trackingService, IStatisticsService statisticsService, DatabaseInitializer databaseInitializer)
        {
            TimeAccountService = timeAccountService;
            ActivityService = activityService;
            ObjectiveService = objectiveService;
            TrackingService = trackingService;
            StatisticsService = statisticsService;
            _databaseInitializer = databaseInitializer;
        }
        
        public void Initialize()
        {
            var patches = new List<IDatabasePatch>
            {
                new InitialPatch(),
                new DemoDataPatch()
            };

            _databaseInitializer.Run(patches);

            TrackingService.CompleteActiveEntryInPastIfExisting(DateOnly.FromDateTime(DateTime.Now));
        }
    }
}

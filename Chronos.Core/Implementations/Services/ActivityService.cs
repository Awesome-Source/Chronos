using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;
using Chronos.Core.Contracts.Services;

namespace Chronos.Core.Implementations.Services
{
    internal class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityService(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public void Create(string name, int timeAccountId)
        {            
            _activityRepository.Add(name, timeAccountId);
        }

        public IReadOnlyList<Activity> GetAll()
        {
            return _activityRepository.GetValues();
        }

        public void Update(int id, string name, int timeAccountId)
        {
            _activityRepository.Update(id, name, timeAccountId);
        }

        public void Remove(int activityId)
        {
            _activityRepository.Remove(activityId);
        }
    }
}

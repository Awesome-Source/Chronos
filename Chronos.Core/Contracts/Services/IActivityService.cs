using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Services
{
    public interface IActivityService
    {
        void Create(string name, int timeAccountId);
        IReadOnlyList<Activity> GetAll();
        void Update(int id, string name, int timeAccountId);
        void Remove(int activityId);
    }
}

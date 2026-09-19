using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Services
{
    public interface IObjectiveService
    {
        void Create(string name, string description, int categoryId);
        IReadOnlyList<Objective> GetAll();
        void Update(int id, string name, string description, int categoryId, bool isDone);
        void Remove(int objectiveId);
    }
}

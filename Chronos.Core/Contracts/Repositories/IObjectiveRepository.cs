using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Repositories
{
    internal interface IObjectiveRepository
    {
        int Add(string name, string description);
        IReadOnlyList<Objective> GetValues();
        void Remove(int id);
        void Update(int id, string name, string description);
    }
}

using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Repositories
{
    internal interface ICategoryRepository
    {
        int Add(string name);
        IReadOnlyList<Category> GetValues();
        void Remove(int id);
        void Update(int id, string name);
    }
}

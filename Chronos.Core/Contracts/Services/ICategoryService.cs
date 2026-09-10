using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Services
{
    public interface ICategoryService
    {
        void Create(string name);
        IReadOnlyList<Category> GetAll();
        void Update(int id, string name);
        void Remove(int categoryId);
    }
}

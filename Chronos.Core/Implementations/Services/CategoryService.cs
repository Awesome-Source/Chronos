using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;
using Chronos.Core.Contracts.Services;

namespace Chronos.Core.Implementations.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void Create(string name)
        {
            _categoryRepository.Add(name);
        }

        public IReadOnlyList<Category> GetAll()
        {
            return _categoryRepository.GetValues();
        }

        public void Remove(int categoryId)
        {
            _categoryRepository.Remove(categoryId);
        }

        public void Update(int id, string name)
        {
            _categoryRepository.Update(id, name);
        }
    }
}

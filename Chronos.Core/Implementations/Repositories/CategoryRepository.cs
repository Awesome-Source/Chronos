using Apollo.Core.Interfaces;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;

namespace Chronos.Core.Implementations.Repositories
{
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly IDatabaseAccessor _databaseAccessor;

        public CategoryRepository(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public int Add(string name)
        {
            var parameters = new Dictionary<string, object>
            {
                {"NAME", name }
            };

            return _databaseAccessor.ExecuteQuery("INSERT INTO categories (name) VALUES (@NAME) RETURNING id", rp => rp.GetInt("id"), parameters).Single();
        }

        public IReadOnlyList<Category> GetValues()
        {
            return _databaseAccessor.ExecuteQuery("SELECT id, name from categories", ParseCategories);
        }

        private Category ParseCategories(IRowParser parser)
        {
            return new Category(parser.GetInt("id"), parser.GetString("name"));
        }

        public void Remove(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID", id}
            };

            _databaseAccessor.ExecuteNonQuery("DELETE FROM categories WHERE id = @ID", parameters);
        }

        public void Update(int id, string name)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID", id },
                {"NAME", name }
            };

            _databaseAccessor.ExecuteNonQuery("UPDATE categories SET name = @NAME WHERE id = @ID", parameters);
        }
    }
}

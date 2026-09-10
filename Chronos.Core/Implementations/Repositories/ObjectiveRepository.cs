using Apollo.Core.Interfaces;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;

namespace Chronos.Core.Implementations.Repositories
{
    internal class ObjectiveRepository : IObjectiveRepository
    {
        private readonly IDatabaseAccessor _databaseAccessor;

        public ObjectiveRepository(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public int Add(string name, string description, int categoryId)
        {
            var parameters = new Dictionary<string, object>
            {
                {"NAME", name },
                {"DESCRIPTION", description},
                {"CATEGORY", categoryId}
            };

            return _databaseAccessor.ExecuteQuery("INSERT INTO objectives (name, description, category_id) VALUES (@NAME, @DESCRIPTION, @CATEGORY) RETURNING id", rp => rp.GetInt("id"), parameters).Single();
        }

        public IReadOnlyList<Objective> GetValues()
        {
            return _databaseAccessor.ExecuteQuery("SELECT o.id, o.name, o.description, c.name AS category_name, c.id AS category_id FROM objectives o INNER JOIN categories c ON o.category_id = c.id", ParseObjectives);
        }

        private Objective ParseObjectives(IRowParser parser)
        {
            return new Objective(parser.GetInt("id"), parser.GetString("name"), parser.GetString("description"), parser.GetString("category_name"), parser.GetInt("category_id"));
        }

        public void Remove(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID", id}
            };

            _databaseAccessor.ExecuteNonQuery("DELETE FROM objectives WHERE id = @ID", parameters);
        }

        public void Update(int id, string name, string description, int categoryId)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID", id },
                {"NAME", name },
                {"DESCRIPTION", description },
                {"CATEGORY", categoryId}
            };

            _databaseAccessor.ExecuteNonQuery("UPDATE objectives SET name = @NAME, description = @DESCRIPTION, category_id = @CATEGORY WHERE id = @ID", parameters);
        }
    }
}

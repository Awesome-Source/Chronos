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

        public int Add(string name, string description)
        {
            var parameters = new Dictionary<string, object>
            {
                {"NAME", name },
                {"DESCRIPTION", description}
            };

            return _databaseAccessor.ExecuteQuery("INSERT INTO objectives (name, description) VALUES (@NAME, @DESCRIPTION) RETURNING id", rp => rp.GetInt("id"), parameters).Single();
        }

        public IReadOnlyList<Objective> GetValues()
        {
            return _databaseAccessor.ExecuteQuery("SELECT id, name, description from objectives", ParseObjectives);
        }

        private Objective ParseObjectives(IRowParser parser)
        {
            return new Objective(parser.GetInt("id"), parser.GetString("name"), parser.GetString("description"));
        }

        public void Remove(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID", id}
            };

            _databaseAccessor.ExecuteNonQuery("DELETE FROM objectives WHERE id = @ID", parameters);
        }

        public void Update(int id, string name, string description)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID", id },
                {"NAME", name },
                {"DESCRIPTION", description }
            };

            _databaseAccessor.ExecuteNonQuery("UPDATE objectives SET name = @NAME, description = @DESCRIPTION WHERE id = @ID", parameters);
        }
    }
}

using Apollo.Core.Interfaces;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;

namespace Chronos.Core.Implementations.Repositories
{
    internal class ActivityRepository : IActivityRepository
    {
        private readonly IDatabaseAccessor _databaseAccessor;

        public ActivityRepository(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public int Add(string name, int timeAccountId)
        {
            var parameters = new Dictionary<string, object>
            {
                {"NAME", name },
                {"TIMEACCOUNT", timeAccountId }
            };

            return _databaseAccessor.ExecuteQuery("INSERT INTO activities (name, time_account_id) VALUES (@NAME, @TIMEACCOUNT) RETURNING id", rp => rp.GetInt("id"), parameters).Single();
        }

        public IReadOnlyList<Activity> GetValues()
        {
            return _databaseAccessor.ExecuteQuery("SELECT a.id, a.name, a.time_account_id, ta.name AS time_account_name from activities a INNER JOIN time_accounts ta ON a.time_account_id = ta.id", ParseActivities);
        }

        private Activity ParseActivities(IRowParser parser)
        {
            return new Activity(parser.GetInt("id"), parser.GetString("name"), parser.GetInt("time_account_id"), parser.GetString("time_account_name"));
        }

        public void Remove(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID", id}
            };

            _databaseAccessor.ExecuteNonQuery("DELETE FROM activities WHERE id = @ID", parameters);
        }

        public void Update(int id, string name, int timeAccountId)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID", id },
                {"NAME", name },
                {"TIMEACCOUNT", timeAccountId }
            };

            _databaseAccessor.ExecuteNonQuery("UPDATE activities SET name = @NAME, time_account_id = @TIMEACCOUNT WHERE id = @ID", parameters);
        }
    }
}

using System.Drawing;
using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Repositories
{
    internal interface IActivityRepository
    {
        int Add(string name, int timeAccountId);
        IReadOnlyList<Activity> GetValues();
        void Remove(int id);
        void Update(int id, string name, int timeAccountId);
    }
}

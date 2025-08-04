using System.Drawing;
using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Repositories
{
    internal interface ITimeAccountRepository
    {
        int Add(string name, Color color, bool isWorkTime);
        IReadOnlyList<TimeAccount> GetValues();
        void Remove(int id);
        void Update(int id, string name, Color color, bool isWorkTime);
    }
}

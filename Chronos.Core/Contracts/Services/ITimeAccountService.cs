using System.Drawing;
using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Services
{
    public interface ITimeAccountService
    {
        void Create(string name, Color color, bool isWorkTime);
        IReadOnlyList<TimeAccount> GetAll();
        void Update(int id, string name, Color color, bool isWorkTime);
        void Remove(int timeAccountId);
    }
}

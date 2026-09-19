using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Services
{
    public interface ITimeAccountService
    {
        void Create(string name, string colorHex, bool isWorkTime);
        IReadOnlyList<TimeAccount> GetAll();
        void Update(int id, string name, string colorHex, bool isWorkTime);
        void Remove(int timeAccountId);
    }
}

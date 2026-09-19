using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;
using Chronos.Core.Contracts.Services;

namespace Chronos.Core.Implementations.Services
{
    internal class TimeAccountService : ITimeAccountService
    {
        private readonly ITimeAccountRepository _timeAccountRepository;

        public TimeAccountService(ITimeAccountRepository timeAccountRepository)
        {
            _timeAccountRepository = timeAccountRepository;
        }

        public void Create(string name, string colorHex, bool isWorkTime)
        {
            _timeAccountRepository.Add(name, colorHex, isWorkTime);
        }

        public IReadOnlyList<TimeAccount> GetAll()
        {
            return _timeAccountRepository.GetValues();
        }

        public void Update(int id, string name, string colorHex, bool isWorkTime)
        {
            _timeAccountRepository.Update(id, name, colorHex, isWorkTime);
        }

        public void Remove(int timeAccountId)
        {
            _timeAccountRepository.Remove(timeAccountId);
        }
    }
}

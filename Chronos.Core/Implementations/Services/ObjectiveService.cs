using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;
using Chronos.Core.Contracts.Services;

namespace Chronos.Core.Implementations.Services
{
    internal class ObjectiveService : IObjectiveService
    {
        private readonly IObjectiveRepository _objectiveRepository;

        public ObjectiveService(IObjectiveRepository objectiveRepository)
        {
            _objectiveRepository = objectiveRepository;
        }

        public int Create(string name, string description, int categoryId)
        {
            return _objectiveRepository.Add(name, description, categoryId);
        }

        public IReadOnlyList<Objective> GetAll()
        {
            return _objectiveRepository.GetValues();
        }

        public void Update(int id, string name, string description, int categoryId, bool isDone)
        {
            _objectiveRepository.Update(id, name, description, categoryId, isDone);
        }

        public void Remove(int objectiveId)
        {
            _objectiveRepository.Remove(objectiveId);
        }
    }
}

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

        public void Create(string name, string description, int categoryId)
        {            
            _objectiveRepository.Add(name, description, categoryId);
        }

        public IReadOnlyList<Objective> GetAll()
        {
            return _objectiveRepository.GetValues();
        }

        public void Update(int id, string name, string description, int categoryId)
        {
            _objectiveRepository.Update(id, name, description, categoryId);
        }

        public void Remove(int objectiveId)
        {
            _objectiveRepository.Remove(objectiveId);
        }
    }
}

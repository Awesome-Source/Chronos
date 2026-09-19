using Chronos.Asp.Contracts;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chronos.Asp.Controllers
{
    [ApiController]
    [Route("api/objectives")]
    public class ObjectivesController : ControllerBase
    {
        private readonly IObjectiveService _objectiveService;

        public ObjectivesController(IObjectiveService objectiveService)
        {
            _objectiveService = objectiveService;
        }

        [HttpGet]
        public ActionResult<IReadOnlyList<Objective>> GetAll()
        {
            return Ok(_objectiveService.GetAll());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateObjectiveRequest request)
        {
            _objectiveService.Create(request.Name, request.Description, request.CategoryId);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateObjectiveRequest request)
        {
            _objectiveService.Update(id, request.Name, request.Description, request.CategoryId);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Remove(int id)
        {
            _objectiveService.Remove(id);
            return NoContent();
        }
    }
}

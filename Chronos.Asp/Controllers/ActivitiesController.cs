using Chronos.Asp.Contracts;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chronos.Asp.Controllers
{
    [ApiController]
    [Route("api/activities")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet]
        public ActionResult<IReadOnlyList<Activity>> GetAll()
        {
            return Ok(_activityService.GetAll());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateActivityRequest request)
        {
            _activityService.Create(request.Name, request.TimeAccountId);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateActivityRequest request)
        {
            _activityService.Update(id, request.Name, request.TimeAccountId);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Remove(int id)
        {
            _activityService.Remove(id);
            return NoContent();
        }
    }
}

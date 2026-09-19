using Chronos.Asp.Contracts;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chronos.Asp.Controllers
{
    [ApiController]
    [Route("api/timeaccounts")]
    public class TimeAccountsController : ControllerBase
    {
        private readonly ITimeAccountService _timeAccountService;

        public TimeAccountsController(ITimeAccountService timeAccountService)
        {
            _timeAccountService = timeAccountService;
        }

        [HttpGet]
        public ActionResult<IReadOnlyList<TimeAccount>> GetAll()
        {
            return Ok(_timeAccountService.GetAll());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateTimeAccountRequest request)
        {
            _timeAccountService.Create(request.Name, request.ColorHex, request.IsWorkTime);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateTimeAccountRequest request)
        {
            _timeAccountService.Update(id, request.Name, request.ColorHex, request.IsWorkTime);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Remove(int id)
        {
            _timeAccountService.Remove(id);
            return NoContent();
        }
    }
}

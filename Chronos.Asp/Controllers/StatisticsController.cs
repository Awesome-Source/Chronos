using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chronos.Asp.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("productive-time-account-balances")]
        public ActionResult<IReadOnlyList<RelativeTimeAccountBalance>> GetProductiveTimeAccountBalances()
        {
            return Ok(_statisticsService.RetrieveAllProductiveTimeAccountBalances());
        }
    }
}

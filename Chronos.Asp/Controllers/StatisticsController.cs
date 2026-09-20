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

        [HttpGet("productive-time-account-balances/current-week")]
        public ActionResult<IReadOnlyList<RelativeTimeAccountBalance>> GetCurrentWeekProductiveTimeAccountBalances()
        {
            return Ok(_statisticsService.RetrieveCurrentWeekProductiveTimeAccountBalances(DateOnly.FromDateTime(DateTime.Now)));
        }

        [HttpGet("daily-time-account-durations/current-week")]
        public ActionResult<IReadOnlyList<DailyTimeAccountBreakdown>> GetCurrentWeekDailyBreakdown()
        {
            return Ok(_statisticsService.RetrieveCurrentWeekDailyBreakdown(DateOnly.FromDateTime(DateTime.Now)));
        }
    }
}

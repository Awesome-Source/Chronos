using Chronos.Asp.Contracts;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chronos.Asp.Controllers
{
    [ApiController]
    [Route("api/tracking")]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingService _trackingService;

        public TrackingController(ITrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        [HttpGet("targets/today")]
        public ActionResult<IReadOnlyList<EvaluatedTrackingTarget>> GetTodaysTargets()
        {
            return Ok(_trackingService.GetEvaluatedTrackingTargetsForDay(DateTime.Now));
        }

        [HttpPost("targets")]
        public ActionResult<CreateTrackingTargetResponse> CreateTarget([FromBody] CreateTrackingTargetRequest request)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var id = _trackingService.CreateTarget(today, request.ActivityId, request.ObjectiveId, request.IsPlannedActivity);
            return Ok(new CreateTrackingTargetResponse(id));
        }

        [HttpPost("targets/{id:int}/start")]
        public IActionResult StartTracking(int id, [FromBody] StartTrackingRequest request)
        {
            _trackingService.StartTracking(id, request.Start);
            return NoContent();
        }

        [HttpPost("stop")]
        public IActionResult StopTracking([FromBody] StopTrackingRequest request)
        {
            _trackingService.StopTracking(request.End);
            return NoContent();
        }

        [HttpGet("records")]
        public ActionResult<IReadOnlyList<TrackingRecord>> GetRecordsForDay([FromQuery] DateOnly date)
        {
            return Ok(_trackingService.GetRecordsForDay(date));
        }

        [HttpPut("records/{id:int}")]
        public IActionResult UpdateRecord(int id, [FromBody] UpdateTrackingRecordRequest request)
        {
            _trackingService.UpdateRecord(id, request.Start, request.End);
            return NoContent();
        }

        [HttpGet("timesheet")]
        public ActionResult<IReadOnlyList<EvaluatedTrackingTarget>> GetTimeSheetForDay([FromQuery] DateOnly date)
        {
            return Ok(_trackingService.GetTimeSheetForDay(date));
        }

        [HttpGet("latest-day-before")]
        public ActionResult<LatestTrackingDayResponse> GetLatestDayBefore([FromQuery] DateOnly date)
        {
            if (!_trackingService.TryGetLatestTrackingDayBefore(date, out var latestDateBefore))
            {
                return NotFound();
            }

            return Ok(new LatestTrackingDayResponse(latestDateBefore));
        }
    }
}

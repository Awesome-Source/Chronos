namespace Chronos.Asp.Contracts
{
    public record CreateTimeAccountRequest(string Name, string ColorHex, bool IsWorkTime);
    public record UpdateTimeAccountRequest(string Name, string ColorHex, bool IsWorkTime);

    public record CreateActivityRequest(string Name, int TimeAccountId);
    public record UpdateActivityRequest(string Name, int TimeAccountId);

    public record CreateObjectiveRequest(string Name, string Description, int CategoryId);
    public record CreateObjectiveResponse(int Id);
    public record UpdateObjectiveRequest(string Name, string Description, int CategoryId, bool IsDone);

    public record CreateCategoryRequest(string Name);
    public record UpdateCategoryRequest(string Name);

    public record CreateTrackingTargetRequest(int ActivityId, int ObjectiveId, bool IsPlannedActivity);
    public record CreateTrackingTargetResponse(int Id);
    public record StartTrackingRequest(TimeOnly Start);
    public record StopTrackingRequest(TimeOnly End);
    public record UpdateTrackingRecordRequest(TimeOnly Start, TimeOnly End);
    public record LatestTrackingDayResponse(DateOnly Date);
}

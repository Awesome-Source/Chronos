using System.Diagnostics.CodeAnalysis;
using Apollo.Core.Interfaces;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;
using Chronos.Core.Extensions;

namespace Chronos.Core.Implementations.Repositories
{
    internal class TrackingTargetRepository : ITrackingTargetRepository
    {
        private readonly IDatabaseAccessor _databaseAccessor;

        public TrackingTargetRepository(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public int Create(int trackingDayId, int activityId, int objectiveId, bool isPlannedActivity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"DAY_ID", trackingDayId },
                {"ACTIVITY", activityId },
                {"OBJECTIVE", objectiveId },
                {"PLANNED", isPlannedActivity.ToIntRepresentation() }
            };

            return _databaseAccessor.ExecuteQuery("INSERT INTO tracking_targets (tracking_day_id, activity_id, objective_id, is_planned) VALUES (@DAY_ID, @ACTIVITY, @OBJECTIVE, @PLANNED) RETURNING id", rp => rp.GetInt("id"), parameters).Single();
        }

        public IReadOnlyList<EvaluatedTrackingTarget> GetEvaluatedTrackingTargetsForDay(int trackingDayId)
        {
            return  RetrieveEvaluatedTrackingTargetsFromCompletedRecords(trackingDayId);
        }

        public IReadOnlyList<EvaluatedTrackingTarget> GetEvaluatedTrackingTargetsForDay(int trackingDayId, TimeOnly evaluationTime)
        {
            var evaluatedTrackingTargets = RetrieveEvaluatedTrackingTargetsFromCompletedRecords(trackingDayId);
            if (!TryRetrieveActiveEvaluatedTrackingTarget(evaluationTime, out var activeEvaluatedTrackingRecord))
            {
                return evaluatedTrackingTargets;
            }

            return MergeEvaluatedTrackingTargets(evaluatedTrackingTargets, activeEvaluatedTrackingRecord);
        }

        private static IReadOnlyList<EvaluatedTrackingTarget> MergeEvaluatedTrackingTargets(List<EvaluatedTrackingTarget> evaluatedTrackingTargets, EvaluatedTrackingTarget activeEvaluatedTrackingRecord)
        {
            var existingEvaluatedTrackingRecord = evaluatedTrackingTargets.FirstOrDefault(ett => ett.InternalId == activeEvaluatedTrackingRecord.InternalId);

            if (existingEvaluatedTrackingRecord == null)
            {
                evaluatedTrackingTargets.Add(activeEvaluatedTrackingRecord);
                return evaluatedTrackingTargets;
            }

            existingEvaluatedTrackingRecord.AccumulatedTime += activeEvaluatedTrackingRecord.AccumulatedTime;
            existingEvaluatedTrackingRecord.IsActive = true;
            return evaluatedTrackingTargets;
        }

        private bool TryRetrieveActiveEvaluatedTrackingTarget(TimeOnly evaluationTime, [NotNullWhen(true)] out EvaluatedTrackingTarget? evaluatedTrackingTarget)
        {
            var sql = @"SELECT 
                            atr.tracking_target_id AS id, 
                            a.name AS activity_name, 
                            o.name AS objective_name,
                            ta.name AS time_account_name,
                            tt.is_planned,
                            (@END_TIME - atr.start_time) AS accumulated_duration
                        FROM active_tracking_record atr
                        INNER JOIN tracking_targets tt ON atr.tracking_target_id = tt.id
                        INNER JOIN activities a ON tt.activity_id = a.id 
                        INNER JOIN time_accounts ta ON a.time_account_id = ta.id 
                        INNER JOIN objectives o ON tt.objective_id = o.id
                            ";

            var parameters = new Dictionary<string, object>
            {
                {"END_TIME", evaluationTime.ToSecondsSinceMidnight() }
            };

            var evaluatedTrackingTargets = _databaseAccessor.ExecuteQuery(sql, rp => ParseEvaluatedTrackingTarget(rp, true), parameters);
            if(evaluatedTrackingTargets.Count == 0)
            {
                evaluatedTrackingTarget = null;
                return false;
            }

            evaluatedTrackingTarget = evaluatedTrackingTargets.Single();
            return true;
        }

        private List<EvaluatedTrackingTarget> RetrieveEvaluatedTrackingTargetsFromCompletedRecords(int trackingDayId)
        {
            var parameters = new Dictionary<string, object>
            {
                {"DAY_ID", trackingDayId }
            };

            var sql = @"SELECT 
                            tt.id, 
                            a.name AS activity_name, 
                            o.name AS objective_name,
                            ta.name AS time_account_name,
                            tt.is_planned, 
                            (SELECT SUM(tr.duration) FROM tracking_records tr WHERE tr.tracking_target_id = tt.id) AS accumulated_duration
                        FROM tracking_targets tt 
                        INNER JOIN activities a ON tt.activity_id = a.id 
                        INNER JOIN time_accounts ta ON a.time_account_id = ta.id 
                        INNER JOIN objectives o ON tt.objective_id = o.id 
                        WHERE tt.tracking_day_id = @DAY_ID";

            var evaluatedTrackingTargets = _databaseAccessor.ExecuteQuery(sql, rp => ParseEvaluatedTrackingTarget(rp, false), parameters);
            return evaluatedTrackingTargets;
        }

        private EvaluatedTrackingTarget ParseEvaluatedTrackingTarget(IRowParser parser, bool isActive)
        {
            return new EvaluatedTrackingTarget(
                parser.GetInt("id"), 
                parser.GetString("activity_name"), 
                parser.GetString("time_account_name"), 
                parser.GetString("objective_name"), 
                parser.GetInt("is_planned").ToBoolFromIntRepresentation(), 
                TimeSpan.FromSeconds(parser.GetNullableInt("accumulated_duration") ?? 0),
                isActive);
        }
    }
}

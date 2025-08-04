using Apollo.Core.Interfaces;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;
using Chronos.Core.Extensions;

namespace Chronos.Core.Implementations.Repositories
{
    internal class TrackingRecordRepository : ITrackingRecordRepository
    {
        private readonly IDatabaseAccessor _databaseAccessor;

        public TrackingRecordRepository(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public void CompleteActiveRecord(TimeOnly end)
        {
            _databaseAccessor.ExecuteInTransaction(withinTransactionExecutor =>
            {
                var activeTrackingRecords = QueryActiveTrackingRecords(withinTransactionExecutor);

                if (activeTrackingRecords.Count == 0)
                {
                    return;
                }

                var activeTrackingRecord = activeTrackingRecords.Single();

                InsertTrackingRecord(withinTransactionExecutor, activeTrackingRecord.TrackingTargetId, activeTrackingRecord.Start, end);
                ClearActiveTrackingRecord(withinTransactionExecutor);
            });
        }

        public void CompleteActiveRecordAndStartNew(int targetId, TimeOnly newStart)
        {
            _databaseAccessor.ExecuteInTransaction(withinTransactionExecutor =>
            {
                var activeTrackingRecords = QueryActiveTrackingRecords(withinTransactionExecutor);

                if (activeTrackingRecords.Count == 0)
                {
                    InsertActiveTrackingRecord(targetId, newStart, withinTransactionExecutor);
                    return;
                }

                var activeTrackingRecord = activeTrackingRecords.Single();

                InsertTrackingRecord(withinTransactionExecutor, activeTrackingRecord.TrackingTargetId, activeTrackingRecord.Start, newStart);
                ReplaceActiveTrackingRecord(targetId, newStart, withinTransactionExecutor);
            });
        }

        private List<ActiveTrackingRecord> QueryActiveTrackingRecords(IWithinTransactionExecutor withinTransactionExecutor)
        {
            return withinTransactionExecutor.ExecuteQuery("SELECT tracking_target_id, start_time FROM active_tracking_record", ParseActiveTrackingRecord);
        }

        private List<ActiveTrackingRecord> QueryActiveTrackingRecordsInPast(IWithinTransactionExecutor withinTransactionExecutor, int currentTrackingDayId)
        {
            var parameters = new Dictionary<string, object>
            {
                {"DAY_ID", currentTrackingDayId }
            };

            var sql = @"SELECT atr.tracking_target_id, atr.start_time FROM active_tracking_record atr INNER JOIN tracking_targets tt ON atr.tracking_target_id = tt.id WHERE tt.tracking_day_id <> @DAY_ID";

            return withinTransactionExecutor.ExecuteQuery(sql, ParseActiveTrackingRecord, parameters);
        }

        private static void ReplaceActiveTrackingRecord(int targetId, TimeOnly newStart, IWithinTransactionExecutor withinTransactionExecutor)
        {
            ClearActiveTrackingRecord(withinTransactionExecutor);

            var parameters = new Dictionary<string, object>
                {
                    {"TARGET_ID", targetId },
                    {"START", newStart.ToSecondsSinceMidnight() }
                };

            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO active_tracking_record (tracking_target_id, start_time) VALUES (@TARGET_ID, @START)", parameters);
        }

        private static void ClearActiveTrackingRecord(IWithinTransactionExecutor withinTransactionExecutor)
        {
            withinTransactionExecutor.ExecuteNonQuery("DELETE FROM active_tracking_record");
        }

        private static void InsertTrackingRecord(IWithinTransactionExecutor withinTransactionExecutor, int targetId, TimeOnly start, TimeOnly end)
        {
            var parameters = new Dictionary<string, object>
            {
                {"TARGET_ID", targetId },
                {"START", start.ToSecondsSinceMidnight() },
                {"END", end.ToSecondsSinceMidnight() },
                {"DURATION", (int) (end - start).TotalSeconds }
            };

            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO tracking_records (tracking_target_id, start_time, end_time, duration) VALUES (@TARGET_ID, @START, @END, @DURATION)", parameters);
        }

        private static void InsertActiveTrackingRecord(int targetId, TimeOnly start, IWithinTransactionExecutor withinTransactionExecutor)
        {
            var parameters = new Dictionary<string, object>
            {
                {"TARGET_ID", targetId },
                {"START", start.ToSecondsSinceMidnight() }
            };

            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO active_tracking_record (tracking_target_id, start_time) VALUES (@TARGET_ID, @START)", parameters);
        }

        private ActiveTrackingRecord ParseActiveTrackingRecord(IRowParser parser)
        {
            return new ActiveTrackingRecord(parser.GetInt("tracking_target_id"), parser.GetInt("start_time").FromSecondsSinceMidnight());
        }

        public IReadOnlyList<TrackingRecord> GetRecordsForDay(int trackingDayId)
        {
            var parameters = new Dictionary<string, object>
            {
                {"DAY_ID", trackingDayId }
            };

            var sql = @"SELECT 
                            tr.id,
                            tr.start_time, 
                            tr.end_time, 
                            tr.duration,
                            a.id AS activity_id,
                            a.name AS activity_name,
                            o.id AS objective_id,
                            o.name AS objective_name,
                            0 AS is_active
                        FROM tracking_records tr 
                        INNER JOIN tracking_targets tt ON tr.tracking_target_id = tt.id
                        INNER JOIN activities a ON tt.activity_id = a.id 
                        INNER JOIN objectives o ON tt.objective_id = o.id
                        WHERE tt.tracking_day_id = @DAY_ID
                        UNION ALL
                        SELECT 
                            atr.id,
                            atr.start_time, 
                            0, 
                            0,
                            aa.id AS activity_id,
                            aa.name AS activity_name,
                            ao.id AS objective_id,
                            ao.name AS objective_name,
                            1 AS is_active
                        FROM active_tracking_record atr 
                        INNER JOIN tracking_targets att ON atr.tracking_target_id = att.id
                        INNER JOIN activities aa ON att.activity_id = aa.id 
                        INNER JOIN objectives ao ON att.objective_id = ao.id
                        WHERE att.tracking_day_id = @DAY_ID";

            return _databaseAccessor.ExecuteQuery(sql, ParseRecords, parameters);
        }

        private TrackingRecord ParseRecords(IRowParser parser)
        {
            return new TrackingRecord(
                parser.GetInt("id"),
                parser.GetInt("start_time").FromSecondsSinceMidnight(),
                parser.GetInt("end_time").FromSecondsSinceMidnight(),
                TimeSpan.FromSeconds(parser.GetInt("duration")),
                parser.GetInt("activity_id"),
                parser.GetString("activity_name"),
                parser.GetInt("objective_id"),
                parser.GetString("objective_name"),
                parser.GetInt("is_active").ToBoolFromIntRepresentation()
                );
        }

        public void UpdateRecord(int recordId, TimeOnly start, TimeOnly end)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID", recordId },
                {"START", start.ToSecondsSinceMidnight() },
                {"END", end.ToSecondsSinceMidnight() },
                {"DURATION", (int) (end - start).TotalSeconds }
            };

            _databaseAccessor.ExecuteNonQuery("UPDATE tracking_records SET start_time = @START, end_time = @END, duration = @DURATION WHERE id = @ID", parameters);
        }

        public void CompleteActiveEntryInPastIfExisting(int currentTrackingDayId)
        {
            _databaseAccessor.ExecuteInTransaction(withinTransactionExecutor =>
            {
                var activeTrackingRecords = QueryActiveTrackingRecordsInPast(withinTransactionExecutor, currentTrackingDayId);

                if (activeTrackingRecords.Count == 0)
                {
                    return;
                }

                var activeTrackingRecord = activeTrackingRecords.Single();

                InsertTrackingRecord(withinTransactionExecutor, activeTrackingRecord.TrackingTargetId, activeTrackingRecord.Start, new TimeOnly(23, 59, 59));
                ClearActiveTrackingRecord(withinTransactionExecutor);
            });
        }
    }
}

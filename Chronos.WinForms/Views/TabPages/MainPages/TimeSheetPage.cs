using System.ComponentModel;
using Chronos.Core;
using Chronos.Core.Contracts.DataObjects;
using Chronos.WinForms;
using Chronos.WinForms.DataObjects;

namespace Chronos.Views.TabPages
{
    public partial class TimeSheetPage : UserControl
    {
        private BindingList<TrackingTargetGridEntry> _timeSheetEntries = new();
        private BindingList<TimeAccountSummaryGridEntry> _timeAccountSummaryGridEntries = new();
        private BindingList<ActivitySummaryGridEntry> _activitySummaryGridEntries = new();
        private BindingList<ObjectiveSummaryGridEntry> _objectiveSummaryGridEntries = new();
        private BindingList<DayStatisticsGridEntry> _dayStatisticsGridEntries = new();
        private readonly ChronosCore _chronosCore;

        public TimeSheetPage(ChronosCore chronosCore)
        {
            InitializeComponent();
            _chronosCore = chronosCore;
            _dateTimePicker.Value = DateTime.Today.AddDays(-1); 
            _gridTimeSheet.BindDataSource(_timeSheetEntries);
            _gridTimeSheet.GridView.CellFormatting += GridView_CellFormatting;

            _gridTimeAccountSheet.BindDataSource(_timeAccountSummaryGridEntries);
            _gridTimeAccountSheet.GridView.CellFormatting += GridView_CellFormatting;

            _gridActivitySheet.BindDataSource(_activitySummaryGridEntries);
            _gridActivitySheet.GridView.CellFormatting += GridView_CellFormatting;

            _gridObjectiveSheet.BindDataSource(_objectiveSummaryGridEntries);
            _gridObjectiveSheet.GridView.CellFormatting += GridView_CellFormatting;

            _gridDayStatistics.BindDataSource(_dayStatisticsGridEntries);
            _gridDayStatistics.GridView.CellFormatting += GridView_CellFormatting;
            RefreshView();
        }

        private void GridView_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is TimeOnly timeOnly)
            {
                e.Value = $"{timeOnly.Hour}:{timeOnly.Minute}:{timeOnly.Second}";
            }
        }

        private void RefreshView()
        {
            _timeSheetEntries.Clear();
            _timeAccountSummaryGridEntries.Clear();
            _activitySummaryGridEntries.Clear();
            _objectiveSummaryGridEntries.Clear();
            _dayStatisticsGridEntries.Clear();
            var selectedDate = DateOnly.FromDateTime(_dateTimePicker.Value);
            var isToday = selectedDate == DateOnly.FromDateTime(DateTime.Today);

            if(isToday)
            {
                return;
            }

            IReadOnlyList<EvaluatedTrackingTarget> evaluatedTrackingTargets;

            try
            {
                evaluatedTrackingTargets = _chronosCore.TrackingService.GetTimeSheetForDay(selectedDate);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not retrieve time sheet data.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            _timeSheetEntries.AddRange(evaluatedTrackingTargets.Select(ett => new TrackingTargetGridEntry(ett)));

            var timeAccountSummaryGridEntries = evaluatedTrackingTargets
                .GroupBy(ett => ett.TimeAccountName) //TODO group by id
                .Select(g => new TimeAccountSummaryGridEntry(g.Key, TimeSpan.FromSeconds(g.Select(ett => ett.AccumulatedTime.TotalSeconds).Sum())));

            _timeAccountSummaryGridEntries.AddRange(timeAccountSummaryGridEntries);

            var activitySummaryGridEntries = evaluatedTrackingTargets
                .GroupBy(ett => ett.ActivityName) //TODO group by id
                .Select(g => new ActivitySummaryGridEntry(g.Key, TimeSpan.FromSeconds(g.Select(ett => ett.AccumulatedTime.TotalSeconds).Sum())));

            _activitySummaryGridEntries.AddRange(activitySummaryGridEntries);

            var objectiveSummaryGridEntries = evaluatedTrackingTargets
                .GroupBy(ett => ett.ObjectiveName) //TODO group by id
                .Select(g => new ObjectiveSummaryGridEntry(g.Key, TimeSpan.FromSeconds(g.Select(ett => ett.AccumulatedTime.TotalSeconds).Sum())));

            _objectiveSummaryGridEntries.AddRange(objectiveSummaryGridEntries);

            var totalTime = TimeSpan.FromSeconds(evaluatedTrackingTargets.Sum(ett => ett.AccumulatedTime.TotalSeconds));
            var plannedTime = TimeSpan.FromSeconds(evaluatedTrackingTargets.Where(ett => ett.IsPlannedActivity).Sum(ett => ett.AccumulatedTime.TotalSeconds));
            var unplannedTime = TimeSpan.FromSeconds(evaluatedTrackingTargets.Where(ett => !ett.IsPlannedActivity).Sum(ett => ett.AccumulatedTime.TotalSeconds));
            //TODO productive time

            _dayStatisticsGridEntries.Add(new DayStatisticsGridEntry("Total", totalTime));
            _dayStatisticsGridEntries.Add(new DayStatisticsGridEntry("Planned", plannedTime));
            _dayStatisticsGridEntries.Add(new DayStatisticsGridEntry("Unplanned", unplannedTime));
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                return;
            }

            RefreshView();
        }

        private void TimeSheetPage_VisibleChanged(object sender, EventArgs e)
        {
            RefreshView();
        }
    }
}

using System.ComponentModel;
using Chronos.Core;
using Chronos.Views.Dialogs;
using Chronos.WinForms.DataObjects;
using Chronos.WinForms.Views.Common;

namespace Chronos.Views.TabPages
{
    public partial class TrackingPage : UserControl
    {
        private readonly ChronosCore _chronosCore;
        private readonly BindingList<TrackingTargetGridEntry> _trackingTargetGridEntries = new BindingList<TrackingTargetGridEntry>();

        public TrackingPage(ChronosCore chronosCore)
        {
            InitializeComponent();
            _chronosCore = chronosCore;

            _gridTrackingTargets.GridView.CellFormatting += GridView_CellFormatting;
            _gridTrackingTargets.BindDataSource(_trackingTargetGridEntries);
            
            RefreshView();
            StartTimer();
        }

        private void StartTimer()
        {
            var timer = new System.Timers.Timer(10 * 1000);
            timer.Elapsed += Timer_Tick;
            timer.SynchronizingObject = null;
            timer.Enabled = true;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            BeginInvoke(RefreshView);
        }

        private void GridView_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            var gridEntry = (TrackingTargetGridEntry)_gridTrackingTargets.GridView.Rows[e.RowIndex].DataBoundItem;

            e.CellStyle!.BackColor = gridEntry.IsActive
                ? Color.LimeGreen
                : Color.Silver;
        }

        private void RefreshView()
        {
            _trackingTargetGridEntries.Clear();
            var evaluatedTrackingTargets = _chronosCore.TrackingService.GetEvaluatedTrackingTargetsForDay(DateTime.Now);

            foreach (var trackingTarget in evaluatedTrackingTargets)
            {
                _trackingTargetGridEntries.Add(new TrackingTargetGridEntry(trackingTarget));
            }

            _labelTotalTime.Text = $"Total time: {TimeSpan.FromSeconds(_trackingTargetGridEntries.Sum(e => e.AccumulatedTime.TotalSeconds))}";
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridTrackingTargets.GridView, out TrackingTargetGridEntry? trackingTargetGridEntry))
            {
                return;
            }

            _chronosCore.TrackingService.StartTracking(trackingTargetGridEntry.TrackingTargetId, TimeOnly.FromDateTime(DateTime.Now));
            RefreshView();
        }

        private void ButtonNew_Click(object sender, EventArgs e)
        {
            var activities = _chronosCore.ActivityService.GetAll();
            var objectives = _chronosCore.ObjectiveService.GetAll();
            var dialog = new StartActivityDialog(activities, objectives);
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                var targetId = _chronosCore.TrackingService.CreateTarget(DateOnly.FromDateTime(DateTime.Now), dialog.ActivityId, dialog.ObjectiveId, dialog.IsPlannedActivity);
                _chronosCore.TrackingService.StartTracking(targetId, TimeOnly.FromDateTime(DateTime.Now));
                RefreshView();
            }
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            _chronosCore.TrackingService.StopTracking(TimeOnly.FromDateTime(DateTime.Now));
            RefreshView();
        }
    }
}

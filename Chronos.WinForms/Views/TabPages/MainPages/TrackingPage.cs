using System.ComponentModel;
using Chronos.Core;
using Chronos.Core.Contracts.DataObjects;
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
            IReadOnlyList<EvaluatedTrackingTarget> evaluatedTrackingTargets;

            try
            {
                evaluatedTrackingTargets = _chronosCore.TrackingService.GetEvaluatedTrackingTargetsForDay(DateTime.Now);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not retrieve tracking targets.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            try
            {
                _chronosCore.TrackingService.StartTracking(trackingTargetGridEntry.TrackingTargetId, TimeOnly.FromDateTime(DateTime.Now));
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not start tracking.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }

        private void ButtonNew_Click(object sender, EventArgs e)
        {
            IReadOnlyList<Activity> activities;
            IReadOnlyList<Objective> objectives;

            try
            {
                activities = _chronosCore.ActivityService.GetAll();
                 objectives = _chronosCore.ObjectiveService.GetAll();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not retrieve activities or objectives.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var dialog = new StartActivityDialog(activities, objectives);
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            try
            {
                var targetId = _chronosCore.TrackingService.CreateTarget(DateOnly.FromDateTime(DateTime.Now), dialog.ActivityId, dialog.ObjectiveId, dialog.IsPlannedActivity);
                _chronosCore.TrackingService.StartTracking(targetId, TimeOnly.FromDateTime(DateTime.Now));
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not start tracking.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            try
            {
                _chronosCore.TrackingService.StopTracking(TimeOnly.FromDateTime(DateTime.Now));
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not stop tracking.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }
    }
}

using System.ComponentModel;
using Chronos.Core;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Views.Dialogs;
using Chronos.WinForms.DataObjects;
using Chronos.WinForms.Views.Common;

namespace Chronos.Views.TabPages
{
    public partial class RecordsPage : UserControl
    {
        private BindingList<TrackingRecordGridEntry> _records = new();
        private readonly ChronosCore _chronosCore;

        public RecordsPage(ChronosCore chronosCore)
        {
            InitializeComponent();
            _chronosCore = chronosCore;
            _dateTimePicker.Value = DateTime.Today;
            _gridRecords.BindDataSource(_records);
            _gridRecords.GridView.CellFormatting += GridView_CellFormatting;
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
            _records.Clear();
            var selectedDate = DateOnly.FromDateTime(_dateTimePicker.Value);

            IReadOnlyList<TrackingRecord> records;

            try
            {
                records = _chronosCore.TrackingService.GetRecordsForDay(selectedDate);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not retrieve records.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
             
            foreach (var record in records)
            {
                _records.Add(new TrackingRecordGridEntry(record));
            }
        }

        private void RecordsPage_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                return;
            }

            RefreshView();
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void GridActionBar_AddClicked(object sender, EventArgs e)
        {
            //TODO
            MessageBox.Show("Not implemented yet.");
        }

        private void GridActionBar_EditClicked(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridRecords.GridView, out TrackingRecordGridEntry? trackingRecordGridEntry))
            {
                return;
            }

            if (trackingRecordGridEntry.IsActive)
            {
                MessageBox.Show(this, "An active record cannot be edited.");
                return;
            }

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

            var dialog = new ManageTrackingRecordDialog(activities, objectives);
            dialog.ActivityId = trackingRecordGridEntry.ActivityId;
            dialog.ObjectiveId = trackingRecordGridEntry.ObjectiveId;
            dialog.Start = trackingRecordGridEntry.Start.ToTimeSpan();
            dialog.End = trackingRecordGridEntry.End.ToTimeSpan();

            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult != DialogResult.OK)
            {
                return;
            }            

            try
            {
                _chronosCore.TrackingService.UpdateRecord(trackingRecordGridEntry.Id, TimeOnly.FromTimeSpan(dialog.Start), TimeOnly.FromTimeSpan(dialog.End));
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not update record.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }

        private void GridActionBar_RemoveClicked(object sender, EventArgs e)
        {
            //TODO
            MessageBox.Show("Not implemented yet.");
        }
    }
}

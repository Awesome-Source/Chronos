using System.ComponentModel;
using Chronos.Core;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Views.Dialogs;
using Chronos.WinForms.DataObjects;
using Chronos.WinForms.Views.Common;

namespace Chronos.Views.TabPages
{
    public partial class ActivityPage : UserControl
    {
        private readonly ChronosCore _chronosCore;
        private BindingList<ActivityGridEntry> _activities = new BindingList<ActivityGridEntry>();
        public ActivityPage(ChronosCore chronosCore)
        {
            InitializeComponent();
            _chronosCore = chronosCore;

            _gridActivities.BindDataSource(_activities);
            RefreshView();
        }

        private void RefreshView()
        {
            _activities.Clear();

            IReadOnlyList<Activity> activities;

            try
            {
                activities = _chronosCore.ActivityService.GetAll();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not retrieve activities.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            foreach (var activity in activities)
            {
                _activities.Add(new ActivityGridEntry(activity));
            }
        }

        private void GridActionBar_AddClicked(object sender, EventArgs e)
        {
            IReadOnlyList<TimeAccount> timeAccounts;

            try
            {
                timeAccounts = _chronosCore.TimeAccountService.GetAll();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not retrieve time accounts.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            var dialog = new ManageActivityDialog(timeAccounts);
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            try
            {
                _chronosCore.ActivityService.Create(dialog.ActivityName, dialog.TimeAccountId);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not create activity.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }

        private void GridActionBar_EditClicked(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridActivities.GridView, out ActivityGridEntry? activityGridEntry))
            {
                return;
            }

            IReadOnlyList<TimeAccount> timeAccounts;

            try
            {
                timeAccounts = _chronosCore.TimeAccountService.GetAll();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not retrieve time accounts.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var dialog = new ManageActivityDialog(timeAccounts);
            dialog.ActivityName = activityGridEntry.Name;
            dialog.TimeAccountId = activityGridEntry.TimeAccountId;

            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            try
            {
                _chronosCore.ActivityService.Update(activityGridEntry.Id, dialog.ActivityName, dialog.TimeAccountId);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not update activity.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }

        private void GridActionBar_RemoveClicked(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridActivities.GridView, out ActivityGridEntry? activityGridEntry))
            {
                return;
            }            

            try
            {
                _chronosCore.ActivityService.Remove(activityGridEntry.Id);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not remove activity.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }
    }
}

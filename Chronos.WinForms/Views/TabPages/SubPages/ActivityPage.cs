using System.ComponentModel;
using Chronos.Core;
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
            foreach (var activity in _chronosCore.ActivityService.GetAll())
            {
                _activities.Add(new ActivityGridEntry(activity));
            }
        }

        private void GridActionBar_AddClicked(object sender, EventArgs e)
        {
            var dialog = new ManageActivityDialog(_chronosCore.TimeAccountService.GetAll());
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                _chronosCore.ActivityService.Create(dialog.ActivityName, dialog.TimeAccountId);
                RefreshView();
            }
        }

        private void GridActionBar_EditClicked(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridActivities.GridView, out ActivityGridEntry? activityGridEntry))
            {
                return;
            }

            var dialog = new ManageActivityDialog(_chronosCore.TimeAccountService.GetAll());
            dialog.ActivityName = activityGridEntry.Name;
            dialog.TimeAccountId = activityGridEntry.TimeAccountId;

            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                _chronosCore.ActivityService.Update(activityGridEntry.Id, dialog.ActivityName, dialog.TimeAccountId);
                RefreshView();
            }
        }

        private void GridActionBar_RemoveClicked(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridActivities.GridView, out ActivityGridEntry? activityGridEntry))
            {
                return;
            }

            _chronosCore.ActivityService.Remove(activityGridEntry.Id);
            RefreshView();
        }
    }
}

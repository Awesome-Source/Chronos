using System.ComponentModel;
using Chronos.Core;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Views.Dialogs;
using Chronos.WinForms.DataObjects;
using Chronos.WinForms.Views.Common;

namespace Chronos.Views.TabPages
{
    public partial class TimeAccountPage : UserControl
    {
        private readonly ChronosCore _chronosCore;
        private BindingList<TimeAccountGridEntry> _timeAccounts = new BindingList<TimeAccountGridEntry>();

        public TimeAccountPage(ChronosCore chronosCore)
        {
            InitializeComponent();
            _chronosCore = chronosCore;

            _gridTimeAccounts.BindDataSource(_timeAccounts);
            RefreshView();
        }

        private void RefreshView()
        {
            _timeAccounts.Clear();

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

            foreach (var timeAccount in timeAccounts)
            {
                _timeAccounts.Add(new TimeAccountGridEntry(timeAccount));
            }
        }

        private void GridActionBar_AddClicked(object sender, EventArgs e)
        {
            var dialog = new ManageTimeAccountDialog();
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            try
            {
                _chronosCore.TimeAccountService.Create(dialog.TimeAccountName, dialog.TimeAccountColor, dialog.TimeAccountWorktime);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not create time account.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }

        private void GridActionBar_EditClicked(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridTimeAccounts.GridView, out TimeAccountGridEntry? timeAccount))
            {
                return;
            }

            var dialog = new ManageTimeAccountDialog();
            dialog.TimeAccountName = timeAccount.Name;
            dialog.TimeAccountColor = timeAccount.Color;
            dialog.TimeAccountWorktime = timeAccount.IsWorkTime;
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            try
            {
                _chronosCore.TimeAccountService.Update(timeAccount.Id, dialog.TimeAccountName, dialog.TimeAccountColor, dialog.TimeAccountWorktime);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not update time account.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }

        private void GridActionBar_RemoveClicked(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridTimeAccounts.GridView, out TimeAccountGridEntry? timeAccount))
            {
                return;
            }

            try
            {
                _chronosCore.TimeAccountService.Remove(timeAccount.Id);
            }
            catch(Exception exception)
            {
                MessageBox.Show(this, $"Could not delete time account.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            RefreshView();
        }
    }
}

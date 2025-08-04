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
            foreach (var timeAccount in _chronosCore.TimeAccountService.GetAll())
            {
                _timeAccounts.Add(new TimeAccountGridEntry(timeAccount));
            }
        }

        private void GridActionBar_AddClicked(object sender, EventArgs e)
        {
            var dialog = new ManageTimeAccountDialog();
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                _chronosCore.TimeAccountService.Create(dialog.TimeAccountName, dialog.TimeAccountColor, dialog.TimeAccountWorktime);
                RefreshView();
            }
        }

        private void GridActionBar_EditClicked(object sender, EventArgs e)
        {
            if(!GridHelper.TryGetSingleSelectedDataBoundItem(_gridTimeAccounts.GridView, out TimeAccountGridEntry? timeAccount))
            {
                return;
            }

            var dialog = new ManageTimeAccountDialog();
            dialog.TimeAccountName = timeAccount.Name;
            dialog.TimeAccountColor = timeAccount.Color;
            dialog.TimeAccountWorktime = timeAccount.IsWorkTime;
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                _chronosCore.TimeAccountService.Update(timeAccount.Id, dialog.TimeAccountName, dialog.TimeAccountColor, dialog.TimeAccountWorktime);
                RefreshView();
            }
        }

        private void GridActionBar_RemoveClicked(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridTimeAccounts.GridView, out TimeAccountGridEntry? timeAccount))
            {
                return;
            }

            _chronosCore.TimeAccountService.Remove(timeAccount.Id);
            RefreshView();
        }
    }
}

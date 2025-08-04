using System.ComponentModel;
using Chronos.Core;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Views.Dialogs;
using Chronos.WinForms.DataObjects;
using Chronos.WinForms.Views.Common;

namespace Chronos.Views.TabPages
{
    public partial class ObjectivePage : UserControl
    {
        private readonly ChronosCore _chronosCore;
        private BindingList<ObjectiveGridEntry> _objectives = new BindingList<ObjectiveGridEntry>();

        public ObjectivePage(ChronosCore chronosCore)
        {
            InitializeComponent();
            _chronosCore = chronosCore;

            _gridObjectives.BindDataSource(_objectives);
            RefreshView();
        }

        private void RefreshView()
        {
            _objectives.Clear();
            foreach (var objective in _chronosCore.ObjectiveService.GetAll())
            {
                _objectives.Add(new ObjectiveGridEntry(objective));
            }
        }

        private void GridActionBar_AddClicked(object sender, EventArgs e)
        {
            var dialog = new ManageObjectiveDialog();
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                _chronosCore.ObjectiveService.Create(dialog.ObjectiveName, dialog.ObjectiveDescription);
                RefreshView();
            }
        }

        private void GridActionBar_EditClicked(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridObjectives.GridView, out ObjectiveGridEntry? objectiveGridEntry))
            {
                return;
            }

            var dialog = new ManageObjectiveDialog();
            dialog.ObjectiveName = objectiveGridEntry.Name;
            dialog.ObjectiveDescription = objectiveGridEntry.Description;

            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                _chronosCore.ObjectiveService.Update(objectiveGridEntry.Id, dialog.ObjectiveName, dialog.ObjectiveDescription);
                RefreshView();
            }
        }

        private void GridActionBar_RemoveClicked(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridObjectives.GridView, out ObjectiveGridEntry? objectiveGridEntry))
            {
                return;
            }

            _chronosCore.ObjectiveService.Remove(objectiveGridEntry.Id);
            RefreshView();
        }
    }
}

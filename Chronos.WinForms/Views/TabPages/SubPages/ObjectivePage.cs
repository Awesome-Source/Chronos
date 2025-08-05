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

            IReadOnlyList<Objective> objectives;

            try
            {
                objectives = _chronosCore.ObjectiveService.GetAll();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not retrieve objectives.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var objective in objectives)
            {
                _objectives.Add(new ObjectiveGridEntry(objective));
            }
        }

        private void GridActionBar_AddClicked(object sender, EventArgs e)
        {
            var dialog = new ManageObjectiveDialog();
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            try
            {
                _chronosCore.ObjectiveService.Create(dialog.ObjectiveName, dialog.ObjectiveDescription);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not create objective.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
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

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            try
            {
                _chronosCore.ObjectiveService.Update(objectiveGridEntry.Id, dialog.ObjectiveName, dialog.ObjectiveDescription);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not update objective.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }

        private void GridActionBar_RemoveClicked(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridObjectives.GridView, out ObjectiveGridEntry? objectiveGridEntry))
            {
                return;
            }

            try
            {
                _chronosCore.ObjectiveService.Remove(objectiveGridEntry.Id);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not delete objective.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }
    }
}

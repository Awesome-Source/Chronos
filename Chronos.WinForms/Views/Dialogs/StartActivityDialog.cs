using Chronos.Core.Contracts.DataObjects;
using Chronos.WinForms.DataObjects;

namespace Chronos.Views.Dialogs
{
    public partial class StartActivityDialog : Form
    {
        public int ActivityId
        {
            get => (_comboBoxActivity.SelectedItem as ActivityComboboxItem)?.InternalId ?? -1;
            set => _comboBoxActivity.SelectedIndex = _comboBoxActivity.Items.OfType<ActivityComboboxItem>().ToList().FindIndex(ta => ta.InternalId == value);
        }

        public int ObjectiveId
        {
            get => (_comboBoxObjective.SelectedItem as ObjectiveComboboxItem)?.InternalId ?? -1;
            set => _comboBoxObjective.SelectedIndex = _comboBoxObjective.Items.OfType<ObjectiveComboboxItem>().ToList().FindIndex(ta => ta.InternalId == value);
        }

        public bool IsPlannedActivity
        {
            get => _checkBoxPlannedActivity.Checked;
            set => _checkBoxPlannedActivity.Checked = value;
        }

        public StartActivityDialog(IReadOnlyList<Activity> activities, IReadOnlyList<Objective> objectives)
        {
            InitializeComponent();

            foreach (var activity in activities)
            {
                _comboBoxActivity.Items.Add(new ActivityComboboxItem(activity));
            }

            foreach (var objective in objectives)
            {
                _comboBoxObjective.Items.Add(new ObjectiveComboboxItem(objective));
            }
        }
    }
}

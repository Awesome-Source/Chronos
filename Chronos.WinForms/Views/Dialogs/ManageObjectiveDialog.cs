using Chronos.Core.Contracts.DataObjects;
using Chronos.WinForms.DataObjects;

namespace Chronos.Views.Dialogs
{
    public partial class ManageObjectiveDialog : Form
    {
        public string ObjectiveName
        {
            get => _textBoxName.Text;
            set => _textBoxName.Text = value;
        }
        public string ObjectiveDescription
        {
            get => _textBoxDescription.Text;
            set => _textBoxDescription.Text = value;
        }

        public int CategoryId
        {
            get => (_comboBoxCategory.SelectedItem as CategoryComboboxItem)?.InternalId ?? -1;
            set => _comboBoxCategory.SelectedIndex = _comboBoxCategory.Items.OfType<CategoryComboboxItem>().ToList().FindIndex(ta => ta.InternalId == value);
        }

        public ManageObjectiveDialog(IReadOnlyList<Category> categories)
        {
            InitializeComponent();

            foreach(var category in categories)
            {
                _comboBoxCategory.Items.Add(new CategoryComboboxItem(category));
            }
        }
    }
}

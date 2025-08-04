using Chronos.Core.Contracts.DataObjects;
using Chronos.WinForms.DataObjects;

namespace Chronos.Views.Dialogs
{
    public partial class ManageActivityDialog : Form
    {
        public string ActivityName
        {
            get => _textBoxName.Text;
            set => _textBoxName.Text = value;
        }

        public int TimeAccountId
        {
            get => (_comboBoxTimeAccount.SelectedItem as TimeAccountComboboxItem)?.InternalId ?? -1;
            set => _comboBoxTimeAccount.SelectedIndex = _comboBoxTimeAccount.Items.OfType<TimeAccountComboboxItem>().ToList().FindIndex(ta => ta.InternalId == value);
        }

        public ManageActivityDialog(IReadOnlyList<TimeAccount> timeAccounts)
        {
            InitializeComponent();

            foreach (var timeAccount in timeAccounts)
            {
                _comboBoxTimeAccount.Items.Add(new TimeAccountComboboxItem(timeAccount));
            }            
        }
    }
}

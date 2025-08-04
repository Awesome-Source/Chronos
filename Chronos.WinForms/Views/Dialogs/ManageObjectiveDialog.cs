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

        public ManageObjectiveDialog()
        {
            InitializeComponent();            
        }
    }
}

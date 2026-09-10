namespace Chronos.Views.Dialogs
{
    public partial class ManageCategoryDialog : Form
    {
        public string CategoryName
        {
            get => _textBoxName.Text;
            set => _textBoxName.Text = value;
        }

        public ManageCategoryDialog()
        {
            InitializeComponent();            
        }
    }
}

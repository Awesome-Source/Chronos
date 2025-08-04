namespace Chronos.Views.Common
{
    public partial class GridActionBar : UserControl
    {
        public event EventHandler? AddClicked;
        public event EventHandler? EditClicked;
        public event EventHandler? RemoveClicked;

        public GridActionBar()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddClicked?.Invoke(this, e);
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            EditClicked?.Invoke(this, e);
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            RemoveClicked?.Invoke(this, e);
        }
    }
}

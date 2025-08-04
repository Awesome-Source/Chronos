namespace Chronos.Views.Dialogs
{
    public partial class ManageTimeAccountDialog : Form
    {
        public string TimeAccountName
        {
            get => _textBoxName.Text;
            set => _textBoxName.Text = value;
        }
        public Color TimeAccountColor
        {
            get => _colorDialog.Color;
            set
            {
                _panelColor.BackColor = value;
                _colorDialog.Color = value;
            }
        }

        public bool TimeAccountWorktime
        {
            get => _checkBoxWorkTime.Checked;
            set => _checkBoxWorkTime.Checked = value;
        }

        public ManageTimeAccountDialog()
        {
            InitializeComponent();
        }

        private void PanelColor_Click(object sender, EventArgs e)
        {
            var dialogResult = _colorDialog.ShowDialog();
            if(dialogResult == DialogResult.OK)
            {
                _panelColor.BackColor = _colorDialog.Color;
            }
        }
    }
}

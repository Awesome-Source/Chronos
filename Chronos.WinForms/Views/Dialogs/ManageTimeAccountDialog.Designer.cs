namespace Chronos.Views.Dialogs
{
    partial class ManageTimeAccountDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageTimeAccountDialog));
            _labelName = new Label();
            _labelColor = new Label();
            _labelWorkTime = new Label();
            _textBoxName = new TextBox();
            _panelColor = new Panel();
            _checkBoxWorkTime = new CheckBox();
            _colorDialog = new ColorDialog();
            _buttonOk = new Button();
            _buttonCancel = new Button();
            SuspendLayout();
            // 
            // _labelName
            // 
            _labelName.AutoSize = true;
            _labelName.Location = new Point(8, 5);
            _labelName.Margin = new Padding(2, 0, 2, 0);
            _labelName.Name = "_labelName";
            _labelName.Size = new Size(39, 15);
            _labelName.TabIndex = 0;
            _labelName.Text = "Name";
            // 
            // _labelColor
            // 
            _labelColor.AutoSize = true;
            _labelColor.Location = new Point(8, 37);
            _labelColor.Margin = new Padding(2, 0, 2, 0);
            _labelColor.Name = "_labelColor";
            _labelColor.Size = new Size(36, 15);
            _labelColor.TabIndex = 1;
            _labelColor.Text = "Color";
            // 
            // _labelWorkTime
            // 
            _labelWorkTime.AutoSize = true;
            _labelWorkTime.Location = new Point(8, 69);
            _labelWorkTime.Margin = new Padding(2, 0, 2, 0);
            _labelWorkTime.Name = "_labelWorkTime";
            _labelWorkTime.Size = new Size(59, 15);
            _labelWorkTime.TabIndex = 2;
            _labelWorkTime.Text = "Worktime";
            // 
            // _textBoxName
            // 
            _textBoxName.Location = new Point(92, 5);
            _textBoxName.Margin = new Padding(2, 2, 2, 2);
            _textBoxName.Name = "_textBoxName";
            _textBoxName.Size = new Size(192, 23);
            _textBoxName.TabIndex = 3;
            // 
            // _panelColor
            // 
            _panelColor.BackColor = Color.Gray;
            _panelColor.Location = new Point(92, 37);
            _panelColor.Margin = new Padding(2, 2, 2, 2);
            _panelColor.Name = "_panelColor";
            _panelColor.Size = new Size(190, 19);
            _panelColor.TabIndex = 4;
            _panelColor.Click += PanelColor_Click;
            // 
            // _checkBoxWorkTime
            // 
            _checkBoxWorkTime.AutoSize = true;
            _checkBoxWorkTime.Checked = true;
            _checkBoxWorkTime.CheckState = CheckState.Checked;
            _checkBoxWorkTime.Location = new Point(92, 69);
            _checkBoxWorkTime.Margin = new Padding(2, 2, 2, 2);
            _checkBoxWorkTime.Name = "_checkBoxWorkTime";
            _checkBoxWorkTime.Size = new Size(15, 14);
            _checkBoxWorkTime.TabIndex = 5;
            _checkBoxWorkTime.UseVisualStyleBackColor = true;
            // 
            // _buttonOk
            // 
            _buttonOk.DialogResult = DialogResult.OK;
            _buttonOk.FlatStyle = FlatStyle.Flat;
            _buttonOk.Location = new Point(132, 114);
            _buttonOk.Margin = new Padding(2, 2, 2, 2);
            _buttonOk.Name = "_buttonOk";
            _buttonOk.Size = new Size(78, 26);
            _buttonOk.TabIndex = 6;
            _buttonOk.Text = "Ok";
            _buttonOk.UseVisualStyleBackColor = true;
            // 
            // _buttonCancel
            // 
            _buttonCancel.DialogResult = DialogResult.Cancel;
            _buttonCancel.FlatStyle = FlatStyle.Flat;
            _buttonCancel.Location = new Point(215, 114);
            _buttonCancel.Margin = new Padding(2, 2, 2, 2);
            _buttonCancel.Name = "_buttonCancel";
            _buttonCancel.Size = new Size(78, 26);
            _buttonCancel.TabIndex = 7;
            _buttonCancel.Text = "Cancel";
            _buttonCancel.UseVisualStyleBackColor = true;
            // 
            // ManageTimeAccountDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(304, 151);
            Controls.Add(_buttonCancel);
            Controls.Add(_buttonOk);
            Controls.Add(_checkBoxWorkTime);
            Controls.Add(_panelColor);
            Controls.Add(_textBoxName);
            Controls.Add(_labelWorkTime);
            Controls.Add(_labelColor);
            Controls.Add(_labelName);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2, 2, 2, 2);
            Name = "ManageTimeAccountDialog";
            Text = "Time Account";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label _labelName;
        private Label _labelColor;
        private Label _labelWorkTime;
        private TextBox _textBoxName;
        private Panel _panelColor;
        private CheckBox _checkBoxWorkTime;
        private ColorDialog _colorDialog;
        private Button _buttonOk;
        private Button _buttonCancel;
    }
}
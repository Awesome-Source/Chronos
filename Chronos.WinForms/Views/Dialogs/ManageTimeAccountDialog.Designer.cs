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
            _labelName.Location = new Point(12, 9);
            _labelName.Name = "_labelName";
            _labelName.Size = new Size(59, 25);
            _labelName.TabIndex = 0;
            _labelName.Text = "Name";
            // 
            // _labelColor
            // 
            _labelColor.AutoSize = true;
            _labelColor.Location = new Point(12, 62);
            _labelColor.Name = "_labelColor";
            _labelColor.Size = new Size(55, 25);
            _labelColor.TabIndex = 1;
            _labelColor.Text = "Color";
            // 
            // _labelWorkTime
            // 
            _labelWorkTime.AutoSize = true;
            _labelWorkTime.Location = new Point(12, 115);
            _labelWorkTime.Name = "_labelWorkTime";
            _labelWorkTime.Size = new Size(89, 25);
            _labelWorkTime.TabIndex = 2;
            _labelWorkTime.Text = "Worktime";
            // 
            // _textBoxName
            // 
            _textBoxName.Location = new Point(132, 9);
            _textBoxName.Name = "_textBoxName";
            _textBoxName.Size = new Size(272, 31);
            _textBoxName.TabIndex = 3;
            // 
            // _panelColor
            // 
            _panelColor.BackColor = Color.Gray;
            _panelColor.Location = new Point(132, 62);
            _panelColor.Name = "_panelColor";
            _panelColor.Size = new Size(272, 31);
            _panelColor.TabIndex = 4;
            _panelColor.Click += PanelColor_Click;
            // 
            // _checkBoxWorkTime
            // 
            _checkBoxWorkTime.AutoSize = true;
            _checkBoxWorkTime.Checked = true;
            _checkBoxWorkTime.CheckState = CheckState.Checked;
            _checkBoxWorkTime.Location = new Point(132, 115);
            _checkBoxWorkTime.Name = "_checkBoxWorkTime";
            _checkBoxWorkTime.Size = new Size(22, 21);
            _checkBoxWorkTime.TabIndex = 5;
            _checkBoxWorkTime.UseVisualStyleBackColor = true;
            // 
            // _buttonOk
            // 
            _buttonOk.DialogResult = DialogResult.OK;
            _buttonOk.FlatStyle = FlatStyle.Flat;
            _buttonOk.Location = new Point(192, 182);
            _buttonOk.Name = "_buttonOk";
            _buttonOk.Size = new Size(112, 34);
            _buttonOk.TabIndex = 6;
            _buttonOk.Text = "Ok";
            _buttonOk.UseVisualStyleBackColor = true;
            // 
            // _buttonCancel
            // 
            _buttonCancel.DialogResult = DialogResult.Cancel;
            _buttonCancel.FlatStyle = FlatStyle.Flat;
            _buttonCancel.Location = new Point(310, 182);
            _buttonCancel.Name = "_buttonCancel";
            _buttonCancel.Size = new Size(112, 34);
            _buttonCancel.TabIndex = 7;
            _buttonCancel.Text = "Cancel";
            _buttonCancel.UseVisualStyleBackColor = true;
            // 
            // ManageTimeAccountDialog
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(434, 228);
            Controls.Add(_buttonCancel);
            Controls.Add(_buttonOk);
            Controls.Add(_checkBoxWorkTime);
            Controls.Add(_panelColor);
            Controls.Add(_textBoxName);
            Controls.Add(_labelWorkTime);
            Controls.Add(_labelColor);
            Controls.Add(_labelName);
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
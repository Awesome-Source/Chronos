namespace Chronos.Views.Dialogs
{
    partial class ManageTrackingRecordDialog
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
            _comboBoxObjective = new ComboBox();
            _labelActivity = new Label();
            _labelObjective = new Label();
            _buttonCancel = new Button();
            _buttonOk = new Button();
            _comboBoxActivity = new ComboBox();
            _textBoxStart = new TextBox();
            _textBoxEnd = new TextBox();
            _labelStart = new Label();
            _labelEnd = new Label();
            SuspendLayout();
            // 
            // _comboBoxObjective
            // 
            _comboBoxObjective.BackColor = Color.Silver;
            _comboBoxObjective.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboBoxObjective.Enabled = false;
            _comboBoxObjective.FlatStyle = FlatStyle.Flat;
            _comboBoxObjective.FormattingEnabled = true;
            _comboBoxObjective.Location = new Point(172, 48);
            _comboBoxObjective.Name = "_comboBoxObjective";
            _comboBoxObjective.Size = new Size(270, 33);
            _comboBoxObjective.TabIndex = 0;
            // 
            // _labelActivity
            // 
            _labelActivity.AutoSize = true;
            _labelActivity.Location = new Point(12, 9);
            _labelActivity.Name = "_labelActivity";
            _labelActivity.Size = new Size(70, 25);
            _labelActivity.TabIndex = 1;
            _labelActivity.Text = "Activity";
            // 
            // _labelObjective
            // 
            _labelObjective.AutoSize = true;
            _labelObjective.Location = new Point(12, 51);
            _labelObjective.Name = "_labelObjective";
            _labelObjective.Size = new Size(86, 25);
            _labelObjective.TabIndex = 2;
            _labelObjective.Text = "Objective";
            // 
            // _buttonCancel
            // 
            _buttonCancel.DialogResult = DialogResult.Cancel;
            _buttonCancel.FlatStyle = FlatStyle.Flat;
            _buttonCancel.Location = new Point(330, 198);
            _buttonCancel.Name = "_buttonCancel";
            _buttonCancel.Size = new Size(112, 34);
            _buttonCancel.TabIndex = 9;
            _buttonCancel.Text = "Cancel";
            _buttonCancel.UseVisualStyleBackColor = true;
            // 
            // _buttonOk
            // 
            _buttonOk.DialogResult = DialogResult.OK;
            _buttonOk.FlatStyle = FlatStyle.Flat;
            _buttonOk.Location = new Point(212, 198);
            _buttonOk.Name = "_buttonOk";
            _buttonOk.Size = new Size(112, 34);
            _buttonOk.TabIndex = 8;
            _buttonOk.Text = "Ok";
            _buttonOk.UseVisualStyleBackColor = true;
            // 
            // _comboBoxActivity
            // 
            _comboBoxActivity.BackColor = Color.Silver;
            _comboBoxActivity.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboBoxActivity.Enabled = false;
            _comboBoxActivity.FlatStyle = FlatStyle.Flat;
            _comboBoxActivity.FormattingEnabled = true;
            _comboBoxActivity.Location = new Point(172, 9);
            _comboBoxActivity.Name = "_comboBoxActivity";
            _comboBoxActivity.Size = new Size(270, 33);
            _comboBoxActivity.TabIndex = 10;
            // 
            // _textBoxStart
            // 
            _textBoxStart.Location = new Point(172, 87);
            _textBoxStart.Name = "_textBoxStart";
            _textBoxStart.Size = new Size(270, 31);
            _textBoxStart.TabIndex = 11;
            // 
            // _textBoxEnd
            // 
            _textBoxEnd.Location = new Point(172, 124);
            _textBoxEnd.Name = "_textBoxEnd";
            _textBoxEnd.Size = new Size(270, 31);
            _textBoxEnd.TabIndex = 12;
            // 
            // _labelStart
            // 
            _labelStart.AutoSize = true;
            _labelStart.Location = new Point(12, 90);
            _labelStart.Name = "_labelStart";
            _labelStart.Size = new Size(48, 25);
            _labelStart.TabIndex = 13;
            _labelStart.Text = "Start";
            // 
            // _labelEnd
            // 
            _labelEnd.AutoSize = true;
            _labelEnd.Location = new Point(12, 127);
            _labelEnd.Name = "_labelEnd";
            _labelEnd.Size = new Size(42, 25);
            _labelEnd.TabIndex = 14;
            _labelEnd.Text = "End";
            // 
            // ManageTrackingRecordDialog
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(454, 244);
            Controls.Add(_labelEnd);
            Controls.Add(_labelStart);
            Controls.Add(_textBoxEnd);
            Controls.Add(_textBoxStart);
            Controls.Add(_comboBoxActivity);
            Controls.Add(_buttonCancel);
            Controls.Add(_buttonOk);
            Controls.Add(_labelObjective);
            Controls.Add(_labelActivity);
            Controls.Add(_comboBoxObjective);
            Name = "ManageTrackingRecordDialog";
            Text = "Manage Record";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox _comboBoxObjective;
        private Label _labelActivity;
        private Label _labelObjective;
        private Button _buttonCancel;
        private Button _buttonOk;
        private ComboBox _comboBoxActivity;
        private TextBox _textBoxStart;
        private TextBox _textBoxEnd;
        private Label _labelStart;
        private Label _labelEnd;
    }
}
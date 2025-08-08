namespace Chronos.Views.Dialogs
{
    partial class ManageActivityDialog
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageActivityDialog));
            _comboBoxTimeAccount = new ComboBox();
            _labelName = new Label();
            _labelTimeAccount = new Label();
            _textBoxName = new TextBox();
            _buttonCancel = new Button();
            _buttonOk = new Button();
            SuspendLayout();
            // 
            // _comboBoxTimeAccount
            // 
            _comboBoxTimeAccount.BackColor = Color.Silver;
            _comboBoxTimeAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboBoxTimeAccount.FlatStyle = FlatStyle.Flat;
            _comboBoxTimeAccount.FormattingEnabled = true;
            _comboBoxTimeAccount.Location = new Point(172, 43);
            _comboBoxTimeAccount.Name = "_comboBoxTimeAccount";
            _comboBoxTimeAccount.Size = new Size(270, 33);
            _comboBoxTimeAccount.TabIndex = 0;
            // 
            // _labelName
            // 
            _labelName.AutoSize = true;
            _labelName.Location = new Point(12, 9);
            _labelName.Name = "_labelName";
            _labelName.Size = new Size(59, 25);
            _labelName.TabIndex = 1;
            _labelName.Text = "Name";
            // 
            // _labelTimeAccount
            // 
            _labelTimeAccount.AutoSize = true;
            _labelTimeAccount.Location = new Point(12, 51);
            _labelTimeAccount.Name = "_labelTimeAccount";
            _labelTimeAccount.Size = new Size(120, 25);
            _labelTimeAccount.TabIndex = 2;
            _labelTimeAccount.Text = "Time Account";
            // 
            // _textBoxName
            // 
            _textBoxName.Location = new Point(172, 6);
            _textBoxName.Name = "_textBoxName";
            _textBoxName.Size = new Size(270, 31);
            _textBoxName.TabIndex = 3;
            // 
            // _buttonCancel
            // 
            _buttonCancel.DialogResult = DialogResult.Cancel;
            _buttonCancel.FlatStyle = FlatStyle.Flat;
            _buttonCancel.Location = new Point(330, 118);
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
            _buttonOk.Location = new Point(212, 118);
            _buttonOk.Name = "_buttonOk";
            _buttonOk.Size = new Size(112, 34);
            _buttonOk.TabIndex = 8;
            _buttonOk.Text = "Ok";
            _buttonOk.UseVisualStyleBackColor = true;
            // 
            // ManageActivityDialog
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(454, 164);
            Controls.Add(_buttonCancel);
            Controls.Add(_buttonOk);
            Controls.Add(_textBoxName);
            Controls.Add(_labelTimeAccount);
            Controls.Add(_labelName);
            Controls.Add(_comboBoxTimeAccount);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ManageActivityDialog";
            Text = "Activity";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox _comboBoxTimeAccount;
        private Label _labelName;
        private Label _labelTimeAccount;
        private TextBox _textBoxName;
        private Button _buttonCancel;
        private Button _buttonOk;
    }
}
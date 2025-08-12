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
            _comboBoxTimeAccount.Location = new Point(120, 26);
            _comboBoxTimeAccount.Margin = new Padding(2, 2, 2, 2);
            _comboBoxTimeAccount.Name = "_comboBoxTimeAccount";
            _comboBoxTimeAccount.Size = new Size(190, 23);
            _comboBoxTimeAccount.TabIndex = 0;
            // 
            // _labelName
            // 
            _labelName.AutoSize = true;
            _labelName.Location = new Point(8, 5);
            _labelName.Margin = new Padding(2, 0, 2, 0);
            _labelName.Name = "_labelName";
            _labelName.Size = new Size(39, 15);
            _labelName.TabIndex = 1;
            _labelName.Text = "Name";
            // 
            // _labelTimeAccount
            // 
            _labelTimeAccount.AutoSize = true;
            _labelTimeAccount.Location = new Point(8, 31);
            _labelTimeAccount.Margin = new Padding(2, 0, 2, 0);
            _labelTimeAccount.Name = "_labelTimeAccount";
            _labelTimeAccount.Size = new Size(82, 15);
            _labelTimeAccount.TabIndex = 2;
            _labelTimeAccount.Text = "Time Account";
            // 
            // _textBoxName
            // 
            _textBoxName.Location = new Point(120, 4);
            _textBoxName.Margin = new Padding(2, 2, 2, 2);
            _textBoxName.Name = "_textBoxName";
            _textBoxName.Size = new Size(190, 23);
            _textBoxName.TabIndex = 3;
            // 
            // _buttonCancel
            // 
            _buttonCancel.DialogResult = DialogResult.Cancel;
            _buttonCancel.FlatStyle = FlatStyle.Flat;
            _buttonCancel.Location = new Point(229, 69);
            _buttonCancel.Margin = new Padding(2, 2, 2, 2);
            _buttonCancel.Name = "_buttonCancel";
            _buttonCancel.Size = new Size(78, 26);
            _buttonCancel.TabIndex = 9;
            _buttonCancel.Text = "Cancel";
            _buttonCancel.UseVisualStyleBackColor = true;
            // 
            // _buttonOk
            // 
            _buttonOk.DialogResult = DialogResult.OK;
            _buttonOk.FlatStyle = FlatStyle.Flat;
            _buttonOk.Location = new Point(146, 69);
            _buttonOk.Margin = new Padding(2, 2, 2, 2);
            _buttonOk.Name = "_buttonOk";
            _buttonOk.Size = new Size(78, 26);
            _buttonOk.TabIndex = 8;
            _buttonOk.Text = "Ok";
            _buttonOk.UseVisualStyleBackColor = true;
            // 
            // ManageActivityDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(318, 106);
            Controls.Add(_buttonCancel);
            Controls.Add(_buttonOk);
            Controls.Add(_textBoxName);
            Controls.Add(_labelTimeAccount);
            Controls.Add(_labelName);
            Controls.Add(_comboBoxTimeAccount);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2, 2, 2, 2);
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
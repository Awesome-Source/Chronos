namespace Chronos.Views.Dialogs
{
    partial class ManageCategoryDialog
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageCategoryDialog));
            _labelName = new Label();
            _textBoxName = new TextBox();
            _buttonCancel = new Button();
            _buttonOk = new Button();
            SuspendLayout();
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
            // _textBoxName
            // 
            _textBoxName.Location = new Point(120, 4);
            _textBoxName.Margin = new Padding(2);
            _textBoxName.Name = "_textBoxName";
            _textBoxName.Size = new Size(190, 23);
            _textBoxName.TabIndex = 3;
            // 
            // _buttonCancel
            // 
            _buttonCancel.DialogResult = DialogResult.Cancel;
            _buttonCancel.FlatStyle = FlatStyle.Flat;
            _buttonCancel.Location = new Point(229, 75);
            _buttonCancel.Margin = new Padding(2);
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
            _buttonOk.Location = new Point(146, 75);
            _buttonOk.Margin = new Padding(2);
            _buttonOk.Name = "_buttonOk";
            _buttonOk.Size = new Size(78, 26);
            _buttonOk.TabIndex = 8;
            _buttonOk.Text = "Ok";
            _buttonOk.UseVisualStyleBackColor = true;
            // 
            // ManageCategoryDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(318, 112);
            Controls.Add(_buttonCancel);
            Controls.Add(_buttonOk);
            Controls.Add(_textBoxName);
            Controls.Add(_labelName);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            Name = "ManageCategoryDialog";
            Text = "Category";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label _labelName;
        private TextBox _textBoxName;
        private Button _buttonCancel;
        private Button _buttonOk;
    }
}
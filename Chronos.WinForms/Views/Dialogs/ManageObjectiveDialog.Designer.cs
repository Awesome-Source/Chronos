namespace Chronos.Views.Dialogs
{
    partial class ManageObjectiveDialog
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageObjectiveDialog));
            _labelName = new Label();
            _labelDescription = new Label();
            _textBoxName = new TextBox();
            _buttonCancel = new Button();
            _buttonOk = new Button();
            _textBoxDescription = new TextBox();
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
            // _labelDescription
            // 
            _labelDescription.AutoSize = true;
            _labelDescription.Location = new Point(8, 31);
            _labelDescription.Margin = new Padding(2, 0, 2, 0);
            _labelDescription.Name = "_labelDescription";
            _labelDescription.Size = new Size(67, 15);
            _labelDescription.TabIndex = 2;
            _labelDescription.Text = "Description";
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
            _buttonCancel.Location = new Point(229, 75);
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
            _buttonOk.Location = new Point(146, 75);
            _buttonOk.Margin = new Padding(2, 2, 2, 2);
            _buttonOk.Name = "_buttonOk";
            _buttonOk.Size = new Size(78, 26);
            _buttonOk.TabIndex = 8;
            _buttonOk.Text = "Ok";
            _buttonOk.UseVisualStyleBackColor = true;
            // 
            // _textBoxDescription
            // 
            _textBoxDescription.Location = new Point(120, 31);
            _textBoxDescription.Margin = new Padding(2, 2, 2, 2);
            _textBoxDescription.Name = "_textBoxDescription";
            _textBoxDescription.Size = new Size(190, 23);
            _textBoxDescription.TabIndex = 10;
            // 
            // ManageObjectiveDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(318, 112);
            Controls.Add(_textBoxDescription);
            Controls.Add(_buttonCancel);
            Controls.Add(_buttonOk);
            Controls.Add(_textBoxName);
            Controls.Add(_labelDescription);
            Controls.Add(_labelName);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2, 2, 2, 2);
            Name = "ManageObjectiveDialog";
            Text = "Objective";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label _labelName;
        private Label _labelDescription;
        private TextBox _textBoxName;
        private Button _buttonCancel;
        private Button _buttonOk;
        private TextBox _textBoxDescription;
    }
}
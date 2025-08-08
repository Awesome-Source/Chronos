namespace Chronos.Views.Dialogs
{
    partial class StartActivityDialog
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(StartActivityDialog));
            _labelActivity = new Label();
            _labelObjective = new Label();
            _buttonCancel = new Button();
            _buttonOk = new Button();
            _comboBoxActivity = new ComboBox();
            _comboBoxObjective = new ComboBox();
            _labelPlannedActivity = new Label();
            _checkBoxPlannedActivity = new CheckBox();
            SuspendLayout();
            // 
            // _labelActivity
            // 
            _labelActivity.AutoSize = true;
            _labelActivity.Location = new Point(12, 15);
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
            _buttonCancel.Location = new Point(330, 161);
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
            _buttonOk.Location = new Point(212, 161);
            _buttonOk.Name = "_buttonOk";
            _buttonOk.Size = new Size(112, 34);
            _buttonOk.TabIndex = 8;
            _buttonOk.Text = "Ok";
            _buttonOk.UseVisualStyleBackColor = true;
            // 
            // _comboBoxActivity
            // 
            _comboBoxActivity.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboBoxActivity.FormattingEnabled = true;
            _comboBoxActivity.Location = new Point(143, 12);
            _comboBoxActivity.Name = "_comboBoxActivity";
            _comboBoxActivity.Size = new Size(298, 33);
            _comboBoxActivity.TabIndex = 10;
            // 
            // _comboBoxObjective
            // 
            _comboBoxObjective.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboBoxObjective.FormattingEnabled = true;
            _comboBoxObjective.Location = new Point(144, 51);
            _comboBoxObjective.Name = "_comboBoxObjective";
            _comboBoxObjective.Size = new Size(298, 33);
            _comboBoxObjective.TabIndex = 11;
            // 
            // _labelPlannedActivity
            // 
            _labelPlannedActivity.AutoSize = true;
            _labelPlannedActivity.Location = new Point(12, 90);
            _labelPlannedActivity.Name = "_labelPlannedActivity";
            _labelPlannedActivity.Size = new Size(94, 25);
            _labelPlannedActivity.TabIndex = 12;
            _labelPlannedActivity.Text = "Is planned";
            // 
            // _checkBoxPlannedActivity
            // 
            _checkBoxPlannedActivity.AutoSize = true;
            _checkBoxPlannedActivity.Location = new Point(144, 94);
            _checkBoxPlannedActivity.Name = "_checkBoxPlannedActivity";
            _checkBoxPlannedActivity.Size = new Size(22, 21);
            _checkBoxPlannedActivity.TabIndex = 13;
            _checkBoxPlannedActivity.UseVisualStyleBackColor = true;
            // 
            // StartActivityDialog
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(454, 207);
            Controls.Add(_checkBoxPlannedActivity);
            Controls.Add(_labelPlannedActivity);
            Controls.Add(_comboBoxObjective);
            Controls.Add(_comboBoxActivity);
            Controls.Add(_buttonCancel);
            Controls.Add(_buttonOk);
            Controls.Add(_labelObjective);
            Controls.Add(_labelActivity);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "StartActivityDialog";
            Text = "Start Activity";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label _labelActivity;
        private Label _labelObjective;
        private Button _buttonCancel;
        private Button _buttonOk;
        private ComboBox _comboBoxActivity;
        private ComboBox _comboBoxObjective;
        private Label _labelPlannedActivity;
        private CheckBox _checkBoxPlannedActivity;
    }
}
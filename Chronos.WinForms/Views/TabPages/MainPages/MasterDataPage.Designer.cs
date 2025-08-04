namespace Chronos.Views.TabPages
{
    partial class MasterDataPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _buttonTimeAccounts = new Button();
            _buttonActivities = new Button();
            _buttonObjectives = new Button();
            _panelContent = new Panel();
            SuspendLayout();
            // 
            // _buttonTimeAccounts
            // 
            _buttonTimeAccounts.FlatAppearance.BorderSize = 0;
            _buttonTimeAccounts.FlatStyle = FlatStyle.Flat;
            _buttonTimeAccounts.Location = new Point(3, 3);
            _buttonTimeAccounts.Name = "_buttonTimeAccounts";
            _buttonTimeAccounts.Size = new Size(175, 34);
            _buttonTimeAccounts.TabIndex = 0;
            _buttonTimeAccounts.Text = "Time Accounts";
            _buttonTimeAccounts.UseVisualStyleBackColor = true;
            _buttonTimeAccounts.Click += ButtonTimeAccounts_Click;
            // 
            // _buttonActivities
            // 
            _buttonActivities.FlatAppearance.BorderSize = 0;
            _buttonActivities.FlatStyle = FlatStyle.Flat;
            _buttonActivities.Location = new Point(180, 3);
            _buttonActivities.Name = "_buttonActivities";
            _buttonActivities.Size = new Size(179, 34);
            _buttonActivities.TabIndex = 1;
            _buttonActivities.Text = "Activities";
            _buttonActivities.UseVisualStyleBackColor = true;
            _buttonActivities.Click += ButtonActivities_Click;
            // 
            // _buttonObjectives
            // 
            _buttonObjectives.FlatAppearance.BorderSize = 0;
            _buttonObjectives.FlatStyle = FlatStyle.Flat;
            _buttonObjectives.Location = new Point(356, 3);
            _buttonObjectives.Name = "_buttonObjectives";
            _buttonObjectives.Size = new Size(184, 34);
            _buttonObjectives.TabIndex = 2;
            _buttonObjectives.Text = "Objectives";
            _buttonObjectives.UseVisualStyleBackColor = true;
            _buttonObjectives.Click += ButtonObjectives_Click;
            // 
            // _panelContent
            // 
            _panelContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _panelContent.BackColor = Color.DarkGray;
            _panelContent.Location = new Point(0, 32);
            _panelContent.Name = "_panelContent";
            _panelContent.Size = new Size(1375, 711);
            _panelContent.TabIndex = 3;
            // 
            // MasterDataPage
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            Controls.Add(_panelContent);
            Controls.Add(_buttonObjectives);
            Controls.Add(_buttonActivities);
            Controls.Add(_buttonTimeAccounts);
            Name = "MasterDataPage";
            Size = new Size(1375, 743);
            ResumeLayout(false);
        }

        #endregion

        private Button _buttonTimeAccounts;
        private Button _buttonActivities;
        private Button _buttonObjectives;
        private Panel _panelContent;
    }
}

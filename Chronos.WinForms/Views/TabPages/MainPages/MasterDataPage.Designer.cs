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
            _buttonTimeAccounts.Location = new Point(2, 2);
            _buttonTimeAccounts.Margin = new Padding(2, 2, 2, 0);
            _buttonTimeAccounts.Name = "_buttonTimeAccounts";
            _buttonTimeAccounts.Size = new Size(122, 26);
            _buttonTimeAccounts.TabIndex = 0;
            _buttonTimeAccounts.Text = "Time Accounts";
            _buttonTimeAccounts.UseVisualStyleBackColor = true;
            _buttonTimeAccounts.Click += ButtonTimeAccounts_Click;
            // 
            // _buttonActivities
            // 
            _buttonActivities.FlatAppearance.BorderSize = 0;
            _buttonActivities.FlatStyle = FlatStyle.Flat;
            _buttonActivities.Location = new Point(126, 2);
            _buttonActivities.Margin = new Padding(2, 2, 2, 0);
            _buttonActivities.Name = "_buttonActivities";
            _buttonActivities.Size = new Size(125, 26);
            _buttonActivities.TabIndex = 1;
            _buttonActivities.Text = "Activities";
            _buttonActivities.UseVisualStyleBackColor = true;
            _buttonActivities.Click += ButtonActivities_Click;
            // 
            // _buttonObjectives
            // 
            _buttonObjectives.FlatAppearance.BorderSize = 0;
            _buttonObjectives.FlatStyle = FlatStyle.Flat;
            _buttonObjectives.Location = new Point(249, 2);
            _buttonObjectives.Margin = new Padding(2, 2, 2, 0);
            _buttonObjectives.Name = "_buttonObjectives";
            _buttonObjectives.Size = new Size(129, 26);
            _buttonObjectives.TabIndex = 2;
            _buttonObjectives.Text = "Objectives";
            _buttonObjectives.UseVisualStyleBackColor = true;
            _buttonObjectives.Click += ButtonObjectives_Click;
            // 
            // _panelContent
            // 
            _panelContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _panelContent.BackColor = Color.DarkGray;
            _panelContent.Location = new Point(0, 26);
            _panelContent.Margin = new Padding(2, 0, 2, 2);
            _panelContent.Name = "_panelContent";
            _panelContent.Size = new Size(962, 418);
            _panelContent.TabIndex = 3;
            // 
            // MasterDataPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            Controls.Add(_panelContent);
            Controls.Add(_buttonObjectives);
            Controls.Add(_buttonActivities);
            Controls.Add(_buttonTimeAccounts);
            Margin = new Padding(2);
            Name = "MasterDataPage";
            Size = new Size(962, 446);
            ResumeLayout(false);
        }

        #endregion

        private Button _buttonTimeAccounts;
        private Button _buttonActivities;
        private Button _buttonObjectives;
        private Panel _panelContent;
    }
}

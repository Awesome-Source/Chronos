namespace Chronos.Views.TabPages
{
    partial class ActivityPage
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
            _gridActionBar = new Chronos.Views.Common.GridActionBar();
            _gridActivities = new Chronos.WinForms.Views.Common.ReadOnlyGrid();
            SuspendLayout();
            // 
            // _gridActionBar
            // 
            _gridActionBar.BackColor = Color.DarkGray;
            _gridActionBar.Location = new Point(0, 0);
            _gridActionBar.Name = "_gridActionBar";
            _gridActionBar.Size = new Size(587, 66);
            _gridActionBar.TabIndex = 5;
            _gridActionBar.AddClicked += GridActionBar_AddClicked;
            _gridActionBar.EditClicked += GridActionBar_EditClicked;
            _gridActionBar.RemoveClicked += GridActionBar_RemoveClicked;
            // 
            // _gridActivities
            // 
            _gridActivities.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _gridActivities.Location = new Point(0, 72);
            _gridActivities.Name = "_gridActivities";
            _gridActivities.Size = new Size(587, 514);
            _gridActivities.TabIndex = 6;
            // 
            // ActivityPage
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkGray;
            Controls.Add(_gridActivities);
            Controls.Add(_gridActionBar);
            Name = "ActivityPage";
            Size = new Size(587, 586);
            ResumeLayout(false);
        }

        #endregion
        private Common.GridActionBar _gridActionBar;
        private WinForms.Views.Common.ReadOnlyGrid _gridActivities;
    }
}

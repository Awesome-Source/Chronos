namespace Chronos.Views.TabPages
{
    partial class DashboardPage
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
            _gridTimeAccountStatistics = new Chronos.WinForms.Views.Common.ReadOnlyGrid();
            _labelTimeAccountStatistics = new Label();
            SuspendLayout();
            // 
            // _gridTimeAccountStatistics
            // 
            _gridTimeAccountStatistics.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _gridTimeAccountStatistics.Location = new Point(3, 82);
            _gridTimeAccountStatistics.Name = "_gridTimeAccountStatistics";
            _gridTimeAccountStatistics.Size = new Size(1445, 724);
            _gridTimeAccountStatistics.TabIndex = 3;
            // 
            // _labelTimeAccountStatistics
            // 
            _labelTimeAccountStatistics.AutoSize = true;
            _labelTimeAccountStatistics.Location = new Point(3, 26);
            _labelTimeAccountStatistics.Name = "_labelTimeAccountStatistics";
            _labelTimeAccountStatistics.Size = new Size(411, 25);
            _labelTimeAccountStatistics.TabIndex = 4;
            _labelTimeAccountStatistics.Text = "Accumulated durations per Time Account (all time)";
            // 
            // DashboardPage
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            Controls.Add(_labelTimeAccountStatistics);
            Controls.Add(_gridTimeAccountStatistics);
            Name = "DashboardPage";
            Size = new Size(1451, 809);
            VisibleChanged += DashboardPage_VisibleChanged;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private WinForms.Views.Common.ReadOnlyGrid _gridTimeAccountStatistics;
        private Label _labelTimeAccountStatistics;
    }
}

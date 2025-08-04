namespace Chronos.Views.TabPages
{
    partial class TrackingPage
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
            _buttonStart = new Button();
            _buttonNew = new Button();
            _buttonStop = new Button();
            _gridTrackingTargets = new Chronos.WinForms.Views.Common.ReadOnlyGrid();
            _labelTotalTime = new Label();
            SuspendLayout();
            // 
            // _buttonStart
            // 
            _buttonStart.FlatStyle = FlatStyle.Flat;
            _buttonStart.Location = new Point(16, 32);
            _buttonStart.Margin = new Padding(16, 32, 3, 32);
            _buttonStart.Name = "_buttonStart";
            _buttonStart.Size = new Size(206, 34);
            _buttonStart.TabIndex = 1;
            _buttonStart.Text = "Start Selected";
            _buttonStart.UseVisualStyleBackColor = true;
            _buttonStart.Click += ButtonStart_Click;
            // 
            // _buttonNew
            // 
            _buttonNew.FlatStyle = FlatStyle.Flat;
            _buttonNew.Location = new Point(228, 32);
            _buttonNew.Margin = new Padding(3, 32, 3, 32);
            _buttonNew.Name = "_buttonNew";
            _buttonNew.Size = new Size(206, 34);
            _buttonNew.TabIndex = 2;
            _buttonNew.Text = "New Actvity";
            _buttonNew.UseVisualStyleBackColor = true;
            _buttonNew.Click += ButtonNew_Click;
            // 
            // _buttonStop
            // 
            _buttonStop.FlatStyle = FlatStyle.Flat;
            _buttonStop.Location = new Point(440, 32);
            _buttonStop.Margin = new Padding(3, 32, 3, 32);
            _buttonStop.Name = "_buttonStop";
            _buttonStop.Size = new Size(206, 34);
            _buttonStop.TabIndex = 3;
            _buttonStop.Text = "Stop Tracking";
            _buttonStop.UseVisualStyleBackColor = true;
            _buttonStop.Click += ButtonStop_Click;
            // 
            // _gridTrackingTargets
            // 
            _gridTrackingTargets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _gridTrackingTargets.Location = new Point(0, 101);
            _gridTrackingTargets.Name = "_gridTrackingTargets";
            _gridTrackingTargets.Size = new Size(1326, 713);
            _gridTrackingTargets.TabIndex = 4;
            // 
            // _labelTotalTime
            // 
            _labelTotalTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _labelTotalTime.Location = new Point(1076, 32);
            _labelTotalTime.Margin = new Padding(3, 0, 32, 0);
            _labelTotalTime.Name = "_labelTotalTime";
            _labelTotalTime.Size = new Size(218, 34);
            _labelTotalTime.TabIndex = 5;
            _labelTotalTime.Text = "Total Time:";
            _labelTotalTime.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TrackingPage
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            Controls.Add(_labelTotalTime);
            Controls.Add(_gridTrackingTargets);
            Controls.Add(_buttonStop);
            Controls.Add(_buttonNew);
            Controls.Add(_buttonStart);
            Name = "TrackingPage";
            Size = new Size(1326, 814);
            ResumeLayout(false);
        }

        #endregion
        private Button _buttonStart;
        private Button _buttonNew;
        private Button _buttonStop;
        private WinForms.Views.Common.ReadOnlyGrid _gridTrackingTargets;
        private Label _labelTotalTime;
    }
}

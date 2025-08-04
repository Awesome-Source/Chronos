namespace Chronos
{
    partial class MainView
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
            _panelLeft = new Panel();
            _panelTabHighlight = new Panel();
            _buttonStatistics = new Button();
            _buttonMasterData = new Button();
            _buttonTimeSheet = new Button();
            _buttonRecords = new Button();
            _buttonTracking = new Button();
            _buttonDashboard = new Button();
            _panelMainContent = new Panel();
            _panelLeft.SuspendLayout();
            SuspendLayout();
            // 
            // _panelLeft
            // 
            _panelLeft.BackColor = Color.FromArgb(40, 40, 40);
            _panelLeft.Controls.Add(_panelTabHighlight);
            _panelLeft.Controls.Add(_buttonStatistics);
            _panelLeft.Controls.Add(_buttonMasterData);
            _panelLeft.Controls.Add(_buttonTimeSheet);
            _panelLeft.Controls.Add(_buttonRecords);
            _panelLeft.Controls.Add(_buttonTracking);
            _panelLeft.Controls.Add(_buttonDashboard);
            _panelLeft.Dock = DockStyle.Left;
            _panelLeft.Location = new Point(0, 0);
            _panelLeft.Name = "_panelLeft";
            _panelLeft.Size = new Size(300, 969);
            _panelLeft.TabIndex = 0;
            // 
            // _panelTabHighlight
            // 
            _panelTabHighlight.BackColor = Color.LimeGreen;
            _panelTabHighlight.Location = new Point(12, 72);
            _panelTabHighlight.Name = "_panelTabHighlight";
            _panelTabHighlight.Size = new Size(10, 80);
            _panelTabHighlight.TabIndex = 1;
            // 
            // _buttonStatistics
            // 
            _buttonStatistics.FlatAppearance.BorderSize = 0;
            _buttonStatistics.FlatStyle = FlatStyle.Flat;
            _buttonStatistics.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _buttonStatistics.ForeColor = Color.White;
            _buttonStatistics.ImageAlign = ContentAlignment.MiddleLeft;
            _buttonStatistics.Location = new Point(30, 564);
            _buttonStatistics.Name = "_buttonStatistics";
            _buttonStatistics.Size = new Size(270, 80);
            _buttonStatistics.TabIndex = 3;
            _buttonStatistics.Text = "Statistics";
            _buttonStatistics.TextImageRelation = TextImageRelation.ImageBeforeText;
            _buttonStatistics.UseVisualStyleBackColor = true;
            _buttonStatistics.Click += ButtonStatistics_Click;
            // 
            // _buttonMasterData
            // 
            _buttonMasterData.FlatAppearance.BorderSize = 0;
            _buttonMasterData.FlatStyle = FlatStyle.Flat;
            _buttonMasterData.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _buttonMasterData.ForeColor = Color.White;
            _buttonMasterData.ImageAlign = ContentAlignment.MiddleLeft;
            _buttonMasterData.Location = new Point(30, 687);
            _buttonMasterData.Name = "_buttonMasterData";
            _buttonMasterData.Size = new Size(270, 80);
            _buttonMasterData.TabIndex = 3;
            _buttonMasterData.Text = "Master Data";
            _buttonMasterData.TextImageRelation = TextImageRelation.ImageBeforeText;
            _buttonMasterData.UseVisualStyleBackColor = true;
            _buttonMasterData.Click += ButtonMasterData_Click;
            // 
            // _buttonTimeSheet
            // 
            _buttonTimeSheet.FlatAppearance.BorderSize = 0;
            _buttonTimeSheet.FlatStyle = FlatStyle.Flat;
            _buttonTimeSheet.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _buttonTimeSheet.ForeColor = Color.White;
            _buttonTimeSheet.ImageAlign = ContentAlignment.MiddleLeft;
            _buttonTimeSheet.Location = new Point(30, 441);
            _buttonTimeSheet.Name = "_buttonTimeSheet";
            _buttonTimeSheet.Size = new Size(270, 80);
            _buttonTimeSheet.TabIndex = 3;
            _buttonTimeSheet.Text = "Time Sheet";
            _buttonTimeSheet.TextImageRelation = TextImageRelation.ImageBeforeText;
            _buttonTimeSheet.UseVisualStyleBackColor = true;
            _buttonTimeSheet.Click += ButtonTimeSheet_Click;
            // 
            // _buttonRecords
            // 
            _buttonRecords.FlatAppearance.BorderSize = 0;
            _buttonRecords.FlatStyle = FlatStyle.Flat;
            _buttonRecords.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _buttonRecords.ForeColor = Color.White;
            _buttonRecords.ImageAlign = ContentAlignment.MiddleLeft;
            _buttonRecords.Location = new Point(30, 318);
            _buttonRecords.Name = "_buttonRecords";
            _buttonRecords.Size = new Size(270, 80);
            _buttonRecords.TabIndex = 2;
            _buttonRecords.Text = "Records";
            _buttonRecords.TextImageRelation = TextImageRelation.ImageBeforeText;
            _buttonRecords.UseVisualStyleBackColor = true;
            _buttonRecords.Click += ButtonRecords_Click;
            // 
            // _buttonTracking
            // 
            _buttonTracking.FlatAppearance.BorderSize = 0;
            _buttonTracking.FlatStyle = FlatStyle.Flat;
            _buttonTracking.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _buttonTracking.ForeColor = Color.White;
            _buttonTracking.ImageAlign = ContentAlignment.MiddleLeft;
            _buttonTracking.Location = new Point(30, 195);
            _buttonTracking.Name = "_buttonTracking";
            _buttonTracking.Size = new Size(270, 80);
            _buttonTracking.TabIndex = 1;
            _buttonTracking.Text = "Tracking";
            _buttonTracking.TextImageRelation = TextImageRelation.ImageBeforeText;
            _buttonTracking.UseVisualStyleBackColor = true;
            _buttonTracking.Click += ButtonTracking_Click;
            // 
            // _buttonDashboard
            // 
            _buttonDashboard.FlatAppearance.BorderSize = 0;
            _buttonDashboard.FlatStyle = FlatStyle.Flat;
            _buttonDashboard.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _buttonDashboard.ForeColor = Color.White;
            _buttonDashboard.ImageAlign = ContentAlignment.MiddleLeft;
            _buttonDashboard.Location = new Point(30, 72);
            _buttonDashboard.Name = "_buttonDashboard";
            _buttonDashboard.Size = new Size(270, 80);
            _buttonDashboard.TabIndex = 0;
            _buttonDashboard.Text = "Dashboard";
            _buttonDashboard.TextImageRelation = TextImageRelation.ImageBeforeText;
            _buttonDashboard.UseVisualStyleBackColor = true;
            _buttonDashboard.Click += ButtonDashboard_Click;
            // 
            // _panelMainContent
            // 
            _panelMainContent.Dock = DockStyle.Fill;
            _panelMainContent.Location = new Point(300, 0);
            _panelMainContent.Name = "_panelMainContent";
            _panelMainContent.Size = new Size(1475, 969);
            _panelMainContent.TabIndex = 1;
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(1775, 969);
            Controls.Add(_panelMainContent);
            Controls.Add(_panelLeft);
            Name = "MainView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chronos";
            FormClosing += MainView_FormClosing;
            _panelLeft.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel _panelLeft;
        private Button _buttonDashboard;
        private Button _buttonStatistics;
        private Button _buttonMasterData;
        private Button _buttonTimeSheet;
        private Button _buttonRecords;
        private Button _buttonTracking;
        private Panel _panelTabHighlight;
        private Panel _panelMainContent;
    }
}
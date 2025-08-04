namespace Chronos.Views.TabPages
{
    partial class TimeSheetPage
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
            _dateTimePicker = new DateTimePicker();
            _gridTimeSheet = new Chronos.WinForms.Views.Common.ReadOnlyGrid();
            _gridTimeAccountSheet = new Chronos.WinForms.Views.Common.ReadOnlyGrid();
            _gridActivitySheet = new Chronos.WinForms.Views.Common.ReadOnlyGrid();
            _gridObjectiveSheet = new Chronos.WinForms.Views.Common.ReadOnlyGrid();
            _gridDayStatistics = new Chronos.WinForms.Views.Common.ReadOnlyGrid();
            _tableLayoutPanel = new TableLayoutPanel();
            _tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _dateTimePicker
            // 
            _dateTimePicker.Format = DateTimePickerFormat.Short;
            _dateTimePicker.Location = new Point(32, 32);
            _dateTimePicker.Margin = new Padding(32);
            _dateTimePicker.Name = "_dateTimePicker";
            _dateTimePicker.Size = new Size(300, 31);
            _dateTimePicker.TabIndex = 0;
            _dateTimePicker.ValueChanged += DateTimePicker_ValueChanged;
            // 
            // _gridTimeSheet
            // 
            _gridTimeSheet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _tableLayoutPanel.SetColumnSpan(_gridTimeSheet, 4);
            _gridTimeSheet.Location = new Point(3, 3);
            _gridTimeSheet.Name = "_gridTimeSheet";
            _gridTimeSheet.Size = new Size(1194, 284);
            _gridTimeSheet.TabIndex = 1;
            // 
            // _gridTimeAccountSheet
            // 
            _gridTimeAccountSheet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _gridTimeAccountSheet.Location = new Point(3, 293);
            _gridTimeAccountSheet.Name = "_gridTimeAccountSheet";
            _gridTimeAccountSheet.Size = new Size(294, 285);
            _gridTimeAccountSheet.TabIndex = 2;
            // 
            // _gridActivitySheet
            // 
            _gridActivitySheet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _gridActivitySheet.Location = new Point(303, 293);
            _gridActivitySheet.Name = "_gridActivitySheet";
            _gridActivitySheet.Size = new Size(294, 285);
            _gridActivitySheet.TabIndex = 3;
            // 
            // _gridObjectiveSheet
            // 
            _gridObjectiveSheet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _gridObjectiveSheet.Location = new Point(603, 293);
            _gridObjectiveSheet.Name = "_gridObjectiveSheet";
            _gridObjectiveSheet.Size = new Size(294, 285);
            _gridObjectiveSheet.TabIndex = 4;
            // 
            // _gridDayStatistics
            // 
            _gridDayStatistics.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _gridDayStatistics.Location = new Point(903, 293);
            _gridDayStatistics.Name = "_gridDayStatistics";
            _gridDayStatistics.Size = new Size(294, 285);
            _gridDayStatistics.TabIndex = 5;
            // 
            // _tableLayoutPanel
            // 
            _tableLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _tableLayoutPanel.ColumnCount = 4;
            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _tableLayoutPanel.Controls.Add(_gridTimeAccountSheet, 0, 1);
            _tableLayoutPanel.Controls.Add(_gridTimeSheet, 0, 0);
            _tableLayoutPanel.Controls.Add(_gridDayStatistics, 3, 1);
            _tableLayoutPanel.Controls.Add(_gridActivitySheet, 1, 1);
            _tableLayoutPanel.Controls.Add(_gridObjectiveSheet, 2, 1);
            _tableLayoutPanel.Location = new Point(0, 98);
            _tableLayoutPanel.Name = "_tableLayoutPanel";
            _tableLayoutPanel.RowCount = 2;
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            _tableLayoutPanel.Size = new Size(1200, 581);
            _tableLayoutPanel.TabIndex = 6;
            // 
            // TimeSheetPage
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            Controls.Add(_tableLayoutPanel);
            Controls.Add(_dateTimePicker);
            Name = "TimeSheetPage";
            Size = new Size(1200, 679);
            VisibleChanged += TimeSheetPage_VisibleChanged;
            _tableLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DateTimePicker _dateTimePicker;
        private WinForms.Views.Common.ReadOnlyGrid _gridTimeSheet;
        private WinForms.Views.Common.ReadOnlyGrid _gridTimeAccountSheet;
        private WinForms.Views.Common.ReadOnlyGrid _gridActivitySheet;
        private WinForms.Views.Common.ReadOnlyGrid _gridObjectiveSheet;
        private WinForms.Views.Common.ReadOnlyGrid _gridDayStatistics;
        private TableLayoutPanel _tableLayoutPanel;
    }
}

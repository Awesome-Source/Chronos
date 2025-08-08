namespace Chronos.Views.TabPages
{
    partial class RecordsPage
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
            _gridRecords = new Chronos.WinForms.Views.Common.ReadOnlyGrid();
            _dateTimePicker = new DateTimePicker();
            _gridActionBar = new Chronos.Views.Common.GridActionBar();
            SuspendLayout();
            // 
            // _gridRecords
            // 
            _gridRecords.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _gridRecords.Location = new Point(0, 59);
            _gridRecords.Margin = new Padding(1, 1, 1, 1);
            _gridRecords.Name = "_gridRecords";
            _gridRecords.Size = new Size(1056, 410);
            _gridRecords.TabIndex = 0;
            // 
            // _dateTimePicker
            // 
            _dateTimePicker.Format = DateTimePickerFormat.Short;
            _dateTimePicker.Location = new Point(22, 19);
            _dateTimePicker.Margin = new Padding(22, 19, 22, 19);
            _dateTimePicker.Name = "_dateTimePicker";
            _dateTimePicker.Size = new Size(211, 23);
            _dateTimePicker.TabIndex = 1;
            _dateTimePicker.ValueChanged += DateTimePicker_ValueChanged;
            // 
            // _gridActionBar
            // 
            _gridActionBar.BackColor = Color.Silver;
            _gridActionBar.Location = new Point(257, 10);
            _gridActionBar.Margin = new Padding(1, 1, 1, 1);
            _gridActionBar.Name = "_gridActionBar";
            _gridActionBar.Size = new Size(274, 42);
            _gridActionBar.TabIndex = 2;
            _gridActionBar.AddClicked += GridActionBar_AddClicked;
            _gridActionBar.EditClicked += GridActionBar_EditClicked;
            _gridActionBar.RemoveClicked += GridActionBar_RemoveClicked;
            // 
            // RecordsPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            Controls.Add(_gridActionBar);
            Controls.Add(_dateTimePicker);
            Controls.Add(_gridRecords);
            Margin = new Padding(2, 2, 2, 2);
            Name = "RecordsPage";
            Size = new Size(1056, 469);
            VisibleChanged += RecordsPage_VisibleChanged;
            ResumeLayout(false);
        }

        #endregion

        private WinForms.Views.Common.ReadOnlyGrid _gridRecords;
        private DateTimePicker _dateTimePicker;
        private Common.GridActionBar _gridActionBar;
    }
}

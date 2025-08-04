namespace Chronos.Views.TabPages
{
    partial class ObjectivePage
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
            _gridObjectives = new Chronos.WinForms.Views.Common.ReadOnlyGrid();
            SuspendLayout();
            // 
            // _gridActionBar
            // 
            _gridActionBar.BackColor = Color.DarkGray;
            _gridActionBar.Location = new Point(0, 0);
            _gridActionBar.Name = "_gridActionBar";
            _gridActionBar.Size = new Size(574, 66);
            _gridActionBar.TabIndex = 9;
            _gridActionBar.AddClicked += GridActionBar_AddClicked;
            _gridActionBar.EditClicked += GridActionBar_EditClicked;
            _gridActionBar.RemoveClicked += GridActionBar_RemoveClicked;
            // 
            // _gridObjectives
            // 
            _gridObjectives.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _gridObjectives.Location = new Point(0, 72);
            _gridObjectives.Name = "_gridObjectives";
            _gridObjectives.Size = new Size(574, 426);
            _gridObjectives.TabIndex = 10;
            // 
            // ObjectivePage
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkGray;
            Controls.Add(_gridObjectives);
            Controls.Add(_gridActionBar);
            Name = "ObjectivePage";
            Size = new Size(574, 498);
            ResumeLayout(false);
        }

        #endregion
        private Common.GridActionBar _gridActionBar;
        private WinForms.Views.Common.ReadOnlyGrid _gridObjectives;
    }
}

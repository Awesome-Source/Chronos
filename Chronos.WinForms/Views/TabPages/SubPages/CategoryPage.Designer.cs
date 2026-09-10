namespace Chronos.Views.TabPages
{
    partial class CategoryPage
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
            _gridCategories = new Chronos.WinForms.Views.Common.ReadOnlyGrid();
            SuspendLayout();
            // 
            // _gridActionBar
            // 
            _gridActionBar.BackColor = Color.DarkGray;
            _gridActionBar.Location = new Point(0, 0);
            _gridActionBar.Margin = new Padding(1);
            _gridActionBar.Name = "_gridActionBar";
            _gridActionBar.Size = new Size(402, 42);
            _gridActionBar.TabIndex = 9;
            _gridActionBar.AddClicked += GridActionBar_AddClicked;
            _gridActionBar.EditClicked += GridActionBar_EditClicked;
            _gridActionBar.RemoveClicked += GridActionBar_RemoveClicked;
            // 
            // _gridCategories
            // 
            _gridCategories.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _gridCategories.Location = new Point(0, 44);
            _gridCategories.Margin = new Padding(1);
            _gridCategories.Name = "_gridCategories";
            _gridCategories.Size = new Size(402, 255);
            _gridCategories.TabIndex = 10;
            // 
            // CategoryPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkGray;
            Controls.Add(_gridCategories);
            Controls.Add(_gridActionBar);
            Margin = new Padding(2);
            Name = "CategoryPage";
            Size = new Size(402, 299);
            ResumeLayout(false);
        }

        #endregion
        private Common.GridActionBar _gridActionBar;
        private WinForms.Views.Common.ReadOnlyGrid _gridCategories;
    }
}

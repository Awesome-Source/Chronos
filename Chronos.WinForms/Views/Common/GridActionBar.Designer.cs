namespace Chronos.Views.Common
{
    partial class GridActionBar
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
            _buttonRemove = new Button();
            _buttonEdit = new Button();
            _buttonAdd = new Button();
            SuspendLayout();
            // 
            // _buttonRemove
            // 
            _buttonRemove.FlatStyle = FlatStyle.Flat;
            _buttonRemove.Location = new Point(176, 8);
            _buttonRemove.Margin = new Padding(2, 2, 2, 2);
            _buttonRemove.Name = "_buttonRemove";
            _buttonRemove.Size = new Size(78, 26);
            _buttonRemove.TabIndex = 6;
            _buttonRemove.Text = "Remove";
            _buttonRemove.UseVisualStyleBackColor = true;
            _buttonRemove.Click += ButtonRemove_Click;
            // 
            // _buttonEdit
            // 
            _buttonEdit.FlatStyle = FlatStyle.Flat;
            _buttonEdit.Location = new Point(94, 8);
            _buttonEdit.Margin = new Padding(2, 2, 2, 2);
            _buttonEdit.Name = "_buttonEdit";
            _buttonEdit.Size = new Size(78, 26);
            _buttonEdit.TabIndex = 5;
            _buttonEdit.Text = "Edit";
            _buttonEdit.UseVisualStyleBackColor = true;
            _buttonEdit.Click += ButtonEdit_Click;
            // 
            // _buttonAdd
            // 
            _buttonAdd.FlatStyle = FlatStyle.Flat;
            _buttonAdd.Location = new Point(11, 8);
            _buttonAdd.Margin = new Padding(2, 2, 2, 2);
            _buttonAdd.Name = "_buttonAdd";
            _buttonAdd.Size = new Size(78, 26);
            _buttonAdd.TabIndex = 4;
            _buttonAdd.Text = "Add";
            _buttonAdd.UseVisualStyleBackColor = true;
            _buttonAdd.Click += ButtonAdd_Click;
            // 
            // GridActionBar
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkGray;
            Controls.Add(_buttonRemove);
            Controls.Add(_buttonEdit);
            Controls.Add(_buttonAdd);
            Margin = new Padding(2, 2, 2, 2);
            Name = "GridActionBar";
            Size = new Size(266, 42);
            ResumeLayout(false);
        }

        #endregion

        private Button _buttonRemove;
        private Button _buttonEdit;
        private Button _buttonAdd;
    }
}

namespace Chronos.Views.TabPages
{
    partial class StatisticsPage
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(111, 69);
            label1.Name = "label1";
            label1.Size = new Size(290, 25);
            label1.TabIndex = 0;
            label1.Text = "Percentage per Time Account (ever)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(111, 135);
            label2.Name = "label2";
            label2.Size = new Size(322, 25);
            label2.TabIndex = 1;
            label2.Text = "Percentage per Time Account (last year)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(111, 216);
            label3.Name = "label3";
            label3.Size = new Size(329, 25);
            label3.TabIndex = 2;
            label3.Text = "Percentage per Time Account (last week)";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(111, 289);
            label4.Name = "label4";
            label4.Size = new Size(303, 25);
            label4.TabIndex = 3;
            label4.Text = "Percentage per Time Account (today)";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(692, 85);
            label5.Name = "label5";
            label5.Size = new Size(296, 25);
            label5.TabIndex = 4;
            label5.Text = "Context switches per day (last week)";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(692, 144);
            label6.Name = "label6";
            label6.Size = new Size(270, 25);
            label6.TabIndex = 5;
            label6.Text = "Context switches per day (today)";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(692, 329);
            label7.Name = "label7";
            label7.Size = new Size(338, 25);
            label7.TabIndex = 6;
            label7.Text = "Percentage planned activity vs unplanned";
            // 
            // StatisticsPage
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "StatisticsPage";
            Size = new Size(1149, 723);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
    }
}

namespace Example_Project
{
    partial class CreateWorkerPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.ID_label = new System.Windows.Forms.Label();
            this.ID_textBox = new System.Windows.Forms.TextBox();
            this.Name_label1 = new System.Windows.Forms.Label();
            this.Name_textBox = new System.Windows.Forms.TextBox();
            this.Job_label2 = new System.Windows.Forms.Label();
            this.Add_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button_back = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // ID_label
            //
            this.ID_label.AutoSize = true;
            this.ID_label.Font = new System.Drawing.Font("David", 14.25F);
            this.ID_label.Location = new System.Drawing.Point(614, 95);
            this.ID_label.Name = "ID_label";
            this.ID_label.Size = new System.Drawing.Size(97, 19);
            this.ID_label.TabIndex = 2;
            this.ID_label.Text = " תעודת זהות";
            //
            // ID_textBox
            //
            this.ID_textBox.Font = new System.Drawing.Font("David", 12F);
            this.ID_textBox.Location = new System.Drawing.Point(400, 92);
            this.ID_textBox.Name = "ID_textBox";
            this.ID_textBox.Size = new System.Drawing.Size(200, 27);
            this.ID_textBox.TabIndex = 3;
            //
            // Name_label1
            //
            this.Name_label1.AutoSize = true;
            this.Name_label1.Font = new System.Drawing.Font("David", 14.25F);
            this.Name_label1.Location = new System.Drawing.Point(644, 145);
            this.Name_label1.Name = "Name_label1";
            this.Name_label1.Size = new System.Drawing.Size(67, 19);
            this.Name_label1.TabIndex = 4;
            this.Name_label1.Text = "שם מלא";
            //
            // Name_textBox
            //
            this.Name_textBox.Font = new System.Drawing.Font("David", 12F);
            this.Name_textBox.Location = new System.Drawing.Point(400, 142);
            this.Name_textBox.Name = "Name_textBox";
            this.Name_textBox.Size = new System.Drawing.Size(200, 27);
            this.Name_textBox.TabIndex = 5;
            //
            // Job_label2
            //
            this.Job_label2.AutoSize = true;
            this.Job_label2.Font = new System.Drawing.Font("David", 14.25F);
            this.Job_label2.Location = new System.Drawing.Point(657, 195);
            this.Job_label2.Name = "Job_label2";
            this.Job_label2.Size = new System.Drawing.Size(54, 19);
            this.Job_label2.TabIndex = 6;
            this.Job_label2.Text = "תפקיד";
            //
            // Add_button
            //
            this.Add_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Add_button.Font = new System.Drawing.Font("David", 14.25F, System.Drawing.FontStyle.Bold);
            this.Add_button.Location = new System.Drawing.Point(100, 150);
            this.Add_button.Name = "Add_button";
            this.Add_button.Size = new System.Drawing.Size(200, 80);
            this.Add_button.TabIndex = 8;
            this.Add_button.Text = "הוסף למערכת";
            this.Add_button.UseVisualStyleBackColor = true;
            this.Add_button.Click += new System.EventHandler(this.Add_button_Click);
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("David", 36F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(200, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(500, 55);
            this.label3.TabIndex = 9;
            this.label3.Text = "הזן את הפרטים הבאים";
            //
            // comboBox1
            //
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(400, 195);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(200, 24);
            this.comboBox1.TabIndex = 10;
            //
            // button_back
            //
            this.button_back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_back.Font = new System.Drawing.Font("David", 14.25F, System.Drawing.FontStyle.Bold);
            this.button_back.Location = new System.Drawing.Point(350, 370);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(131, 42);
            this.button_back.TabIndex = 11;
            this.button_back.Text = "חזרה";
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            //
            // CreateWorkerPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Add_button);
            this.Controls.Add(this.Job_label2);
            this.Controls.Add(this.Name_textBox);
            this.Controls.Add(this.Name_label1);
            this.Controls.Add(this.ID_textBox);
            this.Controls.Add(this.ID_label);
            this.Name = "CreateWorkerPanel";
            this.Size = new System.Drawing.Size(900, 500);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label ID_label;
        private System.Windows.Forms.TextBox ID_textBox;
        private System.Windows.Forms.Label Name_label1;
        private System.Windows.Forms.TextBox Name_textBox;
        private System.Windows.Forms.Label Job_label2;
        private System.Windows.Forms.Button Add_button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button_back;
    }
}

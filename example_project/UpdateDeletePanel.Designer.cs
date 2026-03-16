namespace Example_Project
{
    partial class UpdateDeletePanel
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
            this.label_title = new System.Windows.Forms.Label();
            this.label_id = new System.Windows.Forms.Label();
            this.textBox_id = new System.Windows.Forms.TextBox();
            this.button_search = new System.Windows.Forms.Button();
            this.label_name = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label_titleField = new System.Windows.Forms.Label();
            this.comboBox_title = new System.Windows.Forms.ComboBox();
            this.button_update = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_back = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // label_title
            //
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("David", 36F, System.Drawing.FontStyle.Bold);
            this.label_title.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label_title.Location = new System.Drawing.Point(200, 20);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(500, 55);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "עדכון / מחיקת עובד";
            //
            // label_id
            //
            this.label_id.AutoSize = true;
            this.label_id.Font = new System.Drawing.Font("David", 14.25F);
            this.label_id.Location = new System.Drawing.Point(614, 100);
            this.label_id.Name = "label_id";
            this.label_id.Size = new System.Drawing.Size(97, 19);
            this.label_id.TabIndex = 1;
            this.label_id.Text = "תעודת זהות";
            //
            // textBox_id
            //
            this.textBox_id.Font = new System.Drawing.Font("David", 12F);
            this.textBox_id.Location = new System.Drawing.Point(400, 97);
            this.textBox_id.Name = "textBox_id";
            this.textBox_id.Size = new System.Drawing.Size(200, 27);
            this.textBox_id.TabIndex = 2;
            //
            // button_search
            //
            this.button_search.Font = new System.Drawing.Font("David", 10F, System.Drawing.FontStyle.Bold);
            this.button_search.Location = new System.Drawing.Point(300, 97);
            this.button_search.Name = "button_search";
            this.button_search.Size = new System.Drawing.Size(80, 27);
            this.button_search.TabIndex = 3;
            this.button_search.Text = "חפש";
            this.button_search.UseVisualStyleBackColor = true;
            this.button_search.Click += new System.EventHandler(this.button_search_Click);
            //
            // label_name
            //
            this.label_name.AutoSize = true;
            this.label_name.Font = new System.Drawing.Font("David", 14.25F);
            this.label_name.Location = new System.Drawing.Point(644, 150);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(67, 19);
            this.label_name.TabIndex = 4;
            this.label_name.Text = "שם מלא";
            //
            // textBox_name
            //
            this.textBox_name.Font = new System.Drawing.Font("David", 12F);
            this.textBox_name.Location = new System.Drawing.Point(400, 147);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(200, 27);
            this.textBox_name.TabIndex = 5;
            //
            // label_titleField
            //
            this.label_titleField.AutoSize = true;
            this.label_titleField.Font = new System.Drawing.Font("David", 14.25F);
            this.label_titleField.Location = new System.Drawing.Point(657, 200);
            this.label_titleField.Name = "label_titleField";
            this.label_titleField.Size = new System.Drawing.Size(54, 19);
            this.label_titleField.TabIndex = 6;
            this.label_titleField.Text = "תפקיד";
            //
            // comboBox_title
            //
            this.comboBox_title.FormattingEnabled = true;
            this.comboBox_title.Location = new System.Drawing.Point(400, 200);
            this.comboBox_title.Name = "comboBox_title";
            this.comboBox_title.Size = new System.Drawing.Size(200, 24);
            this.comboBox_title.TabIndex = 7;
            //
            // button_update
            //
            this.button_update.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_update.Font = new System.Drawing.Font("David", 14.25F, System.Drawing.FontStyle.Bold);
            this.button_update.Location = new System.Drawing.Point(100, 150);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(150, 60);
            this.button_update.TabIndex = 8;
            this.button_update.Text = "עדכן";
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            //
            // button_delete
            //
            this.button_delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_delete.Font = new System.Drawing.Font("David", 14.25F, System.Drawing.FontStyle.Bold);
            this.button_delete.ForeColor = System.Drawing.Color.Red;
            this.button_delete.Location = new System.Drawing.Point(100, 230);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(150, 60);
            this.button_delete.TabIndex = 9;
            this.button_delete.Text = "מחק";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            //
            // button_back
            //
            this.button_back.Font = new System.Drawing.Font("David", 14.25F, System.Drawing.FontStyle.Bold);
            this.button_back.Location = new System.Drawing.Point(350, 370);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(131, 42);
            this.button_back.TabIndex = 10;
            this.button_back.Text = "חזרה";
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            //
            // UpdateDeletePanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.button_update);
            this.Controls.Add(this.comboBox_title);
            this.Controls.Add(this.label_titleField);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label_name);
            this.Controls.Add(this.button_search);
            this.Controls.Add(this.textBox_id);
            this.Controls.Add(this.label_id);
            this.Controls.Add(this.label_title);
            this.Name = "UpdateDeletePanel";
            this.Size = new System.Drawing.Size(900, 500);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Label label_id;
        private System.Windows.Forms.TextBox textBox_id;
        private System.Windows.Forms.Button button_search;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label_titleField;
        private System.Windows.Forms.ComboBox comboBox_title;
        private System.Windows.Forms.Button button_update;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_back;
    }
}

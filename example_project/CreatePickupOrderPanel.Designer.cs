namespace Example_Project
{
    partial class CreatePickupOrderPanel
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
            this.label_worker = new System.Windows.Forms.Label();
            this.comboBox_worker = new System.Windows.Forms.ComboBox();
            this.label_orderId = new System.Windows.Forms.Label();
            this.textBox_orderId = new System.Windows.Forms.TextBox();
            this.label_orderDate = new System.Windows.Forms.Label();
            this.dateTimePicker_orderDate = new System.Windows.Forms.DateTimePicker();
            this.label_totalPrice = new System.Windows.Forms.Label();
            this.textBox_totalPrice = new System.Windows.Forms.TextBox();
            this.label_pickupTime = new System.Windows.Forms.Label();
            this.dateTimePicker_pickupTime = new System.Windows.Forms.DateTimePicker();
            this.label_branch = new System.Windows.Forms.Label();
            this.textBox_branch = new System.Windows.Forms.TextBox();
            this.Add_button = new System.Windows.Forms.Button();
            this.button_back = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // label_title
            //
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.label_title.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label_title.Location = new System.Drawing.Point(250, 20);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(300, 39);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "הזמנת איסוף חדשה";
            //
            // label_worker
            //
            this.label_worker.AutoSize = true;
            this.label_worker.Font = new System.Drawing.Font("David", 12F);
            this.label_worker.Location = new System.Drawing.Point(620, 80);
            this.label_worker.Name = "label_worker";
            this.label_worker.Size = new System.Drawing.Size(50, 19);
            this.label_worker.TabIndex = 1;
            this.label_worker.Text = "עובד";
            //
            // comboBox_worker
            //
            this.comboBox_worker.Font = new System.Drawing.Font("David", 12F);
            this.comboBox_worker.FormattingEnabled = true;
            this.comboBox_worker.Location = new System.Drawing.Point(380, 77);
            this.comboBox_worker.Name = "comboBox_worker";
            this.comboBox_worker.Size = new System.Drawing.Size(230, 27);
            this.comboBox_worker.TabIndex = 2;
            //
            // label_orderId
            //
            this.label_orderId.AutoSize = true;
            this.label_orderId.Font = new System.Drawing.Font("David", 12F);
            this.label_orderId.Location = new System.Drawing.Point(580, 120);
            this.label_orderId.Name = "label_orderId";
            this.label_orderId.Size = new System.Drawing.Size(90, 19);
            this.label_orderId.TabIndex = 3;
            this.label_orderId.Text = "מספר הזמנה";
            //
            // textBox_orderId
            //
            this.textBox_orderId.Font = new System.Drawing.Font("David", 12F);
            this.textBox_orderId.Location = new System.Drawing.Point(380, 117);
            this.textBox_orderId.Name = "textBox_orderId";
            this.textBox_orderId.Size = new System.Drawing.Size(190, 27);
            this.textBox_orderId.TabIndex = 4;
            //
            // label_orderDate
            //
            this.label_orderDate.AutoSize = true;
            this.label_orderDate.Font = new System.Drawing.Font("David", 12F);
            this.label_orderDate.Location = new System.Drawing.Point(580, 160);
            this.label_orderDate.Name = "label_orderDate";
            this.label_orderDate.Size = new System.Drawing.Size(90, 19);
            this.label_orderDate.TabIndex = 5;
            this.label_orderDate.Text = "תאריך הזמנה";
            //
            // dateTimePicker_orderDate
            //
            this.dateTimePicker_orderDate.Font = new System.Drawing.Font("David", 12F);
            this.dateTimePicker_orderDate.Location = new System.Drawing.Point(380, 157);
            this.dateTimePicker_orderDate.Name = "dateTimePicker_orderDate";
            this.dateTimePicker_orderDate.Size = new System.Drawing.Size(190, 27);
            this.dateTimePicker_orderDate.TabIndex = 6;
            //
            // label_totalPrice
            //
            this.label_totalPrice.AutoSize = true;
            this.label_totalPrice.Font = new System.Drawing.Font("David", 12F);
            this.label_totalPrice.Location = new System.Drawing.Point(600, 200);
            this.label_totalPrice.Name = "label_totalPrice";
            this.label_totalPrice.Size = new System.Drawing.Size(70, 19);
            this.label_totalPrice.TabIndex = 7;
            this.label_totalPrice.Text = "מחיר כולל";
            //
            // textBox_totalPrice
            //
            this.textBox_totalPrice.Font = new System.Drawing.Font("David", 12F);
            this.textBox_totalPrice.Location = new System.Drawing.Point(380, 197);
            this.textBox_totalPrice.Name = "textBox_totalPrice";
            this.textBox_totalPrice.Size = new System.Drawing.Size(190, 27);
            this.textBox_totalPrice.TabIndex = 8;
            //
            // label_pickupTime
            //
            this.label_pickupTime.AutoSize = true;
            this.label_pickupTime.Font = new System.Drawing.Font("David", 12F);
            this.label_pickupTime.ForeColor = System.Drawing.Color.DarkGreen;
            this.label_pickupTime.Location = new System.Drawing.Point(590, 250);
            this.label_pickupTime.Name = "label_pickupTime";
            this.label_pickupTime.Size = new System.Drawing.Size(80, 19);
            this.label_pickupTime.TabIndex = 9;
            this.label_pickupTime.Text = "זמן איסוף";
            //
            // dateTimePicker_pickupTime
            //
            this.dateTimePicker_pickupTime.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dateTimePicker_pickupTime.Font = new System.Drawing.Font("David", 12F);
            this.dateTimePicker_pickupTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_pickupTime.Location = new System.Drawing.Point(380, 247);
            this.dateTimePicker_pickupTime.Name = "dateTimePicker_pickupTime";
            this.dateTimePicker_pickupTime.ShowUpDown = true;
            this.dateTimePicker_pickupTime.Size = new System.Drawing.Size(190, 27);
            this.dateTimePicker_pickupTime.TabIndex = 10;
            //
            // label_branch
            //
            this.label_branch.AutoSize = true;
            this.label_branch.Font = new System.Drawing.Font("David", 12F);
            this.label_branch.ForeColor = System.Drawing.Color.DarkGreen;
            this.label_branch.Location = new System.Drawing.Point(610, 290);
            this.label_branch.Name = "label_branch";
            this.label_branch.Size = new System.Drawing.Size(60, 19);
            this.label_branch.TabIndex = 11;
            this.label_branch.Text = "סניף";
            //
            // textBox_branch
            //
            this.textBox_branch.Font = new System.Drawing.Font("David", 12F);
            this.textBox_branch.Location = new System.Drawing.Point(380, 287);
            this.textBox_branch.Name = "textBox_branch";
            this.textBox_branch.Size = new System.Drawing.Size(190, 27);
            this.textBox_branch.TabIndex = 12;
            //
            // Add_button
            //
            this.Add_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Add_button.Font = new System.Drawing.Font("David", 14.25F, System.Drawing.FontStyle.Bold);
            this.Add_button.Location = new System.Drawing.Point(100, 200);
            this.Add_button.Name = "Add_button";
            this.Add_button.Size = new System.Drawing.Size(180, 70);
            this.Add_button.TabIndex = 13;
            this.Add_button.Text = "הוסף הזמנה";
            this.Add_button.UseVisualStyleBackColor = true;
            this.Add_button.Click += new System.EventHandler(this.Add_button_Click);
            //
            // button_back
            //
            this.button_back.Font = new System.Drawing.Font("David", 14.25F, System.Drawing.FontStyle.Bold);
            this.button_back.Location = new System.Drawing.Point(350, 370);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(131, 42);
            this.button_back.TabIndex = 14;
            this.button_back.Text = "חזרה";
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            //
            // CreatePickupOrderPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.Add_button);
            this.Controls.Add(this.textBox_branch);
            this.Controls.Add(this.label_branch);
            this.Controls.Add(this.dateTimePicker_pickupTime);
            this.Controls.Add(this.label_pickupTime);
            this.Controls.Add(this.textBox_totalPrice);
            this.Controls.Add(this.label_totalPrice);
            this.Controls.Add(this.dateTimePicker_orderDate);
            this.Controls.Add(this.label_orderDate);
            this.Controls.Add(this.textBox_orderId);
            this.Controls.Add(this.label_orderId);
            this.Controls.Add(this.comboBox_worker);
            this.Controls.Add(this.label_worker);
            this.Controls.Add(this.label_title);
            this.Name = "CreatePickupOrderPanel";
            this.Size = new System.Drawing.Size(900, 500);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Label label_worker;
        private System.Windows.Forms.ComboBox comboBox_worker;
        private System.Windows.Forms.Label label_orderId;
        private System.Windows.Forms.TextBox textBox_orderId;
        private System.Windows.Forms.Label label_orderDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_orderDate;
        private System.Windows.Forms.Label label_totalPrice;
        private System.Windows.Forms.TextBox textBox_totalPrice;
        private System.Windows.Forms.Label label_pickupTime;
        private System.Windows.Forms.DateTimePicker dateTimePicker_pickupTime;
        private System.Windows.Forms.Label label_branch;
        private System.Windows.Forms.TextBox textBox_branch;
        private System.Windows.Forms.Button Add_button;
        private System.Windows.Forms.Button button_back;
    }
}

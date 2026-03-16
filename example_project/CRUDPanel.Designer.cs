namespace Example_Project
{
    partial class CRUDPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_new = new System.Windows.Forms.Button();
            this.button_exist = new System.Windows.Forms.Button();
            this.button_delivery = new System.Windows.Forms.Button();
            this.button_pickup = new System.Windows.Forms.Button();
            this.back = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("David", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label1.Location = new System.Drawing.Point(18, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(593, 79);
            this.label1.TabIndex = 0;
            this.label1.Text = "ניהול המערכת";
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(43, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(659, 120);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            //
            // button_new
            //
            this.button_new.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_new.Font = new System.Drawing.Font("David", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button_new.Location = new System.Drawing.Point(43, 155);
            this.button_new.Name = "button_new";
            this.button_new.Size = new System.Drawing.Size(267, 80);
            this.button_new.TabIndex = 2;
            this.button_new.Text = "יצירת עובד חדש";
            this.button_new.UseVisualStyleBackColor = true;
            this.button_new.Click += new System.EventHandler(this.button_new_Click);
            //
            // button_exist
            //
            this.button_exist.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_exist.Font = new System.Drawing.Font("David", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button_exist.Location = new System.Drawing.Point(435, 155);
            this.button_exist.Name = "button_exist";
            this.button_exist.Size = new System.Drawing.Size(267, 80);
            this.button_exist.TabIndex = 3;
            this.button_exist.Text = "צפייה בעובד קיים";
            this.button_exist.UseVisualStyleBackColor = true;
            this.button_exist.Click += new System.EventHandler(this.button_exist_Click);
            //
            // button_delivery
            //
            this.button_delivery.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_delivery.Font = new System.Drawing.Font("David", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button_delivery.ForeColor = System.Drawing.Color.DarkRed;
            this.button_delivery.Location = new System.Drawing.Point(43, 255);
            this.button_delivery.Name = "button_delivery";
            this.button_delivery.Size = new System.Drawing.Size(267, 80);
            this.button_delivery.TabIndex = 4;
            this.button_delivery.Text = "הזמנת משלוח חדשה";
            this.button_delivery.UseVisualStyleBackColor = true;
            this.button_delivery.Click += new System.EventHandler(this.button_delivery_Click);
            //
            // button_pickup
            //
            this.button_pickup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_pickup.Font = new System.Drawing.Font("David", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button_pickup.ForeColor = System.Drawing.Color.DarkGreen;
            this.button_pickup.Location = new System.Drawing.Point(435, 255);
            this.button_pickup.Name = "button_pickup";
            this.button_pickup.Size = new System.Drawing.Size(267, 80);
            this.button_pickup.TabIndex = 5;
            this.button_pickup.Text = "הזמנת איסוף חדשה";
            this.button_pickup.UseVisualStyleBackColor = true;
            this.button_pickup.Click += new System.EventHandler(this.button_pickup_Click);
            //
            // back
            //
            this.back.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.back.Location = new System.Drawing.Point(301, 370);
            this.back.Name = "back";
            this.back.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.back.Size = new System.Drawing.Size(117, 40);
            this.back.TabIndex = 9;
            this.back.Text = "חזור";
            this.back.UseVisualStyleBackColor = true;
            this.back.Click += new System.EventHandler(this.back_Click);
            //
            // CRUDPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.back);
            this.Controls.Add(this.button_pickup);
            this.Controls.Add(this.button_delivery);
            this.Controls.Add(this.button_exist);
            this.Controls.Add(this.button_new);
            this.Controls.Add(this.groupBox1);
            this.Name = "CRUDPanel";
            this.Size = new System.Drawing.Size(900, 500);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_new;
        private System.Windows.Forms.Button button_exist;
        private System.Windows.Forms.Button button_delivery;
        private System.Windows.Forms.Button button_pickup;
        private System.Windows.Forms.Button back;
    }
}

namespace Example_Project
{
    partial class OrderDetailsPanel
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
            this.label_orderId = new System.Windows.Forms.Label();
            this.label_orderDate = new System.Windows.Forms.Label();
            this.label_totalPrice = new System.Windows.Forms.Label();
            this.label_worker = new System.Windows.Forms.Label();
            this.label_orderType = new System.Windows.Forms.Label();
            this.label_itemsTitle = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.back = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            //
            // label_title
            //
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label_title.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label_title.Location = new System.Drawing.Point(300, 15);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(250, 39);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "פרטי הזמנה";
            //
            // label_orderId
            //
            this.label_orderId.AutoSize = true;
            this.label_orderId.Font = new System.Drawing.Font("David", 12F);
            this.label_orderId.Location = new System.Drawing.Point(550, 70);
            this.label_orderId.Name = "label_orderId";
            this.label_orderId.Size = new System.Drawing.Size(100, 19);
            this.label_orderId.TabIndex = 1;
            this.label_orderId.Text = "מספר הזמנה:";
            //
            // label_orderDate
            //
            this.label_orderDate.AutoSize = true;
            this.label_orderDate.Font = new System.Drawing.Font("David", 12F);
            this.label_orderDate.Location = new System.Drawing.Point(550, 95);
            this.label_orderDate.Name = "label_orderDate";
            this.label_orderDate.Size = new System.Drawing.Size(100, 19);
            this.label_orderDate.TabIndex = 2;
            this.label_orderDate.Text = "תאריך:";
            //
            // label_totalPrice
            //
            this.label_totalPrice.AutoSize = true;
            this.label_totalPrice.Font = new System.Drawing.Font("David", 12F);
            this.label_totalPrice.Location = new System.Drawing.Point(550, 120);
            this.label_totalPrice.Name = "label_totalPrice";
            this.label_totalPrice.Size = new System.Drawing.Size(100, 19);
            this.label_totalPrice.TabIndex = 3;
            this.label_totalPrice.Text = "סה\"כ:";
            //
            // label_worker
            //
            this.label_worker.AutoSize = true;
            this.label_worker.Font = new System.Drawing.Font("David", 12F);
            this.label_worker.Location = new System.Drawing.Point(250, 70);
            this.label_worker.Name = "label_worker";
            this.label_worker.Size = new System.Drawing.Size(100, 19);
            this.label_worker.TabIndex = 4;
            this.label_worker.Text = "עובד:";
            //
            // label_orderType
            //
            this.label_orderType.AutoSize = true;
            this.label_orderType.Font = new System.Drawing.Font("David", 12F);
            this.label_orderType.ForeColor = System.Drawing.Color.DarkRed;
            this.label_orderType.Location = new System.Drawing.Point(250, 95);
            this.label_orderType.Name = "label_orderType";
            this.label_orderType.Size = new System.Drawing.Size(100, 19);
            this.label_orderType.TabIndex = 5;
            this.label_orderType.Text = "סוג:";
            //
            // label_itemsTitle
            //
            this.label_itemsTitle.AutoSize = true;
            this.label_itemsTitle.Font = new System.Drawing.Font("David", 14F, System.Drawing.FontStyle.Bold);
            this.label_itemsTitle.Location = new System.Drawing.Point(680, 160);
            this.label_itemsTitle.Name = "label_itemsTitle";
            this.label_itemsTitle.Size = new System.Drawing.Size(150, 22);
            this.label_itemsTitle.TabIndex = 6;
            this.label_itemsTitle.Text = "פריטים בהזמנה:";
            //
            // dataGridView1
            //
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(50, 190);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(800, 220);
            this.dataGridView1.TabIndex = 7;
            //
            // back
            //
            this.back.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.back.Location = new System.Drawing.Point(380, 430);
            this.back.Name = "back";
            this.back.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.back.Size = new System.Drawing.Size(131, 40);
            this.back.TabIndex = 8;
            this.back.Text = "חזור";
            this.back.UseVisualStyleBackColor = true;
            this.back.Click += new System.EventHandler(this.back_Click);
            //
            // OrderDetailsPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.back);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label_itemsTitle);
            this.Controls.Add(this.label_orderType);
            this.Controls.Add(this.label_worker);
            this.Controls.Add(this.label_totalPrice);
            this.Controls.Add(this.label_orderDate);
            this.Controls.Add(this.label_orderId);
            this.Controls.Add(this.label_title);
            this.Name = "OrderDetailsPanel";
            this.Size = new System.Drawing.Size(900, 500);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Label label_orderId;
        private System.Windows.Forms.Label label_orderDate;
        private System.Windows.Forms.Label label_totalPrice;
        private System.Windows.Forms.Label label_worker;
        private System.Windows.Forms.Label label_orderType;
        private System.Windows.Forms.Label label_itemsTitle;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button back;
    }
}

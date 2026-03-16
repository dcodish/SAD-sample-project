namespace Example_Project
{
    partial class MoadAPanel
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
            this.back = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // back
            //
            this.back.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.back.Location = new System.Drawing.Point(350, 370);
            this.back.Name = "back";
            this.back.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.back.Size = new System.Drawing.Size(131, 42);
            this.back.TabIndex = 0;
            this.back.Text = "חזור";
            this.back.UseVisualStyleBackColor = true;
            this.back.Click += new System.EventHandler(this.back_Click);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(300, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 39);
            this.label1.TabIndex = 1;
            this.label1.Text = "מועד א'";
            //
            // MoadAPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.back);
            this.Name = "MoadAPanel";
            this.Size = new System.Drawing.Size(900, 500);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button back;
        private System.Windows.Forms.Label label1;
    }
}

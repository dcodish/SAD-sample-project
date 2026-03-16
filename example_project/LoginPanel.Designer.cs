namespace Example_Project
{
    partial class LoginPanel
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
            this.button_moadA = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.id = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.enter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(126, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(677, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "ברוכים הבאים לעולם הסודי של מתרגלי ניתוץ";
            //
            // button_moadA
            //
            this.button_moadA.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button_moadA.Location = new System.Drawing.Point(55, 307);
            this.button_moadA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_moadA.Name = "button_moadA";
            this.button_moadA.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button_moadA.Size = new System.Drawing.Size(191, 102);
            this.button_moadA.TabIndex = 2;
            this.button_moadA.Text = "לצפייה במועד א\'";
            this.button_moadA.UseVisualStyleBackColor = true;
            this.button_moadA.Click += new System.EventHandler(this.button_moadA_Click);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(580, 214);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "שם משתמש";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.Location = new System.Drawing.Point(622, 251);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "סיסמא";
            //
            // id
            //
            this.id.Location = new System.Drawing.Point(347, 212);
            this.id.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.id.Name = "id";
            this.id.Size = new System.Drawing.Size(230, 22);
            this.id.TabIndex = 5;
            //
            // password
            //
            this.password.Location = new System.Drawing.Point(347, 251);
            this.password.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(230, 22);
            this.password.TabIndex = 6;
            //
            // enter
            //
            this.enter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enter.Location = new System.Drawing.Point(404, 296);
            this.enter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.enter.Name = "enter";
            this.enter.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.enter.Size = new System.Drawing.Size(117, 40);
            this.enter.TabIndex = 7;
            this.enter.Text = "כניסה";
            this.enter.UseVisualStyleBackColor = true;
            this.enter.Click += new System.EventHandler(this.enter_Click);
            //
            // LoginPanel
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.Controls.Add(this.enter);
            this.Controls.Add(this.password);
            this.Controls.Add(this.id);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_moadA);
            this.Controls.Add(this.label1);
            this.Name = "LoginPanel";
            this.Size = new System.Drawing.Size(900, 500);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_moadA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox id;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Button enter;
    }
}

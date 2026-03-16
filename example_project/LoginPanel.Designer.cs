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
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            id = new System.Windows.Forms.TextBox();
            password = new System.Windows.Forms.TextBox();
            enter = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 177);
            label1.Location = new System.Drawing.Point(343, 114);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(526, 46);
            label1.TabIndex = 0;
            label1.Text = "ברוכים הבאים לפרויקט לדוגמה";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 177);
            label2.Location = new System.Drawing.Point(725, 334);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(123, 26);
            label2.TabIndex = 3;
            label2.Text = "שם משתמש";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 177);
            label3.Location = new System.Drawing.Point(778, 392);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(76, 26);
            label3.TabIndex = 4;
            label3.Text = "סיסמא";
            // 
            // id
            // 
            id.Location = new System.Drawing.Point(434, 331);
            id.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            id.Name = "id";
            id.Size = new System.Drawing.Size(286, 31);
            id.TabIndex = 5;
            // 
            // password
            // 
            password.Location = new System.Drawing.Point(434, 392);
            password.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            password.Name = "password";
            password.Size = new System.Drawing.Size(286, 31);
            password.TabIndex = 6;
            // 
            // enter
            // 
            enter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            enter.Location = new System.Drawing.Point(505, 462);
            enter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            enter.Name = "enter";
            enter.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            enter.Size = new System.Drawing.Size(146, 62);
            enter.TabIndex = 7;
            enter.Text = "כניסה";
            enter.UseVisualStyleBackColor = true;
            enter.Click += enter_Click;
            // 
            // LoginPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(255, 192, 192);
            Controls.Add(enter);
            Controls.Add(password);
            Controls.Add(id);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "LoginPanel";
            Size = new System.Drawing.Size(1125, 781);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox id;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Button enter;
    }
}

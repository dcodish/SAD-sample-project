using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Example_Project
{
    public partial class LoginPanel : UserControl
    {
        public LoginPanel()
        {
            InitializeComponent();
        }

        private void enter_Click(object sender, EventArgs e)
        {
            Worker emp = Worker.seekWorker(id.Text);
            if (emp == null)
            {
                MessageBox.Show("עובד לא נמצא", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (!password.Text.Equals("1234"))
            {
                MessageBox.Show("סיסמה שגויה", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            //ceo id - 1111
            if (id.Text.Equals("1111"))
            {
                mainForm.showPanel(new CRUDPanel());
            }
            else
            {
                mainForm.showPanel(new WatchOrdersPanel(emp));
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

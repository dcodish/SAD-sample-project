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
    public partial class CRUDPanel : UserControl
    {
        public CRUDPanel()
        {
            InitializeComponent();
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new LoginPanel());
        }

        private void button_new_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new CreateWorkerPanel());
        }

        private void button_exist_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new UpdateDeletePanel());
        }

        private void button_delivery_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new CreateDeliveryOrderPanel());
        }

        private void button_pickup_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new CreatePickupOrderPanel());
        }

        private void back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new LoginPanel());
        }
    }
}

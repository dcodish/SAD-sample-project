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
    public partial class CreatePickupOrderPanel : UserControl
    {
        public CreatePickupOrderPanel()
        {
            InitializeComponent();
            foreach (Worker w in Program.Workers)
            {
                comboBox_worker.Items.Add(w.getId() + " - " + w.getName());
            }
            int nextId = Order.getNextOrderId();
            textBox_orderId.Text = nextId.ToString();
            textBox_orderId.Enabled = false;
        }

        private void Add_button_Click(object sender, EventArgs e)
        {
            if (comboBox_worker.SelectedIndex == -1)
            {
                MessageBox.Show("יש לבחור עובד", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox_totalPrice.Text))
            {
                MessageBox.Show("יש להזין מחיר", "שגיאה", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox_branch.Text))
            {
                MessageBox.Show("יש להזין סניף", "שגיאה", MessageBoxButtons.OK);
                return;
            }

            string workerId = comboBox_worker.Text.Split('-')[0].Trim();
            Worker w = Worker.seekWorker(workerId);

            int orderId = int.Parse(textBox_orderId.Text);
            DateTime orderDate = dateTimePicker_orderDate.Value;
            int totalPrice = int.Parse(textBox_totalPrice.Text);
            DateTime pickupTime = dateTimePicker_pickupTime.Value;
            string branch = textBox_branch.Text;

            PickupOrder o = new PickupOrder(w, orderId, orderDate, totalPrice, pickupTime, branch, true);

            mainForm.showPanel(new CRUDPanel());
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new CRUDPanel());
        }
    }
}

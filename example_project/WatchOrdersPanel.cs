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
    public partial class WatchOrdersPanel : UserControl
    {
        Worker w;
        public WatchOrdersPanel(Worker w)
        {
            InitializeComponent();
            this.w = w;
            loadOrders();
        }

        private void loadOrders()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("workerId", typeof(string));
            dt.Columns.Add("orderId", typeof(int));
            dt.Columns.Add("orderDate", typeof(DateTime));
            dt.Columns.Add("orderTotalPrice", typeof(int));
            dt.Columns.Add("orderType", typeof(string));
            dt.Columns.Add("details", typeof(string));

            foreach (Order o in w.getOrders())
                {
                    string orderType = "רגילה";
                    string details = "";

                    if (o is DeliveryOrder)
                    {
                        DeliveryOrder d = (DeliveryOrder)o;
                        orderType = "משלוח";
                        details = d.getDeliveryAddress() + " | " + d.getDeliveryDate().ToShortDateString();
                    }
                    else if (o is PickupOrder)
                    {
                        PickupOrder p = (PickupOrder)o;
                        orderType = "איסוף";
                        details = p.getBranchLocation() + " | " + p.getPickupTime().ToString("dd/MM/yyyy HH:mm");
                    }

                    dt.Rows.Add(w.getId(), o.getOrderId(), o.getOrderDate(), o.getOrderTotalPrice(), orderType, details);
                }

            dataGridView1.DataSource = dt;
        }

        //לחיצה על שורה בטבלה — מעבר למסך פרטי הזמנה
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  //לחיצה על כותרת — מתעלמים

            int orderId = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["orderId"].Value.ToString());
            Order order = Order.seekOrder(orderId);
            if (order != null)
            {
                mainForm.showPanel(new OrderDetailsPanel(order, w));
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new LoginPanel());
        }
    }
}

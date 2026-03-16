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
    public partial class OrderDetailsPanel : UserControl
    {
        private Order order;
        private Worker worker;

        public OrderDetailsPanel(Order order, Worker worker)
        {
            InitializeComponent();
            this.order = order;
            this.worker = worker;
            loadOrderInfo();
            loadOrderItems();
        }

        //הצגת פרטי ההזמנה הבסיסיים
        private void loadOrderInfo()
        {
            label_orderId.Text = "מספר הזמנה: " + order.getOrderId();
            label_orderDate.Text = "תאריך: " + order.getOrderDate().ToShortDateString();
            label_totalPrice.Text = "סה\"כ: " + order.getOrderTotalPrice();
            label_worker.Text = "עובד: " + worker.getName();

            //הצגת סוג ההזמנה
            if (order is DeliveryOrder)
            {
                DeliveryOrder d = (DeliveryOrder)order;
                label_orderType.Text = "סוג: משלוח | " + d.getDeliveryAddress() + " | " + d.getDeliveryDate().ToShortDateString();
            }
            else if (order is PickupOrder)
            {
                PickupOrder p = (PickupOrder)order;
                label_orderType.Text = "סוג: איסוף | " + p.getBranchLocation() + " | " + p.getPickupTime().ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                label_orderType.Text = "סוג: רגילה";
            }
        }

        //הצגת פריטי ההזמנה (מחלקת קישור — Association Class)
        private void loadOrderItems()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("מזהה מוצר", typeof(int));
            dt.Columns.Add("שם מוצר", typeof(string));
            dt.Columns.Add("קטגוריה", typeof(string));
            dt.Columns.Add("כמות", typeof(int));
            dt.Columns.Add("מחיר ליחידה", typeof(double));
            dt.Columns.Add("סה\"כ שורה", typeof(double));

            List<OrderItem> items = order.getOrderItems();
            foreach (OrderItem item in items)
            {
                Product p = item.getProduct();
                dt.Rows.Add(
                    p.getProductId(),
                    p.getProductName(),
                    p.getCategory(),
                    item.getQuantity(),
                    item.getUnitPrice(),
                    item.getTotalPrice()
                );
            }

            dataGridView1.DataSource = dt;
        }

        private void back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new WatchOrdersPanel(worker));
        }
    }
}

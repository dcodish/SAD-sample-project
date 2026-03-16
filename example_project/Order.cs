using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Example_Project
{
    public class Order
    {
        // =====================================================================
        // שדות
        // protected — כדי שמחלקות הבן (DeliveryOrder, PickupOrder) יוכלו לגשת
        // =====================================================================
        private Worker worker;
        protected int orderId;
        protected DateTime orderDate;
        protected int orderTotalPrice;
        private List<OrderItem> orderItems;

        // =====================================================================
        // בנאי
        // =====================================================================
        public Order(Worker w, int id, DateTime date, int totalPrice, bool is_new)
        {
            this.orderId = id;
            this.orderDate = date;
            this.orderTotalPrice = totalPrice;
            this.worker = w;
            if (is_new)
            {
                this.createOrder();
                w.addOrder(this);
                Program.Orders.Add(this);
            }
        }

        // =====================================================================
        // Getters & Setters
        // =====================================================================
        public int getOrderId() { return this.orderId; }
        public DateTime getOrderDate() { return this.orderDate; }
        public int getOrderTotalPrice() { return this.orderTotalPrice; }
        public Worker getWorker() { return this.worker; }
        public void setWorker(Worker w) { this.worker = w; }

        // =====================================================================
        // ניהול רשימת פריטים (OrderItems) — קשר Many-to-Many עם Product
        // =====================================================================
        public void addOrderItem(OrderItem item)
        {
            if (this.orderItems == null)
                this.orderItems = new List<OrderItem>();
            if (!this.orderItems.Contains(item))
                this.orderItems.Add(item);
        }

        public List<OrderItem> getOrderItems()
        {
            if (this.orderItems == null)
                this.orderItems = new List<OrderItem>();
            return this.orderItems;
        }

        // =====================================================================
        // פעולות מול בסיס הנתונים
        // virtual — כדי שמחלקות הבן יוכלו לדרוס (override)
        // =====================================================================
        public virtual void createOrder()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE SP_add_order @worker, @orderId, @orderDate, @OrderTotalPrice";
            cmd.Parameters.AddWithValue("@worker", this.worker.getId());
            cmd.Parameters.AddWithValue("@orderId", this.orderId);
            cmd.Parameters.AddWithValue("@orderDate", this.orderDate);
            cmd.Parameters.AddWithValue("@OrderTotalPrice", this.orderTotalPrice);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // =====================================================================
        // מתודות סטטיות — טעינה וחיפוש
        // =====================================================================

        //טעינת כל ההזמנות מבסיס הנתונים — כולל ירושה (DeliveryOrder, PickupOrder)
        public static void initOrders()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE dbo.Get_all_Orders_Full";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Orders = new List<Order>();

            while (rdr.Read())
            {
                //עמודות: 0=workerId, 1=orderId, 2=orderDate, 3=totalPrice,
                //        4=deliveryAddress, 5=deliveryDate, 6=pickupTime, 7=branchLocation
                Worker w = Worker.seekWorker(rdr.GetValue(0).ToString());
                int id = int.Parse(rdr.GetValue(1).ToString());
                DateTime date = DateTime.Parse(rdr.GetValue(2).ToString());
                int totalPrice = int.Parse(rdr.GetValue(3).ToString());

                Order o;

                //זיהוי סוג ההזמנה לפי שדות הירושה
                if (rdr.GetValue(4) != DBNull.Value)  // deliveryAddress לא ריק → משלוח
                {
                    string deliveryAddress = rdr.GetValue(4).ToString();
                    DateTime deliveryDate = DateTime.Parse(rdr.GetValue(5).ToString());
                    o = new DeliveryOrder(w, id, date, totalPrice,
                        deliveryAddress, deliveryDate, false);
                }
                else if (rdr.GetValue(6) != DBNull.Value)  // pickupTime לא ריק → איסוף
                {
                    DateTime pickupTime = DateTime.Parse(rdr.GetValue(6).ToString());
                    string branchLocation = rdr.GetValue(7).ToString();
                    o = new PickupOrder(w, id, date, totalPrice,
                        pickupTime, branchLocation, false);
                }
                else  // הזמנה רגילה
                {
                    o = new Order(w, id, date, totalPrice, false);
                }

                Program.Orders.Add(o);
                w.addOrder(o);
            }
        }

        //חיפוש הזמנה לפי מזהה
        public static Order seekOrder(int id)
        {
            foreach (Order o in Program.Orders)
            {
                if (o.getOrderId() == id)
                    return o;
            }
            return null;
        }

        //חישוב מספר הזמנה הבא
        public static int getNextOrderId()
        {
            int maxId = 0;
            foreach (Order o in Program.Orders)
            {
                if (o.getOrderId() > maxId)
                    maxId = o.getOrderId();
            }
            return maxId + 1;
        }
    }
}

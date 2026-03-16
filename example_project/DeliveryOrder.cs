using System;
using Microsoft.Data.SqlClient;

namespace Example_Project
{
    public class DeliveryOrder : Order
    {
        private string deliveryAddress;
        private DateTime deliveryDate;

        public DeliveryOrder(Worker w, int id, DateTime date, int totalPrice,
            string deliveryAddress, DateTime deliveryDate, bool is_new)
            : base(w, id, date, totalPrice, false)
        {
            this.deliveryAddress = deliveryAddress;
            this.deliveryDate = deliveryDate;
            if (is_new)
            {
                this.createOrder();
                w.addOrder(this);
                Program.Orders.Add(this);
            }
        }

        public string getDeliveryAddress() { return this.deliveryAddress; }
        public DateTime getDeliveryDate() { return this.deliveryDate; }

        //שמירה בבסיס הנתונים - גם בטבלת האב וגם בטבלת הבן
        public override void createOrder()
        {
            //שמירה בטבלת Orders (טבלת האב)
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE SP_add_order @worker, @orderId, @orderDate, @OrderTotalPrice";
            cmd.Parameters.AddWithValue("@worker", this.getWorker().getId());
            cmd.Parameters.AddWithValue("@orderId", this.orderId);
            cmd.Parameters.AddWithValue("@orderDate", this.orderDate);
            cmd.Parameters.AddWithValue("@OrderTotalPrice", this.orderTotalPrice);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);

            //שמירה בטבלת DeliveryOrders (טבלת הבן)
            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "EXECUTE SP_add_delivery_order @orderId, @deliveryAddress, @deliveryDate";
            cmd2.Parameters.AddWithValue("@orderId", this.orderId);
            cmd2.Parameters.AddWithValue("@deliveryAddress", this.deliveryAddress);
            cmd2.Parameters.AddWithValue("@deliveryDate", this.deliveryDate);
            SC.execute_non_query(cmd2);
        }
    }
}

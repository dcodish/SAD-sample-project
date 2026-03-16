using System;
using Microsoft.Data.SqlClient;

namespace Example_Project
{
    public class PickupOrder : Order
    {
        private DateTime pickupTime;
        private string branchLocation;

        public PickupOrder(Worker w, int id, DateTime date, int totalPrice,
            DateTime pickupTime, string branchLocation, bool is_new)
            : base(w, id, date, totalPrice, false)
        {
            this.pickupTime = pickupTime;
            this.branchLocation = branchLocation;
            if (is_new)
            {
                this.createOrder();
                w.addOrder(this);
                Program.Orders.Add(this);
            }
        }

        public DateTime getPickupTime() { return this.pickupTime; }
        public string getBranchLocation() { return this.branchLocation; }

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

            //שמירה בטבלת PickupOrders (טבלת הבן)
            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "EXECUTE SP_add_pickup_order @orderId, @pickupTime, @branchLocation";
            cmd2.Parameters.AddWithValue("@orderId", this.orderId);
            cmd2.Parameters.AddWithValue("@pickupTime", this.pickupTime);
            cmd2.Parameters.AddWithValue("@branchLocation", this.branchLocation);
            SC.execute_non_query(cmd2);
        }
    }
}

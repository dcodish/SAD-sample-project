using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Example_Project
{
    /// <summary>
    /// מחלקת קישור (Association Class) — מייצגת את הקשר Many-to-Many בין Order ל-Product.
    /// כל OrderItem מכיל הפניה להזמנה, הפניה למוצר, וכן שדות ייחודיים לקשר: כמות ומחיר ליחידה.
    ///
    /// ב-DB: טבלת OrderItems עם מפתח ראשי מורכב (orderId + productId).
    /// </summary>
    public class OrderItem
    {
        // =====================================================================
        // שדות
        // הפניות לשני הצדדים של הקשר + שדות ייחודיים לקשר
        // =====================================================================
        private Order order;        // הפניה להזמנה
        private Product product;    // הפניה למוצר
        private int quantity;       // כמות — שדה ייחודי לקשר
        private double unitPrice;   // מחיר ליחידה — שדה ייחודי לקשר

        // =====================================================================
        // בנאי
        // =====================================================================
        public OrderItem(Order order, Product product, int quantity, double unitPrice, bool is_new)
        {
            this.order = order;
            this.product = product;
            this.quantity = quantity;
            this.unitPrice = unitPrice;
            if (is_new)
            {
                this.createOrderItem();
                //הוספה לרשימות הקשר של שני הצדדים
                order.addOrderItem(this);
                Program.OrderItems.Add(this);
            }
        }

        // =====================================================================
        // Getters
        // =====================================================================
        public Order getOrder() { return this.order; }
        public Product getProduct() { return this.product; }
        public int getQuantity() { return this.quantity; }
        public double getUnitPrice() { return this.unitPrice; }
        public double getTotalPrice() { return this.quantity * this.unitPrice; }

        // =====================================================================
        // פעולות DB
        // =====================================================================
        public void createOrderItem()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE SP_add_order_item @orderId, @productId, @quantity, @unitPrice";
            cmd.Parameters.AddWithValue("@orderId", this.order.getOrderId());
            cmd.Parameters.AddWithValue("@productId", this.product.getProductId());
            cmd.Parameters.AddWithValue("@quantity", this.quantity);
            cmd.Parameters.AddWithValue("@unitPrice", this.unitPrice);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // =====================================================================
        // מתודות סטטיות — טעינה
        //
        // שימו לב לסדר הטעינה! חייבים לטעון קודם:
        //   1. Products (כי OrderItem מפנה ל-Product)
        //   2. Orders (כי OrderItem מפנה ל-Order)
        //   3. רק אז OrderItems
        // =====================================================================
        public static void initOrderItems()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE dbo.Get_all_OrderItems";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.OrderItems = new List<OrderItem>();

            while (rdr.Read())
            {
                //חיפוש ההזמנה והמוצר לפי המזהים מה-DB
                int orderId = int.Parse(rdr.GetValue(0).ToString());
                int productId = int.Parse(rdr.GetValue(1).ToString());
                int quantity = int.Parse(rdr.GetValue(2).ToString());
                double unitPrice = double.Parse(rdr.GetValue(3).ToString());

                Order o = Order.seekOrder(orderId);
                Product p = Product.seekProduct(productId);

                if (o != null && p != null)
                {
                    OrderItem item = new OrderItem(o, p, quantity, unitPrice, false);
                    //קישור לרשימה של ההזמנה
                    o.addOrderItem(item);
                    Program.OrderItems.Add(item);
                }
            }
        }
    }
}

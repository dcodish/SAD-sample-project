using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Example_Project
{
    public class Product
    {
        // =====================================================================
        // שדות
        // =====================================================================
        private int productId;
        private string productName;
        private double price;
        private string category;

        // =====================================================================
        // בנאי
        // =====================================================================
        public Product(int id, string name, double price, string category, bool is_new)
        {
            this.productId = id;
            this.productName = name;
            this.price = price;
            this.category = category;
            if (is_new)
            {
                this.createProduct();
                Program.Products.Add(this);
            }
        }

        // =====================================================================
        // Getters & Setters
        // =====================================================================
        public int getProductId() { return this.productId; }
        public string getProductName() { return this.productName; }
        public double getPrice() { return this.price; }
        public string getCategory() { return this.category; }

        public void setProductName(string name) { this.productName = name; }
        public void setPrice(double price) { this.price = price; }
        public void setCategory(string category) { this.category = category; }

        // =====================================================================
        // פעולות DB
        // =====================================================================
        public void createProduct()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE SP_add_product @id, @name, @price, @category";
            cmd.Parameters.AddWithValue("@id", this.productId);
            cmd.Parameters.AddWithValue("@name", this.productName);
            cmd.Parameters.AddWithValue("@price", this.price);
            cmd.Parameters.AddWithValue("@category", this.category);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // =====================================================================
        // מתודות סטטיות — טעינה וחיפוש
        // =====================================================================
        public static void initProducts()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE dbo.Get_all_Products";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Products = new List<Product>();

            while (rdr.Read())
            {
                int id = int.Parse(rdr.GetValue(0).ToString());
                string name = rdr.GetValue(1).ToString();
                double price = double.Parse(rdr.GetValue(2).ToString());
                string category = rdr.GetValue(3).ToString();

                Product p = new Product(id, name, price, category, false);
                Program.Products.Add(p);
            }
        }

        public static Product seekProduct(int id)
        {
            foreach (Product p in Program.Products)
            {
                if (p.getProductId() == id)
                    return p;
            }
            return null;
        }
    }
}

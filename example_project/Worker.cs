using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Example_Project
{
    public class Worker
    {
        // =====================================================================
        // שדות
        // =====================================================================
        private string workerId;
        private string workerName;
        private Title workerTitle;  // הפניה לאובייקט Title (נטען מטבלת Lookup)
        private List<Order> orders;

        // =====================================================================
        // בנאי
        // is_new = true  → מופע חדש שנוצר ע"י המשתמש → שמור ב-DB
        // is_new = false → מופע שנטען מה-DB → לא שומרים שוב
        // =====================================================================
        public Worker(string id, string name, Title title, bool is_new)
        {
            this.workerId = id;
            this.workerName = name;
            this.workerTitle = title;
            if (is_new)
            {
                this.createWorker();
                Program.Workers.Add(this);
            }
        }

        // =====================================================================
        // Getters & Setters
        // =====================================================================
        public string getId() { return this.workerId; }
        public string getName() { return this.workerName; }
        public Title getTitle() { return this.workerTitle; }

        public void setName(string name) { this.workerName = name; }
        public void setTitle(Title t) { this.workerTitle = t; }

        // =====================================================================
        // ניהול רשימת ההזמנות של העובד (קשר One-to-Many)
        // =====================================================================
        public List<Order> getOrders()
        {
            if (orders == null)
                orders = new List<Order>();
            return orders;
        }

        public void addOrder(Order newOrder)
        {
            if (newOrder == null)
                return;
            if (this.orders == null)
                this.orders = new List<Order>();
            if (!this.orders.Contains(newOrder))
            {
                this.orders.Add(newOrder);
                newOrder.setWorker(this);
            }
        }

        public void removeOrder(Order oldOrder)
        {
            if (oldOrder == null)
                return;
            if (this.orders != null && this.orders.Contains(oldOrder))
            {
                this.orders.Remove(oldOrder);
            }
        }

        // =====================================================================
        // פעולות מול בסיס הנתונים (CRUD)
        // שומרים את titleId (לא את שם התפקיד!) — Foreign Key לטבלת Titles
        // =====================================================================
        public void createWorker()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE SP_add_worker @id, @name, @titleId";
            cmd.Parameters.AddWithValue("@id", this.workerId);
            cmd.Parameters.AddWithValue("@name", this.workerName);
            cmd.Parameters.AddWithValue("@titleId", this.workerTitle.getTitleId());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void updateWorker()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE dbo.SP_Update_worker @id, @name, @titleId";
            cmd.Parameters.AddWithValue("@id", this.workerId);
            cmd.Parameters.AddWithValue("@name", this.workerName);
            cmd.Parameters.AddWithValue("@titleId", this.workerTitle.getTitleId());
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        public void deleteWorker()
        {
            Program.Workers.Remove(this);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE dbo.SP_delete_worker @id";
            cmd.Parameters.AddWithValue("@id", this.workerId);
            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(cmd);
        }

        // =====================================================================
        // מתודות סטטיות — טעינה וחיפוש
        // =====================================================================

        //טעינת כל העובדים מבסיס הנתונים לרשימה בזיכרון
        public static void initWorkers()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE dbo.Get_all_Workers";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Workers = new List<Worker>();

            while (rdr.Read())
            {
                string id = rdr.GetValue(0).ToString();
                string name = rdr.GetValue(1).ToString();
                //חיפוש התפקיד לפי מזהה מטבלת Lookup
                int titleId = int.Parse(rdr.GetValue(2).ToString());
                Title title = Title.seekTitleById(titleId);

                Worker w = new Worker(id, name, title, false);
                Program.Workers.Add(w);
            }
        }

        //חיפוש עובד לפי תעודת זהות
        public static Worker seekWorker(string id)
        {
            foreach (Worker w in Program.Workers)
            {
                if (w.getId() == id)
                    return w;
            }
            return null;
        }
    }
}

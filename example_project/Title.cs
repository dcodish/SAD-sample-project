using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Example_Project
{
    /// <summary>
    /// מחלקת Title — מייצגת ערך מטבלת Lookup בבסיס הנתונים.
    /// הערכים נטענים מה-DB בהפעלת התוכנית (לא מקודדים בקוד!).
    /// כך אפשר להוסיף תפקידים חדשים דרך ה-DB בלי לשנות קוד.
    /// </summary>
    public class Title
    {
        // =====================================================================
        // שדות
        // =====================================================================
        private int titleId;
        private string titleName;

        // =====================================================================
        // בנאי
        // =====================================================================
        public Title(int id, string name)
        {
            this.titleId = id;
            this.titleName = name;
        }

        // =====================================================================
        // Getters
        // =====================================================================
        public int getTitleId() { return this.titleId; }
        public string getTitleName() { return this.titleName; }

        //לתצוגה ב-ComboBox ובמקומות אחרים
        public override string ToString()
        {
            return this.titleName;
        }

        // =====================================================================
        // מתודות סטטיות — טעינה וחיפוש
        // =====================================================================

        //טעינת כל התפקידים מטבלת Lookup בבסיס הנתונים
        public static void initTitles()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE dbo.Get_all_Titles";
            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);

            Program.Titles = new List<Title>();

            while (rdr.Read())
            {
                int id = int.Parse(rdr.GetValue(0).ToString());
                string name = rdr.GetValue(1).ToString();

                Title t = new Title(id, name);
                Program.Titles.Add(t);
            }
        }

        //חיפוש תפקיד לפי מזהה
        public static Title seekTitleById(int id)
        {
            foreach (Title t in Program.Titles)
            {
                if (t.getTitleId() == id)
                    return t;
            }
            return null;
        }

        //חיפוש תפקיד לפי שם
        public static Title seekTitleByName(string name)
        {
            foreach (Title t in Program.Titles)
            {
                if (t.getTitleName() == name)
                    return t;
            }
            return null;
        }
    }
}

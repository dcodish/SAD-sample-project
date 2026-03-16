using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace Example_Project
{
    /// <summary>
    /// מחלקה שאחראית על החיבור לבסיס הנתונים וביצוע שאילתות.
    /// כל פעולה מול בסיס הנתונים עוברת דרך מחלקה זו.
    ///
    /// שתי סוגי פעולות:
    /// 1. execute_non_query - פעולות שמשנות נתונים (INSERT, UPDATE, DELETE)
    /// 2. execute_query    - פעולות שמחזירות נתונים (SELECT)
    /// </summary>
    class SQL_CON
    {
        SqlConnection conn;

        public SQL_CON()
        {
            //חיבור לשרת המקומי - לפיתוח מהבית
            conn = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=SAD_0;Integrated Security=True;TrustServerCertificate=True");
            //חיבור לשרת באוניברסיטה - יש לבטל את ההערה ולהעיר את השורה למעלה
            //conn = new SqlConnection("Data Source=IEMDBS;Initial Catalog=SAD_0;Integrated Security=True");
        }

        /// <summary>
        /// ביצוע פעולה שמשנה נתונים בבסיס הנתונים (INSERT, UPDATE, DELETE).
        /// הפעולה לא מחזירה נתונים - רק מבצעת שינוי.
        ///
        /// הזרימה:
        /// 1. פתיחת חיבור לבסיס הנתונים
        /// 2. קישור הפקודה לחיבור
        /// 3. ביצוע הפקודה
        /// 4. סגירת החיבור (תמיד! גם אם הייתה שגיאה)
        /// </summary>
        /// <param name="cmd">פקודת SQL מוכנה עם פרמטרים</param>
        public void execute_non_query(SqlCommand cmd)
        {
            try
            {
                conn.Open();              // שלב 1: פתיחת חיבור
                cmd.Connection = conn;    // שלב 2: קישור הפקודה לחיבור
                cmd.ExecuteNonQuery();     // שלב 3: ביצוע (INSERT/UPDATE/DELETE)
                MessageBox.Show("הפעולה בוצעה בהצלחה", "הודעה", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בביצוע הפעולה: " + ex.Message, "שגיאה", MessageBoxButtons.OK);
            }
            finally
            {
                // שלב 4: סגירת החיבור - חייבת לקרות תמיד!
                // finally מתבצע גם אם הייתה שגיאה וגם אם לא
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// ביצוע שאילתה שמחזירה נתונים מבסיס הנתונים (SELECT).
        /// מחזירה SqlDataReader - אובייקט שמאפשר לקרוא את התוצאות שורה אחרי שורה.
        ///
        /// הזרימה:
        /// 1. פתיחת חיבור לבסיס הנתונים
        /// 2. קישור הפקודה לחיבור
        /// 3. ביצוע השאילתה וקבלת Reader
        /// 4. החזרת ה-Reader לשימוש הקוד הקורא
        ///
        /// שימו לב: החיבור לא נסגר כאן! הוא נשאר פתוח כי ה-Reader צריך אותו
        /// כדי לקרוא את השורות. החיבור ייסגר כשה-Reader יסיים.
        /// </summary>
        /// <param name="cmd">פקודת SQL מוכנה עם פרמטרים</param>
        /// <returns>SqlDataReader לקריאת התוצאות, או null אם הייתה שגיאה</returns>
        public SqlDataReader execute_query(SqlCommand cmd)
        {
            try
            {
                conn.Open();              // שלב 1: פתיחת חיבור
                cmd.Connection = conn;    // שלב 2: קישור הפקודה לחיבור
                SqlDataReader reader = cmd.ExecuteReader();  // שלב 3: ביצוע SELECT
                return reader;            // שלב 4: החזרת התוצאות
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בביצוע השאילתה: " + ex.Message, "שגיאה", MessageBoxButtons.OK);
                return null;
            }
        }
    }
}

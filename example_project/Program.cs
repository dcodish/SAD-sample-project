using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Example_Project
{
    static class Program
    {
        // =====================================================================
        // רשימות בזיכרון
        // רשימה סטטית לכל ישות (Entity) במערכת.
        // הרשימות נטענות מבסיס הנתונים בהפעלת התוכנית.
        //
        // שימו לב: אין רשימה נפרדת ל-DeliveryOrder ול-PickupOrder!
        // הם יורשים מ-Order ולכן נכנסים לרשימת Orders (פולימורפיזם).
        //
        // כשמוסיפים ישות חדשה:
        //   1. הוסיפו רשימה כאן
        //   2. כתבו מתודת init סטטית במחלקת ה-Entity (ראו Worker.init_workers)
        //   3. קראו לה מ-initLists בסדר הנכון
        // =====================================================================

        public static List<Title> Titles;       // טבלת Lookup — תפקידים (נטען ראשון!)
        public static List<Worker> Workers;
        public static List<Product> Products;
        public static List<Order> Orders;
        public static List<OrderItem> OrderItems;  // מחלקת קישור (Association Class)

        // =====================================================================
        // אתחול כל הרשימות
        //
        // סדר הטעינה חשוב!
        //   1. קודם: ישויות בסיסיות (שאחרים מפנים אליהן)
        //   2. אחר כך: ישויות שמכילות Foreign Key
        //   3. אחרון: מחלקות קישור (שמפנות לשני צדדים)
        //
        // דוגמה: Worker ו-Product חייבים להיטען לפני Order,
        //         ו-Order חייב להיטען לפני OrderItem.
        // =====================================================================

        public static void initLists()
        {
            Title.initTitles();          // 1. תפקידים (Lookup — נטען ראשון!)
            Worker.initWorkers();        // 2. עובדים (מפנים לתפקידים)
            Product.initProducts();      // 3. מוצרים (בסיסי)
            Order.initOrders();          // 4. הזמנות (מפנות לעובדים)
            OrderItem.initOrderItems();  // 5. פריטי הזמנה (מפנות להזמנות ולמוצרים)
        }

        // =====================================================================
        // נקודת ההתחלה של התוכנית
        // =====================================================================

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            initLists();
            Application.Run(new mainForm());
        }
    }
}

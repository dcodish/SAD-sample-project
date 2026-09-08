# הקמת בסיס הנתונים

מדריך זה מסביר כיצד ליצור את הטבלאות ואת ה-Stored Procedures בבסיס הנתונים של הקבוצה.

## דרישות מקדימות

ודאו שהשלמתם את [מדריך ההתקנה](01-installation.md), ושבסיס הנתונים של הקבוצה
כבר נוצר ב-Azure (ראו [`PREREQS.md`](../PREREQS.md) חלק C).

> **בסיס הנתונים עצמו כבר קיים.** ב-Azure יוצרים אותו דרך הפורטל, לא דרך סקריפט.
> לכן הסקריפט `create_database.sql` **לא** מכיל `CREATE DATABASE` ולא `USE` —
> הוא רק יוצר את הטבלאות והנתונים בתוך בסיס הנתונים שאליו התחברתם.
> Azure SQL אינו תומך בפקודת `USE`: כל חיבור מתבצע ישירות לבסיס נתונים אחד.

## 1. הרצת סקריפט יצירת הטבלאות

### אפשרות א׳: דרך Claude Code (מומלץ)

אם הגדרתם את ה-MCP (ראו [`MCP_SETUP.md`](../MCP_SETUP.md)), פשוט בקשו מ-Claude:

> הרץ את `scripts/create_database.sql` מול בסיס הנתונים דרך ה-mssql MCP.
> שים לב ש-`GO` הוא מפריד של SSMS ולא פקודת T-SQL — פצל את הקובץ לפי `GO`
> ושלח כל בלוק בקריאת execute_sql נפרדת.

### אפשרות ב׳: דרך SSMS

1. פתחו SSMS והתחברו לבסיס הנתונים של הקבוצה ב-Azure
   (Server: `<servername>.database.windows.net`, אימות `SQL Server Authentication`)
2. ודאו שבתיבת בחירת בסיס הנתונים למעלה נבחר **בסיס הנתונים של הקבוצה** ולא `master`
3. לחצו **File > Open > File**
4. פתחו את הקובץ `scripts\create_database.sql` מתוך תיקיית הפרויקט
5. לחצו **Execute** (או F5)
6. ודאו שאין הודעות שגיאה

## 2. אימות בסיס הנתונים

לאחר הרצת הסקריפט, ודאו שהכל נוצר כראוי:

### ב-SSMS
1. בצד שמאל, פתחו את **Databases > בסיס הנתונים שלכם > Tables**
2. ודאו שקיימות שבע טבלאות:
   - `dbo.Titles` — טבלת עזר (reference) לתפקידים
   - `dbo.Workers`
   - `dbo.Orders`
   - `dbo.DeliveryOrders`
   - `dbo.PickupOrders`
   - `dbo.Products`
   - `dbo.OrderItems`
3. פתחו את **Programmability > Stored Procedures** וודאו שקיימים:
   - `dbo.Get_all_Titles`
   - `dbo.Get_all_Workers`
   - `dbo.Get_all_Orders` — שאילתה בסיסית
   - `dbo.Get_all_Orders_Full` — עם LEFT JOIN לטבלאות הירושה
   - `dbo.Get_all_Products`
   - `dbo.Get_all_OrderItems`
   - `dbo.SP_add_worker`
   - `dbo.SP_Update_worker`
   - `dbo.SP_delete_worker`
   - `dbo.SP_add_order`
   - `dbo.SP_add_delivery_order`
   - `dbo.SP_add_pickup_order`
   - `dbo.SP_add_product`
   - `dbo.SP_add_order_item`

### בדיקת הנתונים
לחצו Right Click על טבלת `Workers` ובחרו **Select Top 1000 Rows**. אמורים להופיע 4 רשומות:

| workerId | workerName | workerTitle |
|----------|------------|-------------|
| 1111     | admin      | מנהל משמרת  |
| 123      | shelly     | מנהל משמרת  |
| 345      | liel       | ראש צוות    |
| 678      | david      | מנהל משמרת  |

## 3. מבנה בסיס הנתונים

### טבלת Titles (תפקידים — טבלת עזר / Reference Table)
| שדה | סוג | תיאור |
|-----|------|--------|
| titleId | INT | מזהה תפקיד (Primary Key) |
| titleName | NVARCHAR(50) | שם התפקיד |

> **טבלת עזר (Reference Table)** — טבלה שמכילה את רשימת הערכים האפשריים לצורך התייחסות בלבד.
> הטבלה **לא נטענת** לזיכרון — התפקידים מוגדרים כ-enum ב-C#.

### טבלת Workers (עובדים)
| שדה | סוג | תיאור |
|-----|------|--------|
| workerId | VARCHAR(20) | מזהה עובד (Primary Key) |
| workerName | NVARCHAR(20) | שם העובד |
| workerTitle | NVARCHAR(50) | תפקיד העובד (טקסט, למשל "מנהל משמרת") |

### טבלת Orders (הזמנות) — טבלת אב
| שדה | סוג | תיאור |
|-----|------|--------|
| orderId | INT | מזהה הזמנה (Primary Key) |
| workerId | VARCHAR(20) | מזהה עובד (Foreign Key → Workers) |
| orderDate | DATE | תאריך ההזמנה |
| orderTotalPrice | INT | סכום כולל |

### טבלת DeliveryOrders (הזמנות משלוח) — ירושה מ-Orders
| שדה | סוג | תיאור |
|-----|------|--------|
| orderId | INT | מזהה הזמנה (Primary Key + Foreign Key → Orders) |
| deliveryAddress | VARCHAR(100) | כתובת למשלוח |
| deliveryDate | DATE | תאריך משלוח |

### טבלת PickupOrders (הזמנות איסוף) — ירושה מ-Orders
| שדה | סוג | תיאור |
|-----|------|--------|
| orderId | INT | מזהה הזמנה (Primary Key + Foreign Key → Orders) |
| pickupTime | DATETIME | מועד איסוף |
| branchLocation | VARCHAR(50) | סניף לאיסוף |

### טבלת Products (מוצרים)
| שדה | סוג | תיאור |
|-----|------|--------|
| productId | INT | מזהה מוצר (Primary Key) |
| productName | NVARCHAR(50) | שם המוצר |
| price | FLOAT | מחיר |
| category | NVARCHAR(30) | קטגוריה |

### טבלת OrderItems (פריטי הזמנה) — Association Class
| שדה | סוג | תיאור |
|-----|------|--------|
| orderId | INT | מזהה הזמנה (Primary Key + Foreign Key → Orders) |
| productId | INT | מזהה מוצר (Primary Key + Foreign Key → Products) |
| quantity | INT | כמות |
| unitPrice | FLOAT | מחיר ליחידה |

> **מפתח ראשי מורכב (Composite PK):** טבלת OrderItems משתמשת בשילוב של `orderId` + `productId` כמפתח ראשי.

### קשרים
- **Workers ↔ Orders:** קשר One-to-Many — לעובד אחד יכולות להיות הזמנות רבות
  - Foreign Key: `Orders.workerId` → `Workers.workerId`
- **Orders ↔ DeliveryOrders:** ירושה (Table-per-Subclass) — הזמנת משלוח מרחיבה הזמנה
  - Foreign Key: `DeliveryOrders.orderId` → `Orders.orderId`
- **Orders ↔ PickupOrders:** ירושה (Table-per-Subclass) — הזמנת איסוף מרחיבה הזמנה
  - Foreign Key: `PickupOrders.orderId` → `Orders.orderId`
- **Orders ↔ Products:** קשר Many-to-Many דרך טבלת OrderItems
  - Foreign Key: `OrderItems.orderId` → `Orders.orderId`
  - Foreign Key: `OrderItems.productId` → `Products.productId`

## 4. עדכון Connection String בפרויקט

הפרויקט מגיע עם שלוש אפשרויות חיבור בקובץ `SQL_CON.cs` — אחת פעילה ושתיים בהערה:

```csharp
// אפשרות 1 (ברירת מחדל) - SQL Server מקומי
conn = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=SAD_0;...");

// אפשרות 2 - Azure SQL (זו האפשרות של הקורס)
//conn = new SqlConnection("Server=tcp:<servername>.database.windows.net,1433;Initial Catalog=<database>;User ID=<username>;Password=<password>;Encrypt=True;...");
```

**בקורס הזה עוברים לאפשרות 2:** הפכו את שורת אפשרות 1 להערה, בטלו את ההערה
משורת אפשרות 2, ומלאו את ארבעת הערכים של בסיס הנתונים של הקבוצה.

> **אזהרה:** השורה הזו תכיל סיסמה אמיתית. אל תעלו אותה ל-git.
> בפרויקט שלכם עדיף להחזיק את מחרוזת החיבור ב-`app.config` ולהוסיף אותו ל-`.gitignore`.

## מה הלאה?

המשיכו למדריך הבא: [סקירת הפרויקט](03-project-overview.md)

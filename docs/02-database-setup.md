# הקמת בסיס הנתונים

מדריך זה מסביר כיצד ליצור את בסיס הנתונים המקומי עבור הפרויקט.

## דרישות מקדימות

ודאו שהשלמתם את [מדריך ההתקנה](01-installation.md) ושכל הכלים מותקנים.

## 1. יצירת בסיס הנתונים

### אפשרות א׳: דרך Command Prompt (מומלץ)

1. פתחו Command Prompt
2. הריצו את הפקודה הבאה:
   ```
   sqlcmd -S "localhost\SQLEXPRESS" -E -i "scripts\create_database.sql"
   ```
   > **שימו לב:** יש להריץ מתוך תיקיית הפרויקט הראשית, שם נמצאת תיקיית `scripts`.
3. אם ההרצה הצליחה, תראו הודעות על שורות שנוספו (rows affected)

### אפשרות ב׳: דרך SSMS

1. פתחו SSMS והתחברו ל-`localhost\SQLEXPRESS`
2. לחצו **File > Open > File**
3. פתחו את הקובץ `scripts\create_database.sql` מתוך תיקיית הפרויקט
4. לחצו **Execute** (או F5)
5. ודאו שאין הודעות שגיאה

## 2. אימות בסיס הנתונים

לאחר הרצת הסקריפט, ודאו שהכל נוצר כראוי:

### ב-SSMS
1. בצד שמאל, פתחו את **Databases > SAD_0 > Tables**
2. ודאו שקיימות שש טבלאות:
   - `dbo.Workers`
   - `dbo.Orders`
   - `dbo.DeliveryOrders`
   - `dbo.PickupOrders`
   - `dbo.Products`
   - `dbo.OrderItems`
3. פתחו את **Programmability > Stored Procedures** וודאו שקיימים:
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

### טבלת Workers (עובדים)
| שדה | סוג | תיאור |
|-----|------|--------|
| workerId | VARCHAR(20) | מזהה עובד (Primary Key) |
| workerName | NVARCHAR(20) | שם העובד |
| workerTitle | NVARCHAR(50) | תפקיד |

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

הפרויקט מגיע עם שני connection strings בקובץ `SQL_CON.cs`:

```csharp
//חיבור לשרת המקומי - לפיתוח מהבית
conn = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=SAD_0;...");
//חיבור לשרת באוניברסיטה - יש לבטל את ההערה ולהעיר את השורה למעלה
//conn = new SqlConnection("Data Source=IEMDBS;Initial Catalog=SAD_0;...");
```

- **לעבודה מהבית:** השאירו את ההגדרה כפי שהיא (localhost)
- **לעבודה באוניברסיטה:** העירו (comment out) את השורה הראשונה, ובטלו את ההערה מהשורה השנייה

## מה הלאה?

המשיכו למדריך הבא: [סקירת הפרויקט](03-project-overview.md)

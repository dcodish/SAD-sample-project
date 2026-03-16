# סקירת פרויקט הדוגמה

מדריך זה מסביר את מבנה פרויקט הדוגמה וכיצד הוא עובד. הבנת הפרויקט הזה תעזור לכם לבנות את הפרויקט שלכם.

## דרישות מקדימות

ודאו שהשלמתם את:
- [מדריך ההתקנה](01-installation.md)
- [הקמת בסיס הנתונים](02-database-setup.md)

## 1. פתיחת הפרויקט

1. פתחו את Visual Studio 2022
2. בחרו **Open a project or solution**
3. נווטו לתיקיית הפרויקט ופתחו את `Example_Project.sln`
4. המתינו ש-Visual Studio יטען את הפרויקט ויבצע NuGet Restore

## 2. מבנה הקבצים

```
Example_Project/
├── Program.cs               ← נקודת ההתחלה — רשימות + initLists + Main
├── SQL_CON.cs               ← חיבור וביצוע שאילתות לבסיס הנתונים
│
│  מחלקות Entity (כל ישות מכילה: שדות, CRUD, טעינה, חיפוש)
├── Worker.cs                ← עובד + initWorkers + seekWorker
├── Order.cs                 ← הזמנה (אב) + initOrders + getNextOrderId
├── DeliveryOrder.cs         ← הזמנת משלוח (יורש מ-Order)
├── PickupOrder.cs           ← הזמנת איסוף (יורש מ-Order)
├── Product.cs               ← מוצר
├── OrderItem.cs             ← פריט בהזמנה
├── Title.cs                 ← Enum תפקידים + TitleHelper
│
│  פאנלים (WinForms Panels)
├── LoginPanel.cs            ← פאנל כניסה (Login)
├── CRUDPanel.cs             ← פאנל ניהול ראשי
├── CreateWorkerPanel.cs     ← יצירת עובד חדש
├── UpdateDeletePanel.cs     ← עדכון/מחיקת עובד
├── CreateDeliveryOrderPanel.cs ← יצירת הזמנת משלוח
├── CreatePickupOrderPanel.cs   ← יצירת הזמנת איסוף
├── WatchOrdersPanel.cs      ← צפייה בהזמנות
├── OrderDetailsPanel.cs     ← פרטי הזמנה
└── MoadAPanel.cs            ← מודעה / אודות
```

## 3. ארכיטקטורה - איך הפרויקט עובד

### עקרון מפתח: All-In-Memory

הפרויקט עובד לפי העיקרון של **טעינה לזיכרון**:
1. בהפעלת התוכנית, **כל** הנתונים נטענים מבסיס הנתונים לרשימות בזיכרון
2. במהלך העבודה, הפעולות מתבצעות על הרשימות **וגם** על בסיס הנתונים
3. כך אנחנו עובדים מהר (מהזיכרון) ושומרים על עקביות (בסיס הנתונים)

```
┌──────────────┐       הפעלה        ┌──────────────┐
│  SQL Server  │  ──────────────>   │   זיכרון     │
│  (DB)        │                    │   (Lists)    │
│              │  <──────────────   │              │
│  Workers     │   Create/Update/   │  Workers[]   │
│  Orders      │   Delete           │  Orders[]    │
└──────────────┘                    └──────────────┘
```

### הרשימות הראשיות (Program.cs)

```csharp
public static List<Worker> Workers;      // רשימת כל העובדים
public static List<Product> Products;    // רשימת כל המוצרים
public static List<Order> Orders;        // רשימת כל ההזמנות (כולל DeliveryOrder, PickupOrder)
public static List<OrderItem> OrderItems; // רשימת כל פריטי ההזמנות (מחלקת קישור)
```

רשימות אלו הן `static` — כלומר נגישות מכל מקום בתוכנית דרך `Program.Workers`, `Program.Orders` וכו'.

> **שימו לב:** הרשימות מוגדרות ב-`Program.cs`, אבל פעולות הטעינה (`initWorkers`, `initOrders`) והחיפוש (`seekWorker`) מוגדרות כפעולות סטטיות בתוך מחלקות ה-Entity עצמן (`Worker.cs`, `Order.cs`).

## 4. המחלקות (Classes)

### Worker — עובד
| שדה | סוג | תיאור |
|-----|------|--------|
| workerId | string | תעודת זהות |
| workerName | string | שם העובד |
| workerTitle | Title (enum) | תפקיד |
| orders | List\<Order\> | רשימת ההזמנות של העובד (private) |

**פעולות:**
- `createWorker()` — הוספה לבסיס הנתונים (Stored Procedure)
- `updateWorker()` — עדכון בבסיס הנתונים
- `deleteWorker()` — מחיקה מבסיס הנתונים ומהרשימה

**פעולות סטטיות (שייכות למחלקה, לא למופע):**
- `Worker.initWorkers()` — טעינת כל העובדים מה-DB לרשימה
- `Worker.seekWorker(id)` — חיפוש עובד לפי ת.ז.

### Order — הזמנה
| שדה | סוג | תיאור |
|-----|------|--------|
| worker | Worker | העובד שביצע את ההזמנה |
| orderId | int | מזהה הזמנה |
| orderDate | DateTime | תאריך |
| orderTotalPrice | int | מחיר כולל |
| orderItems | List\<OrderItem\> | רשימת הפריטים בהזמנה (private) |

**פעולות סטטיות:**
- `Order.initOrders()` — טעינת כל ההזמנות מה-DB (כולל ירושה)
- `Order.seekOrder(id)` — חיפוש הזמנה לפי מזהה
- `Order.getNextOrderId()` — חישוב מזהה הזמנה הבא

### DeliveryOrder — הזמנת משלוח (יורש מ-Order)
| שדה | סוג | תיאור |
|-----|------|--------|
| deliveryAddress | string | כתובת משלוח |
| deliveryDate | DateTime | תאריך משלוח |

### PickupOrder — הזמנת איסוף (יורש מ-Order)
| שדה | סוג | תיאור |
|-----|------|--------|
| pickupTime | DateTime | זמן איסוף |
| branchLocation | string | סניף |

### Product — מוצר
| שדה | סוג | תיאור |
|-----|------|--------|
| productId | int | מזהה מוצר |
| productName | string | שם המוצר |
| price | double | מחיר |
| category | string | קטגוריה |

### OrderItem — פריט בהזמנה (מחלקת קישור)
קשר Many-to-Many בין Order ל-Product.
| שדה | סוג | תיאור |
|-----|------|--------|
| order | Order | הפניה להזמנה |
| product | Product | הפניה למוצר |
| quantity | int | כמות |
| unitPrice | double | מחיר ליחידה |

### Title — תפקיד (Enum)
```csharp
public enum Title
{
    מנהל_משמרת,
    ראש_צוות,
    עובד_חדש
}
```
> ב-Enum אי אפשר רווחים — משתמשים בקו תחתון. מחלקת `TitleHelper` ממירה בין קו תחתון (C#) לרווחים (DB/תצוגה).

## 5. חיבור לבסיס הנתונים (SQL_CON.cs)

מחלקת `SQL_CON` אחראית על כל התקשורת עם בסיס הנתונים:

- **`execute_query()`** — להרצת שאילתות שמחזירות נתונים (SELECT)
- **`execute_non_query()`** — להרצת פקודות שמשנות נתונים (INSERT, UPDATE, DELETE)

שתי הפעולות עובדות עם **Stored Procedures** — שאילתות מוכנות שנמצאות בבסיס הנתונים.

## 6. זרימת התוכנית (Flow)

```
הפעלה (Program.Main)
    │
    ├── טעינת נתונים מ-DB לרשימות (initLists)
    │
    └── פתיחת טופס הכניסה (LoginPanel)
            │
            ├── הזנת ת.ז. וסיסמה
            │
            ├── אם ת.ז. = "1111" (מנהל) ──> פאנל CRUDPanel
            │       │
            │       ├── יצירת עובד חדש (CreateWorkerPanel)
            │       ├── עדכון/מחיקת עובד (UpdateDeletePanel)
            │       ├── הזמנת משלוח (CreateDeliveryOrderPanel)
            │       └── הזמנת איסוף (CreatePickupOrderPanel)
            │
            └── אם עובד רגיל ──> צפייה בהזמנות (WatchOrdersPanel)
                                    │
                                    └── לחיצה על הזמנה ──> פרטי הזמנה (OrderDetailsPanel)
```

## 7. הרצת הפרויקט

1. ב-Visual Studio, לחצו **F5** (או Start)
2. ייפתח טופס הכניסה
3. להתחברות כמנהל: ת.ז. `1111`, סיסמה `1234`
   - תוכלו ליצור, לעדכן ולמחוק עובדים
4. להתחברות כעובד: ת.ז. של עובד קיים (למשל `123`), סיסמה `1234`
   - תוכלו לצפות בהזמנות

## 8. איך להתאים לפרויקט שלכם

כדי לבנות את המערכת שלכם על בסיס פרויקט הדוגמה:

ראו את המדריך המפורט: [סדר פיתוח — 5 הצעדים הראשונים](06-development-order.md)

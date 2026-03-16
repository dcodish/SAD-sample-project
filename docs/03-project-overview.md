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
public static List<Worker> Workers;  // רשימת כל העובדים
public static List<Order> Orders;    // רשימת כל ההזמנות
```

רשימות אלו הן `static` — כלומר נגישות מכל מקום בתוכנית דרך `Program.Workers` ו-`Program.Orders`.

> **שימו לב:** הרשימות מוגדרות ב-`Program.cs`, אבל פעולות הטעינה (`initWorkers`, `initOrders`) והחיפוש (`seekWorker`) מוגדרות כפעולות סטטיות בתוך מחלקות ה-Entity עצמן (`Worker.cs`, `Order.cs`).

## 4. המחלקות (Classes)

### Worker — עובד
| שדה | סוג | תיאור |
|-----|------|--------|
| WorkerId | string | תעודת זהות |
| WorkerName | string | שם העובד |
| workerTitle | Title (enum) | תפקיד |
| orders | List\<Order\> | רשימת ההזמנות של העובד |

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
| OrderTotalPrice | int | מחיר כולל |

### Title — תפקיד (Enum)
```csharp
public enum Title
{
    manager,
    senior,
    junior
}
```

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
            │       └── עדכון/מחיקת עובד (UpdateDeletePanel)
            │
            └── אם עובד רגיל ──> צפייה בהזמנות (WatchOrdersPanel)
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

1. **הגדירו את ה-Entities שלכם** — צרו מחלקות (כמו `Worker` ו-`Order`) עבור כל ישות ב-Class Diagram שלכם
2. **צרו את הטבלאות** — בנו את בסיס הנתונים לפי ה-Class Diagram
3. **כתבו Stored Procedures** — עבור כל פעולת CRUD על כל ישות
4. **בנו את הטפסים** — צרו WinForms עבור כל מסך במערכת
5. **טענו לזיכרון** — ב-`Program.cs`, צרו רשימות וטענו את הנתונים בהפעלה

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
├── Title.cs                 ← Enum תפקידים + TitleHelper (המרה בין enum לטקסט)
│
│  פאנלים (WinForms Panels)
├── LoginPanel.cs            ← פאנל כניסה (Login)
├── CRUDPanel.cs             ← פאנל ניהול ראשי
├── CreateWorkerPanel.cs     ← יצירת עובד חדש
├── UpdateDeletePanel.cs     ← עדכון/מחיקת עובד
├── CreateDeliveryOrderPanel.cs ← יצירת הזמנת משלוח
├── CreatePickupOrderPanel.cs   ← יצירת הזמנת איסוף
├── WatchOrdersPanel.cs      ← צפייה בהזמנות
└── OrderDetailsPanel.cs     ← פרטי הזמנה
```

## 3. ארכיטקטורה - איך הפרויקט עובד

### עקרון מפתח: All-In-Memory

הפרויקט עובד לפי העיקרון של **טעינה לזיכרון**:
1. בהפעלת התוכנית, **כל** הנתונים נטענים מבסיס הנתונים לרשימות בזיכרון
2. במהלך העבודה, הפעולות מתבצעות על הרשימות **וגם** על בסיס הנתונים
3. כך אנחנו עובדים מהר (מהזיכרון) ושומרים על עקביות (בסיס הנתונים)

```
┌──────────────┐       הפעלה        ┌────────────────┐
│  SQL Server  │  ──────────────>   │   זיכרון       │
│  (DB)        │                    │   (Lists)      │
│              │  <──────────────   │                │
│  Titles      │   Create/Update/   │                │
│  Workers     │   Delete           │  Workers[]     │
│  Products    │                    │  Products[]    │
│  Orders      │                    │  Orders[]      │
│  OrderItems  │                    │  OrderItems[]  │
└──────────────┘                    └────────────────┘
```

### הרשימות הראשיות (Program.cs)

```csharp
public static List<Worker> Workers;      // רשימת כל העובדים
public static List<Product> Products;    // רשימת כל המוצרים
public static List<Order> Orders;        // רשימת כל ההזמנות (כולל DeliveryOrder, PickupOrder)
public static List<OrderItem> OrderItems; // רשימת כל פריטי ההזמנות (מחלקת קישור)
```

> **שימו לב:** אין רשימה של `Title` — התפקידים מוגדרים כ-enum ב-C# ולא נטענים מה-DB.

רשימות אלו הן `static` — כלומר נגישות מכל מקום בתוכנית דרך `Program.Workers`, `Program.Orders` וכו'.

> **שימו לב:** הרשימות מוגדרות ב-`Program.cs`, אבל פעולות הטעינה (`initWorkers`, `initOrders`) והחיפוש (`seekWorker`) מוגדרות כפעולות סטטיות בתוך מחלקות ה-Entity עצמן (`Worker.cs`, `Order.cs`). תפקידים (Title) הם enum ולכן אינם נטענים מה-DB ואין להם רשימה ב-Program.cs.

## 4. המחלקות (Classes)

### Worker — עובד

**שדות:**
| שדה | סוג | הרשאה | תיאור |
|-----|------|--------|--------|
| workerId | string | private | תעודת זהות |
| workerName | string | private | שם העובד |
| workerTitle | Title (enum) | private | תפקיד |
| orders | List\<Order\> | private | רשימת ההזמנות של העובד (קשר One-to-Many) |

**בנאי:**
- `Worker(string id, string name, Title title, bool is_new)` — אם `is_new = true`: שומר ב-DB ומוסיף לרשימה. אם `false`: מופע שנטען מה-DB.

**Getters & Setters:**
- `getId()` — מחזיר `string` (תעודת זהות)
- `getName()` — מחזיר `string` (שם העובד)
- `getTitle()` — מחזיר `Title` (תפקיד)
- `setName(string name)` — עדכון שם העובד
- `setTitle(Title t)` — עדכון תפקיד

**פעולות ניהול קשר (One-to-Many עם Order):**
- `getOrders()` — מחזיר `List<Order>` (רשימת ההזמנות של העובד)
- `addOrder(Order newOrder)` — הוספת הזמנה לרשימה + קריאה ל-`newOrder.setWorker(this)`
- `removeOrder(Order oldOrder)` — הסרת הזמנה מהרשימה

**פעולות CRUD (מול בסיס הנתונים):**
- `createWorker()` — הוספה לבסיס הנתונים דרך Stored Procedure (`SP_add_worker`)
- `updateWorker()` — עדכון בבסיס הנתונים דרך Stored Procedure (`SP_Update_worker`)
- `deleteWorker()` — מחיקה מהרשימה בזיכרון ומבסיס הנתונים (`SP_delete_worker`)

**פעולות סטטיות (שייכות למחלקה, לא למופע):**
- `Worker.initWorkers()` — טעינת כל העובדים מה-DB לרשימה `Program.Workers`
- `Worker.seekWorker(string id)` — חיפוש עובד לפי ת.ז. ברשימת הזיכרון, מחזיר `Worker` או `null`

---

### Order — הזמנה (מחלקת אב לירושה)

**שדות:**
| שדה | סוג | הרשאה | תיאור |
|-----|------|--------|--------|
| worker | Worker | private | העובד שביצע את ההזמנה |
| orderId | int | protected | מזהה הזמנה (protected כדי שמחלקות הבן יוכלו לגשת) |
| orderDate | DateTime | protected | תאריך ההזמנה |
| orderTotalPrice | int | protected | מחיר כולל |
| orderItems | List\<OrderItem\> | private | רשימת הפריטים בהזמנה (קשר Many-to-Many עם Product) |

**בנאי:**
- `Order(Worker w, int id, DateTime date, int totalPrice, bool is_new)` — אם `is_new = true`: קורא ל-`createOrder()`, מוסיף לרשימת העובד ול-`Program.Orders`.

**Getters & Setters:**
- `getOrderId()` — מחזיר `int`
- `getOrderDate()` — מחזיר `DateTime`
- `getOrderTotalPrice()` — מחזיר `int`
- `getWorker()` — מחזיר `Worker`
- `setWorker(Worker w)` — עדכון העובד

**פעולות ניהול קשר (Many-to-Many עם Product דרך OrderItem):**
- `addOrderItem(OrderItem item)` — הוספת פריט לרשימת הפריטים
- `getOrderItems()` — מחזיר `List<OrderItem>`

**פעולות CRUD:**
- `virtual createOrder()` — שמירה בטבלת Orders ב-DB דרך `SP_add_order`. הפעולה מוגדרת כ-**virtual** כדי שמחלקות הבן (`DeliveryOrder`, `PickupOrder`) יוכלו לדרוס (override) אותה.

**פעולות סטטיות:**
- `Order.initOrders()` — טעינת כל ההזמנות מה-DB, כולל זיהוי סוג הירושה (DeliveryOrder / PickupOrder / Order רגיל) לפי שדות ב-DB
- `Order.seekOrder(int id)` — חיפוש הזמנה לפי מזהה, מחזיר `Order` או `null`
- `Order.getNextOrderId()` — מחשב את מזהה ההזמנה הבא (MAX + 1)

---

### DeliveryOrder — הזמנת משלוח (יורש מ-Order)

> מחלקה שיורשת מ-`Order`. הבנאי קורא ל-`base(w, id, date, totalPrice, false)` — שולח `false` לבנאי האב כדי שהאב לא ישמור ב-DB, ומחלקת הבן מנהלת את השמירה בעצמה.

**שדות נוספים (מעבר לשדות שיורש מ-Order):**
| שדה | סוג | הרשאה | תיאור |
|-----|------|--------|--------|
| deliveryAddress | string | private | כתובת משלוח |
| deliveryDate | DateTime | private | תאריך משלוח |

**בנאי:**
- `DeliveryOrder(Worker w, int id, DateTime date, int totalPrice, string deliveryAddress, DateTime deliveryDate, bool is_new)` — קורא ל-`base(...)` עם `false`, ואם `is_new = true`: קורא ל-`createOrder()`, מוסיף לרשימת העובד ול-`Program.Orders`.

**Getters:**
- `getDeliveryAddress()` — מחזיר `string`
- `getDeliveryDate()` — מחזיר `DateTime`

**פעולות CRUD:**
- `override createOrder()` — דורס את הפעולה של האב. שומר **בשתי טבלאות**: קודם בטבלת Orders (`SP_add_order`) ואז בטבלת DeliveryOrders (`SP_add_delivery_order`).

---

### PickupOrder — הזמנת איסוף (יורש מ-Order)

> מחלקה שיורשת מ-`Order`. הבנאי קורא ל-`base(w, id, date, totalPrice, false)` — שולח `false` לבנאי האב כדי שהאב לא ישמור ב-DB, ומחלקת הבן מנהלת את השמירה בעצמה.

**שדות נוספים (מעבר לשדות שיורש מ-Order):**
| שדה | סוג | הרשאה | תיאור |
|-----|------|--------|--------|
| pickupTime | DateTime | private | זמן איסוף |
| branchLocation | string | private | סניף |

**בנאי:**
- `PickupOrder(Worker w, int id, DateTime date, int totalPrice, DateTime pickupTime, string branchLocation, bool is_new)` — קורא ל-`base(...)` עם `false`, ואם `is_new = true`: קורא ל-`createOrder()`, מוסיף לרשימת העובד ול-`Program.Orders`.

**Getters:**
- `getPickupTime()` — מחזיר `DateTime`
- `getBranchLocation()` — מחזיר `string`

**פעולות CRUD:**
- `override createOrder()` — דורס את הפעולה של האב. שומר **בשתי טבלאות**: קודם בטבלת Orders (`SP_add_order`) ואז בטבלת PickupOrders (`SP_add_pickup_order`).

---

### Product — מוצר

**שדות:**
| שדה | סוג | הרשאה | תיאור |
|-----|------|--------|--------|
| productId | int | private | מזהה מוצר |
| productName | string | private | שם המוצר |
| price | double | private | מחיר |
| category | string | private | קטגוריה |

**בנאי:**
- `Product(int id, string name, double price, string category, bool is_new)` — אם `is_new = true`: שומר ב-DB ומוסיף ל-`Program.Products`.

**Getters & Setters:**
- `getProductId()` — מחזיר `int`
- `getProductName()` — מחזיר `string`
- `getPrice()` — מחזיר `double`
- `getCategory()` — מחזיר `string`
- `setProductName(string name)` — עדכון שם המוצר
- `setPrice(double price)` — עדכון מחיר
- `setCategory(string category)` — עדכון קטגוריה

**פעולות CRUD:**
- `createProduct()` — הוספה לבסיס הנתונים דרך `SP_add_product`

**פעולות סטטיות:**
- `Product.initProducts()` — טעינת כל המוצרים מה-DB לרשימה `Program.Products`
- `Product.seekProduct(int id)` — חיפוש מוצר לפי מזהה, מחזיר `Product` או `null`

---

### OrderItem — פריט בהזמנה (מחלקת קישור / Association Class)

> **מחלקת קישור (Association Class):** מייצגת את הקשר **Many-to-Many** בין `Order` ל-`Product`. בנוסף להפניות לשני הצדדים, היא מכילה שדות ייחודיים לקשר (כמות, מחיר ליחידה). ב-DB: טבלת `OrderItems` עם מפתח ראשי מורכב (`orderId` + `productId`).

**שדות:**
| שדה | סוג | הרשאה | תיאור |
|-----|------|--------|--------|
| order | Order | private | הפניה להזמנה (צד אחד של הקשר) |
| product | Product | private | הפניה למוצר (צד שני של הקשר) |
| quantity | int | private | כמות — שדה ייחודי לקשר |
| unitPrice | double | private | מחיר ליחידה — שדה ייחודי לקשר |

**בנאי:**
- `OrderItem(Order order, Product product, int quantity, double unitPrice, bool is_new)` — אם `is_new = true`: שומר ב-DB, מוסיף לרשימת הפריטים של ההזמנה (`order.addOrderItem`) ול-`Program.OrderItems`.

**Getters:**
- `getOrder()` — מחזיר `Order`
- `getProduct()` — מחזיר `Product`
- `getQuantity()` — מחזיר `int`
- `getUnitPrice()` — מחזיר `double`
- `getTotalPrice()` — מחזיר `double` (מחושב: `quantity * unitPrice`)

**פעולות CRUD:**
- `createOrderItem()` — הוספה לבסיס הנתונים דרך `SP_add_order_item`

**פעולות סטטיות:**
- `OrderItem.initOrderItems()` — טעינת כל פריטי ההזמנות מה-DB. **חשוב:** חייבים לטעון קודם Products ו-Orders, כי `initOrderItems` משתמש ב-`Order.seekOrder()` וב-`Product.seekProduct()` כדי לקשר כל פריט להזמנה ולמוצר שלו.

---

### Title — תפקיד (Enum)

> `Title` הוא **enum** (מנייה) שמגדיר את התפקידים האפשריים. הערכים ב-enum משתמשים בקו תחתון (underscore), למשל `מנהל_משמרת`.
>
> `TitleHelper` הוא **static class** שמספק המרה בין שם ה-enum (עם קו תחתון) לטקסט תצוגה/DB (עם רווחים).

**ערכי ה-Enum:**
| ערך | טקסט תצוגה |
|-----|------------|
| `מנהל_משמרת` | מנהל משמרת |
| `ראש_צוות` | ראש צוות |
| `עובד_חדש` | עובד חדש |

**TitleHelper (static class):**
- `TitleHelper.ToDisplayString(Title title)` — ממיר ערך enum לטקסט תצוגה (מחליף `_` ברווח)
- `TitleHelper.FromDisplayString(string displayString)` — ממיר טקסט תצוגה לערך enum (מחליף רווח ב-`_`)

> **שימו לב:** בבסיס הנתונים קיימת טבלת `Titles` כטבלת עזר/reference, אך היא **לא נטענת** לזיכרון ואין לה רשימה ב-`Program.cs`. העובדים שומרים את התפקיד כטקסט (`NVARCHAR(50)`) בטבלת Workers, ו-`initWorkers()` ממיר אותו ל-enum באמצעות `TitleHelper.FromDisplayString()`.

## 5. חיבור לבסיס הנתונים (SQL_CON.cs)

מחלקת `SQL_CON` אחראית על כל התקשורת עם בסיס הנתונים:

- **`execute_query()`** — להרצת שאילתות שמחזירות נתונים (SELECT)
- **`execute_non_query()`** — להרצת פקודות שמשנות נתונים (INSERT, UPDATE, DELETE)

שתי הפעולות עובדות עם **Stored Procedures** — שאילתות מוכנות שנמצאות בבסיס הנתונים.

## 6. זרימת התוכנית (Flow)

```
הפעלה (Program.Main)
    │
    ├── טעינת נתונים מ-DB לרשימות (initLists: Worker.initWorkers() ראשון, ואז Products, Orders, OrderItems)
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

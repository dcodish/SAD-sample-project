# מיפוי Class Diagram לבסיס הנתונים

מדריך זה מסביר כיצד לתרגם את ה-Class Diagram שלכם למבנה בסיס נתונים (טבלאות, שדות, קשרים).
נשתמש בדוגמאות מתוך פרויקט הדוגמה.

## 1. מיפוי טיפוסי נתונים (Data Types)

כל שדה ב-Class Diagram צריך להיות מתורגם לטיפוס מתאים גם ב-C# וגם ב-SQL Server.

### טבלת המרה

| Class Diagram | C# | SQL Server | הערות |
|--------------|-----|------------|-------|
| String | `string` | `VARCHAR(n)` | n = אורך מקסימלי. השתמשו ב-`NVARCHAR` עבור עברית |
| Integer | `int` | `INT` | |
| Float / Double | `double` | `FLOAT` | |
| Boolean | `bool` | `BIT` | 0 = false, 1 = true |
| Date | `DateTime` | `DATE` | תאריך בלבד |
| DateTime | `DateTime` | `DATETIME` | תאריך + שעה |
| Enum | `enum` | `VARCHAR(n)` | נשמר כטקסט ב-DB, ראו סעיף 2 |

### דוגמה מהפרויקט — Worker

```
Class Diagram:            C# (Worker.cs):              SQL (Workers table):
─────────────            ──────────────               ─────────────────────
workerId: String    →    string WorkerId         →    workerId VARCHAR(20)
workerName: String  →    string WorkerName       →    workerName VARCHAR(20)
workerTitle: Title  →    Title workerTitle        →    workerTitle VARCHAR(50)
```

> **שימו לב:** כשבוחרים אורך ל-`VARCHAR`, חשבו על הערך הארוך ביותר שיכנס.
> למשל, שם עובד של 20 תווים מספיק לרוב השמות, אבל כתובת תצטרך 100 תווים.

## 2. Enum — מיפוי ערכים מוגדרים מראש

כשיש ב-Class Diagram שדה עם ערכים קבועים (למשל תפקיד: מנהל משמרת/ראש צוות/עובד חדש), משתמשים ב-Enum.

### ב-C# — הגדרת ה-Enum (Title.cs):
```csharp
public enum Title
{
    מנהל_משמרת,
    ראש_צוות,
    עובד_חדש
}
```

> **שימו לב:** ב-Enum אי אפשר להשתמש ברווחים! לכן משתמשים בקו תחתון `_` במקום רווח.

### ב-C# — שימוש במחלקה:
```csharp
private Title workerTitle;  // השדה הוא מסוג Title
```

### ב-SQL — נשמר כטקסט עם רווחים:
```sql
workerTitle NVARCHAR(50)  -- הערך נשמר כ-"מנהל משמרת", "ראש צוות" וכו'
```

> **שימו לב:** משתמשים ב-`NVARCHAR` (עם N) כדי לתמוך בעברית ותווים מיוחדים.
> `VARCHAR` רגיל לא תומך בעברית!

### הבעיה: קו תחתון ב-C# vs רווחים ב-DB

| איפה | מה נשמר | דוגמה |
|------|---------|-------|
| C# (Enum) | קו תחתון (חובה) | `Title.מנהל_משמרת` |
| SQL Server | רווחים (טבעי) | `"מנהל משמרת"` |
| טופס (תצוגה) | רווחים (קריא) | `"מנהל משמרת"` |

### הפתרון: מחלקת עזר (TitleHelper)

```csharp
public static class TitleHelper
{
    //המרה מ-enum לטקסט תצוגה (קו תחתון → רווח)
    public static string ToDisplayString(Title title)
    {
        return title.ToString().Replace('_', ' ');
    }

    //המרה מטקסט תצוגה ל-enum (רווח → קו תחתון)
    public static Title FromDisplayString(string displayString)
    {
        string enumString = displayString.Replace(' ', '_');
        return (Title)Enum.Parse(typeof(Title), enumString);
    }
}
```

### שימוש ב-Helper — כל המקרים:

**שמירה ל-DB** — ממירים Enum לטקסט עם רווחים:
```csharp
c.Parameters.AddWithValue("@title", TitleHelper.ToDisplayString(this.workerTitle));
// Title.מנהל_משמרת → "מנהל משמרת"
```

**טעינה מ-DB** — ממירים טקסט עם רווחים ל-Enum:
```csharp
Title T = TitleHelper.FromDisplayString(rdr.GetValue(2).ToString());
// "מנהל משמרת" → "מנהל_משמרת" → Title.מנהל_משמרת
```

**מילוי ComboBox** — מציגים עם רווחים:
```csharp
foreach (Title t in Enum.GetValues(typeof(Title)))
{
    comboBox1.Items.Add(TitleHelper.ToDisplayString(t));
}
```

**קריאה מ-ComboBox** — ממירים חזרה ל-Enum:
```csharp
Title title = TitleHelper.FromDisplayString(comboBox1.Text);
```

### מתי להשתמש ב-Enum?
- כשיש רשימה **קבועה וקצרה** של ערכים (3-20 ערכים)
- כשהערכים **לא משתנים** בזמן ריצה
- דוגמאות: סטטוס הזמנה, תפקיד, סוג תשלום
- אם הערכים מכילים רווחים — **צרו מחלקת Helper** כמו בדוגמה

## 3. ירושה (Inheritance) — Table-per-Subclass

כשיש ב-Class Diagram ירושה (חץ עם משולש ריק), צריך להחליט איך לייצג אותה בבסיס הנתונים.
אנחנו משתמשים בשיטת **Table-per-Subclass**.

### העיקרון

```
         ┌──────────┐
         │  Order   │   ← מחלקת אב (Base Class)
         │──────────│
         │ orderId  │
         │ orderDate│
         │ totalPrice│
         └────┬─────┘
              │
     ┌────────┴────────┐
     │                 │
┌────┴─────┐    ┌──────┴──────┐
│ Delivery │    │   Pickup    │  ← מחלקות בן (Subclasses)
│ Order    │    │   Order     │
│──────────│    │─────────────│
│ address  │    │ pickupTime  │
│ delivDate│    │ branchLoc   │
└──────────┘    └─────────────┘
```

### הכלל: טבלה לכל מחלקה

- **טבלת אב** — מכילה את כל השדות המשותפים
- **טבלת בן** — מכילה **רק** את השדות הייחודיים + Foreign Key לטבלת האב

```sql
-- טבלת האב — כל השדות המשותפים
CREATE TABLE Orders (
    workerId VARCHAR(20),
    orderId INT PRIMARY KEY,
    orderDate DATE,
    orderTotalPrice INT,
    FOREIGN KEY (workerId) REFERENCES Workers(workerId)
);

-- טבלת בן — רק שדות ייחודיים + FK לאב
CREATE TABLE DeliveryOrders (
    orderId INT PRIMARY KEY,              -- גם PK וגם FK!
    deliveryAddress VARCHAR(100),
    deliveryDate DATE,
    FOREIGN KEY (orderId) REFERENCES Orders(orderId)
);

-- טבלת בן נוספת
CREATE TABLE PickupOrders (
    orderId INT PRIMARY KEY,              -- גם PK וגם FK!
    pickupTime DATETIME,
    branchLocation VARCHAR(50),
    FOREIGN KEY (orderId) REFERENCES Orders(orderId)
);
```

> **נקודה חשובה:** ה-`orderId` בטבלת הבן הוא **גם Primary Key וגם Foreign Key**.
> זה מבטיח שכל שורה בטבלת הבן קשורה בדיוק לשורה אחת בטבלת האב.

### ב-C# — הגדרת הירושה:

**מחלקת האב (Order.cs):**
```csharp
public class Order
{
    public Worker worker;
    protected int orderId;          // protected כדי שמחלקות הבן יוכלו לגשת
    protected DateTime orderDate;
    protected int OrderTotalPrice;

    public virtual void createOrder()  // virtual כדי שהבן יוכל לדרוס
    {
        // שמירה בטבלת Orders בלבד
    }
}
```

**מחלקת הבן (DeliveryOrder.cs):**
```csharp
public class DeliveryOrder : Order      // ירושה מ-Order
{
    private string deliveryAddress;     // שדות ייחודיים
    private DateTime deliveryDate;

    public DeliveryOrder(...)
        : base(w, Id, Date, totalPrice, false)  // קריאה לבנאי האב
    {
        this.deliveryAddress = deliveryAddress;
        this.deliveryDate = deliveryDate;
        if (is_new)
        {
            this.createOrder();
            w.addOrder(this);
            Program.Orders.Add(this);
        }
    }

    public override void createOrder()   // דריסת המתודה של האב
    {
        // שלב 1: שמירה בטבלת האב (Orders)
        SqlCommand c = new SqlCommand();
        c.CommandText = "EXECUTE SP_add_order @worker, @orderId, ...";
        // ...

        // שלב 2: שמירה בטבלת הבן (DeliveryOrders)
        SqlCommand c2 = new SqlCommand();
        c2.CommandText = "EXECUTE SP_add_delivery_order @orderId, @deliveryAddress, ...";
        // ...
    }
}
```

### נקודות חשובות בירושה:

| מושג | הסבר | דוגמה |
|------|-------|-------|
| `protected` | שדה שנגיש גם למחלקות הבן | `protected int orderId` |
| `virtual` | מתודה שמחלקת הבן יכולה לדרוס | `public virtual void createOrder()` |
| `override` | דריסת מתודה של האב | `public override void createOrder()` |
| `: base(...)` | קריאה לבנאי של האב | `: base(w, Id, Date, price, false)` |
| `is` | בדיקת סוג בזמן ריצה | `if (o is DeliveryOrder)` |

### טעינה מ-DB — שימוש ב-LEFT JOIN

כדי לטעון את כל ההזמנות כולל סוגי הירושה, משתמשים ב-LEFT JOIN:

```sql
CREATE PROCEDURE Get_all_Orders_Full
AS
    SELECT o.workerId, o.orderId, o.orderDate, o.orderTotalPrice,
           d.deliveryAddress, d.deliveryDate,
           p.pickupTime, p.branchLocation
    FROM Orders o
    LEFT JOIN DeliveryOrders d ON o.orderId = d.orderId
    LEFT JOIN PickupOrders p ON o.orderId = p.orderId;
```

ב-C#, בודקים איזה שדות הם `NULL` כדי לדעת מאיזה סוג ההזמנה:

```csharp
if (rdr.GetValue(4) != DBNull.Value)        // יש deliveryAddress?
    o = new DeliveryOrder(...);              // → הזמנת משלוח
else if (rdr.GetValue(6) != DBNull.Value)    // יש pickupTime?
    o = new PickupOrder(...);                // → הזמנת איסוף
else
    o = new Order(...);                      // → הזמנה רגילה
```

### שמירה ב-DB — שתי שאילתות

כשיוצרים הזמנת בן (למשל DeliveryOrder), חייבים לשמור **בשתי טבלאות**:

```
1. INSERT INTO Orders (...)           ← שדות משותפים בטבלת האב
2. INSERT INTO DeliveryOrders (...)   ← שדות ייחודיים בטבלת הבן
```

זו הסיבה שמתודת `createOrder()` ב-DeliveryOrder מבצעת **שתי קריאות** ל-DB.

## 4. מחלקת קישור (Association Class) — קשר Many-to-Many

כשיש ב-Class Diagram קשר Many-to-Many (למשל: הזמנה מכילה מוצרים רבים, ומוצר יכול להופיע בהזמנות רבות), צריך **מחלקת קישור** (Association Class).

### הדוגמה: Order ↔ Product דרך OrderItem

```
Order ──── OrderItem ──── Product
            │
            ├── quantity (כמות)
            └── unitPrice (מחיר ליחידה)
```

**OrderItem** היא לא סתם טבלת חיבור — יש לה **שדות משלה** (כמות, מחיר ליחידה).

### ב-SQL — טבלה עם מפתח ראשי מורכב

```sql
CREATE TABLE Products (
    productId INT PRIMARY KEY,
    productName NVARCHAR(50),
    price FLOAT,
    category NVARCHAR(30)
);

-- טבלת הקישור — המפתח הראשי מורכב משני FK
CREATE TABLE OrderItems (
    orderId INT NOT NULL,
    productId INT NOT NULL,
    quantity INT,
    unitPrice FLOAT,
    PRIMARY KEY (orderId, productId),              -- מפתח מורכב!
    FOREIGN KEY (orderId) REFERENCES Orders(orderId),
    FOREIGN KEY (productId) REFERENCES Products(productId)
);
```

> **נקודה חשובה:** המפתח הראשי של OrderItems הוא **מורכב** — שילוב של `orderId` + `productId`.
> זה מבטיח שאותו מוצר לא יופיע פעמיים באותה הזמנה.

### ב-C# — מחלקת הקישור

```csharp
public class OrderItem
{
    //הפניות לשני הצדדים של הקשר
    private Order order;
    private Product product;

    //שדות ייחודיים לקשר
    private int quantity;
    private double unitPrice;

    public OrderItem(Order order, Product product, int quantity, double unitPrice, bool is_new)
    {
        this.order = order;
        this.product = product;
        this.quantity = quantity;
        this.unitPrice = unitPrice;
        if (is_new)
        {
            this.createOrderItem();
            order.addOrderItem(this);          // קישור להזמנה
            Program.OrderItems.Add(this);
        }
    }
}
```

### הרשימות בצד ה-Order

ב-`Order.cs` מוסיפים רשימה של פריטים:
```csharp
public List<OrderItem> orderItems;

public void addOrderItem(OrderItem item)
{
    if (this.orderItems == null)
        this.orderItems = new List<OrderItem>();
    if (!this.orderItems.Contains(item))
        this.orderItems.Add(item);
}
```

### טעינה — סדר הטעינה קריטי!

מחלקת קישור מפנה ל**שני צדדים** — לכן חייבים לטעון אותה **אחרונה**:

```csharp
// ב-Program.cs
public static void initLists()
{
    Worker.initWorkers();       // 1. עובדים (בסיסי)
    Product.initProducts();     // 2. מוצרים (בסיסי)
    Order.initOrders();         // 3. הזמנות (מפנות לעובדים)
    OrderItem.initOrderItems(); // 4. אחרון! (מפנה להזמנות ולמוצרים)
}
```

בטעינת OrderItems, מחפשים את ההזמנה והמוצר לפי מזהה:
```csharp
public static void initOrderItems()
{
    // ...
    while (rdr.Read())
    {
        int orderId = int.Parse(rdr.GetValue(0).ToString());
        int productId = int.Parse(rdr.GetValue(1).ToString());
        int quantity = int.Parse(rdr.GetValue(2).ToString());
        double unitPrice = double.Parse(rdr.GetValue(3).ToString());

        //חיפוש שני הצדדים של הקשר
        Order o = Order.seekOrder(orderId);
        Product p = Product.seekProduct(productId);

        if (o != null && p != null)
        {
            OrderItem item = new OrderItem(o, p, quantity, unitPrice, false);
            o.addOrderItem(item);              // קישור להזמנה
            Program.OrderItems.Add(item);
        }
    }
}
```

### מתי צריך מחלקת קישור?

| מצב | פתרון |
|------|--------|
| One-to-Many (עובד → הזמנות) | Foreign Key + רשימה |
| Many-to-Many **בלי** שדות נוספים | טבלת חיבור פשוטה (נדיר) |
| Many-to-Many **עם** שדות נוספים | **מחלקת קישור** (Association Class) |

> **כלל אצבע:** אם הקשר עצמו מכיל מידע (כמות, תאריך, מחיר) — צריך מחלקת קישור.

## 5. סיכום — צ'קליסט למיפוי Class Diagram ל-DB

- [ ] לכל מחלקה ב-Class Diagram — צרו טבלה
- [ ] לכל שדה — בחרו טיפוס SQL מתאים (ראו טבלת ההמרה)
- [ ] לכל Enum — צרו `enum` ב-C# ושמרו כ-`NVARCHAR` ב-DB + מחלקת Helper
- [ ] לכל ירושה — צרו טבלת בן עם PK+FK שמצביע לטבלת האב
- [ ] לכל קשר One-to-Many — הוסיפו Foreign Key + רשימה
- [ ] לכל קשר Many-to-Many — צרו מחלקת קישור עם מפתח מורכב
- [ ] לכל טבלה — כתבו Stored Procedures עבור CRUD
- [ ] ב-`Program.cs` — טענו את כל הנתונים לרשימות בזיכרון **בסדר הנכון**

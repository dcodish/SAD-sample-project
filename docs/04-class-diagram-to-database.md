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
| Lookup | `class` | `INT` (FK) | מזהה שמצביע לטבלת Lookup, ראו סעיף 2 |

### דוגמה מהפרויקט — Worker

```
Class Diagram:            C# (Worker.cs):              SQL (Workers table):
─────────────            ──────────────               ─────────────────────
workerId: String    →    string WorkerId         →    workerId VARCHAR(20)
workerName: String  →    string WorkerName       →    workerName VARCHAR(20)
workerTitle: Title  →    Title workerTitle        →    titleId INT (FK → Titles)
```

> **שימו לב:** כשבוחרים אורך ל-`VARCHAR`, חשבו על הערך הארוך ביותר שיכנס.
> למשל, שם עובד של 20 תווים מספיק לרוב השמות, אבל כתובת תצטרך 100 תווים.

## 2. טבלאות Lookup — מיפוי ערכים מוגדרים מראש

כשיש ב-Class Diagram שדה עם ערכים קבועים (למשל תפקיד: מנהל משמרת/ראש צוות/עובד חדש), שומרים אותם ב**טבלת Lookup** בבסיס הנתונים.

### הרעיון

במקום לקבע (hardcode) ערכים בקוד, שומרים אותם בטבלה נפרדת ב-DB.
כך, הוספת ערך חדש (למשל תפקיד חדש) דורשת רק הוספת שורה ב-DB — **בלי שינוי קוד!**

### ב-SQL — מבנה הטבלאות

```sql
-- טבלת Lookup — הערכים הקבועים
CREATE TABLE Titles (
    titleId INT PRIMARY KEY,
    titleName NVARCHAR(50)
);

-- טבלת Workers — מצביעה ל-Titles דרך FK
CREATE TABLE Workers (
    workerId VARCHAR(20) PRIMARY KEY,
    workerName NVARCHAR(50),
    titleId INT,
    FOREIGN KEY (titleId) REFERENCES Titles(titleId)
);
```

> **שימו לב:** ב-Workers שומרים רק את `titleId` (מספר) — לא את שם התפקיד כטקסט.
> זה מבטיח עקביות ומונע שגיאות כתיב.

### ב-C# — מחלקת Title (לא Enum!)

Title היא **מחלקה רגילה** שנטענת מה-DB:

```csharp
public class Title
{
    private int titleId;
    private string titleName;

    public Title(int titleId, string titleName)
    {
        this.titleId = titleId;
        this.titleName = titleName;
    }

    // Getters
    public int TitleId { get { return titleId; } }
    public string TitleName { get { return titleName; } }

    // חשוב! כך ה-ComboBox יציג את שם התפקיד
    public override string ToString()
    {
        return titleName;
    }

    // טעינת כל התפקידים מה-DB לרשימה
    public static void initTitles()
    {
        // SELECT * FROM Titles
        // ... לכל שורה יוצרים אובייקט Title ומוסיפים ל-Program.Titles
    }

    // חיפוש תפקיד לפי מזהה
    public static Title seekTitleById(int id)
    {
        foreach (Title t in Program.Titles)
            if (t.titleId == id) return t;
        return null;
    }

    // חיפוש תפקיד לפי שם
    public static Title seekTitleByName(string name)
    {
        foreach (Title t in Program.Titles)
            if (t.titleName == name) return t;
        return null;
    }
}
```

### סדר הטעינה — Lookup קודם!

טבלאות Lookup חייבות להיטען **לפני** הישויות שמפנות אליהן:

```csharp
// ב-Program.cs
public static void initLists()
{
    Title.initTitles();         // 1. קודם טבלאות Lookup!
    Worker.initWorkers();       // 2. עובדים (מפנים ל-Titles)
    Product.initProducts();     // 3. מוצרים
    Order.initOrders();         // 4. הזמנות (מפנות לעובדים)
    OrderItem.initOrderItems(); // 5. אחרון! (מפנה להזמנות ולמוצרים)
}
```

> **למה?** כש-Worker נטען מה-DB, הוא מכיל `titleId`.
> כדי להמיר אותו לאובייקט `Title`, הרשימה `Program.Titles` כבר חייבת להיות מוכנה.

### ב-Worker — טעינת Title לפי מזהה

```csharp
// בבנאי או ב-initWorkers:
int titleId = int.Parse(rdr.GetValue(2).ToString());
Title t = Title.seekTitleById(titleId);   // מוצאים את אובייקט ה-Title
Worker w = new Worker(id, name, t, false);
```

**שמירה ל-DB** — שומרים את המזהה בלבד:
```csharp
c.Parameters.AddWithValue("@titleId", this.workerTitle.TitleId);
```

### מילוי ComboBox מרשימת Lookup

```csharp
// מילוי ה-ComboBox עם כל התפקידים
foreach (Title t in Program.Titles)
{
    comboBox1.Items.Add(t);  // ToString() מציג את titleName
}
```

### קריאה מ-ComboBox

```csharp
// המשתמש בחר תפקיד — מקבלים אובייקט Title
Title title = (Title)comboBox1.SelectedItem;
```

> **שימו לב:** בזכות `ToString()` שמחזיר את `titleName`, ה-ComboBox מציג את שם התפקיד,
> ובזכות הקאסט ל-`Title` מקבלים חזרה את האובייקט עם ה-`titleId`.

### מתי להשתמש בטבלת Lookup?

| מצב | פתרון |
|------|--------|
| ערכים קבועים שעשויים להשתנות (תפקידים, סטטוסים, קטגוריות) | **טבלת Lookup** |
| נתונים שמשתמשים יוצרים (לקוחות, הזמנות) | **טבלה רגילה** |

- **כן:** תפקידים, סטטוס הזמנה, סוגי תשלום, קטגוריות מוצרים
- **לא:** לקוחות, הזמנות, מוצרים — אלה ישויות שגדלות כל הזמן

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
    Title.initTitles();         // 1. קודם טבלאות Lookup!
    Worker.initWorkers();       // 2. עובדים (מפנים ל-Titles)
    Product.initProducts();     // 3. מוצרים (בסיסי)
    Order.initOrders();         // 4. הזמנות (מפנות לעובדים)
    OrderItem.initOrderItems(); // 5. אחרון! (מפנה להזמנות ולמוצרים)
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
- [ ] לכל ערך קבוע (תפקיד, סטטוס, קטגוריה) — צרו **טבלת Lookup** ב-DB ומחלקה ב-C# עם `initTitles()`, `seekById()`, `seekByName()`
- [ ] לכל ירושה — צרו טבלת בן עם PK+FK שמצביע לטבלת האב
- [ ] לכל קשר One-to-Many — הוסיפו Foreign Key + רשימה
- [ ] לכל קשר Many-to-Many — צרו מחלקת קישור עם מפתח מורכב
- [ ] לכל טבלה — כתבו Stored Procedures עבור CRUD
- [ ] ב-`Program.cs` — טענו את כל הנתונים לרשימות בזיכרון **בסדר הנכון** (טבלאות Lookup קודם!)

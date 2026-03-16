# צ'קליסט מימוש — מהדיאגרמה לקוד עובד

מדריך זה מלווה אתכם מהרגע שבסיס הנתונים מחובר ועובד, דרך בניית המחלקות, ועד למסכים עובדים.
עבדו לפי הסדר — כל שלב בונה על הקודם.

> **דוגמאות מהפרויקט:** בכל שלב מצוין הקובץ הרלוונטי מפרויקט הדוגמה. פתחו אותו וראו איך זה נראה בפועל.

---

## שלב 1: טבלאות Lookup וערכי Enum

לפני שמתחילים לבנות ישויות, בדקו ב-Class Diagram אם יש שדות עם **ערכים קבועים** (כמו סטטוס, סוג, תפקיד, קטגוריה).

> **דוגמה בפרויקט:** `Title.cs` — enum לתפקידי עובדים, `Titles` — טבלת Lookup ב-DB

### בבסיס הנתונים:
- [ ] צרו טבלת Lookup (reference) לכל קבוצת ערכים קבועים (ראו: טבלת `Titles` ב-`scripts/create_database.sql`)
- [ ] הכניסו את הערכים לטבלה
- [ ] צרו Stored Procedure לשליפת הערכים (`Get_all_XYZ`) (ראו: `Get_all_Titles`)

### ב-C#:
- [ ] צרו קובץ `XYZ.cs` עם `enum` שמכיל את אותם ערכים (ראו: `Title.cs`)
- [ ] אם הערכים מכילים רווחים — השתמשו בקו תחתון ב-enum (ראו: `מנהל_משמרת` ב-`Title.cs`)
- [ ] אם יש קו תחתון — צרו `XYZHelper` עם `ToDisplayString()` ו-`FromDisplayString()` (ראו: `TitleHelper` ב-`Title.cs`)

### דוגמה:
```
DB:  טבלת Statuses עם "ממתין לאישור", "בטיפול", "הושלם"
C#:  enum Status { ממתין_לאישור, בטיפול, הושלם }
     StatusHelper.ToDisplayString(Status.ממתין_לאישור) → "ממתין לאישור"
```

### אימות:
- [ ] כל ערך ב-enum קיים גם בטבלת ה-Lookup
- [ ] ה-Helper ממיר נכון לשני הכיוונים

---

## שלב 2: טבלאות וישויות בסיסיות

התחילו מישויות שאין להן תלות באחרות (אין Foreign Key לישות אחרת).
ב-Class Diagram, אלו ישויות שאף חץ לא נכנס אליהן (או רק חיצי enum).

> **דוגמה בפרויקט:** `Worker.cs` — ישות בסיסית, `Product.cs` — ישות בסיסית נוספת

### לכל ישות בסיסית:

#### בבסיס הנתונים:
- [ ] צרו טבלה עם כל השדות מה-Class Diagram (ראו: טבלת `Workers` ב-`scripts/create_database.sql`)
- [ ] הגדירו Primary Key
- [ ] השתמשו ב-`NVARCHAR` לשדות שיכולים להכיל עברית
- [ ] בחרו טיפוסים מתאימים (ראו טבלת ההמרה ב-[מדריך 04](04-class-diagram-to-database.md))
- [ ] צרו Stored Procedures (ראו: `SP_add_worker`, `SP_Update_worker`, `SP_delete_worker`, `Get_all_Workers`):
  - [ ] `Get_all_XYZ` — שליפת כל הרשומות
  - [ ] `SP_add_XYZ` — הוספה
  - [ ] `SP_update_XYZ` — עדכון
  - [ ] `SP_delete_XYZ` — מחיקה

#### ב-C# — קובץ המחלקה:
- [ ] צרו קובץ `XYZ.cs` (Add > Class) (ראו: `Worker.cs` כתבנית)
- [ ] **שדות** — `private` לכל שדה מה-Class Diagram (ראו: שדות ב-`Worker.cs` שורות 12-15)
- [ ] **בנאי** — מקבל את כל השדות + `bool is_new` (ראו: בנאי ב-`Worker.cs` שורות 22-32)
  - [ ] אם `is_new`: קורא ל-`createXYZ()` ומוסיף ל-`Program.XYZs`
- [ ] **Getters** — `getXYZ()` לכל שדה (ראו: `Worker.cs` שורות 37-39)
- [ ] **Setters** — `setXYZ()` לשדות שניתן לעדכן (ראו: `Worker.cs` שורות 41-42)
- [ ] **פעולות DB** (ראו: `Worker.cs` שורות 81-111):
  - [ ] `createXYZ()` — בונה `SqlCommand` עם ה-SP המתאים
  - [ ] `updateXYZ()` — אותו דפוס
  - [ ] `deleteXYZ()` — מסיר מהרשימה + מוחק מה-DB
- [ ] **מתודות סטטיות** (ראו: `Worker.cs` שורות 117-147):
  - [ ] `initXYZs()` — טוענת מה-DB ל-`Program.XYZs` (עם `is_new = false`)
  - [ ] `seekXYZ(id)` — מחפשת ברשימה לפי מזהה

#### ב-Program.cs:
- [ ] הוסיפו רשימה: `public static List<XYZ> XYZs;` (ראו: `Program.cs` שורות 23-26)
- [ ] הוסיפו קריאה ב-`initLists()`: `XYZ.initXYZs();` (ראו: `Program.cs` שורות 42-45)

### אימות:
- [ ] הפרויקט מתקמפל (Build)
- [ ] הריצו — אין שגיאות בטעינה
- [ ] הוסיפו `MessageBox.Show(Program.XYZs.Count.ToString())` זמנית ב-`Main` כדי לוודא שהנתונים נטענו

---

## שלב 3: ישויות עם תלויות (Foreign Key)

אחרי שהישויות הבסיסיות עובדות, המשיכו לישויות שמפנות אליהן.

> **דוגמה בפרויקט:** `Order.cs` — מפנה ל-`Worker` דרך FK

### לכל ישות עם FK:

#### בבסיס הנתונים:
- [ ] צרו טבלה כמו בשלב 2 (ראו: טבלת `Orders` ב-`scripts/create_database.sql`)
- [ ] הוסיפו `FOREIGN KEY` לטבלת האב
- [ ] צרו Stored Procedures (כנ"ל)

#### ב-C#:
- [ ] כמו שלב 2, אבל:
- [ ] השדה של ה-FK הוא **אובייקט** (לא מזהה): `private Customer customer;` (לא `string customerId`) (ראו: `Order.cs` שורה 14 — `private Worker worker;`)
- [ ] ב-`initXYZs()`: חפשו את האובייקט המקושר (ראו: `Order.cs` `initOrders()` — `Worker.seekWorker(rdr.GetValue(0).ToString())`)
- [ ] ב-`createXYZ()`: שמרו את ה-**מזהה**: `cmd.Parameters.AddWithValue("@customerId", this.customer.getId());` (ראו: `Order.cs` `createOrder()` — `this.getWorker().getId()`)

#### ב-Program.cs:
- [ ] הוסיפו רשימה וקריאה ב-`initLists()`
- [ ] **סדר חשוב!** טענו קודם את ישות האב, אחר כך את הבן (ראו: `Program.cs` — `Worker.initWorkers()` לפני `Order.initOrders()`)

### אימות:
- [ ] הפרויקט מתקמפל
- [ ] בדקו שהקשרים נטענו נכון (האובייקט המקושר לא null)

---

## שלב 4: ירושה (אם יש)

אם ב-Class Diagram יש ירושה (חץ עם משולש ריק):

> **דוגמה בפרויקט:** `DeliveryOrder.cs` ו-`PickupOrder.cs` — יורשים מ-`Order.cs`

### בבסיס הנתונים:
- [ ] טבלת אב — שדות משותפים (כמו בשלב 2/3) (ראו: טבלת `Orders`)
- [ ] טבלת בן — רק שדות ייחודיים + PK שהוא גם FK לטבלת האב (ראו: טבלת `DeliveryOrders`)
- [ ] SP להוספת שורה בטבלת הבן (ראו: `SP_add_delivery_order`)
- [ ] SP לשליפה עם LEFT JOIN שמחזיר את כל הסוגים (ראו: `Get_all_Orders_Full`)

### ב-C#:
- [ ] מחלקת אב: שדות `protected` (לא `private`) + מתודת `createXYZ()` מסוג `virtual` (ראו: `Order.cs` שורות 14-16, 71)
- [ ] מחלקת בן: `public class ChildXYZ : ParentXYZ` (ראו: `DeliveryOrder.cs` שורה 6)
  - [ ] בנאי קורא ל-`: base(..., false)` (ראו: `DeliveryOrder.cs` שורה 13)
  - [ ] `override createXYZ()` — שומר **בשתי טבלאות** (אב + בן) (ראו: `DeliveryOrder.cs` שורות 31-50)
  - [ ] Getters לשדות הייחודיים (ראו: `DeliveryOrder.cs` שורות 26-27)
- [ ] ב-`initXYZs()` של האב: בודקים `DBNull.Value` כדי לזהות את סוג הבן (ראו: `Order.cs` `initOrders()` — בדיקות `rdr.GetValue(4) != DBNull.Value`)

### אימות:
- [ ] יצירת מופע של כל סוג בן — בדקו ששתי הטבלאות מתמלאות
- [ ] טעינה — בדקו שהסוג הנכון נוצר (`if (o is ChildXYZ)`)

---

## שלב 5: מחלקות קישור — Association Class (אם יש)

אם ב-Class Diagram יש קשר Many-to-Many עם שדות על הקשר:

> **דוגמה בפרויקט:** `OrderItem.cs` — מחלקת קישור בין `Order` ל-`Product`

### בבסיס הנתונים:
- [ ] טבלת קישור עם **מפתח ראשי מורכב** (שני FK ביחד) (ראו: טבלת `OrderItems` — PK מורכב מ-`orderId` + `productId`)
- [ ] שדות ייחודיים לקשר (כמות, מחיר, תאריך) (ראו: `quantity`, `unitPrice`)
- [ ] SP להוספה ושליפה (ראו: `SP_add_order_item`, `Get_all_OrderItems`)

### ב-C#:
- [ ] מחלקה עם הפניות לשני הצדדים (ראו: `OrderItem.cs` — `private Order order;` + `private Product product;`)
- [ ] שדות ייחודיים לקשר (ראו: `OrderItem.cs` — `quantity`, `unitPrice`)
- [ ] ב-`initXYZs()`: חפשו את **שני** הצדדים לפי מזהה (ראו: `OrderItem.cs` `initOrderItems()` — `Order.seekOrder()` + `Product.seekProduct()`)
- [ ] הוסיפו לרשימה של הצד הרלוונטי (ראו: `OrderItem.cs` — `o.addOrderItem(item)`)

#### ב-Program.cs:
- [ ] הוסיפו רשימה ב-Program.cs (ראו: `Program.OrderItems`)
- [ ] **סדר חשוב!** טענו אחרון — אחרי שני הצדדים כבר נטענו (ראו: `OrderItem.initOrderItems()` אחרון ב-`initLists()`)

### אימות:
- [ ] בדקו שהקישורים נטענו — לכל הזמנה יש רשימת פריטים

---

## שלב 6: מסכים (Panels)

אחרי שכל הישויות עובדות ונטענות, בנו את הממשק.
ראו [מדריך ניווט בין מסכים](07-form-navigation.md) לפרטים מלאים.

### לכל מסך:
- [ ] צרו UserControl חדש (Add > User Control) — שם שמסתיים ב-`Panel`
- [ ] עצבו ב-Designer (drag & drop)
- [ ] ניווט: `mainForm.showPanel(new XYZPanel());` (ראו: `CRUDPanel.cs` — כפתורי ניווט)
- [ ] כפתור חזרה: `mainForm.showPanel(new PreviousPanel());`

### מסך תצוגה (רשימה):
> **דוגמה בפרויקט:** `WatchOrdersPanel.cs` — מציג הזמנות ב-DataGridView

- [ ] `DataGridView` שנטען מהרשימה בזיכרון
- [ ] בנו `DataTable` ומלאו אותה מהרשימה (ראו: `WatchOrdersPanel.cs` `loadOrders()`)
- [ ] `dataGridView.DataSource = dt;`

### מסך יצירה:
> **דוגמה בפרויקט:** `CreateWorkerPanel.cs` — יצירת עובד, `CreateDeliveryOrderPanel.cs` — יצירת הזמנת משלוח

- [ ] שדות קלט לכל שדה של הישות
- [ ] ComboBox לשדות Enum — מלאו עם `Enum.GetValues` + Helper (ראו: `CreateWorkerPanel.cs` שורות 17-22)
- [ ] ComboBox לשדות FK — מלאו מהרשימה בזיכרון (ראו: `CreateDeliveryOrderPanel.cs` שורות 17-20)
- [ ] ולידציה — בדקו שדות חובה לפני יצירה (ראו: `CreateWorkerPanel.cs` שורות 26-35)
- [ ] יצירה: `new XYZ(..., true)` ואז ניווט חזרה
- [ ] אם יש מזהה אוטומטי — חשבו אותו (ראו: `CreateDeliveryOrderPanel.cs` — `Order.getNextOrderId()`)

### מסך עדכון/מחיקה:
> **דוגמה בפרויקט:** `UpdateDeletePanel.cs` — חיפוש, עדכון ומחיקת עובד

- [ ] שדה חיפוש + כפתור חיפוש
- [ ] **בדיקת null** אחרי `seekXYZ()` — הציגו הודעה אם לא נמצא (ראו: `UpdateDeletePanel.cs` שורות 26-31)
- [ ] מילוי שדות מהאובייקט שנמצא (ראו: `UpdateDeletePanel.cs` שורות 33-42)
- [ ] כפתור עדכון: `xyz.setField(value); xyz.updateXYZ();` (ראו: `UpdateDeletePanel.cs` שורות 45-50)
- [ ] כפתור מחיקה: `xyz.deleteXYZ();` (ראו: `UpdateDeletePanel.cs` שורות 53-57)

### מסך פרטים:
> **דוגמה בפרויקט:** `OrderDetailsPanel.cs` — מציג פרטי הזמנה + פריטים (Association Class)

- [ ] מקבל אובייקט דרך הבנאי (ראו: `OrderDetailsPanel.cs` — מקבל `Order` ו-`Worker`)
- [ ] מציג שדות בסיסיים ב-Labels
- [ ] מציג רשימה מקושרת ב-DataGridView (ראו: `loadOrderItems()`)

### אימות לכל מסך:
- [ ] הריצו ובדקו שהנתונים מוצגים
- [ ] בדקו יצירה — האם הנתון נשמר ומופיע ברשימה?
- [ ] בדקו עדכון — האם השינוי נשמר?
- [ ] בדקו מחיקה — האם הנתון נמחק?
- [ ] בדקו ניווט — האם כפתורי חזרה עובדים?

---

## סדר הטעינה ב-initLists — תזכורת

> **ראו:** `Program.cs` — `initLists()`

```csharp
public static void initLists()
{
    // 1. ישויות בסיסיות (בלי FK)
    // 2. ישויות עם FK (אחרי מה שהן מפנות אליו)
    // 3. מחלקות קישור (אחרונות — מפנות לשני צדדים)
}
```

**הכלל:** אם A מפנה ל-B, אז B חייב להיטען לפני A.

---

## טעויות נפוצות

| טעות | תסמין | פתרון |
|------|-------|-------|
| שכחתם `is_new = false` בטעינה | כפילויות ב-DB | ודאו שה-init קורא לבנאי עם `false` |
| סדר טעינה שגוי | NullReferenceException בטעינה | טענו אב לפני בן |
| שכחתם בדיקת null אחרי seek | קריסה כשמזהה לא קיים | תמיד `if (result == null) return;` |
| שכחתם להוסיף ל-initLists | הרשימה ריקה | בדקו ש-`initLists` קורא ל-init שלכם |
| שם SP לא תואם | שגיאה מה-DB | ודאו שהשם ב-C# זהה לשם ב-DB |
| טיפוסים לא תואמים | Conversion error | ודאו ש-C# type תואם ל-SQL type |
| שכחתם NVARCHAR | עברית מופיעה כ-???? | השתמשו ב-NVARCHAR ולא VARCHAR לטקסט עברי |
| ComboBox ריק | אין ערכים לבחירה | ודאו שמילאתם ב-constructor של ה-Panel |

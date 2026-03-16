# חיבור לבסיס הנתונים — SQL_CON

מדריך זה מסביר כיצד עובד החיבור לבסיס הנתונים בפרויקט, ואיך להשתמש בו.

## 1. הרעיון

כל פעולה מול בסיס הנתונים דורשת 3 דברים:
1. **חיבור** (Connection) — כתובת השרת + שם בסיס הנתונים
2. **פקודה** (Command) — מה לבצע (Stored Procedure + פרמטרים)
3. **ביצוע** — שליחת הפקודה דרך החיבור

מחלקת `SQL_CON` מטפלת בחיבור ובביצוע. הפקודה נבנית במחלקות ה-Entity (כמו `Worker`, `Order`).

## 2. מבנה SQL_CON

```
SQL_CON
├── conn (SqlConnection)         ← אובייקט החיבור
├── SQL_CON()                    ← בנאי - יוצר את החיבור
├── execute_non_query(cmd)       ← לפעולות שמשנות נתונים
└── execute_query(cmd)           ← לפעולות שמחזירות נתונים
```

## 3. Connection String — מחרוזת החיבור

מחרוזת החיבור אומרת ל-C# **איפה** נמצא בסיס הנתונים:

```csharp
"Data Source=localhost\\SQLEXPRESS;Initial Catalog=SAD_0;Integrated Security=True;TrustServerCertificate=True"
```

| חלק | משמעות | דוגמה |
|-----|--------|-------|
| `Data Source` | כתובת השרת | `localhost\SQLEXPRESS` |
| `Initial Catalog` | שם בסיס הנתונים | `SAD_0` |
| `Integrated Security` | אימות Windows | `True` |
| `TrustServerCertificate` | אישור אבטחה מקומי | `True` |

> **שימו לב:** ב-C# צריך `\\` (backslash כפול) כי `\` הוא תו מיוחד.

## 4. שני סוגי פעולות

### סוג 1: `execute_non_query` — פעולות שמשנות נתונים

משמש עבור: **INSERT, UPDATE, DELETE**

הזרימה:
```
פתיחת חיבור → קישור פקודה → ביצוע → סגירת חיבור
     ↓              ↓            ↓           ↓
  conn.Open()  cmd.Connection  Execute    conn.Close()
                  = conn     NonQuery()    (finally)
```

```csharp
public void execute_non_query(SqlCommand cmd)
{
    try
    {
        conn.Open();              // פתיחת חיבור לשרת
        cmd.Connection = conn;    // קישור הפקודה לחיבור
        cmd.ExecuteNonQuery();     // ביצוע הפקודה
    }
    catch (Exception ex)
    {
        // אם משהו נכשל - מציגים הודעת שגיאה
        MessageBox.Show("שגיאה: " + ex.Message);
    }
    finally
    {
        // סגירת החיבור - קורה תמיד, גם אם הייתה שגיאה!
        conn.Close();
    }
}
```

**למה `finally`?**
אם לא נסגור את החיבור, הוא יישאר פתוח ויחסום חיבורים אחרים.
`finally` מבטיח שהסגירה תקרה **תמיד** — גם אם הפקודה הצליחה וגם אם נכשלה.

### סוג 2: `execute_query` — פעולות שמחזירות נתונים

משמש עבור: **SELECT** (דרך Stored Procedures)

הזרימה:
```
פתיחת חיבור → קישור פקודה → ביצוע → החזרת Reader
     ↓              ↓            ↓           ↓
  conn.Open()  cmd.Connection  Execute    return
                  = conn      Reader()    reader
```

```csharp
public SqlDataReader execute_query(SqlCommand cmd)
{
    try
    {
        conn.Open();
        cmd.Connection = conn;
        SqlDataReader reader = cmd.ExecuteReader();  // מחזיר אובייקט לקריאת שורות
        return reader;
    }
    catch (Exception ex)
    {
        MessageBox.Show("שגיאה: " + ex.Message);
        return null;
    }
}
```

**למה אין `finally` עם `conn.Close()` כאן?**
כי ה-Reader עדיין צריך את החיבור הפתוח!
ה-Reader קורא שורה אחרי שורה מבסיס הנתונים — אם נסגור את החיבור, הוא לא יוכל להמשיך לקרוא.

## 5. ההבדל בין שתי הפעולות — סיכום

| | `execute_non_query` | `execute_query` |
|---|---------------------|-----------------|
| **מתי** | INSERT, UPDATE, DELETE | SELECT |
| **מחזיר** | כלום (void) | SqlDataReader |
| **סוגר חיבור** | כן (ב-finally) | לא (Reader צריך אותו) |
| **דוגמה** | הוספת עובד | טעינת כל העובדים |

## 6. איך משתמשים — דוגמה מלאה

### דוגמה 1: הוספת עובד (INSERT)

```csharp
// שלב 1: בניית הפקודה
SqlCommand c = new SqlCommand();
c.CommandText = "EXECUTE SP_add_worker @id, @name, @title";

// שלב 2: הוספת פרמטרים (מונע SQL Injection!)
c.Parameters.AddWithValue("@id", this.WorkerId);
c.Parameters.AddWithValue("@name", this.WorkerName);
c.Parameters.AddWithValue("@title", TitleHelper.ToDisplayString(this.workerTitle));

// שלב 3: ביצוע דרך SQL_CON
SQL_CON SC = new SQL_CON();
SC.execute_non_query(c);
```

### דוגמה 2: טעינת כל העובדים (SELECT)

```csharp
// שלב 1: בניית הפקודה
SqlCommand c = new SqlCommand();
c.CommandText = "EXECUTE dbo.Get_all_Workers";

// שלב 2: ביצוע וקבלת Reader
SQL_CON SC = new SQL_CON();
SqlDataReader rdr = SC.execute_query(c);

// שלב 3: קריאת התוצאות שורה אחרי שורה
while (rdr.Read())
{
    string id = rdr.GetValue(0).ToString();      // עמודה 0 = workerId
    string name = rdr.GetValue(1).ToString();     // עמודה 1 = workerName
    string title = rdr.GetValue(2).ToString();    // עמודה 2 = workerTitle
    // ...
}
```

## 7. שגיאות נפוצות

### "Connection is already open"
```
שגיאה: The connection was not closed. The connection's current state is open.
```
**סיבה:** ניסיתם לפתוח חיבור שכבר פתוח.
**פתרון:** ודאו שאתם יוצרים `new SQL_CON()` לכל פעולה, או שהחיבור נסגר אחרי כל שימוש.

### "Cannot access destination table"
```
שגיאה: Invalid object name 'dbo.TableName'
```
**סיבה:** שם הטבלה או ה-Stored Procedure לא קיים ב-DB.
**פתרון:** ודאו שהרצתם את `create_database.sql` ושהשמות תואמים.

### "Conversion failed"
```
שגיאה: Conversion failed when converting the varchar value 'abc' to data type int.
```
**סיבה:** שלחתם ערך מסוג לא מתאים (למשל טקסט במקום מספר).
**פתרון:** בדקו את הטיפוסים בפרמטרים ובטבלה.

## 8. איך להתאים לפרויקט שלכם

אין צורך לשנות את `SQL_CON` — אפשר להשתמש בה כמו שהיא!
מה שכן צריך:
1. עדכנו את ה-Connection String אם שם בסיס הנתונים שלכם שונה
2. במחלקות ה-Entity שלכם, בנו פקודות `SqlCommand` עם הפרמטרים הנכונים
3. השתמשו ב-`execute_non_query` עבור INSERT/UPDATE/DELETE
4. השתמשו ב-`execute_query` עבור SELECT

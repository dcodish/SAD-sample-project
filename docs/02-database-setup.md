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
2. ודאו שקיימות שתי טבלאות:
   - `dbo.Workers`
   - `dbo.Orders`
3. פתחו את **Programmability > Stored Procedures** וודאו שקיימים:
   - `dbo.Get_all_Workers`
   - `dbo.Get_all_Orders`
   - `dbo.SP_add_worker`
   - `dbo.SP_Update_worker`
   - `dbo.SP_delete_worker`
   - `dbo.SP_add_order`

### בדיקת הנתונים
לחצו Right Click על טבלת `Workers` ובחרו **Select Top 1000 Rows**. אמורים להופיע 3 רשומות:

| workerId | workerName | workerTitle |
|----------|------------|-------------|
| 123      | shelly     | manager     |
| 345      | liel       | senior      |
| 678      | david      | manager     |

## 3. מבנה בסיס הנתונים

### טבלת Workers (עובדים)
| שדה | סוג | תיאור |
|-----|------|--------|
| workerId | VARCHAR(20) | מזהה עובד (Primary Key) |
| workerName | VARCHAR(20) | שם העובד |
| workerTitle | VARCHAR(50) | תפקיד (manager / senior / junior) |

### טבלת Orders (הזמנות)
| שדה | סוג | תיאור |
|-----|------|--------|
| workerId | VARCHAR(20) | מזהה עובד (Foreign Key → Workers) |
| orderId | INT | מזהה הזמנה (Primary Key) |
| orderDate | DATE | תאריך ההזמנה |
| orderTotalPrice | INT | סכום כולל |

### קשרים
- **Workers ↔ Orders:** קשר של One-to-Many — לעובד אחד יכולות להיות הזמנות רבות
- Foreign Key: `Orders.workerId` → `Workers.workerId`

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

# מדריך התקנה - סביבת פיתוח

מדריך זה מסביר כיצד להתקין את כל הכלים הנדרשים לפרויקט.

## דרישות מקדימות

לפני שמתחילים, ודאו שיש לכם:
- מחשב עם Windows 10/11
- חיבור אינטרנט
- הרשאות מנהל (Administrator) על המחשב

## 1. התקנת Visual Studio 2022

Visual Studio היא סביבת הפיתוח (IDE) בה נעבוד לאורך הפרויקט.

1. היכנסו לאתר: https://visualstudio.microsoft.com/downloads/
2. הורידו את **Visual Studio 2022 Community** (חינמי)
3. הריצו את קובץ ההתקנה
4. במסך הבחירה, סמנו את ה-Workload הבא:
   - **.NET desktop development**
5. לחצו **Install** והמתינו לסיום ההתקנה

> **שימו לב:** ההתקנה יכולה לקחת זמן. ודאו שיש לכם מספיק מקום בדיסק (~10GB).

## 2. התקנת .NET 8 SDK

הפרויקט בנוי על .NET 8. ייתכן ש-Visual Studio כבר התקין אותו, אבל כדאי לוודא.

1. פתחו Command Prompt (חיפוש `cmd` בתפריט Start)
2. הריצו את הפקודה:
   ```
   dotnet --version
   ```
3. אם התוצאה מתחילה ב-`8.` (למשל `8.0.419`) — הכל תקין, עברו לשלב הבא
4. אם לא, הורידו והתקינו מ: https://dotnet.microsoft.com/download/dotnet/8.0
   - בחרו **SDK** (לא Runtime)
   - הורידו את הגרסה עבור Windows x64

## 3. התקנת SQL Server Express

SQL Server Express הוא שרת בסיס הנתונים שנעבוד איתו. זו גרסה חינמית של SQL Server.

1. היכנסו לאתר: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
2. גללו למטה ל-**Express** ולחצו **Download now**
3. הריצו את קובץ ההתקנה
4. בחרו **Basic** — זו האפשרות הפשוטה והמהירה ביותר
5. המתינו לסיום ההתקנה

בסיום ההתקנה תראו מסך סיכום עם:
- **Instance Name:** `SQLEXPRESS`
- **Connection String:** `Server=localhost\SQLEXPRESS;...`

> **שמרו את הפרטים האלה** — תצטרכו אותם בהמשך.

## 4. התקנת SQL Server Management Studio (SSMS)

SSMS הוא הכלי לניהול בסיס הנתונים — יצירת טבלאות, הרצת שאילתות, ועוד.

1. במסך הסיכום של SQL Server Express, לחצו על **Install SSMS**
   - לחלופין, הורידו מ: https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms
2. הריצו את קובץ ההתקנה והמתינו לסיום
3. ייתכן שיידרש restart למחשב בסיום

## 5. אימות ההתקנה

לאחר התקנת כל הכלים, בדקו שהכל עובד:

### בדיקת SQL Server
1. פתחו Command Prompt
2. הריצו:
   ```
   sqlcmd -S "localhost\SQLEXPRESS" -E -Q "SELECT @@VERSION"
   ```
3. אם מופיעה גרסת SQL Server — ההתקנה הצליחה

### בדיקת SSMS
1. פתחו את SSMS (חיפוש `SSMS` בתפריט Start)
2. בשדה **Server name** הזינו: `localhost\SQLEXPRESS`
3. ודאו ש-**Authentication** מוגדר ל-`Windows Authentication`
4. לחצו **Connect**
5. אם ההתחברות הצליחה — הכל מוכן!

## מה הלאה?

המשיכו למדריך הבא: [הקמת בסיס הנתונים](02-database-setup.md)

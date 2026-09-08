# מדריך התקנה - סביבת פיתוח

מדריך זה מסביר כיצד להתקין את כל הכלים הנדרשים לפרויקט.

> **הערה חשובה — שני מסלולים לבסיס הנתונים:**
> בקורס זה יש **שני מסלולים** לבסיס הנתונים: **Azure SQL בענן (מומלץ)** — בסיס נתונים
> אחד משותף לכל הקבוצה, בלי התקנה מקומית; או **SQL Server Express מקומי** — כל אחד
> מתקין מופע משלו. בחרו מסלול אחד לכל הקבוצה. בשני המסלולים מתקינים **SSMS**.
>
> מדריך ההתקנה המלא והמעודכן, כולל השוואה בין המסלולים, נמצא ב-[`PREREQS.md`](../PREREQS.md)
> בשורש הפרויקט (חלק ג׳). המסמך הזה הוא סיכום קצר בעברית.

## דרישות מקדימות

לפני שמתחילים, ודאו שיש לכם:

- מחשב עם Windows 10/11
- חיבור אינטרנט
- הרשאות מנהל (Administrator) על המחשב

## 1. התקנת Visual Studio (2022 או 2025)

Visual Studio היא סביבת הפיתוח (IDE) בה נעבוד לאורך הפרויקט.
**שתי הגרסאות — 2022 ו-2025 — עובדות מצוין** ושתיהן תומכות ב-.NET 8 ובמעצב ה-WinForms.

> **אם כבר מותקנת אצלכם Visual Studio 2022 מקורס קודם — אל תשדרגו.** רק ודאו
> שה-Workload‏ **.NET desktop development** מותקן: פתחו את Visual Studio Installer,
> לחצו **Modify**, וסמנו אותו אם הוא חסר.

אם אין לכם Visual Studio בכלל:

1. היכנסו לאתר: <https://visualstudio.microsoft.com/downloads/>
2. הורידו את **Visual Studio Community** (חינמי) — הגרסה החדשה ביותר שמוצעת
3. הריצו את קובץ ההתקנה
4. במסך הבחירה, סמנו את ה-Workload הבא:
   - **.NET desktop development**
5. לחצו **Install** והמתינו לסיום ההתקנה

> **שימו לב:** ההתקנה יכולה לקחת זמן. ודאו שיש לכם מספיק מקום בדיסק (~10GB).

## 2. התקנת .NET 8 SDK

הפרויקט בנוי על .NET 8. ייתכן ש-Visual Studio כבר התקין אותו, אבל כדאי לוודא.

1. פתחו PowerShell — לחצו על מקש **Windows**, הקלידו `powershell`, ולחצו **Enter**
2. הריצו את הפקודה:
   ```
   dotnet --version
   ```
3. אם התוצאה מתחילה ב-`8.` (למשל `8.0.419`) — הכל תקין, עברו לשלב הבא
4. אם לא, הורידו והתקינו מ: <https://dotnet.microsoft.com/download/dotnet/8.0>
   - בחרו **SDK** (לא Runtime)
   - הורידו את הגרסה עבור Windows x64

בנוסף ל-SDK נדרש גם **.NET 8 Windows Desktop Runtime** — הוא זה שמריץ אפליקציות WinForms.
בדקו עם `dotnet --list-runtimes` שמופיע `Microsoft.WindowsDesktop.App 8.x`.

## 3. בסיס הנתונים — בחירת מסלול

### מסלול א׳ — Azure SQL (מומלץ)

חבר אחד מהקבוצה יוצר בסיס נתונים בענן, וכל שאר חברי הקבוצה מקבלים ממנו את פרטי
החיבור ומתחברים לאותו בסיס נתונים.

היתרונות: אין התקנה כבדה, אין בעיות של TCP/IP ושמות Instance, וכל הקבוצה עובדת
על אותם נתונים בלי VPN.

### מסלול ב׳ — SQL Server Express מקומי

כל חבר קבוצה מתקין מופע SQL Server משלו. לכל אחד יהיה עותק נפרד של בסיס הנתונים,
ולכן יש להריץ את סקריפטי הסכמה אצל כל אחד בנפרד. שימו לב: אחרי ההתקנה יש
**להפעיל TCP/IP** ידנית, אחרת Claude Code לא יצליח להתחבר (SSMS דווקא כן).

### מה צריך לעשות

התהליך המלא לשני המסלולים — כולל השוואה ביניהם, הרשמה ל-Azure for Students,
והגדרת ה-Firewall או הפעלת TCP/IP — מתואר שלב אחר שלב ב-[`PREREQS.md`](../PREREQS.md),
**חלק ג׳**. בצעו אותו לפני השיעור.

אם בחרתם ב-Azure, בסיום התהליך יהיו בידיכם ארבעה ערכים. שמרו אותם — תצטרכו אותם
בכל שאר המדריכים:

| ערך | דוגמה |
|---|---|
| **Server** | `<servername>.database.windows.net` |
| **Database** | שם בסיס הנתונים שבחרתם |
| **User** | שם משתמש ה-SQL admin שהגדרתם |
| **Password** | הסיסמה שהגדרתם |

> **אזהרה:** אלה פרטי גישה. שתפו אותם רק בצ'אט הפרטי של הקבוצה,
> ולעולם אל תעלו אותם ל-git.

## 4. התקנת SQL Server Management Studio (SSMS)

SSMS הוא הכלי לניהול בסיס הנתונים — יצירת טבלאות, הרצת שאילתות, ועוד.
הוא **כלי לקוח בלבד** — הוא לא מכיל שרת, ולכן מתקינים אותו בשני המסלולים.

**כל גרסה עדכנית מתאימה** (19, 20, 21). אם כבר מותקן אצלכם SSMS מקורס קודם —
אל תשדרגו, רק ודאו שהוא נפתח.

1. הורידו מ: <https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms>
2. הריצו את קובץ ההתקנה והמתינו לסיום
3. ייתכן שיידרש restart למחשב בסיום

## 5. אימות ההתקנה

לאחר התקנת כל הכלים, בדקו שהכל עובד:

### בדיקת הכלים — תנו ל-Claude לעשות את זה

הדרך הפשוטה ביותר היא לתת ל-Claude Code להריץ את הבדיקות ולתקן מה שנכשל.
הדביקו ב-Claude Code:

> Check whether `dotnet --version` reports 8.x, `dotnet --list-runtimes` includes `Microsoft.WindowsDesktop.App 8.x`, `git --version` works, and `uvx --version` works. Run them in a fresh shell. Fix anything that fails, then re-run the check to confirm. Report a pass/fail table.

**אם אתם בכל זאת רוצים לבדוק ידנית**, פתחו חלון PowerShell **חדש** והריצו את הפקודות.

> **איך פותחים חלון PowerShell חדש:** לחצו על מקש **Windows**, הקלידו `powershell`,
> ולחצו **Enter**. לחלופין: קליק ימני על תפריט Start ← **Terminal** או **Windows PowerShell**.
> חשוב שהחלון יהיה **חדש** — חלון שהיה פתוח לפני ההתקנות לא מכיר את הכלים שהותקנו זה עתה.

```powershell
dotnet --version          # 8.x.x
git --version             # git version 2.x.x
uvx --version             # uv 0.x.x
```

### בדיקת החיבור לבסיס הנתונים ב-SSMS

1. פתחו את SSMS (חיפוש `SSMS` בתפריט Start)
2. בשדה **Server name** הזינו:
   - **מסלול Azure:** `<servername>.database.windows.net`
   - **מסלול מקומי:** `localhost\SQLEXPRESS` (או שם ה-Instance שלכם)
3. הגדירו **Authentication**:
   - **מסלול Azure:** `SQL Server Authentication`, והזינו **Login** ו-**Password**
     של ה-SQL admin
   - **מסלול מקומי:** `Windows Authentication` (בלי שם משתמש וסיסמה)
4. תחת **Options** סמנו **Trust Server Certificate** (ובמסלול Azure גם **Encrypt connection**)
5. לחצו **Connect**
6. פתחו חלון שאילתה חדש והריצו:
   ```sql
   SELECT @@VERSION;
   ```
   במסלול Azure תופיע גרסה שכתוב בה **Microsoft SQL Azure**; במסלול המקומי תופיע
   גרסת SQL Server רגילה. בשני המקרים — ההתחברות הצליחה!

> **אם מתקבלת שגיאת timeout (מסלול Azure):** כלל ה-Firewall כנראה לא נשמר.
> חזרו ל-[`PREREQS.md`](../PREREQS.md) חלק ג2 שלב 6 וודאו שהכלל
> `0.0.0.0`–`255.255.255.255` קיים ונשמר.

> **אם ההתחברות נכשלת (מסלול מקומי):** ודאו ששירות ה-SQL Server רץ —
> הריצו `Get-Service MSSQL*` ב-PowerShell; הוא אמור להיות **Running**.

> **אם מתקבלת שגיאת "Login failed for user":** שם המשתמש הוא ה-SQL admin login
> שהוגדר ביצירת השרת — **לא** כתובת המייל של חשבון Azure.

## מה הלאה?

המשיכו למדריך הבא: [הקמת בסיס הנתונים](02-database-setup.md)

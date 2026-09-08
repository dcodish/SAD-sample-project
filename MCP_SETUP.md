# חיבור Claude Code לבסיס הנתונים (דרך MCP)

בסיום ההגדרה הזו, ה-session של Claude Code יוכל לקרוא ולכתוב ישירות לבסיס הנתונים שלכם — בלי להעתיק SQL הלוך ושוב בין Claude ל-SSMS. ההגדרה מתבצעת פעם אחת לכל מחשב, לכל תיקיית פרויקט.

**דרישת קדם:** בסיס הנתונים כבר הוקם — ב-Azure SQL (מומלץ) או כ-SQL Server Express מקומי. זה חלק ג׳ ב-[`PREREQS.md`](./PREREQS.md), שמתבצע **לפני** השיעור.

המסמך מכסה את שני המסלולים. ההבדל היחיד ביניהם הוא **קטע ה-`env` ב-`.mcp.json`** (שלב 2) — כל שאר השלבים זהים.

> המסמך הזה הוא מסמך העזר ופתרון התקלות. ההגדרה עצמה במהלך השיעור היא Prompt אחד שמדביקים ב-Claude Code — ראו שלב 1 ב-[`LESSON_STEPS.md`](./LESSON_STEPS.md). קראו את המסמך הזה כשמשהו משתבש, או כשאתם רוצים להבין מה ה-Prompt הזה בעצם עושה.

---

## מה מתקינים

את שרת ה-**MSSQL MCP** של Richard Han ([`microsoft_sql_server_mcp`](https://github.com/RichardHan/mssql_mcp_server)) — תוכנית Python קטנה שחושפת ל-Claude שני כלי MCP:

- `list_tables` — הצגת רשימת הטבלאות בבסיס הנתונים המחובר
- `execute_sql` — הרצת כל SQL (SELECT, INSERT, UPDATE, DELETE, CREATE TABLE, CREATE PROCEDURE וכו׳)

זהו מימוש קהילתי, לא של מיקרוסופט.

**למה לא השרת הרשמי של מיקרוסופט?** כי הוא לא יכול לעשות את מה שאנחנו צריכים.
השרת הנתמך של מיקרוסופט (SQL MCP Server, מבוסס Data API Builder) בנוי סביב **DML בלבד** —
קריאה, הוספה, עדכון ומחיקה של **נתונים** בטבלאות קיימות. בתיעוד הרשמי כתוב במפורש
שהוא "designed to work with data, not schema", כלומר **אינו תומך ב-DDL**:
אי אפשר להריץ דרכו `CREATE TABLE` ואי אפשר ליצור דרכו `CREATE PROCEDURE`.
(מיקרוסופט אף הסירה ביוני 2026 שרת MSSQL MCP ניסיוני קודם שלה מאותה סיבה.)

בקורס הזה שלב 4 כולו הוא DDL: יצירת כל הטבלאות מדיאגרמת המחלקות ויצירת ה-Stored
Procedures. לכן השרת הרשמי אינו מתאים כאן, והחבילה הקהילתית — שחושפת `execute_sql`
גולמי ומאפשרת להריץ כל SQL — היא הבחירה הנכונה לשימוש לימודי מול בסיס נתונים מתכלה.

> **הערה:** בדיוק החופש הזה (הרצת כל SQL, כולל `DROP`) הוא מה שהופך את החבילה הזו
> ללא מתאימה לסביבת ייצור. ראו את הערת האבטחה בסוף המסמך.

השרת מתקשר עם SQL Server דרך **pymssql**, שמדבר TCP בלבד.

- **ב-Azure SQL** אין מה להגדיר: הוא עובד ממילא רק ב-TCP על פורט 1433.
- **ב-SQL Express מקומי** זו נקודת הכשל הנפוצה ביותר: המופע מגיע עם **TCP/IP מכובה כברירת מחדל**. SSMS מתחבר דרך Shared Memory ולכן עובד, אבל ה-MCP נכשל. חובה להפעיל TCP/IP ולקבוע פורט קבוע — ראו `PREREQS` חלק ג2-מ.

---

## שלב 1 — התקנת `uv` (מריץ חבילות Python)

שרת ה-MCP רץ דרך `uvx`, שהוא חלק מ-[astral-sh/uv](https://github.com/astral-sh/uv):

```powershell
winget install astral-sh.uv
```

אמתו בטרמינל **חדש**:

```powershell
uvx --version
```

**מוקש ידוע:** אחרי ההתקנה, `uvx` **לא** יהיה ב-PATH של ה-session הנוכחי של Claude Code — Windows מרענן את ה-PATH רק עבור תהליכים חדשים. במקום להפעיל מחדש הכול, אנחנו כותבים את **הנתיב המלא** ל-`uvx.exe` בתוך `.mcp.json`. מצאו אותו כך:

```powershell
$uvx = (Get-Command uvx -EA SilentlyContinue).Source
if (-not $uvx) { $uvx = @("$env:USERPROFILE\.local\bin\uvx.exe","$env:LOCALAPPDATA\Microsoft\WinGet\Links\uvx.exe","$env:LOCALAPPDATA\Programs\uv\uvx.exe") | Where-Object { Test-Path $_ } | Select-Object -First 1 }
if (-not $uvx) { $uvx = Get-ChildItem "$env:USERPROFILE\.local","$env:LOCALAPPDATA\Microsoft\WinGet\Packages" -Filter uvx.exe -Recurse -EA SilentlyContinue | Select-Object -First 1 -ExpandProperty FullName }
$uvx
```

> **למה כל כך הרבה מקומות?** מיקום ההתקנה של `uv` השתנה בין גרסאות. בגרסאות עדכניות
> הוא יושב ב-`%USERPROFILE%\.local\bin`, ולא תחת `WinGet\Packages` — חיפוש רק שם
> יחזיר ריק, וזו טעות נפוצה. הפקודה למעלה בודקת את כל האפשרויות.

---

## שלב 2 — יצירת `.mcp.json`

‏Claude Code קורא את הגדרות שרתי ה-MCP מקובץ `.mcp.json` בשורש תיקיית הפרויקט:

```json
{
  "mcpServers": {
    "mssql": {
      "command": "<ABSOLUTE_PATH_TO_uvx.exe>",
      "args": ["--from", "microsoft_sql_server_mcp==0.1.0", "--with", "mcp==1.30.0", "mssql_mcp_server"],
      "env": {
        "MSSQL_SERVER": "<servername>.database.windows.net",
        "MSSQL_PORT": "1433",
        "MSSQL_DATABASE": "<your database name>",
        "MSSQL_USER": "<your SQL admin login>",
        "MSSQL_PASSWORD": "<your SQL admin password>",
        "MSSQL_ENCRYPT": "true"
      }
    }
  }
}
```

### מסלול מקומי — SQL Server Express

אם בחרתם במסלול המקומי, קטע ה-`env` נראה כך במקום:

```json
      "env": {
        "MSSQL_SERVER": "localhost",
        "MSSQL_PORT": "14330",
        "MSSQL_DATABASE": "master",
        "MSSQL_WINDOWS_AUTH": "true",
        "MSSQL_ENCRYPT": "false"
      }
```

שלושה הבדלים שחשוב להבין:

- **`MSSQL_SERVER` הוא `localhost` בלבד — בלי `\SQLEXPRESS`.** pymssql לא יודע לפענח שמות Instance בצורה אמינה בלי SQL Browser. במקום זה מציינים את השרת ואת **הפורט הקבוע** שהגדרתם (‏`14330` בדוגמה) ב-`MSSQL_PORT`.
- **`MSSQL_WINDOWS_AUTH` בערך `"true"`** (כמחרוזת, לא כערך בוליאני), ו**בלי** `MSSQL_USER` ו-`MSSQL_PASSWORD` כלל. אם תשאירו אותם, ההזדהות תיכשל.
- **`MSSQL_ENCRYPT` בערך `"false"`** — מופע מקומי לא מוגדר להצפנה.

**`MSSQL_DATABASE` הוא `master` בהתחלה** כי בסיס הנתונים של הפרויקט עדיין לא קיים במסלול הזה. אחרי שתיצרו אותו בשלב 4 של השיעור, עדכנו את הערך לשם החדש והפעילו מחדש את Claude Code. לחלופין אפשר להשאיר `master` ולהתחיל כל בלוק ב-`USE <שם בסיס הנתונים>;` — מגבלת ה-`USE` קיימת רק ב-Azure, לא מקומית.

> **יתרון של המסלול המקומי:** אין סיסמה בקובץ. עדיין הוסיפו את `.mcp.json` ל-`.gitignore` — הוא קובץ הגדרות מקומי שאין סיבה לשתף.

### פרטים שקל לטעות בהם (בשני המסלולים)

> **⚠️ הגרסאות ב-`args` הן חלק מההגדרה — אל תסירו אותן.**
>
> החבילה `microsoft_sql_server_mcp` פורסמה ביוני 2025 בגרסה `0.1.0` ולא עודכנה מאז.
> היא נכתבה מול ה-API של ספריית `mcp` בגרסה 1.x — אבל `mcp` עברה בינתיים לגרסה 2.x.
> בלי הצמדה, `uvx` מוריד את הגרסה החדשה והשרת **קורס מיד בעלייה**:
>
> ```
> AttributeError: 'Server' object has no attribute 'list_resources'
> ```
>
> לכן `--with "mcp==1.30.0"` הוא חובה.
>
> **ואל תצמידו את `pymssql`.** הצמדה מפורשת שלו מובילה לגרסה שאין לה חבילה מוכנה
> ל-Windows, והתוצאה היא `ModuleNotFoundError: pymssql._pymssql`. תנו לו להיפתר לבד.

ארבעה פרטים שקל לטעות בהם:

- **`command` הוא הנתיב המלא ל-`uvx.exe`**, ולא המחרוזת `uvx` לבדה — ראו את מוקש ה-PATH למעלה.
- **`args` חייב להיות בצורת `--from`.** שם החבילה ושם קובץ ההרצה שונים זה מזה; הערך `["microsoft_sql_server_mcp"]` לבדו **לא** יפעיל את השרת.
- **ב-Azure: `MSSQL_DATABASE` הוא בסיס הנתונים של הפרויקט, לא `master`.** ב-Azure SQL כל חיבור קשור לבסיס נתונים יחיד, והפקודה `USE <other_db>` אינה נתמכת — ולכן אף פעם לא צריך `USE` בתחילת בלוק. במסלול המקומי `USE` דווקא עובד, וכך גם התחלה מ-`master`.
- **`MSSQL_ENCRYPT`, ולא `TrustServerCertificate`.** החבילה הזו לא מזהה את השם השני. ב-Azure הערך חייב להיות `"true"` (הצפנה נדרשת); במסלול מקומי `"false"`.

שם המשתמש שלכם ב-Azure SQL הוא ה-**SQL admin login** שבחרתם בעת יצירת השרת — **לא** כתובת המייל של חשבון Azure. במסלול המקומי אין שם משתמש כלל: ההזדהות היא Windows Authentication.

---

## שלב 3 — תיקון שני באגים בחבילה השמורה במטמון

בחבילה שפורסמה יש שני פגמים ששוברים את זרימת העבודה הזו. שניהם תיקון של שורה אחת בעותק ש-`uv` שומר במטמון על המחשב שלכם.

תחילה, הריצו את השרת פעם אחת כדי להוריד את החבילה:

```powershell
& "<uvx path>" --from "microsoft_sql_server_mcp==0.1.0" --with "mcp==1.30.0" mssql_mcp_server --help
```

הפקודה תיכשל עם שגיאת קונפיגורציה — זה בסדר, ההתקנה הצליחה. עכשיו אתרו את `server.py` שבמטמון:

```powershell
Get-ChildItem -Path "$env:LOCALAPPDATA\uv\cache\archive-v0" -Filter "server.py" -Recurse | Where-Object { $_.FullName -like "*mssql_mcp_server*" } | Select-Object -ExpandProperty FullName
```

**באג A — שם פרמטר שגוי.** החבילה מעבירה ל-pymssql את `encrypt=`, אבל שם הפרמטר ב-pymssql הוא `encryption=`, והוא מקבל מחרוזת ולא ערך בוליאני. אתרו את השורה שמגדירה `config["encrypt"] = encrypt_str.lower() == "true"` והחליפו אותה ב:

```python
if encrypt_str.lower() == "true":
    config["encryption"] = "request"
```

**באג B — פקודות DDL נכשלות בתוך טרנזקציה משתמעת.** בקובץ יש **שלוש** שורות
`conn = pymssql.connect(**config)`. אחרי **כל אחת** מהן הוסיפו:

```python
conn.autocommit(True)
```

> **קריטי — ההזחה (indentation):** השורה החדשה חייבת להיות באותה הזחה בדיוק כמו שורת
> ה-`conn = ...` שמעליה (8 רווחים, בתוך בלוק `try:`). הזחה שגויה נותנת `IndentationError`
> והשרת לא יעלה בכלל. אחרי העריכה כדאי לוודא שהקובץ עדיין תקין:
>
> ```powershell
> python -m py_compile "<הנתיב ל-server.py>"
> ```
>
> בלי פלט = הקובץ תקין.

בלי זה, `CREATE TABLE`, `CREATE PROCEDURE` ופקודות DDL אחרות נכשלות עם השגיאה *"statement not allowed within multi-statement transaction"*.

התיקונים האלה נמצאים במטמון של `uv`. אם תנקו את המטמון, או תעברו למחשב אחר — תצטרכו להחיל אותם מחדש.

---

## שלב 4 — הפעלה מחדש של Claude Code ואימות

1. סגרו את Claude Code לחלוטין. פתחו אותו מחדש על תיקיית הפרויקט.

   > **שימו לב:** ההפעלה מחדש סוגרת את השיחה הקודמת. Claude לא ימשיך מעצמו —
   > אתם פותחים שיחה חדשה ומדביקים בה את מה שלמטה.

2. בשיחה **החדשה**, שאלו:

> What MCP tools do you have available?

   ‏Claude אמור להציג את `mssql.list_tables` ואת `mssql.execute_sql`.

3. ואז בקשו:

> Use the mssql tool to run `SELECT @@VERSION` and `SELECT DB_NAME()`.

התוצאה הצפויה: במסלול Azure — טקסט גרסה שמזהה **Microsoft SQL Azure**; במסלול מקומי — גרסת SQL Server רגילה. בשני המקרים `DB_NAME()` מחזיר את בסיס הנתונים המחובר, ובלי שגיאות.

---

## תקלות נפוצות

| תסמין | פתרון |
|---|---|
| `AttributeError: 'Server' object has no attribute 'list_resources'` | **הסיבה הנפוצה ביותר.** ה-`args` שלכם חסרים את `--with "mcp==1.30.0"`, ולכן הותקנה ספריית `mcp` מגרסה 2.x שאינה תואמת. הוסיפו את ההצמדה ל-`.mcp.json` והפעילו מחדש. |
| `ModuleNotFoundError: No module named 'pymssql._pymssql'` | הצמדתם את `pymssql` לגרסה מסוימת. הסירו את ההצמדה שלו לגמרי — רק `mcp` מוצמד. |
| `uvx: command not found` | פתחו טרמינל חדש אחרי התקנת `uv`. ב-`.mcp.json`, השתמשו בנתיב המלא ל-`uvx.exe` ולא בשם לבדו. |
| ‏Claude אומר "no MCP server named mssql" | הפעילו מחדש את Claude Code אחרי עריכת `.mcp.json`. ודאו שהקובץ נמצא ב**שורש** התיקייה ש-Claude Code פתח — לא בתת-תיקייה, ולא בתוך `cloned/`. |
| השרת עולה אבל כל קריאה נכשלת מיד | כמעט תמיד זו צורת ה-`args`. היא חייבת להיות `["--from", "microsoft_sql_server_mcp==0.1.0", "--with", "mcp==1.30.0", "mssql_mcp_server"]` — כולל ההצמדות. |
| הקריאה הראשונה ל-`execute_sql` נתקעת | בהרצה הראשונה `uvx` מוריד את החבילה — תנו לזה 30 שניות. הקריאות הבאות מהירות. |
| "Login failed for user" | שם משתמש או סיסמה שגויים. שם המשתמש הוא ה-**SQL admin login** מהפורטל של Azure, לא כתובת המייל של חשבון Azure. בדקו שוב את שניהם מול מה שיוצר בסיס הנתונים שיתף. |
| Timeout בחיבור, או "Adaptive Server is unavailable" | ה-Firewall של Azure חוסם אתכם. פורטל Azure ← ה**שרת** שלכם (לא בסיס הנתונים) ← Networking ← ודאו שכלל ה-Firewall‏ `0.0.0.0`–`255.255.255.255` קיים **ונשמר**. |
| "Cannot open database ... requested by login" | הערך ב-`MSSQL_DATABASE` לא תואם לשם בסיס הנתונים בפורטל. בדקו איות ואותיות גדולות/קטנות. |
| "statement not allowed within multi-statement transaction" | תיקון באג B חסר או אבד. החילו מחדש את `conn.autocommit(True)` — ראו שלב 3. |
| שגיאת SSL או הצפנה | השתמשו ב-`MSSQL_ENCRYPT: "true"`. הערך `TrustServerCertificate` אינו מפתח שהחבילה הזו מכירה. |
| `Incorrect syntax near 'GO'` | ‏`GO` הוא מפריד בלוקים של SSMS, לא פקודת T-SQL — ה-MCP לא יכול לשלוח אותו. בקשו מ-Claude לפצל את הסקריפט לפי `GO` ולשלוח כל בלוק בקריאת `execute_sql` נפרדת. |
| `USE <db>` נכשל | ‏Azure SQL לא תומך ב-`USE`. אתם לא צריכים אותו: החיבור כבר קשור לבסיס הנתונים של הפרויקט. (במסלול מקומי `USE` כן עובד.) |
| **מקומי:** "Adaptive Server is unavailable" אבל SSMS מתחבר | ‏TCP/IP מכובה במופע, או פורט שגוי | ה-MCP מדבר TCP בלבד ו-SSMS משתמש ב-Shared Memory. הפעילו TCP/IP וקבעו פורט קבוע — `PREREQS` חלק ג2-מ — וודאו שאותו פורט מופיע ב-`MSSQL_PORT`. |
| **מקומי:** "Login failed" למרות Windows Authentication | ‏`MSSQL_USER`/`MSSQL_PASSWORD` נשארו בקובץ | הסירו אותם לגמרי והשאירו רק `MSSQL_WINDOWS_AUTH: "true"` (כמחרוזת). |
| **מקומי:** החיבור נכשל עם `localhost\SQLEXPRESS` ב-`MSSQL_SERVER` | pymssql לא מפענח שמות Instance | כתבו `localhost` בלבד ב-`MSSQL_SERVER`, ואת הפורט ב-`MSSQL_PORT`. |

---

## אם השרת הזה לא עולה — מה האפשרויות

**קודם כול: אל תחפשו שרת MCP אחר באינטרנט.** רוב שרתי ה-MSSQL MCP שתמצאו אינם תומכים
ב-DDL (יצירת טבלאות ו-Stored Procedures), וזה בדיוק מה שאנחנו צריכים בשלב 4.
החלפת השרת בדרך כלל מחליפה בעיה אחת בבעיה גדולה יותר.

עברו על האפשרויות לפי הסדר:

| # | אפשרות | מתי להשתמש | תומך ב-DDL? |
|---|---|---|---|
| 1 | **`microsoft_sql_server_mcp==0.1.0` עם `mcp==1.30.0`** — מה שמתואר במסמך הזה | ברירת המחדל. נסו קודם, כולל שני התיקונים בשלב 3 | ✅ כן |
| 2 | **תוסף MSSQL של VS Code** | אם ה-MCP לא עולה בשום אופן. התוסף מאפשר להריץ SQL מתוך VS Code, כולל DDL — אבל **Claude לא מריץ אותו בשבילכם**, אתם מריצים ידנית | ✅ כן |
| 3 | **בלי MCP בכלל — דרך SSMS** | מוצא אחרון, אבל **תמיד עובד** | ✅ כן |

### אפשרות 3 בפירוט — איך להמשיך בלי MCP

אתם לא תקועים. ה-MCP הוא נוחות, לא תנאי הכרחי: הוא רק חוסך לכם להעתיק SQL בין
Claude ל-SSMS. כל שאר השיעור — הניתוח, ה-`CLAUDE.md`, יצירת הסכמה, ה-Stored Procedures
וקוד ה-C# — עובד בדיוק אותו דבר.

מה שמשתנה: במקום ש-Claude יריץ את ה-SQL, **אתם מריצים אותו**. בכל מקום שבו כתוב
"הרץ את הסקריפט דרך ה-MCP", עשו במקום זה:

1. בקשו מ-Claude לכתוב את הסקריפט לקובץ (הוא עושה את זה ממילא — `scripts/create_database.sql` וכו׳).
2. פתחו את הקובץ ב-SSMS (File ← Open ← File), ודאו שנבחר בסיס הנתונים הנכון, ולחצו **Execute**.
3. אם יש שגיאה — העתיקו את הודעת השגיאה המלאה חזרה ל-Claude ובקשו ממנו לתקן את הקובץ.

> אמרו ל-Claude שאתם עובדים כך, אחרת הוא ינסה לקרוא לכלי שאינו קיים:
>
> > We don't have a working MSSQL MCP server. Do not try to call any mssql tool. Instead, whenever SQL needs to run, write it to a file under `scripts/` and tell me to execute it in SSMS myself. I'll paste back any errors.

**הפסד היחיד:** Claude לא יכול לאמת בעצמו שהטבלאות נוצרו, ולכן שלבי הבדיקה
(למשל 4.6) עוברים אליכם. שווה להשקיע 10 דקות בניסיון להפעיל את ה-MCP לפני שמוותרים.

---

## הערת אבטחה

הכלי `execute_sql` מקבל כל SQL, כולל `DROP TABLE`. ובניגוד למופע מקומי, בסיס הנתונים הזה נגיש מהאינטרנט. לכן:

- **הוסיפו את `.mcp.json` ל-`.gitignore` לפני שאתם מעלים משהו.** במסלול Azure הוא מכיל את סיסמת בסיס הנתונים של הקבוצה כטקסט גלוי, וסיסמה שהועלתה ל-GitHub היא סיסמה שחייבים להחליף. במסלול המקומי אין בו סיסמה, אבל הוא עדיין קובץ הגדרות מקומי שאין סיבה לשתף.
- **שתפו את פרטי הגישה בערוצים פרטיים בלבד** — הצ׳אט הפרטי של הקבוצה, או מנהל סיסמאות. לעולם לא בפורום הקורס, ולעולם לא ב-commit.
- **כלל ה-Firewall‏ `0.0.0.0`–`255.255.255.255` הוא פשרה שמתאימה להקשר לימודי.** בקרת הגישה האמיתית היא ה-SQL login: בלי שם המשתמש והסיסמה, גם Firewall פתוח דוחה כל חיבור. הכלל חוסך מכם להוסיף כתובת IP מחדש בכל פעם שכתובת של חבר צוות משתנה. אל תעבירו את הדפוס הזה לעבודה בסביבת ייצור.
- **זהו בסיס נתונים לימודי מתכלה.** אל תכניסו אליו שום דבר שלא תרצו לאבד, ואל תכוונו את ה-MCP הזה לבסיס נתונים שחשוב לכם.

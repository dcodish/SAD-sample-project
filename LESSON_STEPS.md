---
title: "שלבי השיעור"
subtitle: "מניתוח ל-CRUD עובד — המדריך המלא"
course: "ניתוח ועיצוב מערכות מידע — אוניברסיטת בן-גוריון, הנדסת תעשייה וניהול"
author: "מרצה: דוד קודיש"
lang: he
dir: rtl
---

המדריך המלא של השיעור: כל שלב, כל Prompt, וההסבר למה כל דבר נעשה כפי שהוא נעשה.

---

## מה אנחנו עושים היום

**המטרה:** לקחת את הניתוח והעיצוב שהקבוצה שלכם כבר עשתה — ולהפוך אותם למערכת שרצה.
בסוף השיעור יהיו לכם בסיס נתונים עם הטבלאות שלכם, אפליקציית WinForms שנפתחת,
מסך התחברות אמיתי, ומסכי CRUD עובדים.

### הרעיון שמאחורי השיעור

השיעור הזה הוא לא "איך לתכנת מהר יותר". הוא על **חלוקת העבודה הנכונה בין אדם לסוכן AI**:

| מה שרק אתם יכולים לעשות | מה שהסוכן עושה טוב יותר מכם |
|---|---|
| להחליט מה המערכת צריכה לעשות | לתרגם החלטה שכבר התקבלה לקוד |
| לזהות שהניתוח שלכם סותר את עצמו | לכתוב 40 Stored Procedures בלי טעות הקלדה |
| לדעת מה נכון לתחום העסקי שלכם | לייצר 10 מסכים באותה תבנית |
| **לבדוק שמה שנוצר תואם למה שהתכוונתם** | להריץ, לתקן שגיאות, ולנסות שוב |

הטור השמאלי הוא מה שהופך את השיעור למהיר. **הטור הימני הוא מה שהופך אותו לנכון.**
אם תדלגו על הבדיקות, תקבלו מערכת שמתקמפלת ורצה — ועושה את הדבר הלא נכון, בשקט.

### הקשת של השיעור — ארבעה מעברים

1. **מ-PDF ל-Markdown** (שלב 2) — הניתוח שלכם עובר לפורמט ש-Claude קורא היטב.
2. **מ-Markdown להקשר** (שלב 3) — נבנה `CLAUDE.md`: מסמך אחד שמתאר את הפרויקט שלכם,
   ו-Claude קורא אותו אוטומטית בכל שיחה. **זה הקובץ החשוב ביותר בפרויקט.**
3. **מהקשר לבסיס נתונים** (שלבים 4–4.5) — דיאגרמת המחלקות הופכת לטבלאות אמיתיות עם נתונים.
4. **מבסיס נתונים לאפליקציה** (שלבים 5–6) — הטבלאות הופכות למחלקות C# ולמסכים שרצים.

כל מעבר מסתמך על הקודם. שגיאה בשלב 3 מתגלגלת לשלבים 4, 5 ו-6 — ולכן שלב הבדיקה
בכל שלב הוא לא המלצה, הוא חלק מהעבודה.

### שלושה עקרונות שחוזרים לאורך כל השיעור

- **נשארים ב-Claude Code.** לא קופצים בין SSMS, טרמינל ודפדפן. גם ה-SQL רץ מתוך Claude.
- **הקבצים הם מקור האמת.** כל שינוי סכמה נכנס לקובץ `.sql` ששמור ב-git.
  בסיס הנתונים הוא תוצר נגזר — אם הוא נמחק, מריצים מחדש את הסקריפטים.
- **בודקים את מה ש-Claude מייצר.** הוא מהיר אבל לא זהיר.

### איך לקרוא את המסמך הזה

- **הקטעים בכחול הם Prompts להעתקה** — הדביקו אותם ב-Claude Code כמו שהם (יש כפתור "העתק").
  הם באנגלית בכוונה: כך הם נוסחו ונבדקו.
- **הטקסט סביבם בעברית מסביר מה קורה ולמה.** אל תדלגו עליו —
  ההסבר הוא מה שמאפשר לכם לזהות מתי Claude טעה.
- כל שלב מסתיים ב**נקודת Commit** — שמרו את העבודה לפני שאתם ממשיכים.

---

## פתיחת Session חדש באמצע השיעור

לאורך השיעור תפתחו כמה שיחות חדשות — כשחלון ההקשר מתמלא, או אחרי הפסקה.
**שיחה חדשה מתחילה בלי זיכרון של מה שדיברתם עליו** (הקבצים כמובן נשארים).

**משלב 3 והלאה** זה כמעט לא מורגש: Claude קורא אוטומטית את `CLAUDE.md` ומתמצא מיד.

**לפני שלב 3**, או כשאתם רוצים למקד את Claude מהר, הדביקו את ה-Prompt הקצר הבא
בתחילת השיחה החדשה:

> I'm continuing a university course project — building a C# WinForms order-management-style system from our own analysis documents, driven entirely from Claude Code.
>
> Orient yourself before doing anything: read `CLAUDE.md` at the project root if it exists, list what's in `docs/`, and check whether `scripts/` and a `.sln` already exist. The folder `cloned/` is the course's sample project — read `cloned/PATTERNS.md` for the architecture conventions we follow, but do not copy its domain content.
>
> Then tell me in a few lines where the project currently stands and what you think the next step is, and wait for me to confirm before changing anything.

זה חוסך לכם הסברים חוזרים, ומונע מ-Claude לנחש.

---

## דרישות קדם — התקנה לפני השיעור

**כל התקנות התוכנה מרוכזות ב-[`PREREQS`](./PREREQS.md).** בצעו אותן לפני השיעור — לא במהלכו.

בקצרה, מה שצריך להיות מותקן: Visual Studio Community (2022 או 2025 — שתיהן עובדות),‏ .NET 8 SDK יחד עם Windows Desktop Runtime,‏ SSMS (כל גרסה עדכנית),‏ Git,‏ VSCode, תוסף Claude Code (מחובר עם מנוי Pro/Max פעיל), ‏`uv`, ובסיס נתונים שהוקם עבור הקבוצה — ב-Azure SQL (מומלץ) או מקומי. שלבי ההתקנה המלאים, האימות ופתרון התקלות נמצאים ב-`PREREQS`.

השיעור מניח שכל מה שב-`PREREQS` עובד על המחשב שלכם. אם לא — תקנו את זה קודם.

### ניהול טוקנים — קראו את זה לפני שאתם מתחילים

רוב הסטודנטים עובדים עם מנוי **Pro** ב-20$. הוא מספיק לשיעור בהחלט — אבל רק אם
לא מבזבזים. שתי ההחלטות שמשפיעות הכי הרבה הן **באיזה מודל אתם משתמשים** ו**כמה
ארוכה השיחה**.

#### באיזה מודל להשתמש

| מודל | מתי | איך |
|---|---|---|
| **Sonnet** | **ברירת המחדל שלכם לכל השיעור.** מתאים לרוב עבודת הקוד ועולה משמעותית פחות מ-Opus | `/model sonnet` |
| **Haiku** | לשלבים מכניים: הרצת SQL, יצירת קוד תבניתי, הרצת סקריפטים, יצירת מסכים חוזרים (שלב 6) | `/model haiku` |
| **Opus** | **הימנעו במנוי Pro.** הוא שורף את המכסה הכי מהר, ואין בו צורך למה שאנחנו עושים | — |

**בקצרה: התחילו ב-Sonnet, רדו ל-Haiku בשלבים המכניים, אל תשתמשו ב-Opus.**
השלבים שדורשים שיקול דעת אמיתי (2, 3, ומסך הכניסה ב-5.5) — השאירו ב-Sonnet.

#### מה שורף מכסה יותר מהמודל: שיחה ארוכה

בכל הודעה ש-Claude שולח, **הוא שולח את כל השיחה מחדש**. שאלה קצרה בשיחה שפתוחה
כבר שלוש שעות עולה כמו כל ההיסטוריה הזו. זה הגורם מספר אחת לבזבוז.

- **הריצו `/clear` בין שלבים.** זה **חינם לגמרי** ומאפס את ההקשר. Claude יקרא מחדש
  את `CLAUDE.md` וימשיך לעבוד — הקבצים שלכם לא נוגעים.
- **אל תשאירו שיחה אחת פתוחה כל השיעור.** שלב חדש = `/clear`.
- `/compact` **אינו** חינם — הוא עצמו בקשה גדולה. אם אתם רק עוברים לנושא אחר,
  `/clear` עדיף.
- **`/usage`** מראה כמה נשאר לכם מהמכסה ומה בזבז אותה.

#### שתי הודעות שונות שקל לבלבל ביניהן

- **"Context low" / התראת auto-compact** — **זו לא מגבלת מנוי.** ההקשר של השיחה
  התמלא. הפתרון: `/clear` והמשך עבודה.
- **"You've hit your session limit" / "weekly limit"** — זו כן מגבלת המנוי, והיא
  **משותפת לכל המודלים**. מעבר ל-Haiku *לא* יחזיר לכם גישה; צריך לחכות לאיפוס
  (ההודעה אומרת מתי).
- **"You've hit your Opus limit"** — מגבלה ספציפית למודל אחד. **כאן** מעבר ל-Sonnet
  או Haiku *כן* מחזיר אתכם לעבודה מיד.

> **⚠️ אם הקבוצה חולקת חשבון Claude אחד — שימו לב.** המכסה היא של החשבון, לא של
> המחשב. ארבעה חברי קבוצה שעובדים במקביל על אותו חשבון **לא** מקבלים ארבע מכסות —
> הם מרוקנים מכסה אחת בערך פי ארבע מהר.
>
> לכן, בעבודה על חשבון משותף:
>
> - **עדיף שאדם אחד יריץ את Claude בכל רגע נתון**, ושהשאר יסתכלו, יבדקו את התוצרים
>   ויחליטו יחד. ממילא רוב שלבי השיעור הם החלטה משותפת של הקבוצה.
> - אם כן עובדים במקביל — **הקפידו במיוחד על `/clear`**, כי כל שיחה פתוחה שולחת את
>   ההיסטוריה שלה מחדש בכל הודעה.
> - **הריצו `/usage` מדי פעם** כדי לראות כמה נשאר לחשבון.

#### עוד על Sessions

**Session לא שומר את התקדמות השיחה.** כשסוגרים Session, Claude שוכח את השיחה — אבל
לא את הקבצים שלכם. ה-`CLAUDE.md` הוא מה ש-Claude קורא בתחילת כל Session חדש כדי
להבין את הפרויקט. בצעו commit בסוף כל שלב, כדי שה-Session הבא יתחיל נקי.

---

## שלב 0 — יצירת תיקיית הפרויקט

**מה השלב הזה עושה:** מכין את מקום העבודה. Claude Code קורא וכותב קבצים בתיקייה
**שפתחתם ב-VSCode** — לא בתיקייה שבה הקבצים "אמורים" להיות. אם תפתחו את התיקייה
הלא נכונה, כל שאר השיעור יעבוד על הקבצים הלא נכונים.

**בסיום השלב יהיה לכם:** תיקייה ריקה עם תת-תיקיית `docs`, פתוחה ב-VSCode.

**זמן צפוי:** 2 דקות.

**בצעו את זה לפני שאתם פותחים את VSCode.** דילוג על השלב הזה הוא טעות ההקמה הנפוצה ביותר.

1. **צרו תיקייה ריקה** במחשב — למשל `C:\projects\sad-groupname`. היא חייבת להיות
   **ריקה לחלוטין**. אל תשתמשו בתיקייה שכבר יש בה קבצים, ואל תשתמשו בתיקיית פרויקט
   הדוגמה המשוכפל.

> **איפה למקם את התיקייה — חשוב:**
>
> - **מומלץ:** נתיב קצר ופשוט ישירות על כונן `C:`, למשל `C:\projects\sad-groupname`.
>   אם התיקייה `C:\projects` לא קיימת — צרו אותה.
> - **הימנעו משולחן העבודה ומתיקיית Documents.** במחשבים רבים הן מסונכרנות ל-OneDrive,
>   וזה גורם לקבצים להינעל באמצע בנייה, ל-Visual Studio להתנהג מוזר, ולנתיבים להיות ארוכים מדי.
> - **הימנעו מאותיות עבריות בנתיב.** נתיב כמו `C:\משתמשים\פרויקטים\sad` שובר חלק מכלי
>   הפיתוח (MSBuild,‏ .NET, וכלי שורת פקודה). זה נכון גם לשם המשתמש שלכם ב-Windows —
>   אם הוא בעברית, זו סיבה נוספת לא לעבוד מתוך תיקיית הבית.
> - **הימנעו מרווחים בשם התיקייה.** השתמשו במקף במקום: `sad-groupname` ולא `sad groupname`.
>
> בקיצור: `C:\projects\sad-groupname` — באנגלית, בלי רווחים, בלי OneDrive.
2. **צרו בתוכה תת-תיקייה ריקה בשם `docs`**. השאירו אותה ריקה בינתיים.
3. **פתחו VSCode ← File ← Open Folder** ← נווטו לתיקיית `sad-groupname` שלכם ופתחו אותה.

‏"workspace" ב-VSCode וב-Claude Code פירושו התיקייה שפתחתם. כשהמדריך הזה אומר "תיקיית הפרויקט שלכם" או "פתחו את Claude Code בתיקיית הפרויקט", הכוונה לתיקייה הריקה `sad-groupname`. Claude קורא קבצים מהתיקייה הזו — אם פתחתם את התיקייה הלא נכונה, הוא יקרא את הקבצים הלא נכונים.

---

## שלב 1 — הקמת הפרויקט

**מה השלב הזה עושה:** מחבר את שלושת מקורות המידע ש-Claude יעבוד מולם לאורך השיעור:
המסמכים שלכם (`docs/`), פרויקט הדוגמה של הקורס (`cloned/`), ובסיס הנתונים (דרך ה-MCP).
עד עכשיו Claude ידע לכתוב קוד; מכאן והלאה הוא גם יכול **להריץ SQL בעצמו**.

**בסיום השלב יהיה לכם:** תיקיית פרויקט עם המסמכים והדוגמה, `.gitignore`, ו-Claude
שמצליח להריץ `SELECT @@VERSION` מול בסיס הנתונים שלכם.

**זמן צפוי:** 15–20 דקות. הגדרת ה-MCP היא החלק שהכי נוטה להשתבש — אל תיבהלו.

1. **ודאו שאתם בתיקייה שיצרתם בשלב 0** — היא פתוחה ב-VSCode והיא עדיין ריקה למעט תת-התיקייה `docs`. זו תיקיית העבודה שלכם — היא אינה פרויקט הדוגמה.
2. **הכניסו את קובצי הניתוח והעיצוב הקיימים שלכם ל-`docs/`** — קובצי ה-PDF של חלק א׳ וחלק ב׳ (ניתוח ארגוני, דרישות, דיאגרמת UC, דיאגרמת מחלקות וכו׳).
3. **שכפלו את פרויקט הדוגמה של הקורס לתוך `cloned/`** על ידי בקשה מ-Claude Code:

   > Run this git command to clone the SAD sample project into a subfolder of this project: `git clone https://github.com/dcodish/SAD-sample-project.git cloned`

   (אל תריצו את פקודת git ישירות בטרמינל — ייתכן ש-git לא נמצא ב-PATH של הטרמינל, ותקבלו שגיאת "command not found". בקשה מ-Claude פותרת את זה אוטומטית.)

   קוד ה-C# לעיון נמצא אז ב-`cloned/example_project/`, והמוסכמות המשותפות ב-`cloned/PATTERNS.md`.

4. **הוסיפו `.gitignore`** — בקשו מ-Claude:

   > Create a `.gitignore` file at the root of this project with two entries: `cloned/` and `.mcp.json`

   ‏`cloned/` מונע מפרויקט הדוגמה להיכנס ל-repository של הקבוצה. `.mcp.json` מכיל פרטי גישה לבסיס הנתונים — לעולם אל תעלו אותו.

5. **התקינו את שרת ה-MSSQL MCP** כדי ש-Claude Code יוכל לתקשר עם בסיס הנתונים של הקבוצה ב-Azure SQL.

   דרישת קדם: בסיס הנתונים של הקבוצה חייב כבר להתקיים (הוקם ב-`PREREQS` חלק ג׳ לפני השיעור).

   **☁️ מסלול Azure:** ה-Prompt שלמטה מתאים לכם כמו שהוא. דרושים ארבעת ערכי החיבור:
   שרת, שם בסיס הנתונים, שם משתמש וסיסמה.

   **💻 מסלול מקומי:** השתמשו ב-Prompt שלמטה, עם שני שינויים בלבד —
   בסעיף 2 בקשו מ-Claude **לאתר את ה-Instance בעצמו** (לא לשאול אתכם לשם),
   ובסעיף 3 החליפו את קטע ה-`env` בזה:

   ```json
   "env": {
     "MSSQL_SERVER": "localhost",
     "MSSQL_PORT": "14330",
     "MSSQL_DATABASE": "master",
     "MSSQL_WINDOWS_AUTH": "true",
     "MSSQL_ENCRYPT": "false"
   }
   ```

   שימו לב: `MSSQL_SERVER` הוא `localhost` בלבד (בלי `\SQLEXPRESS` — pymssql לא מפענח
   שמות Instance), הפורט נפרד, ואין `MSSQL_USER`/`MSSQL_PASSWORD` כי משתמשים באימות Windows.
   שאר השלבים, כולל תיקון שני הבאגים, זהים לחלוטין.

   **דף ה-Prompts (`PROMPTS_CHEATSHEET`) מכיל את שני ה-Prompts במלואם**, מוכנים להעתקה,
   אם אתם מעדיפים לא לערוך ידנית.

   פתחו את Claude Code בתיקיית הפרויקט והדביקו את ה-Prompt הבא:

   > Set up the MSSQL MCP server for this project so you can talk to our Azure SQL database. Follow every step.
   >
   > 1. **Install `uv` if missing.** Run `uvx --version`. If not found, run `winget install astral-sh.uv`. Note: after install, `uvx` will NOT be on this session's PATH (Windows only refreshes PATH for new processes). In step 3 you will write the **absolute path** to `uvx.exe` into `.mcp.json`. Find it with: run this — it checks every place uv installs to, because the location varies by uv version:
>    ```powershell
>    $uvx = (Get-Command uvx -EA SilentlyContinue).Source
>    if (-not $uvx) { $uvx = @("$env:USERPROFILE\.local\bin\uvx.exe","$env:LOCALAPPDATA\Microsoft\WinGet\Links\uvx.exe","$env:LOCALAPPDATA\Programs\uv\uvx.exe") | Where-Object { Test-Path $_ } | Select-Object -First 1 }
>    if (-not $uvx) { $uvx = Get-ChildItem "$env:USERPROFILE\.local","$env:LOCALAPPDATA\Microsoft\WinGet\Packages" -Filter uvx.exe -Recurse -EA SilentlyContinue | Select-Object -First 1 -ExpandProperty FullName }
>    $uvx
>    ```
>    Do not search only under `WinGet\Packages` — recent uv versions install to `%USERPROFILE%\.local\bin` and that search finds nothing.
   >
> **Use exactly this MCP server — do not substitute a different one.** The package is `microsoft_sql_server_mcp` version `0.1.0`, and it MUST run with `mcp==1.30.0`. Without that pin, uv resolves mcp 2.x and the server dies on startup with `AttributeError: 'Server' object has no attribute 'list_resources'`. Do NOT pin `pymssql` — let it resolve on its own, or you get `ModuleNotFoundError: pymssql._pymssql`. If this server still fails after you have followed every step, STOP and tell me — do not go looking for an alternative MCP server on your own.
>
   > 2. **Ask me for the Azure SQL connection details** — server name (ends in .database.windows.net), database name, SQL username, and SQL password. Don't guess.
   >
   > 3. **Create `.mcp.json` at the project root** with this shape (fill in the absolute uvx path and the connection details you just asked me for):
   >    ```json
   >    {
   >      "mcpServers": {
   >        "mssql": {
   >          "command": "<ABSOLUTE_PATH_TO_uvx.exe>",
   >          "args": ["--from", "microsoft_sql_server_mcp==0.1.0", "--with", "mcp==1.30.0", "mssql_mcp_server"],
   >          "env": {
   >            "MSSQL_SERVER": "<server>.database.windows.net",
   >            "MSSQL_PORT": "1433",
   >            "MSSQL_DATABASE": "<database>",
   >            "MSSQL_USER": "<username>",
   >            "MSSQL_PASSWORD": "<password>",
   >            "MSSQL_ENCRYPT": "true"
   >          }
   >        }
   >      }
   >    }
   >    ```
   >
   > 4. **Patch two known bugs in the cached package.** First, run the server once to trigger the install: `& "<uvx path>" --from "microsoft_sql_server_mcp==0.1.0" --with "mcp==1.30.0" mssql_mcp_server --help` (it will fail with a config error — that's fine, the install succeeded). Then locate the cached `server.py`:
   >    ```powershell
   >    Get-ChildItem -Path "$env:LOCALAPPDATA\uv\cache\archive-v0" -Filter "server.py" -Recurse | Where-Object { $_.FullName -like "*mssql_mcp_server*" } | Select-Object -ExpandProperty FullName
   >    ```
   >    Apply two edits:
   >    - **Bug A — wrong kwarg name.** Find the block that sets `config["encrypt"] = encrypt_str.lower() == "true"` and replace it with:
   >      ```python
   >      if encrypt_str.lower() == "true":
   >          config["encryption"] = "request"
   >      ```
   >    - **Bug B — DDL fails inside an implicit transaction.** The file has **three** `conn = pymssql.connect(**config)` lines. After **each** of them insert `conn.autocommit(True)` **at exactly the same indentation as the `conn = ...` line it follows** (they sit at 8 spaces, inside a `try:`). Getting the indentation wrong gives an `IndentationError` and the server will not start at all. Afterwards, verify the file still parses.
   >
   > 5. **Add `.mcp.json` to `.gitignore`** — it contains your database credentials.
   >
   > 6. **Tell me to restart Claude Code**, then stop and wait.
   >
   > 7. After the restart I will type **continue** in this same chat. When I do: list your available MCP tools, confirm `mssql.execute_sql` is present, run `SELECT @@VERSION` and `SELECT DB_NAME()`, and show me the results. If the tools are missing, check that `.mcp.json` is at the project root and that its JSON is valid.

   **עכשיו מפעילים מחדש את Claude Code** (סגירה ופתיחה של VSCode).

   ‏VSCode פותח מחדש את השיחה האחרונה, כך שכל ההקשר נשמר. פשוט כתבו בה:

   > continue

   ‏Claude ימשיך מהנקודה שבה עצר ויאמת את החיבור.

   > **אם השיחה לא נפתחה מחדש** (למשל פתחתם תיקייה אחרת) — הדביקו במקום זאת:
   > *"I just set up the MSSQL MCP server in `.mcp.json` and restarted. List your MCP tools, confirm `mssql.execute_sql` is present, then run `SELECT @@VERSION` and `SELECT DB_NAME()`."*

   אם משהו נכשל בדרך, טבלת פתרון התקלות ב-`MCP_SETUP` מכסה את הבעיות הנפוצות.

**נקודת Commit:** ברגע ששלב 1 הושלם, בקשו מ-Claude:

> Initialize a git repository here and make the first commit with `.gitignore` and any files created so far.


אחר כך בצעו push ל-repository של הקבוצה ב-GitHub, אם כבר הוקם. ראו את [`GIT_GROUP_WORKFLOW`](./GIT_GROUP_WORKFLOW.md) להסבר על תיאום בין חברי קבוצה עם git.

**התחילו Session חדש לפני השלב הבא.** שלב 1 מייצר הרבה פלט של התקנות ותיקוני באגים — הקשר שלא תצטרכו שוב.

הריצו `/clear` (חינם) — או פתחו שיחה חדשה.

> **שימו לב:** `CLAUDE.md` עדיין לא קיים בשלב הזה, ולכן השיחה החדשה מתחילה בלי הקשר.
> הדביקו בה את ה-Prompt מהסעיף **"פתיחת Session חדש באמצע השיעור"** בתחילת המסמך.

בסיום שלב 1 התיקייה שלכם נראית כך:
```
sad-<group>/
├── .gitignore
├── .mcp.json                  ← gitignored, has DB connection
├── docs/
│   ├── PartA.pdf
│   └── PartB.pdf
└── cloned/                    ← cloned sample, git-ignored
    ├── PATTERNS.md            ← shared architecture conventions (you'll inherit these)
    ├── CLAUDE.md              ← sample-specific notes (read for reference)
    └── example_project/       ← the actual C# reference code
```

שני הקבצים המרכזיים בתוך פרויקט הדוגמה המשוכפל:
- **`PATTERNS.md`** — תבנית הישות, ניווט בין מסכים, מוסכמות שפה, "התחברות אינה Use Case" ועוד. כל פרויקט סטודנטים יורש את הקובץ הזה כלשונו.
- **`CLAUDE.md`** — מהו פרויקט ניהול ההזמנות לדוגמה, מפת המסמכים שלו והישויות שלו. קראו לצורך התמצאות; **אל תעתיקו** אותו — ה-`CLAUDE.md` שלכם מתאר את *התחום שלכם*.

---

## שלב 2 — חילוץ הניתוח שלכם ל-Markdown מובנה

**מה השלב הזה עושה ולמה:** Claude עובד הרבה יותר טוב כשהניתוח שלכם נמצא ב-markdown מובנה מאשר בקובצי PDF. השלב הזה ממיר את המסמכים שלכם לפורמט ש-Claude יקרא לאורך כל שאר הפרויקט. הוא גם מאלץ אתכם לבדוק את הניתוח שלכם ולאתר פערים וסתירות — עדיף לגלות בעיות עכשיו מאשר אחרי שכתבתם קוד לפיהן.

**זמן צפוי:** 20–25 דקות. אם הזמן בכיתה קצר, חלצו רק את דיאגרמת המחלקות ואת הדרישות (מסמכי העיצוב); דחו את קובצי הניתוח הארגוני (`01-04`) לשיעורי בית — הם לא נחוצים עד שמסיימים את שלב 3.

**מה יש לכם בשלב הזה:** בנקודה הזו בקורס יהיה בידיכם קובץ ה-PDF המלא של חלק א׳ (ניתוח ארגוני, בעיות, ראיונות, תהליכים, דרישות), ומחלק ב׳ תהיה לכם לפחות דיאגרמת מחלקות. ייתכן שדיאגרמות מצבים ורצף עדיין לא סופיות — דלגו על הקבצים האלה והוסיפו אותם בהמשך כשהם מוכנים.

הניתוח של הקבוצה שלכם נמצא בתוך קובץ PDF אחד או שניים גדולים. Claude עובד הרבה יותר טוב מול markdown מובנה מאשר מול PDF, ותיקיית ה-`docs/` של פרויקט הדוגמה מציגה את המבנה שאליו לשאוף. השלב הזה ממיר את תוכן ה-PDF שלכם לאותו מבנה.

השלב הזה משמש גם כמנגנון אילוץ: חילוץ נקי יחשוף מקומות שבהם הניתוח שלכם מעורפל, סותר את עצמו או חסר. עדיף לגלות את זה עכשיו מאשר אחרי שכתבתם קוד לפיו.

### מבנה היעד (שיקוף של `docs/` בפרויקט הדוגמה)

צרו את הקבצים הבאים. התאימו למבנה של פרויקט הדוגמה ב-`cloned/docs/` וב-`cloned/docs/org-analysis/`.

```
docs/
├── org-analysis/
│   ├── 01-organization.md         ← Hebrew. Org profile, current systems.
│   ├── 02-interviews.md           ← Hebrew. Interview transcripts.
│   ├── 03-problems.md             ← Hebrew. Problems table.
│   └── 04-business-processes.md   ← Hebrew. Business process descriptions.
├── 00-requirements.md             ← English. User stories, NFRs, traceability matrix.
├── 00e-use-cases.md               ← English. Two-layer UC specs.
└── design/
    ├── class-diagram.md           ← Entities, attributes, methods, relationships.
    ├── class-diagram.png          ← (You export this from your modeling tool.)
    ├── state-diagram.md
    ├── state-diagram.png
    ├── sequence-diagram.md
    └── sequence-diagram.png
```

השאירו גם את קובצי ה-PDF ב-`docs/` — הם נשארים המסמך המקורי החתום והם מכילים את הדיאגרמות.

הדיאגרמות עצמן **אינן** מומרות ל-ASCII או לטקסט מומצא. רק התוכן הטקסטואלי הנלווה אליהן (תיאורי ישויות, מעברי מצבים, רצפי הודעות, נימוקי הקבוצה, הנחות) מחולץ לקובצי ה-markdown. את קובצי ה-`.png` אתם מוסיפים בעצמכם על ידי ייצוא מכלי המידול (או צילום מסך).

> **על הדיאגרמות:** כל הדיאגרמות שלכם (תרחישי שימוש, מחלקות, מצבים, רצף) נמצאות
> כתמונות בתוך קובצי ה-PDF — ו-Claude יודע לקרוא תמונות. **אתם לא אמורים להעלות שום
> קובץ נוסף**; ה-PDF מספיק.
>
> אם בכל זאת Claude מדווח שהוא לא מצליח לקרוא דיאגרמה מסוימת (רזולוציה נמוכה, חיתוך,
> סריקה מטושטשת) — הוא אמור **לעצור ולבקש**, ולא לנחש. במקרה כזה תוכלו לתת לו את אותה
> דיאגרמה בדרך אחרת: **ייצוא ל-HTML**, ייצוא ל-**PNG/SVG**, או קובץ ה-**Visual Paradigm**
> המקורי. שמרו את הקבצים האלה בהישג יד ליתר ביטחון — ברוב המקרים לא תצטרכו אותם.
>
> **מה שאסור:** לתת ל-Claude "להשלים" דיאגרמה שהוא לא הצליח לקרוא. ישות מומצאת בשלב הזה
> מתגלגלת ל-`CLAUDE.md`, משם לסכמת בסיס הנתונים, ומשם לקוד.

### שלב 2.1 — הרצת החילוץ המרוכז

פתחו את Claude Code בתיקיית הפרויקט והדביקו את ה-Prompt הבא:

> Read all the PDFs in `docs/`. Extract our group's analysis and design into structured markdown files matching this layout:
>
> Analysis stage:
> - `docs/org-analysis/01-organization.md` — organization description and current information systems. Hebrew.
> - `docs/org-analysis/02-interviews.md` — interview transcripts. Hebrew.
> - `docs/org-analysis/03-problems.md` — problems table (problems in the current state, root causes, who raised them, desired outcomes). Hebrew.
> - `docs/org-analysis/04-business-processes.md` — descriptions of the existing business processes and any BPMN narratives. Hebrew.
>
> Requirements stage:
> - `docs/00-requirements.md` — user stories table, NFRs, and traceability matrix. English.
> - `docs/00e-use-cases.md` — full use case specifications. English. Use a two-layer format: a behavioral spec section (technology-neutral, no class or stored-procedure names) followed by a clearly labelled "Implementation Notes" section that maps each step to specific classes and stored procedures. If the source doesn't have implementation notes yet, leave that section as a TODO placeholder rather than inventing content.
>
> Design stage:
> - `docs/design/class-diagram.md` — **read the class diagram image inside the PDF and transcribe the model from it**: every entity with its attributes and methods, every relationship with its multiplicity, every association class, the rationale text for any mediator classes, and the group's design assumptions. The diagram is a picture on a PDF page — look at it, don't reconstruct it from the surrounding prose. Do not draw it back as ASCII art; leave a placeholder `![class diagram](class-diagram.png)` at the top.
> - `docs/design/state-diagram.md` — states, transitions, guards, and entry actions in text. Placeholder image at top.
> - `docs/design/sequence-diagram.md` — participants, ordered messages with parameters, and any alt/opt fragments in text. Placeholder image at top.
>
> If a category above is not present in our PDFs, skip that file and tell me at the end which ones you skipped. If our PDFs contain content categories not covered above, tell me where you placed them.
>
> Rules:
> - Preserve our team's original wording. Do not paraphrase, summarize, or invent content.
> - Match the section structure of the source documents (headings, ordering).
> - **The diagrams are images inside the PDFs — read them.** Every diagram we produced (use case, class, state, sequence) is embedded as a picture on a PDF page. Open the PDFs and actually look at those pages; extract the model from what the diagram shows, not only from the text around it. The PDFs are the source of truth and I should not have to hand you anything else.
> - Do not re-draw diagrams as ASCII art. Extract their *content* in structured text, and leave the image placeholder at the top of the file.
> - **If you genuinely cannot read a diagram** — too low resolution, cropped, rotated, or rendered in a way you cannot interpret — then STOP on that file. Do not guess, do not infer it from the prose, and do not invent entities or relationships. Tell me exactly which diagram you cannot read and why. I can then give you the same diagram another way: an **exported HTML version**, a **PNG/SVG export**, or the original **Visual Paradigm (`.vpp`) file**. Ask me for whichever would help most.
> - The `.png` files I will add myself later by exporting from the modeling tool.
>
> Do all files in this session. Before writing any of them, ask me any clarifying questions about scope or about choices you want me to make.

### שלב 2.2 — בדקו כל קובץ שחולץ. השלב הזה הוא חובה.

**אל תדלגו על זה ואל תעברו לשלב 3 עד שכל קובץ אומת.** כל שגיאה שתעברו עליה כאן תצטבר לתוך ה-`CLAUDE.md` שלכם, ומשם לתוך הקוד.

פתחו כל קובץ שנוצר זה לצד זה עם החלק המתאים ב-PDF ובדקו:

- **ספירות.** מספר ה-User Stories, ה-NFRs, הבעיות, ה-Use Cases, הישויות והמצבים. ודאו שכל רשימה מלאה.
- **טבלאות.** שורות לא התמזגו, לא התפצלו ולא נשמטו. סדר העמודות נשמר. שורת הכותרת לא הפכה לשורת תוכן.
- **ניסוח.** בלי פרפראזות. הניסוח המקורי של הצוות שלכם נשמר.
- **שפה.** חלקים בעברית נשארו בעברית. חלקים באנגלית נשארו באנגלית. בלי תרגום בשוגג.
- **מבנה.** הכותרות תואמות להיררכיית החלקים ב-PDF. שום דבר לא מוספר מחדש ולא שינה רמה.
- **מפרטי Use Case.** תנאי קדם, תנאי סיום, הזרימה הראשית וזרימות ההרחבה — כולם קיימים לכל UC.
- **מסמך דיאגרמת המחלקות.** לכל ישות יש את התכונות והמתודות שלה. לכל קשר יש ריבוי (multiplicity). מחלקות קישור מסומנות ככאלה.
- **אין תוכן מומצא.** כל דבר ש-Claude כתב ואתם לא מזהים — חשוד. ודאו שהוא מגיע מה-PDF, או בקשו מ-Claude להסיר אותו.

כשאתם מוצאים בעיה, אל תערכו את הקובץ בעצמכם בשלב הזה. התעקשו מול Claude עם הנחיה ספציפית:

> In `docs/org-analysis/03-problems.md`, the table has [N] rows. The PDF has [M] rows. Re-read the PDF and reconcile.

או, אם אינכם בטוחים:

> In `docs/00-requirements.md` you wrote NFR-7 as [text]. I don't recognize that wording. Re-read the requirements section of the PDF and check whether you paraphrased or invented it.

חזרו על התהליך עד שכל קובץ תואם למקור שלו. אז בצעו לו commit.

### שלב 2.3 — מפרטי UC דו-שכבתיים

ה-PDF של הקבוצה שלכם כנראה כבר מכיל מפרטי UC שיוצאו מ-VP18, אבל רובם חד-שכבתיים (התנהגות ופרטי מימוש מעורבבים יחד). פורמט היעד מפריד בין השניים:
1. **מפרט פורמלי** — התנהגותי, ניטרלי מבחינה טכנולוגית. בלי שמות מחלקות, בלי שמות Stored Procedures, בלי שמות שדות.
2. **הערות מימוש** — מסומנות בבירור, וממפות כל צעד למחלקות, מתודות ו-Stored Procedures ספציפיים.

אם ה-`00e-use-cases.md` שחולץ אצלכם עדיין לא בנוי כך, בקשו מ-Claude לבנות אותו מחדש. אל תמציאו הערות מימוש שאין להן בסיס בעיצוב שלכם — השאירו TODO.

### שלב 2.4 — דלגו על מה שלא מתאים

לחלק מהתוכן ב-PDF אין מקום ברור במבנה היעד (בחירת הקבוצה בקשר UC מותאם, צילומי מסך מרשתות חברתיות, טקסט "ערך מוסף" וכו׳). דלגו עליו או שלבו אותו בקובץ הקיים הקרוב ביותר. אל תמציאו מסמכים חדשים ברמה העליונה.

**נקודת Commit:** כששלב 2 הסתיים, בצעו commit לכל קובצי ה-markdown:

> Commit all files in `docs/` — extracted analysis and design in markdown form. Commit message: "Phase 2: extract analysis docs to markdown"

בצעו push ל-GitHub כדי שלחברי הצוות תהיה הגרסה העדכנית.

**התחילו Session חדש לפני השלב הבא.** שלב 2 קרא את כל קובצי ה-PDF שלכם לתוך ההקשר — זה החלק הכבד ביותר בשיעור.

הריצו `/clear` (חינם) — או פתחו שיחה חדשה.

> **שימו לב:** `CLAUDE.md` עדיין לא קיים בשלב הזה, ולכן השיחה החדשה מתחילה בלי הקשר.
> הדביקו בה את ה-Prompt מהסעיף **"פתיחת Session חדש באמצע השיעור"** בתחילת המסמך.

---

## שלב 3 — יצירת ה-CLAUDE.md של הפרויקט

**מה השלב הזה עושה:** מרכז את כל מה ש-Claude צריך לדעת על הפרויקט שלכם למסמך אחד
שהוא קורא **אוטומטית בכל שיחה חדשה**. זה מה שמאפשר לכם לפתוח Session חדש ולהמשיך
מאיפה שהפסקתם בלי להסביר הכול מחדש.

**בסיום השלב יהיה לכם:** קובץ `CLAUDE.md` שמתאר את התחום שלכם, הישויות, סדר הטעינה,
ה-Use Cases שבהיקף, וההחלטות שהקבוצה קיבלה.

**זמן צפוי:** 20–30 דקות, רובן בדיקה ותיקון — לא כתיבה.

> **זה השלב הכי חשוב בשיעור.** כל Prompt שתיתנו מכאן והלאה מוגבל למה שכתוב בקובץ הזה.
> שגיאה כאן לא מתגלה בקומפילציה — היא פשוט מייצרת קוד שעושה את הדבר הלא נכון.

הקובץ `CLAUDE.md` בשורש הפרויקט הוא הקובץ ש-Claude Code קורא אוטומטית בכל Session. זהו המסמך החשוב ביותר לקבלת תוצרים שימושיים מ-Claude. בנו אותו בקפידה.

### שלב 3.1 — פתחו את Claude Code בתיקיית הפרויקט

פתחו את VSCode על תיקיית הפרויקט. התחילו Session חדש של Claude Code.

### שלב 3.2 — הדביקו את ה-Prompt הבא

> I have my group's project documents in `docs/` — extracted markdown files (organization, problems, interviews, business processes, requirements, use cases, and design diagrams under `docs/design/`) plus the original PDFs as backup. Prefer the markdown files; consult the PDFs only when the markdown is unclear or when you need to look at a diagram.
>
> The folder `cloned/` is the SAD course's sample project. Read `cloned/PATTERNS.md` (shared architecture conventions for all SAD projects) and `cloned/CLAUDE.md` (the sample project, for context only — do not copy its domain content).
>
> Read all the documents in `docs/` and produce a `CLAUDE.md` at the root of this folder for my project. It should be self-contained — inline the architecture conventions from PATTERNS.md verbatim, then add my project's domain based on what you read: what the system is, the entities and load order from the class diagram, the use cases we're implementing, and any decisions our group has already made.
>
> Ask me clarifying questions before writing if anything is ambiguous.

### שלב 3.3 — ענו על שאלות Claude, ואז תנו לו לכתוב

Claude ישאל שאלות הבהרה לפני שיכתוב את `CLAUDE.md` — זה צפוי ותקין. חלק מהשאלות יהיו על התחום ועל החלטות העיצוב שלכם; ענו עליהן ישירות.

אם שאלה נראית לא קשורה למסמכים שלכם (למשל Claude שואל על משהו שכלל לא מופיע ב-PDF), הפנו אותו מחדש:

> Focus on what's in my docs. If it's not there, skip it or mark it as TODO.

השאלות לוקחות זמן, אבל הן חושפות עמימויות אמיתיות בניתוח שלכם. `CLAUDE.md` שנכתב בלי שאלות הוא בדרך כלל `CLAUDE.md` שניחש את החלקים הקשים.

### שלב 3.4 — בדקו את התוצר. התמקדו במה שלא יצוף בהמשך.

חלק מהשגיאות ב-`CLAUDE.md` ייתפסו עבורכם על ידי הקומפיילר או על ידי Claude עצמו בפעם הבאה שיקרא את הקובץ — שם תכונה שגוי (`reorderQty` במקום `neededQty`) לא יתקמפל, וערך enum מומצא ייכשל בשימוש הראשון. אלה מתקנות את עצמן; אל תבזבזו עליהן את זמן הבדיקה.

השגיאות שכדאי לתפוס עכשיו הן אלה ש**לא** יצופו בהמשך — שגיאות שקטות שגורמות ל-Claude לייצר בביטחון קוד שגוי, בזמן שהכול עדיין מתקמפל:

- **היקף המימוש.** ה-Use Cases שמופיעים כ"בתוך ההיקף" חייבים להתאים למה שהקבוצה שלכם באמת החליטה לממש. אם Claude מציג את ארבעת ה-UC הלא נכונים, תבזבזו אחר צהריים שלם על מימוש הדבר הלא נכון.
- **סדר טעינה.** כל ישות ברשימת סדר הטעינה חייבת להופיע אחרי כל הישויות שיש לה מפתחות זרים אליהן. מחלקת קישור או ישות נגזרת (למשל `Invoice`) שממוקמת מוקדם מדי תגרום ל-null-reference בזמן `initLists()` — קל לפספס עד להרצה.
- **הנחות והחלטות קבוצה.** כל החלטה ספציפית לפרויקט צריכה להיות כזו שהצוות שלכם באמת קיבל — ולא כזו ש-Claude הסיק מפרויקט הדוגמה או החליק מעל סתירה במסמכים שלכם.
- **ספירות ורשימות.** "N use cases", "N user stories", "N problems" — ודאו ש-N נכון ושהפריטים ברשימה מלאים וייחודיים. אם הספירות סוטות כאן, כל סיכום במורד הזרם יורש את הסטייה.
- **סתירות בין מסמכים שהוטאטאו מתחת לשטיח.** אם חלק א׳ אומר דבר אחד וחלק ב׳ אומר דבר אחר, Claude לרוב יבחר באחד מהם בשקט. עצם קיומה של הסתירה חשוב יותר מהאופן שבו היא נפתרה.

בעדיפות נמוכה יותר (אלה נוטות לתקן את עצמן בהמשך — תקנו ב-`CLAUDE.md` רק אם זה מהיר):
- שמות תכונות וערכי enum — הקומפיילר תופס אותם ברגע שמתייחסים אליהם.
- חתימות מתודות — אותו דבר.
- ניסוח ש"מרגיש לא נכון" אבל לא משנה את המשמעות.

### שלב 3.5 — דונו בתיקונים מול Claude

כשאתם מוצאים שגיאה, **אל** תערכו את הקובץ ישירות בשלב הזה. אמרו ל-Claude:

> In the CLAUDE.md you wrote, [specific section] says [what's wrong]. According to [source document, page/section], it should be [what's right]. Fix it.

או, אם אינכם בטוחים מי צודק:

> In the CLAUDE.md you wrote, [section] says [claim]. I think the source documents say something different. Can you re-check `docs/PartX.pdf` and confirm or correct?

זה מאלץ את Claude לקרוא מחדש את המקור במקום לנחש. זה גם מלמד אותו היכן היו הפערים במעבר הראשון שלו — הקשר שימושי לשארית ה-Session.

### שלב 3.6 — חזרו על התהליך עד שהקובץ נכון

חזרו על שלבים 3.4 ו-3.5 עד שכל טענה ב-`CLAUDE.md` מתחקה בבירור אחורה למסמך מקור או להחלטה מודעת. אז בצעו commit.

**למה זה חשוב.** מנקודה זו והלאה, כל Prompt שתיתנו ל-Claude — "צור את הישות הזו", "כתוב את ה-Stored Procedure הזה", "צור את המסך הזה" — מוגבל למה שכתוב ב-`CLAUDE.md`. שגיאות כאן מתפשטות לתוך הקוד שלכם. עשרים דקות של בדיקה עכשיו חוסכות שעות של איתור באגים בדבר הלא נכון בהמשך.

**נקודת Commit:** בצעו commit ל-`CLAUDE.md`:

> Commit `CLAUDE.md` — project AI context file. Commit message: "Phase 3: add CLAUDE.md"

בצעו push ל-GitHub.

**התחילו Session חדש לפני השלב הבא.** מכאן והלאה `CLAUDE.md` מחליף את כל ההסברים.

הריצו `/clear` (חינם) — או פתחו שיחה חדשה.

> מכאן והלאה `CLAUDE.md` נטען אוטומטית בכל שיחה חדשה, כך שאין צורך להסביר מחדש כלום.

---

## שלב 4 — סכמת בסיס הנתונים ו-Stored Procedures בסיסיים

בסיום השלב הזה בסיס הנתונים של הקבוצה ב-Azure SQL יאוכלס בכל הטבלאות מדיאגרמת המחלקות שלכם, ולכל ישות תהיה מערכת Stored Procedures בסיסית ל-CRUD. כל פעולת SQL מונעת מ-Claude Code דרך שרת ה-MSSQL MCP — אתם נשארים ב-Claude Code לאורך כל הדרך. Stored Procedures מורכבים (דוחות, טרנזקציות מרובות ישויות, מעברי מצבים) נדחים לשלב 5 ואילך, שם יש נקודת קריאה אמיתית שמכתיבה את צורתם.

**דרישות קדם:** שלב 1 הושלם (ה-MCP מחובר, ו-Claude אימת שהוא מצליח להריץ `SELECT @@VERSION`).

> **הערה למסלול המקומי:** ב-Azure בסיס הנתונים כבר קיים ולכן אין מה ליצור. במסלול המקומי
> בסיס הנתונים עדיין לא קיים — בקשו מ-Claude ליצור אותו פעם אחת
> (`CREATE DATABASE <שם>;`), ואז לעדכן את `MSSQL_DATABASE` ב-`.mcp.json` לשם הזה
> ולהפעיל מחדש את Claude Code. אחרי ההפעלה מחדש VSCode פותח את אותה שיחה — כתבו בה
> **continue** ו-Claude ימשיך מהנקודה שבה עצר. מכאן והלאה השלבים זהים בשני המסלולים.

### שלב 4.0 — אימות בסיס הנתונים המחובר

ה-`.mcp.json` שלכם כבר מצביע ישירות על בסיס הנתונים של הקבוצה (הוגדר בשלב 1). ודאו ש-Claude נמצא במקום הנכון:

> Use the mssql MCP tool's execute_sql to run `SELECT DB_NAME()` and `SELECT @@VERSION`. Tell me the current database name and confirm it matches our project database.

**הטמיעו את שם בסיס הנתונים ב-`CLAUDE.md` כדי שכל Session חדש יזהה אותו אוטומטית.** הוסיפו סעיף "Database" בשורה אחת:

> ## Database
> The project database is `<your_database_name>` on Azure SQL (`<server>.database.windows.net`). The MCP connects directly to this database — no `USE` statement needed at the start of each batch.

עכשיו לא תצטרכו לחזור על זה ב-Sessions עתידיים.

### שלב 4.1 — כתיבת הסכמה כקובץ `.sql`

למרות שה-MCP יכול להריץ SQL ישירות, שמרו קודם את הסכמה לקובץ. אתם רוצים תוצר ששמור ב-git וניתן להרצה חוזרת — ולא קריאות חד-פעמיות ש-Claude שוכח מהן. הדביקו:

> Read `docs/design/class-diagram.md` and `CLAUDE.md` (the load order section in particular). Generate `scripts/create_database.sql` with `CREATE TABLE` statements for every entity, with these defaults:
>
> - One `INT NOT NULL PRIMARY KEY` per table named `<entity>_id`. **Do not use `IDENTITY(1,1)`** — primary keys are assigned in C# per the Primary Key Strategy in `CLAUDE.md`/`PATTERNS.md`.
> - `NVARCHAR(50)` for short strings (names, statuses, enum-like values). `NVARCHAR(MAX)` only for long free text (notes, descriptions).
> - `INT` for counts, `DECIMAL(10,2)` for money, `DATETIME2` for timestamps.
> - All columns `NOT NULL` by default. If a column is genuinely optional in the design, mark it nullable and add a one-line comment explaining why.
> - Foreign keys with `ON DELETE NO ACTION ON UPDATE NO ACTION` by default.
> - For enum-like attributes (`RegistrationStatus`, `PaymentMethod`, etc.), use `NVARCHAR(20)` with a `CHECK` constraint listing the allowed values. Do not create lookup tables.
> - Add `UNIQUE` only where the class diagram or requirements explicitly say so (e.g., user email).
> - Order the `CREATE TABLE` statements so every FK target exists before its referencing table — same load order documented in `CLAUDE.md`.
>
> Do not invent columns that aren't in the class diagram. If a column's type or nullability is genuinely ambiguous, leave a `-- TODO: <question>` comment instead of guessing.
>
> Do not run the SQL yet — just write the file. I'll review it first.

### שלב 4.2 — בדקו את הקובץ לפני ההרצה

אותה משמעת בדיקה כמו בשלב 3 — התמקדו בשגיאות שקטות:

- **מספר העמודות בכל טבלה** תואם בדיוק לדיאגרמת המחלקות. בלי עמודות מומצאות, ובלי עמודות שנשמטו בשקט.
- **הטיפוסים** מתאימים לנתונים — `NVARCHAR(50)` קטן מדי לכתובת מייל או לשם עברי ארוך; `INT` שגוי לערכים כספיים; `DATE` מאבד את שעת היום במשבצת שיעור.
- **אפשרות ה-NULL** מכוונת. Claude מגדיר הכול כ-`NOT NULL` לפי ה-Prompt; ודאו שהעמודות שהוא סימן כניתנות ל-NULL באמת אופציונליות, ושאלה שלא — באמת חובה.
- **כיוון המפתחות הזרים.** כל מפתח זר צריך להצביע מה*ילד* (צד ה-many) אל ה*הורה* (צד ה-one). לטבלאות של מחלקות קישור יש שני מפתחות זרים, אחד לכל צד.
- **אילוצי CHECK של ערכי enum** מכילים בדיוק את הערכים מדיאגרמת המחלקות — בלי חסרים ובלי מומצאים.
- **סדר הטבלאות** תואם לסדר הטעינה ב-`initLists()` שב-`CLAUDE.md` שלכם. אם לא — תקנו את `CLAUDE.md` או את ה-DDL; הם חייבים להסכים.
- **הערות TODO.** כל `-- TODO` הוא שאלה אמיתית ש-Claude לא הצליח לענות עליה ממסמכי העיצוב. פתרו כל אחת לפני ההרצה.

כשאתם מוצאים בעיה, התעקשו מול Claude עם הפניה ספציפית. לדוגמה:

> Table X column Y should be NVARCHAR(200) not NVARCHAR(50) — email addresses can be longer than 50 chars.

### שלב 4.3 — הרצת הסכמה דרך ה-MCP

כשהקובץ נראה תקין, אמרו ל-Claude:

> Run the contents of `scripts/create_database.sql` against the database via the mssql MCP tool. Execute it as one batch. If it fails, tell me which statement failed and the error — do not silently retry or modify the script without asking.

צפו לכשלים בהרצה הראשונה — שגיאות הקלדה, סדר מפתחות זרים, נקודה-פסיק חסרה. כל שגיאה מתוקנת בקובץ ה-`.sql` ואז מריצים שוב. חזרו על התהליך דרך Claude (הוא יכול גם לתקן את הקובץ וגם להריץ מחדש דרך ה-MCP) עד שהסקריפט רץ מקצה לקצה.

כשזה מצליח, אמתו — עדיין מתוך Claude Code:

> Use list_tables to confirm every table from the class diagram exists. Then use execute_sql with `sp_help <table_name>` on two or three tables to confirm columns and FKs look right.

### שלב 4.4 — יצירת Stored Procedures בסיסיים ל-CRUD

אותה תבנית — קודם קובץ, ואז הרצה דרך ה-MCP. ה-Prompt:

> Generate `scripts/stored_procedures.sql` with basic CRUD stored procedures for every table created in `scripts/create_database.sql`. For each entity, generate:
>
> - `sp_<entity>_create` — inserts a row. Takes the primary key as the first parameter (`@<entity>_id`). Does **not** use `SCOPE_IDENTITY()`. (Per the Primary Key Strategy in `CLAUDE.md`, IDs are assigned in C# before insert.)
> - `sp_<entity>_update` — updates a row by primary key. All non-PK columns are parameters.
> - `sp_<entity>_delete` — deletes by primary key.
> - `sp_<entity>_get_all` — returns all rows.
> - `sp_<entity>_get_by_id` — returns one row by PK.
>
> Parameter names should match column names (`@<column>`). Do not add business logic — these are mechanical CRUD.
>
> For association class tables: the create/update SPs take both FK values as parameters; the get_by_id uses the composite or surrogate key as defined in the schema.
>
> Do not generate report SPs, state-transition SPs, or any procedure that involves more than one table. Those come later.
>
> Write the file but do not run it yet.

עברו על הקובץ בסקירה מהירה (הם מכניים — בדיקה מלאה שורה-שורה לא שווה את הזמן). ואז:

> Run `scripts/stored_procedures.sql` via the mssql MCP tool.

### שלב 4.5 — נתוני דמה

**מה השלב הזה עושה:** ממלא את הטבלאות בנתונים ריאליסטיים, לפני שכותבים שורת C# אחת.

**בסיום השלב יהיה לכם:** בסיס נתונים עם משתמשים אמיתיים להתחבר איתם ורשומות אמיתיות
להציג במסכים.

**זמן צפוי:** 10 דקות.

לפני שאתם מתחילים לכתוב C# בשלב 5, אכלסו את בסיס הנתונים בנתוני בדיקה ריאליסטיים. הסיבות:
- ה-`LoginPanel` שלכם (שלב 5.5) זקוק לפחות למשתמש אחד מכל תפקיד כדי להתחבר איתו.
- הרבה יותר קל לאמת את מסכי ה-CRUD שלכם (שלב 5.6) מול תצוגת רשימה שכבר יש בה שורות מאשר מול טבלה ריקה.
- הקלדה ידנית של שורות דרך הממשק כדי לבדוק את הממשק היא לוגיקה מעגלית — אי אפשר לסמוך על הממשק לפני שבדקתם אותו.

בקשו מ-Claude לייצר את נתוני הדמה דרך ה-MCP:

> Generate `scripts/seed_data.sql` with realistic test data for every table, in load order (insert into base tables first, then FK-bearing, then association classes).
>
> Guidelines:
> - Use real-feeling Hebrew names, real-looking emails, plausible dates, etc. — not "Test User 1", "foo@bar.com".
> - Cover every role/status/enum value at least once, so all UI branches can be tested.
> - For multi-actor systems: include at least 2 users per role (so the login flow can demonstrate role switching).
> - Volume: ~5–10 rows per base entity, ~10–20 for transactional/association tables. Enough to populate a list view, not so much that scrolling becomes painful.
> - Every FK references an existing row from the same script. No orphan references.
> - Passwords (if any) should be obvious test values like `password123` — these are throwaway test users, not production credentials.
>
> Do not insert yet — write the file, I'll review.

סקירה מהירה:
- בדקו כמה שורות לדוגמה לצורך ריאליזם (טקסט בעברית, ערכים סבירים, הפניות מפתח זר).
- ודאו שכל ערך enum או סטטוס מיוצג.
- ודאו שיש לפחות משתמש אחד לכל תפקיד.

ואז הריצו דרך ה-MCP:

> Execute `scripts/seed_data.sql` against the project DB via the mssql MCP. Report any errors per statement.

כשזה מצליח, אמתו עם כמה שאילתות SELECT:

> SELECT count(*) FROM each table. Show me the totals.

הספירות צריכות להתאים למה ש-`seed_data.sql` היה אמור להכניס. אם טבלה ריקה כשהיא לא אמורה להיות — משהו נכשל בשקט ו-Claude צריך לחקור.

**למה זה צעד נפרד ולא חלק מ-4.4:** זו פעולת אתחול חד-פעמית ולא שלב חוזר, ודילוג עליו
(או ביצוע רשלני שלו) שובר את חוויית האימות בשלב 5 בלי לשבור את הבנייה — בדיוק סוג
הבעיה שצריכה נראות משלה.

### שלב 4.6 — בדיקת ישות אחת מקצה לקצה דרך ה-MCP

בחרו ישות אחת. בקשו מ-Claude להריץ עליה מחזור CRUD מלא דרך ה-MCP:

> Using execute_sql via the mssql MCP, run `sp_<entity>_create` with realistic test values, then `sp_<entity>_get_all` to confirm it landed, then `sp_<entity>_update` on the new row, then `sp_<entity>_delete`. Report each result.

אם מערכת ה-Stored Procedures של ישות אחת עובדת מקצה לקצה, כמעט בוודאות גם של האחרות — הייצור מכני. המשיכו הלאה.

### מה בכוונה לא נכלל בשלב 4

- **Stored Procedures לדוחות** (נוכחות חודשית, הכנסה חודשית) — הם מקודדים לוגיקה עסקית שברור יותר לכתוב כשיש נקודת קריאה אמיתית שמכתיבה אותה.
- **Stored Procedures למעברי מצבים** עבור `Registration` (ביטול, ביטול מאוחר, קידום מרשימת המתנה) — מאותה סיבה; כתבו אותם כשה-UC שקורא להם ימומש.
- **טרנזקציות מרובות טבלאות** — נכתבות לפי UC, לא לפי ישות.

אלה שייכים לשלב 5 ואילך.

**נקודת Commit:** בצעו commit לסקריפטי ה-SQL:

> Commit `scripts/` — database schema, stored procedures, and seed data. Commit message: "Phase 4: database schema and stored procedures"


בצעו push ל-GitHub.

**התחילו Session חדש לפני השלב הבא.** פלט ה-SQL וההרצות כבר לא נחוץ — הסקריפטים שמורים בקבצים.

הריצו `/clear` (חינם) — או פתחו שיחה חדשה.

> מכאן והלאה `CLAUDE.md` נטען אוטומטית בכל שיחה חדשה, כך שאין צורך להסביר מחדש כלום.

---

## שלב 5 — שלד פרויקט ה-C# והישות הראשונה מקצה לקצה

בסיום השלב הזה יהיה לכם פרויקט WinForms ב-C# שרץ, ובו:
- מבנה התיקיות והקבצים של פרויקט הדוגמה
- חיבור עובד לבסיס הנתונים
- מחלקת ישות בסיסית אחת (למשל `UserProfile`) שעוקבת אחר תבנית הישות ב-`PATTERNS.md`
- מסך אחד שמבצע Create / Read / Update / Delete עבור אותה ישות
- ‏`MainForm` שפותח את המסך באמצעות `showPanel()`

אחר כך תחזרו על מחזור הישות ← המסך עבור שתי ישויות נוספות בשלב 6.

### שלב 5.1 — יצירת שלד הפרויקט

ב-Claude Code:

> Look at the C# project structure under `cloned/example_project/`. Create a matching scaffold for my project at the root of this folder — a Visual Studio solution + project, same .NET version and references as the sample, same folder layout (entities at the root, panels in a Panels folder, etc.). Use my project name (read `CLAUDE.md` if you need it) instead of the sample's. Do not copy any of the sample's *.cs files — just match the structure.
>
> Copy `<NoWarn>CA1416</NoWarn>` from the sample's csproj into ours. (The sample includes it to suppress the .NET 8 platform-compat analyzer, which floods the build output with hundreds of false-positive warnings on every WinForms call. We don't want students seeing those.)

אחר כך פתחו את ה-solution ב-Visual Studio וודאו שהוא נבנה. (בנייה ריקה, אבל אמורה להצליח.)

### שלב 5.2 — חיבור בסיס הנתונים

בפרויקט הדוגמה יש מחלקה שמחזיקה את ה-`SqlConnection` (בדרך כלל `Program.cs` או `SQL_CON.cs`). בקשו מ-Claude לשכפל אותה — **לפי המסלול שלכם**:

**☁️ מסלול Azure SQL:**

> Look at how the sample project handles the SQL connection (`cloned/example_project/SQL_CON.cs`). Create the equivalent in our project, reading the connection string from `app.config`. Our database is **Azure SQL**, so use this shape and ask me for the four values:
>
>    `Server=tcp:<server>.database.windows.net,1433;Initial Catalog=<database>;User ID=<user>;Password=<password>;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;`
>
> Do NOT use `Trusted_Connection` or `Integrated Security` — Azure SQL does not support Windows Authentication. Then add `app.config` to `.gitignore`, because it will contain our password.

**💻 מסלול מקומי:**

> Look at how the sample project handles the SQL connection (`cloned/example_project/SQL_CON.cs`). Create the equivalent in our project, reading the connection string from `app.config`. Our database is a **local SQL Server instance**, so use this shape:
>
>    `Data Source=localhost\<instance name>;Initial Catalog=<database>;Integrated Security=True;TrustServerCertificate=True`
>
> Use the instance name we found earlier (e.g. `SQLEXPRESS`). Note this is deliberately different from `.mcp.json`: the C# SqlClient driver resolves named instances and supports Windows Authentication, so it uses the instance name and no port — unlike the MCP's Python driver, which needs `localhost` plus the static port.

> **למה מחרוזת החיבור שונה מ-`.mcp.json`?** במסלול המקומי, ה-C# משתמש בדרייבר של מיקרוסופט שיודע לפענח שמות Instance ולעבוד עם אימות Windows — ולכן שם כותבים `localhost\SQLEXPRESS` בלי פורט. ה-MCP משתמש בדרייבר Python אחר שלא יודע לעשות את זה, ולכן שם כותבים `localhost` + פורט קבוע. שתי ההגדרות נכונות, כל אחת לכלי שלה.
>
> **שימו לב:** עד עכשיו רק ה-MCP דיבר עם בסיס הנתונים. מכאן והלאה גם האפליקציה עצמה
> מתחברת אליו — ולכן צריך להגדיר את החיבור בשני מקומות נפרדים.

בנו שוב. עדיין אין מה להריץ, אבל אין שגיאות.

### שלב 5.3 — יצירת הישות הראשונה

בחרו ישות בסיס (בלי מפתחות זרים — `UserProfile`,‏ `Customer`,‏ `Worker` וכו׳, תלוי בתחום שלכם). ב-Claude Code:

> Look at `cloned/example_project/Worker.cs` (or another base entity in the sample) and follow its pattern exactly. Generate `<EntityName>.cs` at the root of our project for the `<EntityName>` entity. Use the column names and types from `scripts/create_database.sql` and the stored procedure names from `scripts/stored_procedures.sql`. Follow the entity pattern in `CLAUDE.md` (sections inlined from `PATTERNS.md`): is_new constructor, createXyz/updateXyz/deleteXyz, static initXyzs, static seekXyz. Add to `Program.<EntityName>s` static list.
>
> Do not invent fields. Do not invent SP names. If anything is unclear, ask before writing.

### שלב 5.4 — בדיקת הישות

אותה משמעת בדיקה. התמקדו ב:
- חתימת הבנאי תואמת לתבנית ה-`is_new` של פרויקט הדוגמה
- כל `createXyz/updateXyz/deleteXyz` משתמש ב-Stored Procedure המתאים מתוך `stored_procedures.sql`
- שמות הפרמטרים תואמים בדיוק לשמות הפרמטרים ב-Stored Procedure
- ‏`initXyzs` קורא ל-Stored Procedure מסוג `_get_all` וטוען מחדש עם `is_new = false`
- הרשימה הסטטית `Program.<entity>s` נוספה ב-`Program.cs`

בנו ופתרו שגיאות קומפילציה דרך Claude.

### שלב 5.5 — עיצוב ויצירת מסך הכניסה

פרויקט הדוגמה לא מגיע עם חוויית כניסה מלוטשת — הוא מתחיל ב-`mainForm` עם רשימת כפתורים שטוחה, בלי התחברות ובלי הפרדת תפקידים. לרוב פרויקטי הסטודנטים זה לא מספיק: יש לכם כמה שחקנים אנושיים (לקוח, מנהל וכו׳), לכל אחד ה-UC שלו והמסכים שלו, וחוויית הפעלה ראשונה אמיתית צריכה לשקף את זה.

בשלב הזה Claude בוחן את השחקנים בפרויקט שלכם ומעצב מסך כניסה שמתאים *לתחום שלכם* — עוד לפני שקיים מסך CRUD ראשון. המסך הראשון בשלב 5.6 יתחבר אז למקום הנכון תחת התפקיד הנכון.

> Read `docs/design/class-diagram.md`, `docs/00e-use-cases.md`, and the UC and actor information in `CLAUDE.md`. Identify the human actors in this project.
>
> **Login is the first screen** for multi-actor projects. Authentication is not in our requirements — the requirements describe what each actor *does*, not how the system identifies them. Derive the login design from the **class diagram**, not from the requirements:
> - Find every entity that has credential-like fields (`email`/`username` + `password`). There may be more than one — e.g., `UserProfile` for customers, `Employee` for staff. Each one is a login source.
> - The role that the logged-in user has determines which home panel they land on after login. Map each credential-holding entity to its corresponding role home.
>
> Decide whether the entry flow should be:
> (a) **Login → per-role home panels** — when there are multiple human actors with distinct screens (e.g., Customer vs Manager). Each role gets its own home panel that hosts only the UCs that actor performs.
> (b) **Flat menu on `mainForm`** — only when there's a single human actor, no credentials on any entity, or no meaningful role distinction.
>
> Tell me which design you've chosen, name every credential-holding entity and the home panel each one routes to, and ask for my confirmation before generating any files.
>
> Once I confirm, generate the entry-flow files in one batch (so they compile together):
> - `mainForm.cs` (+ Designer + resx) — match the sample's `cloned/example_project/mainForm.cs` pattern: hosts a single content area and a `showPanel(UserControl)` method. **`mainForm`'s entry point loads `LoginPanel` first**, not a home panel directly.
> - If you chose (a):
>   - `LoginPanel.cs` (+ Designer + resx) — email + password fields, login button, Hebrew error message for missing/wrong credentials. On click, iterate through every credential-holding entity's in-memory list (`Program.Employees`, `Program.UserProfiles`, etc.) in priority order, match email+password, and `mainForm.showPanel(new <Role>Home())` on the first match. Show a Hebrew "wrong credentials" message if no match. Per `PATTERNS.md`, login is a technical artifact, not a UC — that's fine here.
>   - **Optional dev shortcut**: a small secondary button labelled "כניסת מפתח" (dev login) or similar that bypasses authentication and opens a debug panel listing every panel in the app. Useful for development; remove or hide before submission.
>   - One `<Role>HomePanel.cs` (+ Designer + resx) per role identified. Each home panel has placeholder buttons for that role's UCs (read them from the UC diagram). Buttons can be wired to `MessageBox.Show("TODO")` for now — they'll get real handlers as later CRUD panels are added.
> - If you chose (b): `mainForm` opens directly to a flat menu with placeholder buttons for each UC.
>
> Add a short "Entry Flow" section to `CLAUDE.md` documenting the design you chose, including the list of credential-holding entities and which home each routes to.

בדקו לפני שממשיכים:
- זיהוי השחקנים תואם לדיאגרמת ה-UC (לא הומצאו שחקנים, ולא פוספסו).
- מסך הבית של כל תפקיד מציג רק את ה-UC שאותו תפקיד באמת מבצע — אמתו מול דיאגרמת ה-UC שלכם.
- ההתחברות (אם קיימת) מאמתת מול הישות הנכונה (`UserProfile` ללקוחות, `Employee` לעובדים — או מה שהעיצוב שלכם מגדיר).
- התוויות בעברית נקראות כראוי.
- הסעיף "Entry Flow" נוסף ל-`CLAUDE.md`.

בנו (Ctrl+Shift+B). אמור לעבור בהצלחה. אתם אפילו יכולים ללחוץ F5 כאן כדי לראות את מסך ההתחברות ואת תפריט הבית — הכפתורים עדיין מציגים TODO, אבל חוויית הכניסה אמיתית.

### שלב 5.6 — יצירת מסך ה-CRUD הראשון, מחובר תחת מסך הבית הנכון

בחרו ישות בסיס מדרג 1 למסך הראשון — `UserProfile` אם לקוחות מנהלים את הפרופיל שלהם, או כל דבר שהוא הפשוט ביותר בתחום שלכם. טעינת מפתחות זרים אינה בעיה בדרג הזה.

> Generate `<EntityName>Panel.cs` (UserControl + Designer + resx) — full CRUD for `<EntityName>`: list view of all rows, fields to view/edit one row, Save / Update / Delete / Back buttons. Hebrew UI text. Wire each button to the entity's `createXyz / updateXyz / deleteXyz` methods.
>
> Then replace the TODO placeholder button in the appropriate role's home panel (per the "Entry Flow" section in `CLAUDE.md`) so it now calls `mainForm.showPanel(new <EntityName>Panel())`. Back button on the CRUD panel returns to that role's home.
>
> Match the sample's panel patterns exactly — event-handler shape, return-to-home mechanism, Designer.cs structure.

בדקו:
- מטפלי האירועים של כפתורי המסך באמת קוראים למתודות הישות, ולא ל-placeholders.
- כפתור מסך הבית מחובר עכשיו באמת (לא TODO).
- כפתור החזרה מחזיר למסך הבית של התפקיד הנכון (לא להתחברות, ולא לשורש `mainForm`).
- התוויות בעברית נכונות.

### שלב 5.7 — הריצו

בנו. אחר כך לחצו F5. עברו את המסלול:
1. מסך ההתחברות מופיע (באפשרות א׳). התחברו כמשתמש בדיקה של התפקיד הרלוונטי.
2. מסך הבית הנכון נפתח עבור אותו תפקיד.
3. לחצו על כפתור ישות ה-CRUD. המסך נפתח.
4. בצעו create / read / update / delete דרך הממשק.
5. כפתור החזרה מחזיר למסך הבית של התפקיד.

ודאו שהשורות באמת נוספו או נמחקו על ידי בקשה מ-Claude:

> Use the mssql MCP to run `SELECT * FROM <entity_table>`. Show me what's in the DB right now.

אם משהו נכשל: ל-Claude יש את כל ההקשר (פרויקט הדוגמה, הקוד שלכם, בסיס הנתונים). תארו את התסמין ותנו לו לאבחן.

### מה בכוונה לא נכלל בשלב 5

- **לוגיקת הרשאות מלאה** — מסך ההתחברות שנוצר בשלב 5.5 מאמת מול הרשימות בזיכרון בלבד. הצפנת סיסמאות, ניהול Session ונעילת חשבון הם NFR ולא Use Case, ולפי `PATTERNS.md` הם תוצר טכני שמתווסף בהמשך.
- **מסכים מרובי ישויות** (למשל מסך שמציג לקוח יחד עם ההזמנות שלו) — שייכים לשלב 6.
- **דוחות** — הם קוראים ל-Stored Procedures של דוחות שתכתבו בשלב מאוחר יותר, יחד עם מסך הדוח עצמו.

**נקודת Commit:** בצעו commit לכל קובצי ה-C# החדשים:

> Commit all C# project files — scaffold, DB connection, first entity, entry flow, first CRUD panel. Commit message: "Phase 5: C# scaffold and first entity end-to-end"

בצעו push ל-GitHub כדי שחברי הצוות יוכלו למשוך ולבנות.

**התחילו Session חדש לפני השלב הבא.** שלב 5 ארוך במיוחד. נקו לפני שלב 6, שבו נוצרים הרבה מסכים.

הריצו `/clear` (חינם) — או פתחו שיחה חדשה.

> מכאן והלאה `CLAUDE.md` נטען אוטומטית בכל שיחה חדשה, כך שאין צורך להסביר מחדש כלום.

---

## שלב 6 — יצירת שאר מסכי ה-CRUD

**מה השלב הזה עושה:** משכפל את התבנית שאומתה בשלב 5 לכל שאר הישויות. זה השלב שבו
רואים בבירור מה סוכן AI נותן — מה שלקח לכם 40 דקות לישות אחת לוקח דקות ל-8 ישויות.

**בסיום השלב יהיה לכם:** אפליקציה עם מסך CRUD לכל ישות, מחוברים למסכי הבית הנכונים.

**זמן צפוי:** 20–40 דקות, תלוי במסלול שתבחרו ובמספר הישויות.

שלב 5 ייצר מסך CRUD אחד מקצה לקצה ואימת את התבנית. עכשיו מרחיבים לשאר הישויות — כל ישות בסיס, כל מחלקת קישור, וכל ישות שיש לה UC מסוג CRUD בהיקף שלכם.

> **שימו לב: לכל ישות צריך גם מחלקה וגם מסך.** בשלב 5.3 יצרתם מחלקת ישות **אחת** בלבד.
> לשאר הישויות עדיין אין קובץ `.cs` ואין רשימה סטטית ב-`Program.cs` — ולכן כל מסך כאן
> נוצר יחד עם מחלקת הישות שמאחוריו. מסך שנוצר בלי המחלקה שלו פשוט לא יתקמפל.

יש שתי דרכים. בחרו לפי מידת האמון שלכם בתבנית משלב 5 וכמה אתם נהנים מהדגמה טובה.

### אפשרות א׳ — אחד-אחד (זהירה)

צרו מסך אחד, בדקו, חברו אותו תחת מסך הבית של התפקיד הנכון, ובנו. אחר כך הבא בתור. חזרו עד שסיימתם.

> For `<EntityName>`: first generate the entity class `<EntityName>.cs` if it doesn't exist yet — same pattern as `<AlreadyDoneEntity>.cs`, using the columns from `scripts/create_database.sql` and the stored procedure names from `scripts/stored_procedures.sql`, and add its static list to `Program.cs` in the load order from `CLAUDE.md`.
>
> Then generate `<EntityName>Panel.cs` (+ Designer + resx), following the same pattern as `<AlreadyDonePanel>.cs`. Wire it under `<RoleHomePanel>` (replacing the TODO placeholder). Match the entity pattern, RTL settings, and Hebrew labels from existing panels.

יתרונות: תופסים סטייה מהתבנית מוקדם. קל לבדוק כל מסך מול המקור.
חסרונות: איטי יותר, יותר Prompts, יותר מעברי הקשר.

### אפשרות ב׳ — הכול בבת אחת (מסלול ה-"וואו")

‏Prompt אחד שמייצר את כל מסכי ה-CRUD הנותרים, מחבר כל אחד תחת מסך הבית המתאים, ומדווח מה נבנה. צפו ב-Claude מייצר 8–10 מסכים במכה אחת.

> For every entity in `CLAUDE.md` that doesn't already have one, generate **both** its entity class and its CRUD panel. For each:
> - **The entity class first** — `<EntityName>.cs` following the same pattern as `<AlreadyDoneEntity>.cs`, using the columns from `scripts/create_database.sql` and the stored procedure names from `scripts/stored_procedures.sql`. Add each entity's static list to `Program.cs` in the load order documented in `CLAUDE.md`.
> - Then the panel, following the same pattern as `<AlreadyDonePanel>.cs` exactly — entity-method wiring, RTL settings, Hebrew labels, Back-button mechanism.
> - Wire each panel under the role home panel whose actor owns that UC (read the UC diagram and `CLAUDE.md` to decide).
> - Replace each role home's TODO placeholder buttons with real handlers as you go.
>
> Report at the end which entity classes and which panels you generated, and which role home each panel was wired under.

יתרונות: דרמטי, מהיר, ומשיג את "מטרת השיעור" של שלב 5 בבת אחת. גם חסכוני יותר בטוקנים — ההקשר נקרא פעם אחת ולא עשר פעמים.
חסרונות: אם Claude הבין את התבנית לא נכון בשלב 5, השגיאה מתפשטת לכל המסכים. שלב הבדיקה (הבא) חשוב יותר.

### שלב 6.1 — בדיקה (בכל אחד מהמסלולים)

אחרי אפשרות א׳ או ב׳, בנו ועברו על כל מסך:

- **מחלקות הישויות:** לכל ישות שקיבלה מסך יש גם קובץ `<Entity>.cs` ורשימה סטטית ב-`Program.cs`,
  וסדר הטעינה ב-`initLists()` עדיין תואם ל-`CLAUDE.md` (ישויות בסיס קודם, מחלקות קישור אחרונות).
- **בנייה:** קומפילציה נקייה, בלי אזהרות חדשות מעבר ל-CA1416 (שעדיין מושתקת).
- **סריקה ויזואלית:** לכל מסך — F5 ← התחברו כשחקן הרלוונטי ← היכנסו למסך ← ודאו שהתוויות בעברית נראות נכון (יישור לימין, בלי `?????`), שתצוגת הרשימה מציגה נתונים אמיתיים מנתוני הדמה, ושהכפתורים קיימים.
- **בדיקת חיווט מדגמית:** צרו שורה, עדכנו שורה, מחקו שורה — בשניים-שלושה מסכים. אם שלוש הפעולות עובדות על ישות בסיס ועל מחלקת קישור, סביר מאוד שהשאר תקינים.
- **כיסוי מסכי הבית:** מסך הבית של כל תפקיד צריך להכיל כפתורים מחוברים למסכים אמיתיים (בלי TODO שנותרו עבור UC שנמצאים בהיקף).

כשאתם מוצאים בעיה שיטתית (למשל "כל המסכים משתמשים במוסכמת שמות שגויה ל-Stored Procedures"), התעקשו מול Claude עם Prompt אחד שמתקן הכול:

> Every CRUD panel calls `<wrong>` instead of `<right>`. Fix all of them in one pass and rebuild.

### מה עדיין לא הושלם אחרי שלב 6

- **מתודות מעבר מצבים** לישויות עם מכונות מצבים (למשל לוגיקת ביטול / ביטול מאוחר / קידום מרשימת המתנה של `Registration`). מסך ה-CRUD מטפל רק ב-create/update/delete בסיסיים; מעברי המצבים הם מתודות ו-Stored Procedures נפרדים.
- **‏UC של דוחות** (נוכחות חודשית, הכנסה חודשית וכו׳). הם קוראים ל-Stored Procedures מרובי טבלאות ויש להם מסכים לקריאה בלבד — צורה שונה מ-CRUD.
- **תרחישים חוצי ישויות** (למשל "הרשמה לשיעור" נוגעת ב-Registration + ScheduleSlot + CustomerSubscription בטרנזקציה אחת).

אלה "30% הנותרים" של פרויקטי סטודנטים אמיתיים — מתבצעים בזמנכם החופשי אחרי השיעור, עם Claude כסוכן. השיעור עצמו מסתיים כאן.

**נקודת Commit:** בצעו commit לכל המסכים החדשים:

> Commit all new panel files and wired home panels. Commit message: "Phase 6: all CRUD panels"

בצעו push ל-GitHub. זה רגע טוב לכל חברי הקבוצה למשוך ולבצע בנייה מלאה.

**התחילו Session חדש לפני השלב הבא.** סיימתם את החלק הנלמד בכיתה.

הריצו `/clear` (חינם) — או פתחו שיחה חדשה.

> מכאן והלאה `CLAUDE.md` נטען אוטומטית בכל שיחה חדשה, כך שאין צורך להסביר מחדש כלום.

---

## שלב 7 — מכונות מצבים: הפיחו חיים בדיאגרמות המצבים שלכם

**לא נלמד בשיעור.** מתועד כאן כדי שתוכלו לבצע אותו בעצמכם.

**מה השלב הזה עושה:** מתרגם את דיאגרמות המצבים שלכם לקוד — הדבר הראשון בפרויקט
שאינו CRUD.

**בסיום השלב יהיה לכם:** מתודות מעבר עם תנאי שמירה, Stored Procedures טרנזקציוניים,
וכפתורי פעולה בממשק במקום כפתור Update גנרי.

זה הדבר הראשון בפרויקט שלכם ש**אינו CRUD**. ציירתם דיאגרמות מצבים בעיצוב — כל מצב, כל מעבר, כל תנאי שמירה. השלב הזה מתרגם אותן לקוד, כך שהדיאגרמות מפסיקות להיות תיעוד והופכות ללוגיקה עצמה.

### למה מכונות מצבים ראויות לשלב משלהן

‏CRUD מתייחס לכל ישות כאל אוסף שדות שאפשר לשנות בחופשיות. ישות עם מצבים אינה כזו — למחזור החיים שלה יש חוקים:

- **תנאי שמירה (Guards).** לא כל מעבר חוקי תמיד. ביטול הרשמה פחות מ-24 שעות לפני השיעור מותר, אבל מפעיל מסלול אחר (ביטול מאוחר + חיוב) מאשר ביטול מוקדם יותר.
- **תופעות לוואי בישויות אחרות.** ביטול הרשמה משחרר מקום ב-`ScheduleSlot`, מחזיר קרדיט ב-`CustomerSubscription`, ועשוי לקדם את הבא בתור מרשימת ההמתנה. פעולה אחת, כמה טבלאות.
- **אטומיות.** תופעות הלוואי האלה חייבות להצליח או להיכשל יחד. הרשמות שבוטלו חלקית משבשות את התחום.
- **פעלים מתחום העיסוק, לא CRUD סימטרי.** ‏`cancel()`,‏ `lateCancel()`,‏ `promoteFromWaitlist()`,‏ `confirm()` — אלה המתודות, ולא `update(status='Cancelled')`.

אם סטודנט מנסה לממש את אלה כעדכוני CRUD, האפליקציה מתקמפלת ורצה — אבל הנתונים מתקלקלים בשקט. דיאגרמת המצבים היא התוצר שמונע את זה מלכתחילה; השלב הזה הוא המקום שבו היא משתלמת.

### שלב 7.1 — איתור ישויות עם מצבים

ב-Claude Code:

> Read `docs/design/state-diagram.md` (and any other state diagrams in `docs/design/`). For each entity that has a non-trivial state machine, list:
> - The entity name
> - Every state in the diagram
> - Every transition, including the source state, target state, the trigger (event/method name), the guard (condition for the transition to fire), and any side effects in other entities
>
> Do not implement anything yet. Just produce the inventory and ask for confirmation that it matches my design.

בדקו מול דיאגרמת המצבים שלכם. אם Claude פספס מעבר או המציא אחד — תקנו עכשיו, באותה משמעת בדיקה ותיגור כמו בשלב 3.

### שלב 7.2 — בחרו את מכונת המצבים הראשונה למימוש

אותה לוגיקה כמו בבחירת מסך ה-CRUD הראשון: בחרו קודם את הישות עם המצבים הפשוטה ביותר. אמתו את התבנית, ואז הרחיבו.

אם בפרויקט שלכם יש רק ישות אחת עם מצבים (מקרה נפוץ), השלב הזה טריוויאלי. אם יש יותר מאחת, התחילו עם זו שהמעברים שלה נוגעים בהכי מעט ישויות אחרות.

### שלב 7.3 — יצירת מתודות המעבר וה-Stored Procedures יחד

> For `<EntityName>`, read `docs/design/state-diagram.md` and enumerate the state transitions for this entity. Then generate the transition methods on the entity class and the matching stored procedures.
>
> For each transition:
> - Method on the entity class named after the domain verb (`cancel()`, `lateCancel()`, `promoteFromWaitlist()`) — NOT `update(...)`. Encode the guard inline; if the guard fails, throw or return false with a Hebrew message the UI can show.
> - Matching stored procedure (`sp_<entity>_<verb>`) that updates the state column and applies any side effects in other tables — all inside a `BEGIN TRAN ... COMMIT TRAN` block with `ROLLBACK` on error. This is the first time we use transactions; do it explicitly.
> - In-memory list updates that mirror the DB changes (so the running app sees consistent state without reloading).
>
> Append the SPs to `scripts/stored_procedures.sql` and run them against the DB via the mssql MCP. Then add the methods to the entity class.
>
> Do not change any existing CRUD methods (`create*`, `update*`, `delete*`) — state transitions are new methods alongside them.

### שלב 7.4 — בדיקת קוד מכונת המצבים

התמקדו בשגיאות השקטות:

- **תנאי שמירה קיימים ונכונים.** לכל מעבר שאמור להיות לו תנאי שמירה — יש אחד. ביטול מאוחר באמת בודק את גבול 24 השעות, ולא רק סומך על הקורא.
- **תופעות לוואי אטומיות.** כל Stored Procedure של מעבר משתמש ב-`BEGIN TRAN ... COMMIT TRAN`. אם שחרור המקום או החזר הקרדיט נכשל, גם שינוי המצב מתבטל.
- **אין שינויי מצב דרך `update<Entity>(...)`.** עדכון ה-CRUD הגנרי לא אמור לגעת בעמודת המצב. שינויי מצב עוברים אך ורק דרך מתודות המעבר הייעודיות. (אם לא תאכפו את זה, מישהו בסוף יבצע `reg.update(status='Cancelled')` ויעקוף בשקט כל תנאי שמירה.)
- **הרשימה בזיכרון משקפת את בסיס הנתונים.** אחרי מעבר, מצב הישות ב-`Program.<entity>s` תואם למה שבבסיס הנתונים. דרך קלה לבדוק: הריצו SELECT על השורה דרך ה-MCP אחרי מעבר שבוצע מהממשק.

### שלב 7.5 — חיבור כפתורי ממשק שמפעילים מעברים, לא עדכוני CRUD

פתחו את מסך ה-CRUD הרלוונטי לישות עם המצבים. החליפו את כפתור ה-"Update" או ה-"Save" הגנרי ב**כפתורי פעולה** — אחד לכל מעבר שמופעל על ידי משתמש.

> On `<EntityName>Panel.cs`, replace the generic Update button with verb buttons matching the user-triggered state transitions for this entity (read `docs/design/state-diagram.md` to enumerate them) — one button per transition, Hebrew label describing the verb. Each button calls the corresponding transition method on the entity, handles guard failures by showing the Hebrew error message in a `MessageBox`, and refreshes the list view on success.

חלק מהמעברים מופעלים על ידי המערכת (`promoteFromWaitlist` רץ כשמתפנה מקום, לא מכפתור) — אלה לא מקבלים כפתורים בממשק; קוראים להם ממתודות מעבר אחרות.

### שלב 7.6 — עברו על מסלולי המצבים

לכל מעבר, הריצו אותו מקצה לקצה דרך הממשק:

1. הכינו שורה במצב המקור (דרך נתוני הדמה או דרך מעבר אחר).
2. לחצו על כפתור הפעולה (או הפעילו את אירוע המערכת).
3. ודאו שהשורה נמצאת עכשיו במצב היעד, ושתופעות הלוואי בטבלאות האחרות אכן קרו.

למסלול הבדיקה המרכזי, בקשו מ-Claude:

> Use the mssql MCP to verify the state machine end-to-end:
> 1. SELECT the relevant rows BEFORE the transition.
> 2. After I trigger the transition through the UI, SELECT them again.
> 3. Compare and confirm both the state change and every documented side effect happened.

אם ההפרש לא תואם לדיאגרמת המצבים, המעבר באגי — תקנו את המתודה או את ה-Stored Procedure ובדקו שוב.

### מה בכוונה לא נכלל בשלב 7

- **‏UC של דוחות** (נוכחות חודשית, הכנסה חודשית) — צורה שונה, ומקבלים שלב משלהם אם תגיעו לשם.
- **תרחישים חוצי אגרגטים** (למשל פעולת משתמש אחת שעוברת דרך כמה מכונות מצבים בישויות שונות) — נדיר בפרויקטי סטודנטים; טפלו לפי מקרה.

---

## שלב 8 — דוחות (שיעורי בית)

**לא נלמד בשיעור.** אותו מבנה כמו שלבי השיעור — מתועד כאן כדי שתוכלו לבצע אותו בעצמכם.

**מה השלב הזה עושה:** בונה מסכים לקריאה בלבד עם נתונים מצטברים — צורה שונה לגמרי מ-CRUD.

**בסיום השלב יהיה לכם:** דוח עובד עם מסננים, שמבוסס על Stored Procedure עם `JOIN` ו-`GROUP BY`.

לדוחות יש צורת ממשק שונה מ-CRUD: קריאה בלבד, עם פרמטרים (טווחי תאריכים, מסננים, רשימות נפתחות), והנתונים שהם מציגים **מצטברים** (ספירות, סכומים, אחוזים, קיבוצים) — לא שורות גולמיות. ה-Stored Procedure שמאחורי דוח משתמש ב-`JOIN`, ב-`GROUP BY` ובפונקציות חלון, ולא ב-SELECT מטבלה יחידה. במסך יש מסננים למעלה וטבלת תוצאות מתחת, בלי כפתורי Save/Update/Delete.

### שלב 8.1 — איתור ה-UC של דוחות בהיקף שלכם

> Read `docs/00e-use-cases.md` and `docs/design/sequence-diagram.md` (or wherever your reports are specified). List every UC whose primary actor is a manager/admin and whose behavior is "view summarized data over some range." For each, name: the input parameters (date range, filter values), the columns shown in the output, and any grouping or aggregation rules.

בדקו את הרשימה מול כוונת הקבוצה שלכם. בחרו את הדוח הפשוט ביותר למימוש ראשון.

### שלב 8.2 — יצירת ה-Stored Procedure של הדוח

> Generate `sp_<reportName>` and append it to `scripts/stored_procedures.sql`, then run it via the mssql MCP. The SP takes the report's input parameters (use sensible SQL types — `DATETIME2` for dates, `NVARCHAR` for filter strings, etc.) and returns the aggregated rows defined in Step 8.1's inventory. Use `JOIN` / `GROUP BY` as needed. Hebrew column aliases where the output is user-facing.
>
> Before running, show me the SP and let me review the joins and groupings.

### שלב 8.3 — יצירת מסך הדוח

> Generate `<ReportName>Panel.cs` (+ Designer + resx) — a read-only panel with: filter controls at the top matching the SP's input parameters (DateTimePickers, ComboBoxes, etc.), a "Generate" button, and a DataGridView below. On click, the panel calls the report SP with the user-entered parameters and binds the result set to the grid. RTL layout. Hebrew labels. No Save/Update/Delete buttons — this is read-only.
>
> Wire the panel into the appropriate manager role home (replacing a TODO button if one exists for this report).

### שלב 8.4 — אימות הדוח

הריצו אותו דרך הממשק. בדקו כמה שורות ידנית:
- בחרו שורה אחת מפלט הדוח.
- חזרו לנתוני המקור דרך ה-MCP (`SELECT * FROM <table> WHERE <criteria>`) וודאו ידנית שהחישוב המצטבר תואם.

אם המספרים שגויים, הבאג כמעט תמיד נמצא ב-`GROUP BY` או ב-`JOIN` של ה-Stored Procedure. בקשו מ-Claude לגזור מחדש את ה-Stored Procedure מהדרישות ולהשוות לגרסה שהוא ייצר.

---

## שלב 9 — תרחישי UC מורכבים (שיעורי בית)

**לא נלמד בשיעור.** מתועד כאן כדי שתוכלו לבצע אותו בעצמכם.

חלק מה-UC הם **טרנזקציות מתוזמרות** — פעולת משתמש אחת שנוגעת בכמה ישויות באופן אטומי. "הרשמה לשיעור" היא הדוגמה הקלאסית: היא מפחיתה קרדיטים ב-`CustomerSubscription`, מגדילה את `registrationCount` ב-`ScheduleSlot`, יוצרת שורת `Registration`, ומפעילה התראה. כל הארבע קורות, או שאף אחת לא.

המבנה דומה למעבר במכונת מצבים (שלב 7), אבל ברמת ה-UC ולא קשור למחזור החיים של ישות בודדת. תשתמשו שוב באותה משמעת טרנזקציות.

### שלב 9.1 — איתור התרחישים המורכבים

> Read `docs/00e-use-cases.md` and list every UC whose main scenario touches more than one entity in a single user action. For each, name: the entities involved, the operation on each, the order they must happen in, and any guards (preconditions that must be true before any side effect fires).
>
> Distinguish these from simple CRUD UCs (touch one entity) and state-machine transitions (touch one entity's state column plus minor side effects).

### שלב 9.2 — בחרו קודם את ה-UC המרכזי ביותר

"התרחיש הראשון שרץ מקצה לקצה" צריך להיות זה שמפעיל את החלק הגדול ביותר של התחום שלכם — בדרך כלל הפעולה הנפוצה ביותר של השחקן הראשי. ברגע שהוא עובד, השאר עוקבים אחר אותה תבנית.

### שלב 9.3 — יצירת ה-Stored Procedure המתזמר

> Generate `sp_<flowName>` and append to `scripts/stored_procedures.sql`. The SP takes the operation's inputs as parameters, validates every guard (returns an error code or `RAISERROR` if any fails), then performs every entity update inside a single `BEGIN TRAN ... COMMIT TRAN` block with `ROLLBACK` on any error.
>
> Before running, show me the SP. I want to verify: every guard is enforced; the order of operations is correct; nothing is left for the application code to "remember" to do.

### שלב 9.4 — יצירת המתודה המתזמרת בישות המקור

> Add a domain-verb method to the originating entity (e.g., `UserProfile.registerForClass(slotId)`). The method calls the SP, on success updates the in-memory state of every affected entity (`Program.Registrations` adds the new row, the affected `ScheduleSlot.registrationCount` increments, the affected `CustomerSubscription.remainingCredits` decrements), and on failure surfaces the SP's error message to the caller as a Hebrew string the UI can show.

### שלב 9.5 — חיבור הממשק לתרחיש

> Add a button/control on the relevant panel that triggers this flow. On click, gather user input (the slot to register for, etc.), call the orchestrating method, show a Hebrew success or failure message, and refresh any visible lists so the change is immediately visible.

### שלב 9.6 — הריצו את התרחיש מקצה לקצה

לכל מקרה של תנאי שמירה (כל תרחיש של "מה אם זה נכשל"):
1. הכינו את תנאי הקדם (למשל רוקנו את הקרדיטים במנוי).
2. הפעילו את התרחיש דרך הממשק.
3. ודאו: הודעת השגיאה הנכונה בעברית מופיעה, ו**לא קרה שום שינוי מצב חלקי בבסיס הנתונים** (הבדיקה החשובה ביותר — פתחו SSMS או השתמשו ב-MCP כדי לאמת).

למקרה המוצלח:
1. הכינו תנאי קדם תקינים.
2. הפעילו את התרחיש.
3. ודאו שכל העדכונים הצפויים קרו בבסיס הנתונים, שכל רשימה בזיכרון שהושפעה עקבית, ושהממשק משקף את המצב החדש.

---

## שלב 10 — ליטוש הממשק עם Claude (שיעורי בית)

**לא נלמד בשיעור.** מתועד כאן כדי שתוכלו לבצע אותו בעצמכם.

המסכים שנוצרו בשלבים 5–9 פונקציונליים אבל פשוטים ויזואלית — פקדים אפורים, גופני ברירת מחדל, בלי משמעת ריווח ובלי מיתוג. Claude יכול לקרוא את המסכים הקיימים ולהציע עיצובים עשירים יותר: פריסות טובות יותר, מערכות צבע, גופנים, אייקונים, מצבי hover ומשוב, ועיצובים ברמת wireframe אמיתי שמעוגנים בתחום של הפרויקט שלכם.

זה גם המסלול ל**סגירת הפער בין ה-wireframes של ה-UC שציירתם בעיצוב לבין איך שהאפליקציה באמת נראית.** אם wireframe הגדיר תצוגת לשוניות או רשת כרטיסים, כאן מפיחים בו חיים.

### שלב 10.1 — החליטו מה ללטש

ליטוש הוא עניין של טעם. בחרו יעד: מסך אחד מרכזי (מסך הנחיתה או הדשבורד של התפקיד הראשי), או שדרוג ויזואלי עקבי בכל המסכים. אל תנסו לעשות את שניהם ב-Prompt אחד.

### שלב 10.2 — הזינו ל-Claude את חומרי הגלם

‏Claude מעצב טוב יותר כשהוא רואה מה אתם רוצים, ולא מה שיש לכם. תנו לו:
- תיאור של התחום שלכם בשורה אחת ("אפליקציה לניהול סטודיו פילאטיס, אווירה אינטימית של קבוצות קטנות, קהל היעד נשים, צבעי המותג חמים ונשיים").
- צילום מסך של המסך הנוכחי (גררו לתוך שיחת Claude Code — הוא יודע לקרוא תמונות).
- ה-wireframe המתאים מתוך `order_management_use_case_diagram.html` (או מחלון ה-UC שלכם), אם קיים.
- מראה ייחוס שאתם אוהבים — צילום מסך מאפליקציה אחרת, מסגרת ב-Figma, קישור ל-Dribbble, כל דבר ש-Claude יכול לראות או להביא.

### שלב 10.3 — בקשו עיצוב מחדש עם אילוצים

> Read `<EntityName>Panel.cs` and `<EntityName>Panel.Designer.cs`. Propose a redesign of this panel that:
> - Keeps all the existing controls and event handlers wired exactly as they are (no behavior changes)
> - Improves visual hierarchy, spacing, font choices, and color usage
> - Stays RTL and Hebrew per `PATTERNS.md`
> - Targets the look-and-feel of [reference image or description]
>
> Show me the proposed Designer.cs changes first as a diff. Don't write yet — I want to see the design choices and approve them before they land.

### שלב 10.4 — חזרו ושפרו

עיצוב ויזואלי כמעט אף פעם לא מדויק בניסיון הראשון. דיאלוג טיפוסי: "הכפתורים צמודים מדי", "השתמש בגוון ורוד בהיר יותר", "הגדל את הגופן בכותרות תצוגת הרשימה". כל סבב זול כי ל-Claude כבר יש את כל ההקשר.

### שלב 10.5 — החילו באופן עקבי

ברגע שמסך אחד נראה נכון, הפכו את שפת העיצוב למפורשת כדי ששאר המסכים יירשו אותה:

> Document the design choices we made for `<EntityName>Panel.cs` — color palette (hex values), font choices, spacing rules, button styling — as a short "Visual Design" section in `CLAUDE.md`. Then apply the same rules to all the other panels in one pass.

אחרי זה, מסכים עתידיים ש-Claude ייצר יאמצו את שפת העיצוב בלי שתצטרכו לבקש שוב.

### ממה להימנע

- אל תשברו התנהגות בשם המראה. העיצוב מחדש נוגע ל-`Designer.cs` (פריסה ועיצוב), לא ל-`.cs` של המסך (לוגיקה).
- אל תחליפו תשתית ממשק באמצע הפרויקט (בלי קפיצות מ-WinForms ל-WPF כאן — זו כתיבה מחדש; ראו `ROADMAP.md`).
- אל תכניסו ספריות ממשק של צד שלישי (Telerik,‏ DevExpress) לפני שבדקתם אם WinForms רגיל יכול לשאת את השאיפה הוויזואלית. רוב הזמן הוא יכול.

---

## שלב 11 — מעבר לבסיס נתונים משותף (שיעורי בית)

**לא נלמד בשיעור.** מתועד כאן כדי שתוכלו לבצע אותו בעצמכם.

> **☁️ אם הקבוצה שלכם על Azure SQL — השלב הזה לא רלוונטי לכם. אין מה לעשות.**
>
> בסיס הנתונים שלכם משותף מהרגע הראשון: כולם עבדו מולו לאורך כל השיעור, וגם אפליקציית
> ה-C# כבר מחוברת אליו (שלב 5.2). דלגו לשלב הבא.

**השלב הזה נועד לקבוצות שעבדו על SQL Server מקומי.** במסלול המקומי לכל חבר קבוצה יש
בסיס נתונים נפרד משלו, ולכן אי אפשר להדגים את המערכת על נתונים משותפים ואי אפשר לעבוד
באמת ביחד על אותם נתונים. השלב הזה מעביר את הקבוצה לבסיס נתונים אחד משותף.

### היעד: Azure SQL Database

הדרך למעבר היא בדיוק אותה הקמה שמתוארת ב-[`PREREQS`](./PREREQS.md) חלק ג׳, מסלול א׳:
חבר קבוצה אחד יוצר בסיס נתונים ב-Azure SQL ומשתף עם השאר את ארבעת ערכי החיבור
(שרת, בסיס נתונים, שם משתמש, סיסמה). זה חינם עם Azure for Students ולוקח כ-15 דקות.

בצעו את חלק ג׳ מסלול א׳ ב-`PREREQS` (סעיפים ג1–ג3), ואז חזרו לכאן לשלב 11.1.

> **פרטי הגישה משותפים בצ׳אט פרטי בלבד — לעולם לא ב-git.**

### שלב 11.1 — העבירו את המעבר ל-Claude

ברגע שיש בידיכם את ארבעת פרטי החיבור ל-Azure SQL, Prompt אחד עושה את השאר. פתחו את Claude Code, מלאו את ארבעת המצייני מיקום, והדביקו:

> Switch this project to use a shared SQL Server database in addition to my local one.
>
> Shared SQL connection details:
> - Server: `<server>`
> - Database: `<database>`
> - User: `<username>`
> - Password: `<password>`
>
> Steps:
>
> 1. If my current `.mcp.json` already points at the shared server above, leave it alone and say so — skip to step 3. Otherwise rename it to `.mcp.json.local` (so I can switch back) and create a new `.mcp.json` pointing at the shared server, with `MSSQL_SERVER`, `MSSQL_PORT` (1433), `MSSQL_DATABASE`, `MSSQL_USER`, `MSSQL_PASSWORD`, and `MSSQL_ENCRYPT=true`.
>
> 2. Tell me to restart Claude Code so the new MCP config takes effect, then stop and wait. After the restart I will type **continue** in this same chat — when I do, verify the new connection by calling `list_tables` against the shared DB (it should be empty), then carry on from step 3.
>
> 3. Once verified, run `scripts/create_database.sql`, then `scripts/stored_procedures.sql`, then `scripts/seed_data.sql` against the shared DB via the mssql MCP, in that order. Report any errors per script.
>
> 4. Rename `<ProjectName>/app.config` to `app.config.local`. Then create a new `app.config` with a connection string for the shared server using SQL authentication. Format: `Server=<server>;Database=<database>;User Id=<username>;Password=<password>;TrustServerCertificate=True;Encrypt=True;`. The `Encrypt=True` is required for Azure SQL and harmless elsewhere.
>
> 5. Update the "Database" section in `CLAUDE.md` to note that the project now has two targets: local (`.mcp.json.local` / `app.config.local`) and shared (the currently active configs). Document the rename-swap convention to switch between them.
>
> 6. Build the C# project. Then tell me to F5 and confirm the panels show data from the shared DB.

זה כל המעבר. Claude מבצע את שינויי שמות הקבצים, את הרצת הסקריפטים דרך ה-MCP, את האימות ואת עדכון התיעוד. אתם רק מפעילים מחדש את Claude Code ולוחצים F5 פעם אחת.

**אם משהו נכשל:**
- ‏"Cannot connect" או timeout ← Firewall. ב-Azure: בדקו שוב את Networking ← Firewall rules בפורטל; הכלל `0.0.0.0–255.255.255.255` צריך להיות שמור. באוניברסיטה: VPN או מדיניות רשת.
- ‏"Login failed for user" ← שגיאת הקלדה בשם המשתמש או בסיסמה ב-Prompt. נסו שוב.
- סקריפטים נכשלים באמצע ← הריצו אותם שוב; הם אידמפוטנטיים.

### שלב 11.2 — מעבר הלוך ושוב

תרצו להמשיך לפתח ולהתנסות באופן מבודד, ולעבור לסביבה המשותפת רק מדי פעם. כמה אפשרויות:

- **שני קובצי `.mcp.json`**, והחליפו ביניהם לפי הצורך (`.mcp.json.local`,‏ `.mcp.json.shared` — שנו את שם הפעיל ל-`.mcp.json`).
- **שני קובצי `app.config`**, באותה תבנית.
- או פשוט ערכו את הקובץ הפעיל כשצריך לעבור. לקבוצה של 4–5 סטודנטים, גישת העריכה הידנית בדרך כלל עובדת מצוין.

מה שלא תבחרו, **לעולם אל תעלו את `.mcp.json` או את `app.config`** — הם מכילים פרטי גישה. ה-`.gitignore` כבר מכסה אותם.

### מה זה **לא** מכסה

- **כתיבות במקביל מכמה חברי צוות** — הטרנזקציות משלב 7 והתרחישים המתוזמרים משלב 9 מטפלים באטומיות ברמת הפעולה; שום דבר בקורס הזה לא עוסק בנעילה פסימית או במקביליות אופטימית ברמת הטבלה. לקבוצה של 4–5 אנשים שמבצעת בדיקות, קונפליקטים הם נדירים.
- **מיגרציות** — כשחבר צוות מוסיף עמודה, בסיס הנתונים המשותף זקוק לאותו שינוי. המשמעת היא זו שכבר קבענו: שינויי סכמה נכנסים קודם ל-`scripts/create_database.sql`, מבצעים commit ו-push, וחברי הצוות מושכים ומריצים מחדש את הסקריפטים מול היעדים שלהם.
- **גיבוי של בסיס הנתונים המשותף** — Azure SQL מטפל בזה אוטומטית. אל תסתמכו על המחשבים של חברי הצוות כגיבוי.

---

## מה באמת מחוץ להיקף

מעבר לשלב 10, העבודה שנותרת בפרויקט סטודנטים כבר אינה מעקב אחר תבנית — אלה החלטות ספציפיות לפרויקט ועניינים תפעוליים:

- **פריסה לסביבת ייצור** — הפרדה בין סביבות פיתוח, בדיקות וייצור, וניהול מחרוזות חיבור לכל סביבה.
- **שגרת git קבוצתית** — ענפים, פתרון קונפליקטים, Pull Requests. ראו את [`GIT_GROUP_WORKFLOW`](./GIT_GROUP_WORKFLOW.md) למדריך הבסיסי.
- **הכנת ההגשה** — README, סרטון הדגמה, מסמך הליכה על המערכת.
- **כוונון ביצועים**, אם איכשהו תייצרו מספיק נתונים כדי שיהיה בזה צורך (לא סביר בפרויקט לימודי).

סטודנטים שהפנימו את שלבים 1–7 יעברו את שלבים 8–10 במהירות. סטודנטים שדילגו על שלבי הבדיקה יבלו את שארית הסמסטר באיתור שגיאות שקטות שהצטברו.

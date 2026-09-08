---
title: "מדריך הכנה לפני השיעור"
subtitle: "כל מה שצריך להתקין ולאמת לפני המפגש"
course: "ניתוח ועיצוב מערכות מידע — אוניברסיטת בן-גוריון, הנדסת תעשייה וניהול"
author: "מרצה: דוד קודיש"
lang: he
dir: rtl
---

# דרישות קדם — הכנה לשיעור

כל מה שבמסמך הזה חייב להיות מותקן ועובד **לפני** השיעור. ביצוע ההתקנות בכיתה מבזבז את הזמן של כולם וחוסם את שאר חברי הקבוצה שלכם.

**זמן משוער:** כ-45 דקות אם שום דבר לא נכשל. תכננו שעה ליתר ביטחון. הפעילו מחדש את המחשב לפני שאתם מתחילים.

## מה אנחנו מתקינים, ולמה

לפני שמתחילים — כדאי להבין מה כל כלי עושה. אתם לא מתקינים רשימה אקראית;
כל פריט ברשימה ממלא תפקיד מוגדר בשיעור:

| הכלי | לְמה הוא משמש |
|---|---|
| **VSCode** | העורך שבתוכו רץ Claude Code. כאן תעבדו כמעט כל השיעור. |
| **Claude Code** | הסוכן עצמו. הוא קורא את המסמכים שלכם, כותב את הקוד, ומריץ את ה-SQL. |
| **מנוי Claude Pro/Max** | בלעדיו הסוכן לא פועל. אין גרסה חינמית. |
| **Visual Studio** | לבנייה והרצה של אפליקציית ה-WinForms (F5), ולמעצב המסכים הגרפי. **לא** נערוך בו קוד — את זה Claude עושה. |
| **.NET 8 SDK** | מה שמקמפל את קוד ה-C#. |
| **.NET 8 Desktop Runtime** | מה שמריץ את האפליקציה אחרי שנבנתה. נפרד מה-SDK, וקל לשכוח אותו. |
| **בסיס נתונים** | Azure SQL או SQL Server מקומי — שם יישמרו הטבלאות והנתונים שלכם. |
| **SSMS** | כלי גרפי להצצה בבסיס הנתונים בעיניים שלכם. שימושי לאימות שמה ש-Claude עשה באמת קרה. |
| **Git + GitHub** | ניהול גרסאות ושיתוף בין חברי הקבוצה. הקוד הוא מקור האמת, לא בסיס הנתונים. |
| **uv** | מריץ חבילות Python. בשיעור הוא יפעיל את הרכיב שמחבר את Claude לבסיס הנתונים. |

**התמונה הגדולה:** VSCode ו-Claude Code הם מקום העבודה; Visual Studio ו-.NET בונים
ומריצים; בסיס הנתונים ו-SSMS מחזיקים ומציגים את הנתונים; Git שומר הכול ומשתף.

---

## הגישה: קודם מקימים את Claude, ואז נותנים לו לעשות את השאר

התקנת כלי פיתוח תמיד מזמנת הפתעות — בעיות PATH, תלויות חסרות, התנגשויות גרסאות, ושגיאות התקנה סתומות. במקום להיאבק בזה לבד, תתקינו **רק שני דברים ידנית** (VSCode ו-Claude Code), ואז **תנו ל-Claude להתקין ולפתור את כל השאר**. כשמשהו נכשל, Claude נמצא שם כדי לקרוא את השגיאה ולתקן אותה, במקום שתחפשו stack traces בגוגל.

אז זרימת העבודה היא:

1. **חלק א׳ — הקמה בסיסית (ידני):** VSCode + Claude Code + המנוי שלכם ל-Claude. כ-10 דקות.
2. **חלק ב׳ — התקנה בעזרת Claude:** מדביקים Prompt אחד; Claude מתקין ומאמת את כל השאר (Git, uv, .NET, Visual Studio, SSMS), ופותר תקלות תוך כדי.
3. **חלק ג׳ — הקמת בסיס הנתונים:** שני מסלולים אפשריים — **Azure SQL בענן (מומלץ)** או **SQL Server Express מקומי**. בחרו מסלול אחד לכל הקבוצה.
4. **חלק ד׳ — סיום ידני:** אימות החיבור מ-SSMS לבסיס הנתונים שבחרתם.

אם אתם מעדיפים להתקין הכול ידנית, **נספח ההתקנה הידנית** בסוף המסמך מכיל את כל השלבים לכל כלי.

---

# חלק א׳ — הקמה בסיסית (בצעו ידנית)

## א1. VSCode

הורדה: <https://code.visualstudio.com> — בחרו את מתקין ה-Windows (גרסת ה-user מספיקה).

התקינו עם הגדרות ברירת המחדל. כשהמתקין שואל על "Additional Tasks", סמנו:

- ✅ **Add "Open with Code" action to Windows Explorer file/directory context menu**
- ✅ **Add to PATH**

פתחו את VSCode פעם אחת כדי לוודא שהוא עולה.

## א2. מנוי Claude Pro או Max

‏Claude Code כלול גם ב-Pro וגם ב-Max — אין צורך ברכישה נפרדת. **אין גרסה חינמית** של הסוכן; בלי מנוי, שום דבר בקורס הזה לא יעבוד.

1. היכנסו ל-<https://claude.ai>.
2. הירשמו או התחברו.
3. פרופיל ← **Upgrade** ← **Pro** (מספיק) או **Max** (מגבלות גבוהות יותר).
4. השלימו את התשלום.

## א3. תוסף Claude Code והתחברות

1. ב-VSCode, פתחו Extensions‏ (Ctrl+Shift+X).
2. חפשו **"Claude Code"** (מפורסם על ידי Anthropic) ← **Install**.
3. פתחו את פאנל Claude Code (אייקון Claude בסרגל הצד, או Ctrl+Shift+P ← "Claude Code: Sign In").
4. התחברו עם **אותו חשבון** שבו רכשתם את מנוי ה-Pro/Max. אשרו בדפדפן כשתתבקשו.

**אימות:** שלחו את ההודעה הבאה ב-Claude Code:

> hello, are you connected?

אתם אמורים לקבל תשובה. אם מופיע `no active subscription`, המתינו דקה-שתיים אחרי הרכישה, ואז התנתקו והתחברו מחדש.

ברגע ש-Claude עונה, ההקמה הבסיסית הושלמה. בכל השאר Claude יעזור לכם.

---

# חלק ב׳ — תנו ל-Claude להתקין את השאר

פתחו תיקייה ב-VSCode (בשלב הזה כל תיקייה ריקה מתאימה — את תיקיית הפרויקט האמיתית תיצרו במהלך השיעור). בפאנל של Claude Code, הדביקו את ה-Prompt הבא:

> Help me install and verify the development tools for a course. I'm on Windows. For each tool, first check whether it's already installed (and the right version); if not, install it — prefer `winget` and run the command for me, then verify. If a tool needs a GUI installer you can't drive, give me the exact steps and wait while I do it, then help me verify and troubleshoot. Work through them one at a time and tell me the status of each.
>
> The tools:
> 1. **Git** — any recent version.
> 2. **uv** (Astral's Python runner — package id `astral-sh.uv`). Verify `uvx --version` works.
> 3. **.NET 8 SDK** — verify `dotnet --version` reports 8.x.
> 4. **.NET 8 Windows Desktop Runtime** — separate from the SDK. Verify `dotnet --list-runtimes` shows `Microsoft.WindowsDesktop.App 8.x`.
> 5. **Visual Studio Community** with the ".NET desktop development" workload — **2022 or 2025, either is fine**. If I already have Visual Studio 2022 installed, do NOT make me upgrade: just verify the ".NET desktop development" workload is present and add it via the VS Installer if it is missing. Only install 2025 if I have no Visual Studio at all. This is a large GUI install — guide me and help me pick the right workload.
> 6. **SQL Server Management Studio (SSMS)** — any recent version works (19, 20, 21…). We use it to connect to the course database. If SSMS is already installed from a previous course, just verify it opens — do not reinstall or upgrade it.
> 7. **SQL Server Express** — ONLY if I tell you we are taking the local route. Ask me first: our group is either using Azure SQL in the cloud (in which case skip this entirely) or a local SQL Server Express instance. If I confirm the local route, first check whether SQL Server is ALREADY installed (`Get-Service MSSQL*` and the registry key `HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL`) and list what you find. Do not install a second instance if a usable one already exists — tell me which existing instance to use instead.
>
> After each install, run the verification command and tell me if it passed. If anything fails, diagnose it before moving on. At the end, give me a summary table of what's installed and what (if anything) still needs my attention.

‏Claude יעבור על הרשימה, יריץ פקודות `winget`, יבדוק גרסאות, ויפתור כל תקלה שתצוץ. תנו לו להוביל, וענו על השאלות שלו כשהוא שואל.

**מה Claude יכול ומה לא יכול לעשות כאן:**

- ✅ להתקין Git,‏ uv,‏ .NET SDK ו-.NET Desktop Runtime דרך `winget` ולאמת אותם.
- ✅ לקרוא הודעות שגיאה ולתקן בעיות PATH, גרסאות ותלויות.
- ⚠️ ל-Visual Studio ול-SSMS יש מתקינים גרפיים גדולים. Claude יכול להפעיל אותם דרך `winget` או להנחות אתכם בהורדה, אבל אתם תלחצו על אשף ההתקנה. Claude יעזור לאמת את התוצאה.

---

# חלק ג׳ — הקמת בסיס הנתונים

יש שני מסלולים אפשריים, ושניהם עובדים לאורך כל הקורס. **בחרו מסלול אחד לכל הקבוצה** — לא כל אחד לחוד.

| | **מסלול א׳ — Azure SQL (מומלץ)** | **מסלול ב׳ — SQL Server מקומי** |
|---|---|---|
| בסיס נתונים משותף לכל הקבוצה | ✅ כן — כולם רואים את אותם נתונים | ❌ לא — לכל אחד עותק נפרד |
| התקנה מקומית | אין (רק SSMS) | התקנה כבדה של SQL Server Express |
| עבודה מהבית / מהאוניברסיטה | עובד מכל מקום | רק מהמחשב שעליו הותקן |
| הקמה | ~15 דקות בפורטל, פעם אחת לקבוצה | ~20–30 דקות, לכל אחד בנפרד |
| עלות | חינם (Azure for Students) | חינם |
| תקלות אופייניות | כלל Firewall שלא נשמר | TCP/IP מכובה, שם Instance שגוי |

**למה Azure מומלץ:** בפרויקט קבוצתי כולם צריכים לעבוד מול אותם נתונים. במסלול המקומי לכל חבר קבוצה יש בסיס נתונים משלו, ולכן צריך להריץ מחדש את הסקריפטים אצל כל אחד ואי אפשר להדגים את המערכת על נתונים משותפים. בנוסף, ההתקנה המקומית היא מקור התקלות הנפוץ ביותר בשיעור (TCP/IP מכובה כברירת מחדל, שמות Instance שונים בין מחשבים).

**מתי בכל זאת לבחור במסלול המקומי:** אם אין לכם גישה ל-Azure for Students, אם כבר יש לכם SQL Server Express מותקן ועובד מקורס קודם, או אם אתם מעדיפים לא לעבוד מול הענן.

> **בשני המסלולים התוצאה זהה:** בידיכם בסיס נתונים שאפשר להתחבר אליו גם מ-SSMS וגם מ-Claude Code. כל שאר השיעור זהה — רק פרטי החיבור שונים.

**מתי לבצע:** לפני השיעור, במסגרת הכנת דרישות הקדם. בסיס הנתונים חייב להתקיים כבר בתחילת השיעור — בשלב 1 נחבר אליו את Claude Code.

---

# מסלול א׳ — Azure SQL (מומלץ)

בסיס הנתונים המשותף של הקבוצה יושב על Azure SQL — שרת SQL חינמי בענן שעובד בדיוק כמו ה-SQL Server המקומי שאתם מכירים מקורסים קודמים, אבל לא דורש שום התקנה מקומית ונגיש לכל חברי הקבוצה בלי VPN ובלי הגדרות רשת.

**מי מבצע:** חבר קבוצה אחד יוצר את בסיס הנתונים. כל השאר מקבלים את פרטי החיבור בשלב ג2.

## ג1 — הרשמה ל-Azure for Students (רק יוצר בסיס הנתונים)

1. היכנסו ל-<https://azure.microsoft.com/en-us/free/students/>
2. לחצו **Start free** והתחברו עם המייל המוסדי שלכם (‎.ac.il).
3. מיקרוסופט מאמתת את מעמד הסטודנט לפי דומיין המייל. **אין צורך בכרטיס אשראי.** אם מבקשים מכם כרטיס — אתם בעמוד הלא נכון; אתם צריכים את מסלול הסטודנטים.
4. אחרי האימות יהיה לכם חשבון Azure חינמי עם 100$ קרדיט (מתחדש שנתית) וגישה ל-Azure SQL בשכבה החינמית.

## ג2 — יצירת בסיס הנתונים ב-Azure SQL

בפורטל של Azure‏ (portal.azure.com):

1. שורת החיפוש העליונה ← **SQL databases** ← **+ Create**
2. מלאו:
   - **Resource group:** לחצו "Create new" ← קראו לו `sad-groupname-rg`
   - **Database name:** בחרו שם תיאורי (למשל `sharona_pilates`). **רשמו אותו.**
   - **Server:** לחצו "Create new":
     - **Server name:** חייב להיות ייחודי גלובלית, למשל `sad-groupname-sql`. זה יהפוך ל-`<servername>.database.windows.net` — **רשמו אותו.**
     - **Location:** West Europe (או האזור הקרוב ביותר).
     - **Authentication:** בחרו SQL authentication.
     - **Admin login:** בחרו שם משתמש (**לא** `admin` ולא `root` — הם חסומים). **רשמו אותו.**
     - **Password:** בחרו סיסמה חזקה. **רשמו אותה.**
     - לחצו **OK**.
3. **Compute + storage:** לחצו **Configure database** ← בחרו **General Purpose Serverless** וסמנו את תיבת ה-**free offer**.
4. **לשונית Networking:** Connectivity method: Public endpoint. הגדירו **"Allow Azure services"** ← Yes.
5. **Review + create** ← **Create**. המתינו כ-3 דקות.
6. בסיום הפריסה לחצו **Go to resource** ← לחצו על קישור **שם השרת** ← **Networking** בסרגל הצד ← תחת Firewall rules לחצו **+ Add a firewall rule**: שם הכלל `allow-all`, ‏Start IP‏ `0.0.0.0`, ‏End IP‏ `255.255.255.255`. לחצו OK ואז **Save**.

עכשיו יש בידיכם ארבעה ערכים לשיתוף עם חברי הצוות (שתפו בצ׳אט פרטי — **לעולם לא ב-git**):

- **Server:** `<servername>.database.windows.net`
- **Database:** שם בסיס הנתונים
- **User:** ה-admin login
- **Password:** סיסמת ה-admin

## ג3 — אימות החיבור מ-SSMS

1. פתחו את SSMS.
2. התחברו:
   - **Server Name:** `<servername>.database.windows.net`
   - **Authentication:** SQL Server Authentication
   - **Login:** ה-admin login
   - **Password:** סיסמת ה-admin
   - תחת Options ← סמנו **Trust Server Certificate** ✅ וגם **Encrypt connection** ✅
3. פתחו שאילתה חדשה והריצו:

```sql
SELECT @@VERSION;
SELECT name FROM sys.databases;
```

**התוצאה הצפויה:** פרטי גרסה שמציינים Azure SQL, ובסיס הנתונים שלכם ברשימה. אם אתם מקבלים timeout בחיבור — בדקו שוב את כלל ה-Firewall בשלב ג2.

אם בחרתם במסלול א׳ — סיימתם את חלק ג׳. דלגו לסעיף **ג4 — חשבון GitHub**.

---

# מסלול ב׳ — SQL Server Express מקומי

בצעו את המסלול הזה **רק אם לא בחרתם ב-Azure**. כאן כל חבר קבוצה מתקין מופע SQL Server משלו, ולכל אחד יהיה עותק נפרד של בסיס הנתונים.

## ג1-מ — קודם כול: לבדוק אם כבר מותקן אצלכם SQL Server

**אל תתקינו לפני שבדקתם.** בשיעור הקודם כמה סטודנטים לא ידעו שכבר מותקן אצלהם
SQL Server מקורס קודם, התקינו מופע (Instance) שני, ואז לא ידעו לאיזה מהם להתחבר.
מופע שני לא מזיק, אבל הוא יוצר בלבול: לכל מופע יש בסיסי נתונים משלו, ואם תתחברו
לזה הלא נכון — הטבלאות שלכם פשוט "ייעלמו".

הדביקו ב-Claude Code:

> Check whether SQL Server is already installed on this Windows machine, before I install anything. List every installed instance with its service name, instance name, version, and whether the service is currently running — use `Get-Service MSSQL*` and the registry key `HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL`. Then tell me clearly: do I already have a usable SQL Server instance, or do I need to install one? If I already have one, do NOT tell me to install another — recommend which existing instance to use and why. If I have several, explain the difference between them and recommend one.

- **אם Claude מצא מופע קיים** — מצוין, אל תתקינו כלום. עברו לשלב ג2-מ.
- **אם לא נמצא מופע** — התקינו לפי השלבים הבאים.

### התקנה (רק אם אין לכם מופע קיים)

> **אל תנסו להתקין עם `winget install`.** בדקנו — המתקין של SQL Server הוא "מוריד"
> שפותח חלון גרפי, ו-winget לא מצליח להריץ אותו בשקט. התוצאה היא כישלון עם
> `Access to the path ... is denied` או התקנה שנתקעת. השתמשו באחת משתי הדרכים למטה.

#### דרך א׳ — התקנה גרפית (הפשוטה)

1. היכנסו ל-<https://www.microsoft.com/en-us/sql-server/sql-server-downloads>
2. גללו ל-**Express** ולחצו **Download now**
3. הריצו את קובץ ההתקנה ובחרו **Basic** — האפשרות הפשוטה והמהירה ביותר
4. בסיום ההתקנה יוצג מסך סיכום עם **Instance Name** (בדרך כלל `SQLEXPRESS`).
   **רשמו את שם ה-Instance** — תצטרכו אותו בכל שאר המדריכים.

#### דרך ב׳ — התקנה אוטומטית עם Claude (מומלץ: חוסכת את שלב ג2-מ)

הדרך הזו מפעילה את TCP/IP כבר בזמן ההתקנה, כך שתצטרכו רק לקבוע פורט קבוע אחר כך.
הדביקו ב-Claude Code:

> Install SQL Server 2022 Express unattended on this Windows machine. Do NOT use `winget install` for it — its bootstrapper opens a GUI and winget cannot drive it silently.
>
> Instead: download `https://download.microsoft.com/download/5/1/4/5145fe04-4d30-4b85-b0d1-39533663a2f1/SQL2022-SSEI-Expr.exe` to `C:	emp\sqlsetup`, run it with `/Action=Download /MediaPath=C:	emp\sqlmedia /MediaType=Core /Quiet` to fetch the full media, extract the resulting `SQLEXPR_x64_ENU.exe` with `/Q /X:C:	emp\sqlextract`, then run `C:	emp\sqlextract\setup.exe` **elevated** with these arguments:
>
> `/Q /ACTION=Install /FEATURES=SQLEngine /INSTANCENAME=SQLEXPRESS /SQLSYSADMINACCOUNTS="<my Windows user>" /TCPENABLED=1 /UPDATEENABLED=0 /IACCEPTSQLSERVERLICENSETERMS`
>
> Ask me for my Windows username first (`whoami`) and use it for `/SQLSYSADMINACCOUNTS`, otherwise I will not be able to log in. Launch the elevated step with `Start-Process ... -Verb RunAs` and tell me to approve the UAC prompt. When setup finishes, report the exit code and confirm the `MSSQL$SQLEXPRESS` service is running.

**זמן צפוי:** כ-10 דקות, רובן הורדה (כ-270MB).

> **גם אחרי `/TCPENABLED=1` עדיין צריך את שלב ג2-מ.** ההתקנה מפעילה TCP/IP, אבל משאירה
> **פורט דינמי** שמשתנה בכל הפעלה מחדש של השירות — וכתובת שמשתנה שוברת את `.mcp.json`.
> שלב ג2-מ קובע פורט **קבוע** (14330). בדקנו את זה על מחשב נקי: אחרי ההתקנה הפורט היה
> 51466, ורק אחרי קביעת הפורט הקבוע החיבור היה יציב.

> **מהו "Instance"?** אפשר להתקין כמה עותקים נפרדים של SQL Server על אותו מחשב.
> כל עותק כזה נקרא Instance, יש לו שם משלו (`SQLEXPRESS`,‏ `SQLEXPRESS01`,‏ `SQLEXPRESS02`…),
> והוא מחזיק **בסיסי נתונים נפרדים משלו**. לכן חשוב לדעת באיזה מופע אתם עובדים —
> טבלה שיצרתם במופע אחד לא תופיע באחר.

## ג2-מ — הפעלת TCP/IP (כדי ש-Claude יוכל להתחבר לבסיס הנתונים)

**למה זה נדרש:** בהמשך השיעור Claude Code יתחבר לבסיס הנתונים בעצמו ויריץ עליו SQL
— יצירת טבלאות, הרצת שאילתות וכו׳. הכלי שהוא משתמש בו לשם כך יודע לדבר רק בפרוטוקול
**TCP**, ומופעי SQL Express מגיעים עם TCP/IP **מכובה כברירת מחדל**.

התוצאה המבלבלת: **SSMS יתחבר בלי בעיה, אבל Claude לא יצליח** — כי SSMS משתמש בערוץ
פנימי אחר (Shared Memory) שלא דורש TCP. לכן חשוב לבצע את השלב הזה עכשיו, גם אם
החיבור מ-SSMS עובד לכם מצוין.

הדביקו ב-Claude Code:

> Enable TCP/IP on my local SQL Server instance so a TCP-based client can connect to it.
>
> First, discover the instances yourself — do not ask me for the name. Read `HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL` and run `Get-Service MSSQL*` to list every instance and its service state.
>
> If there is exactly one instance, use it. If there is more than one, show me a short table of them (instance name, version, running or stopped) and recommend which to use — prefer one that is running and is a SQL Server Express edition — then ask me to confirm before changing anything.
>
> For the chosen instance, check TCP/IP under `HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\<instance key>\MSSQLServer\SuperSocketNetLib\Tcp`. If it is disabled, enable it, set a static port of 14330 on IPAll, clear TcpDynamicPorts, and restart that instance's service. This needs an elevated PowerShell — launch it with `Start-Process powershell.exe -Verb RunAs` and tell me to approve the UAC prompt.
>
> When you are done, tell me three things I need to write down: the instance name, the port, and confirmation that the service is running.

## ג3-מ — אימות החיבור מ-SSMS

1. פתחו את SSMS.
2. התחברו:
   - **Server Name:** `localhost\SQLEXPRESS` (או שם ה-Instance שלכם)
   - **Authentication:** Windows Authentication
   - סמנו **Trust Server Certificate** ✅
3. פתחו שאילתה חדשה והריצו:

```sql
SELECT @@VERSION;
```

**התוצאה הצפויה:** פרטי גרסה של SQL Server. אם ההתחברות נכשלת, ודאו שהשירות רץ:
`Get-Service MSSQL*` ב-PowerShell — הוא אמור להיות **Running**.

ארבעת ערכי החיבור שלכם במסלול הזה: השרת הוא `localhost\<instance>`, בסיס הנתונים
ייווצר בשלב 4 של השיעור, והאימות הוא Windows Authentication (בלי שם משתמש וסיסמה).

---

## ג4 — חשבון GitHub (חובה לכל חבר קבוצה)

אתם עובדים בקבוצות, ולקבוצה שלכם דרוש מקום משותף לניהול גרסאות הפרויקט — סקריפטי הסכמה, קוד ה-C# והמסמכים. ברירת המחדל היא GitHub.

> **שימו לב להבדל:** את **חשבון ה-Claude** אתם עשויים לחלוק בקבוצה, אבל **חשבון GitHub הוא אישי לכל אחד**.
> אלה שני דברים נפרדים לגמרי: Claude הוא הכלי שכותב את הקוד, ו-GitHub הוא המקום שבו הקוד נשמר.
> ההתחברות ל-GitHub נשמרת **על המחשב שלכם**, לא בתוך Claude — ולכן כל אחד מכם מבצע את שלב ג4-3 על המחשב שלו,
> עם חשבון ה-GitHub שלו, גם אם כולכם מחוברים לאותו חשבון Claude.

- שכפול של repository הדוגמה של הקורס **לא** דורש חשבון (הוא ציבורי).
- אבל העלאת קוד ל-repository של הקבוצה, או הוספה כ-collaborator, **כן** דורשת חשבון.
- שיטת העבודה של "מקור האמת נמצא ב-git" (שינויי סכמה נכנסים לקובצי `.sql`, מבצעים commit, חברי הצוות מושכים ומריצים מחדש) עובדת רק כשיש remote משותף.

### ג4-1 — כל חבר קבוצה פותח חשבון משלו

היכנסו ל-<https://github.com> ולחצו **Sign up**. חינם.

**רשמו לעצמכם את שם המשתמש (username) שבחרתם** — חבר הקבוצה שיוצר את ה-repository
יצטרך את שמות המשתמש של כולם כדי להזמין אתכם.

### ג4-2 — חבר אחד יוצר את ה-repository ומזמין את השאר

**רק חבר קבוצה אחד מבצע את זה.** הוא יהיה הבעלים של ה-repository.

1. ב-GitHub לחצו על ה-**+** בפינה הימנית העליונה ← **New repository**
2. **Repository name:** למשל `sad-groupname`
3. סמנו **Private** (הפרויקט שלכם לא צריך להיות ציבורי)
4. **אל תסמנו** "Add a README file" — אתם כבר תעלו קבצים משלכם
5. לחצו **Create repository**
6. העתיקו את הכתובת שמופיעה (למשל `https://github.com/username/sad-groupname.git`) ושתפו אותה עם הקבוצה

**עכשיו מזמינים את שאר חברי הקבוצה:**

7. בתוך ה-repository, לחצו על **Settings** (בסרגל העליון של ה-repository, לא של החשבון)
8. בתפריט הצד השמאלי לחצו **Collaborators**
9. ייתכן שתתבקשו להזין שוב את הסיסמה שלכם — זו בדיקת אבטחה רגילה
10. לחצו על הכפתור **Add people**
11. הקלידו את **שם המשתמש ב-GitHub** של חבר הקבוצה (לא את המייל שלו, אלא אם הוא רשום איתו), בחרו אותו מהרשימה ולחצו **Add to this repository**
12. חזרו על שלבים 10–11 לכל חבר קבוצה

### ג4-3 — כל שאר החברים מאשרים את ההזמנה

**זה השלב שהכי הרבה אנשים מפספסים.** הזמנה שלא אושרה = אין גישה.

כל מי שהוזמן צריך:

1. להיכנס למייל שאיתו נרשם ל-GitHub ולחפש הודעה בנושא *"invited you to collaborate"*
2. ללחוץ על **Accept invitation**

לחלופין, אפשר לאשר ישירות דרך <https://github.com/notifications> או בכתובת
`https://github.com/<owner>/<repo>/invitations`.

### ג4-4 — כל חבר מגדיר את ההזדהות על המחשב שלו

כדי שתוכלו להעלות קוד (push), המחשב שלכם צריך להוכיח ל-GitHub מי אתם.
הדרך הפשוטה ביותר היא **GitHub CLI** — כלי שורת פקודה של GitHub שמבצע את כל תהליך
ההתחברות דרך הדפדפן, בלי סיסמאות ובלי מפתחות SSH.

**תנו ל-Claude לעשות את זה עבורכם.** הדביקו ב-Claude Code:

> Set up GitHub authentication on this Windows machine so I can push to a private repository. Install GitHub CLI with `winget install GitHub.cli` if it is not already installed. Then run `gh auth login` for me and walk me through the prompts — I want to authenticate through the browser. Tell me exactly what to click. Afterwards, run `gh auth status` to confirm it worked, and set my git identity with `git config --global user.name` and `git config --global user.email` — ask me for the name and the email address I used for my GitHub account. Finally confirm which GitHub user I am authenticated as.

**מה יקרה בפועל:** ייפתח לכם הדפדפן עם קוד בן שמונה תווים. תדביקו את הקוד, תאשרו,
והחלון ייסגר. מכאן והלאה כל `git push` מהמחשב הזה יעבוד בלי סיסמה.

> **חשוב — הזדהות אישית:** ודאו ש-Claude מגדיר את **השם והמייל שלכם** ב-`git config`,
> ולא של חבר קבוצה אחר. זה מה שקובע את שם הכותב שיופיע לצד כל commit, וכך רואים
> בהיסטוריה מי עשה מה. אם כולכם עובדים תחת אותו חשבון Claude, קל לפספס את זה.

**אם אתם מעדיפים לעשות זאת ידנית:** פתחו PowerShell (מקש **Windows** ← הקלידו
`powershell` ← **Enter**) והריצו:

```powershell
winget install GitHub.cli
gh auth login
git config --global user.name "השם שלכם"
git config --global user.email "המייל שלכם ב-GitHub"
gh auth status
```

ב-`gh auth login` בחרו: **GitHub.com** ← **HTTPS** ← **Yes** (אימות עם פרטי ה-git) ←
**Login with a web browser**. העתיקו את הקוד שמוצג, לחצו Enter, ואשרו בדפדפן.

---

# אימות סופי

**אל תריצו את הבדיקות ידנית — תנו ל-Claude לעשות את זה.** זה בדיוק הרעיון של הקורס:
לא להעתיק שגיאות הלוך ושוב, אלא לתת לסוכן להריץ, לאבחן ולתקן.

פתחו את Claude Code והדביקו:

> Verify my course development environment is ready, then fix anything that is broken. Work through this checklist one item at a time, run the commands yourself, and report a pass/fail table at the end.
>
> 1. `dotnet --version` — must report 8.x
> 2. `dotnet --list-runtimes` — must include `Microsoft.WindowsDesktop.App 8.x`
> 3. `git --version` — any 2.x
> 4. `uvx --version` — any 0.x
> 5. Visual Studio is installed (2022 or 2025 are both fine) and has the ".NET desktop development" workload
> 6. SSMS is installed
>
> Important: run each command in a **fresh** shell so PATH changes from recent installs are picked up. If a command is missing only because PATH has not refreshed in your current session, say so rather than reinstalling.
>
> For anything that fails: diagnose the cause and fix it — install the missing piece, repair the PATH, or add the missing Visual Studio workload — then re-run the check to confirm. Ask me before doing anything that needs a large download or an admin prompt.

‏Claude ירוץ על הרשימה, יתקן מה שצריך, ויחזיר טבלת סיכום. אם משהו דורש הורדה גדולה
או הרשאת מנהל — הוא ישאל אתכם קודם.

**מה שנשאר לכם לבדוק ידנית:** שה-SSMS מתחבר לבסיס הנתונים שלכם — ב-Azure (חלק ג3)
או המקומי (חלק ג3-מ), לפי המסלול שבחרתם. את זה Claude לא יכול ללחוץ במקומכם.

---

# צ׳קליסט להביא לשיעור

ביום השיעור:

- [ ] ‏Claude Code מחובר (הודעת בדיקה מקבלת תשובה)
- [ ] ‏Visual Studio נפתח (2022 או 2025)
- [ ] ‏SSMS מתחבר לבסיס הנתונים שבחרתם — Azure (SQL Server Authentication) או מקומי (Windows Authentication)
- [ ] הפקודות `dotnet --version`,‏ `git --version`,‏ `uvx --version` עובדות בטרמינל חדש
- [ ] יש לכם חשבון GitHub ואתם יכולים לבצע push (או בפלטפורמה שהקבוצה בחרה)
- [ ] קובצי ה-PDF של חלק א׳ וחלק ב׳ של הקבוצה נגישים לכם

אם משהו נכשל בבוקר השיעור — כתבו לצ׳אט הקבוצה **מיד**.

---

# נספח: התקנה ידנית (חלופה)

אם אתם מעדיפים להתקין ידנית, או אם ההתקנה בעזרת Claude נתקעה — הנה כל השלבים לכל כלי.

## Visual Studio Community (2022 או 2025)

**שתי הגרסאות עובדות.** אם כבר מותקנת אצלכם Visual Studio 2022 מקורס קודם — אין צורך לשדרג;
רק ודאו שה-Workload‏ **.NET desktop development** מותקן (VS Installer ← Modify).
אם אין לכם Visual Studio בכלל, התקינו את הגרסה החדשה יותר.

<https://visualstudio.microsoft.com/downloads/> ← **Community**. במסך ה-Workloads, סמנו
**.NET desktop development** (כולל את .NET 8 SDK ואת כלי ה-WinForms).
אימות: `dotnet --version` מדווח `8.x.x`.

> **הערה:** מה שקובע הוא ש-.NET 8 נתמך ושה-Workload של WinForms מותקן — לא מספר הגרסה
> של Visual Studio. שתי הגרסאות תומכות ב-.NET 8 ובמעצב ה-WinForms.

## .NET 8 Windows Desktop Runtime

נפרד מה-SDK; ה-SDK בונה אפליקציות, ה-Desktop Runtime מריץ אותן. בלעדיו תקבלו בהפעלה: "You must install or update .NET to run this application".

<https://dotnet.microsoft.com/download/dotnet/8.0> ← תחת **"Run desktop apps"** ← **"Windows Desktop Runtime x64"**. אימות: `dotnet --list-runtimes` מציג `Microsoft.WindowsDesktop.App 8.x.x`.

## SQL Server Express (רק במסלול המקומי)

נדרש **רק** אם הקבוצה בחרה במסלול ב׳. גרסה 2019 ומעלה:
<https://www.microsoft.com/en-us/sql-server/sql-server-downloads> ← **Express** ← **Download now**.
בחרו התקנת **Basic**. רשמו את שם ה-Instance. אם בחרתם ב-Azure — דלגו על זה לגמרי.

אחרי ההתקנה יש להפעיל TCP/IP כדי ש-Claude Code יוכל להתחבר לבסיס הנתונים — ראו חלק ג2-מ.

## SQL Server Management Studio (SSMS)

<https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms>. התקינו עם הגדרות ברירת מחדל. **כל גרסה עדכנית מתאימה** (19, 20, 21) — אם כבר מותקן אצלכם SSMS מקורס קודם, אל תשדרגו, רק ודאו שהוא נפתח. SSMS הוא כלי לקוח בלבד — הוא מתחבר לבסיס הנתונים שלכם, בענן או מקומי. נדרש בשני המסלולים.

## Git

<https://git-scm.com/download/win>. הגדרות ברירת המחדל מתאימות; אשרו "Checkout Windows-style, commit Unix-style" עבור סופי שורות; בחרו VSCode כעורך ברירת המחדל אם מוצע. אימות: `git --version`.

## uv (מריץ Python — ישמש בשיעור לחיבור Claude Code לבסיס הנתונים)

```powershell
winget install astral-sh.uv
```

או `pip install uv` אם כבר יש לכם Python. אימות: `uvx --version`. אם מופיע "command not found" — פתחו את הטרמינל מחדש.

## תוספים ל-VSCode

מתוך Extensions‏ (Ctrl+Shift+X):

- **C# Dev Kit** (Microsoft) — צביעת תחביר ו-IntelliSense לקובצי ה-`.cs` ש-Claude כותב. בלעדיו, קוד C# נראה כמו טקסט רגיל.
- **C#** (Microsoft) — מותקן אוטומטית יחד עם C# Dev Kit.
- **Claude Code** (Anthropic) — כבר הותקן בחלק א׳.

אל תתקינו תוספים "פופולריים" אקראיים — הם עלולים להפריע לכלים של השיעור.

---

# תקלות נפוצות

| תסמין | סיבה סבירה | פתרון |
|---|---|---|
| ‏SSMS: "Cannot connect to `<server>`.database.windows.net" | כלל ה-Firewall לא נשמר | פורטל Azure ← SQL server ← Networking ← ודאו שכלל ה-Firewall‏ 0.0.0.0–255.255.255.255 קיים ונשמר |
| ‏Azure: "Login failed for user '`<username>`'" | שם משתמש או סיסמה שגויים | הזינו מחדש; שם המשתמש **אינו** המייל שלכם ב-Azure — הוא ה-SQL admin login שהגדרתם בשלב ג2 |
| `dotnet` מדווח 7.x או "not found" | ‏.NET 8 SDK לא מותקן | הריצו מחדש את VS Installer ← Modify ← סמנו ".NET desktop development", או `winget install Microsoft.DotNet.SDK.8` |
| האפליקציה הבנויה מציגה "You must install or update .NET" בהפעלה | ה-Desktop Runtime חסר (הותקן רק ה-SDK) | התקינו .NET 8 Windows Desktop Runtime |
| ‏SSMS: "Cannot connect to localhost\SQLEXPRESS" (מסלול מקומי) | שירות ה-SQL Server לא רץ | הריצו `Get-Service MSSQL*`; אם עצור — `Start-Service 'MSSQL$SQLEXPRESS'` ב-PowerShell כמנהל |
| ‏Claude Code לא מצליח להתחבר לבסיס נתונים מקומי, אבל SSMS כן | ‏TCP/IP מכובה במופע | ראו חלק ג2-מ — Claude מתחבר ב-TCP בלבד, ואילו SSMS משתמש בערוץ פנימי (Shared Memory) |
| שם ה-Instance לא מוכר (מסלול מקומי) | הותקן בשם אחר | הריצו `Get-Service MSSQL*` וראו את השם בפועל (למשל `SQLEXPRESS02`) |
| טקסט בעברית חוזר כ-`?????` | העמודה מוגדרת `VARCHAR` ולא `NVARCHAR` | השתמשו תמיד ב-`NVARCHAR` לעברית |
| `uvx: command not found` אחרי ההתקנה | הטרמינל לא רענן את ה-PATH | פתחו מחדש את הטרמינל |
| ‏Claude Code: "no active subscription" | המנוי טרם התעדכן, או חשבון שגוי | המתינו 2 דקות, התנתקו והתחברו מחדש, ודאו שזה החשבון עם המנוי |
| מעצב הממשק של Visual Studio שבור או חסר | גרסת VS מיושנת, או שה-Workload חסר | Help ← Check for Updates. ודאו שה-Workload‏ ".NET desktop development" מותקן (VS Installer ← Modify). גם 2022 וגם 2025 עובדות — אין צורך לשדרג רק בגלל מספר הגרסה |

**לכל אחת מהתקלות האלה:** הדביקו את התסמין ב-Claude Code. יש לו את ההקשר כדי לאבחן, ובדרך כלל הוא פותר מהר יותר מהטבלה.

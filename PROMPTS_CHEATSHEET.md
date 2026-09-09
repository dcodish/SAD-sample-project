---
title: "דף מרוכז — Prompts"
subtitle: "כל ה-Prompts של השיעור, בסדר ההדבקה"
course: "ניתוח ועיצוב מערכות מידע — אוניברסיטת בן-גוריון, הנדסת תעשייה וניהול"
author: "מרצה: דוד קודיש"
lang: he
dir: rtl
---

# לפני שמתחילים

כל ה-Prompts שתדביקו ב-Claude Code במהלך השיעור, לפי הסדר. להסבר ה*למה* מאחורי כל שלב (משמעת בדיקה, סוגי שגיאות, פתרון תקלות) ראו את `LESSON_STEPS`. לדרישות ההתקנה ראו את `PREREQS`.

> **ה-Prompts עצמם נשארים באנגלית בכוונה** — הם נוסחו ונבדקו כך. ההסברים סביבם בעברית. העתיקו כל Prompt כמו שהוא (יש כפתור "העתק" על כל אחד).

**לפני השיעור:** השלימו כל שלב ב-`PREREQS` וודאו ששלוש הפקודות עובדות בטרמינל חדש:

```powershell
dotnet --version       # 8.x
git --version
uvx --version
```

בנוסף, **בסיס הנתונים של הקבוצה** חייב להיות כבר מוקם (`PREREQS` חלק ג׳). בלעדיו שום דבר בשלב 1 לא יעבוד.

### שני מסלולים — בחרו את שלכם

בכל מקום שבו יש הבדל בין המסלולים, מופיעים **שני Prompts נפרדים** מסומנים כך:

- **☁️ מסלול א׳ — Azure SQL** — בסיס נתונים בענן, משותף לכל הקבוצה.
- **💻 מסלול ב׳ — SQL Server מקומי** — מופע על המחשב שלכם.

**הדביקו רק את זה שמתאים למסלול שהקבוצה שלכם בחרה.** בכל שאר המקומות (שלבים 2, 3, 4.1 והלאה) ה-Prompts זהים לחלוטין בשני המסלולים.

מה שצריך להיות בידיכם לפני שלב 1:

| ☁️ מסלול Azure | 💻 מסלול מקומי |
|---|---|
| שם השרת (מסתיים ב-`.database.windows.net`) | שם ה-Instance (למשל `SQLEXPRESS`) |
| שם בסיס הנתונים | הפורט שהגדרתם (למשל `14330`) |
| שם משתמש וסיסמה של SQL | — (אימות Windows, בלי סיסמה) |

# ניהול טוקנים (מנוי Pro)

| מודל | מתי |
|---|---|
| **Sonnet** — `/model sonnet` | **ברירת המחדל לכל השיעור** |
| **Haiku** — `/model haiku` | שלבים מכניים: הרצת SQL, קוד תבניתי, מסכים חוזרים (שלב 6) |
| **Opus** | **לא במנוי Pro** — שורף את המכסה מהר |

**הריצו `/clear` בין שלבים.** זה חינם, ומונע את הבזבוז הגדול ביותר: כל הודעה שולחת
מחדש את **כל** השיחה. `/usage` מראה כמה נשאר.

> **"Context low" זו לא מגבלת מנוי** — רק ההקשר התמלא; `/clear` ותמשיכו.
> **"You've hit your session limit"** — זו כן, והיא משותפת לכל המודלים (מעבר ל-Haiku לא יעזור).
> **"You've hit your Opus limit"** — כאן מעבר ל-Sonnet או Haiku כן מחזיר אתכם לעבודה.

---

# פתיחת Session חדש

כשחלון ההקשר מתמלא או אחרי הפסקה, תפתחו שיחה חדשה — והיא מתחילה בלי זיכרון של השיחה
הקודמת (הקבצים נשארים). משלב 3 והלאה Claude קורא את `CLAUDE.md` לבד. לפני כן,
או כדי למקד אותו מהר, הדביקו:

> I'm continuing a university course project — building a C# WinForms order-management-style system from our own analysis documents, driven entirely from Claude Code.
>
> Orient yourself before doing anything: read `CLAUDE.md` at the project root if it exists, list what's in `docs/`, and check whether `scripts/` and a `.sln` already exist. The folder `cloned/` is the course's sample project — read `cloned/PATTERNS.md` for the architecture conventions we follow, but do not copy its domain content.
>
> Then tell me in a few lines where the project currently stands and what you think the next step is, and wait for me to confirm before changing anything.

---

# שלב 0 — יצירת תיקיית הפרויקט

**בצעו את זה לפני שאתם פותחים את VSCode.** דילוג על השלב הזה הוא טעות ההקמה הנפוצה ביותר.

1. צרו תיקייה **ריקה לחלוטין** — למשל `C:\projects\sad-groupname`.
   לא תיקייה שכבר יש בה קבצים, ולא תיקיית פרויקט הדוגמה המשוכפל.

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
2. צרו בתוכה תת-תיקייה ריקה בשם `docs`.
3. ‏VSCode ← **File ← Open Folder** ← פתחו את תיקיית `sad-groupname` שלכם.

כל מה שלהלן מניח ש-Claude Code פתוח על *התיקייה הזו*.

# שלב 1 — הקמת הפרויקט

העבירו את קובצי ה-PDF של חלק א׳ וחלק ב׳ לתת-התיקייה `docs/` שיצרתם בשלב 0. אחר כך שכפלו את פרויקט הדוגמה — **בקשו מ-Claude, אל תקלידו בטרמינל** (git לרוב לא נמצא ב-PATH של הטרמינל):

> Run this git command to clone the SAD sample project into a subfolder of this project: `git clone https://github.com/dcodish/SAD-sample-project.git cloned`

ואז:

> Create a `.gitignore` file at the root of this project with two entries: `cloned/` and `.mcp.json`

עכשיו מחברים את בסיס הנתונים. **הדביקו רק את ה-Prompt של המסלול שלכם.**

### ☁️ מסלול א׳ — Azure SQL

> Set up the MSSQL MCP server for this project so you can talk to our Azure SQL database. Follow every step.
>
> 1. **Install `uv` if missing.** Run `uvx --version`. If not found, run `winget install astral-sh.uv`. Note: after install, `uvx` will NOT be on this session's PATH. In step 3 you will write the **absolute path** to `uvx.exe` into `.mcp.json`. Find it with: run this — it checks every place uv installs to, because the location varies by uv version:
>    ```powershell
>    $uvx = (Get-Command uvx -EA SilentlyContinue).Source
>    if (-not $uvx) { $uvx = @("$env:USERPROFILE\.local\bin\uvx.exe","$env:LOCALAPPDATA\Microsoft\WinGet\Links\uvx.exe","$env:LOCALAPPDATA\Programs\uv\uvx.exe") | Where-Object { Test-Path $_ } | Select-Object -First 1 }
>    if (-not $uvx) { $uvx = Get-ChildItem "$env:USERPROFILE\.local","$env:LOCALAPPDATA\Microsoft\WinGet\Packages" -Filter uvx.exe -Recurse -EA SilentlyContinue | Select-Object -First 1 -ExpandProperty FullName }
>    $uvx
>    ```
>    Do not search only under `WinGet\Packages` — recent uv versions install to `%USERPROFILE%\.local\bin` and that search finds nothing..
>
> **Use exactly this MCP server — do not substitute a different one.** The package is `microsoft_sql_server_mcp` version `0.1.0`, and it MUST run with `mcp==1.30.0`. Without that pin, uv resolves mcp 2.x and the server dies on startup with `AttributeError: 'Server' object has no attribute 'list_resources'`. Do NOT pin `pymssql` — let it resolve on its own, or you get `ModuleNotFoundError: pymssql._pymssql`. If this server still fails after you have followed every step, STOP and tell me — do not go looking for an alternative MCP server on your own.
>
> 2. **Ask me for the Azure SQL connection details** — server name (ends in `.database.windows.net`), database name, SQL username, and SQL password. Don't guess.
>
> 3. **Create `.mcp.json` at the project root** with this shape, filling in the absolute uvx path and the connection details:
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
> 4. **Patch two known bugs in the cached package.** First trigger the install: `& "<uvx path>" --from "microsoft_sql_server_mcp==0.1.0" --with "mcp==1.30.0" mssql_mcp_server --help` (it will fail with a config error — that's fine). Then find the cached `server.py`: `Get-ChildItem -Path "$env:LOCALAPPDATA\uv\cache\archive-v0" -Filter "server.py" -Recurse | Where-Object { $_.FullName -like "*mssql_mcp_server*" } | Select-Object -ExpandProperty FullName`. Apply two edits:
>    - **Bug A:** replace the line setting `config["encrypt"] = encrypt_str.lower() == "true"` with:
>      ```python
>      if encrypt_str.lower() == "true":
>          config["encryption"] = "request"
>      ```
>    - **Bug B — DDL fails inside an implicit transaction.** The file has **three** `conn = pymssql.connect(**config)` lines. After **each** of them insert `conn.autocommit(True)` **at exactly the same indentation as the `conn = ...` line it follows** (they sit at 8 spaces, inside a `try:`). Getting the indentation wrong gives an `IndentationError` and the server will not start at all. Afterwards, verify the file still parses.
>
> 5. **Add `.mcp.json` to `.gitignore`** — it contains our database password.
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

### 💻 מסלול ב׳ — SQL Server מקומי

> Set up the MSSQL MCP server for this project so you can talk to my local SQL Server instance. Follow every step.
>
> 1. **Install `uv` if missing.** Run `uvx --version`. If not found, run `winget install astral-sh.uv`. Note: after install, `uvx` will NOT be on this session's PATH. In step 3 you will write the **absolute path** to `uvx.exe` into `.mcp.json`. Find it with: run this — it checks every place uv installs to, because the location varies by uv version:
>    ```powershell
>    $uvx = (Get-Command uvx -EA SilentlyContinue).Source
>    if (-not $uvx) { $uvx = @("$env:USERPROFILE\.local\bin\uvx.exe","$env:LOCALAPPDATA\Microsoft\WinGet\Links\uvx.exe","$env:LOCALAPPDATA\Programs\uv\uvx.exe") | Where-Object { Test-Path $_ } | Select-Object -First 1 }
>    if (-not $uvx) { $uvx = Get-ChildItem "$env:USERPROFILE\.local","$env:LOCALAPPDATA\Microsoft\WinGet\Packages" -Filter uvx.exe -Recurse -EA SilentlyContinue | Select-Object -First 1 -ExpandProperty FullName }
>    $uvx
>    ```
>    Do not search only under `WinGet\Packages` — recent uv versions install to `%USERPROFILE%\.local\bin` and that search finds nothing..
>
> **Use exactly this MCP server — do not substitute a different one.** The package is `microsoft_sql_server_mcp` version `0.1.0`, and it MUST run with `mcp==1.30.0`. Without that pin, uv resolves mcp 2.x and the server dies on startup with `AttributeError: 'Server' object has no attribute 'list_resources'`. Do NOT pin `pymssql` — let it resolve on its own, or you get `ModuleNotFoundError: pymssql._pymssql`. If this server still fails after you have followed every step, STOP and tell me — do not go looking for an alternative MCP server on your own.
>
> 2. **Find my SQL Server instance yourself — do not ask me for the name.** Read `HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL` and run `Get-Service MSSQL*`. If there is more than one instance, show me a short table and ask which to use. Then confirm TCP/IP is enabled for it and tell me which static port it is listening on (we set 14330 during setup). If TCP/IP is disabled, stop and tell me — the MCP cannot connect without it.
>
> 3. **Create `.mcp.json` at the project root** with this shape, filling in the absolute uvx path and the port you found:
>    ```json
>    {
>      "mcpServers": {
>        "mssql": {
>          "command": "<ABSOLUTE_PATH_TO_uvx.exe>",
>          "args": ["--from", "microsoft_sql_server_mcp==0.1.0", "--with", "mcp==1.30.0", "mssql_mcp_server"],
>          "env": {
>            "MSSQL_SERVER": "localhost",
>            "MSSQL_PORT": "14330",
>            "MSSQL_DATABASE": "master",
>            "MSSQL_WINDOWS_AUTH": "true",
>            "MSSQL_ENCRYPT": "false"
>          }
>        }
>      }
>    }
>    ```
>    Note: `MSSQL_SERVER` is bare `localhost` with the port given separately — do NOT write `localhost\SQLEXPRESS`, because pymssql cannot resolve named instances. Do NOT add `MSSQL_USER` or `MSSQL_PASSWORD` — Windows Authentication is used instead.
>
> 4. **Patch two known bugs in the cached package.** First trigger the install: `& "<uvx path>" --from "microsoft_sql_server_mcp==0.1.0" --with "mcp==1.30.0" mssql_mcp_server --help` (it will fail with a config error — that's fine). Then find the cached `server.py`: `Get-ChildItem -Path "$env:LOCALAPPDATA\uv\cache\archive-v0" -Filter "server.py" -Recurse | Where-Object { $_.FullName -like "*mssql_mcp_server*" } | Select-Object -ExpandProperty FullName`. Apply two edits:
>    - **Bug A:** replace the line setting `config["encrypt"] = encrypt_str.lower() == "true"` with:
>      ```python
>      if encrypt_str.lower() == "true":
>          config["encryption"] = "request"
>      ```
>    - **Bug B — DDL fails inside an implicit transaction.** The file has **three** `conn = pymssql.connect(**config)` lines. After **each** of them insert `conn.autocommit(True)` **at exactly the same indentation as the `conn = ...` line it follows** (they sit at 8 spaces, inside a `try:`). Getting the indentation wrong gives an `IndentationError` and the server will not start at all. Afterwards, verify the file still parses.
>
> 5. **Add `.mcp.json` to `.gitignore`** — it is a local machine config and should not be shared.
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

אם משהו נכשל, טבלת פתרון התקלות ב-`MCP_SETUP` מכסה את המקרים הנפוצים.

**נקודת Commit:**

> Initialize a git repository here and make the first commit with `.gitignore` and any files created so far.

---

> 🔄 **התחילו Session חדש לפני השלב הזה.** שלב 1 מילא את ההקשר בפלט התקנות. `CLAUDE.md` עדיין לא קיים — השתמשו ב-Prompt מהסעיף "פתיחת Session חדש".

# שלב 2 — חילוץ הניתוח ל-Markdown מובנה

> Read all the PDFs in `docs/`. Extract our group's analysis and design into structured markdown files matching this layout:
>
> Analysis stage:
> - `docs/org-analysis/01-organization.md` — organization description and current information systems. Hebrew.
> - `docs/org-analysis/02-interviews.md` — interview transcripts. Hebrew.
> - `docs/org-analysis/03-problems.md` — problems table. Hebrew.
> - `docs/org-analysis/04-business-processes.md` — descriptions of the existing business processes and any BPMN narratives. Hebrew.
>
> Requirements stage:
> - `docs/00-requirements.md` — user stories table, NFRs, and traceability matrix. English.
> - `docs/00e-use-cases.md` — full use case specifications. English. Use a two-layer format: a behavioral spec section (technology-neutral, no class or stored-procedure names) followed by a clearly labelled "Implementation Notes" section. If the source doesn't have implementation notes yet, leave that section as a TODO placeholder rather than inventing content.
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

**ואז בדקו כל קובץ שחולץ מול ה-PDF.** התעקשו מול Claude כשספירות, טבלאות או ניסוחים סוטים מהמקור.

---

> 🔄 **התחילו Session חדש לפני השלב הזה.** שלב 2 קרא את כל ה-PDF שלכם — זה החלק הכבד ביותר. `CLAUDE.md` עדיין לא קיים — השתמשו ב-Prompt מהסעיף "פתיחת Session חדש".

# שלב 3 — יצירת ה-CLAUDE.md של הפרויקט

> I have my group's project documents in `docs/` — extracted markdown files (organization, problems, interviews, business processes, requirements, use cases, and design diagrams under `docs/design/`) plus the original PDFs as backup. Prefer the markdown files; consult the PDFs only when the markdown is unclear or when you need to look at a diagram.
>
> The folder `cloned/` is the SAD course's sample project. Read `cloned/PATTERNS.md` (shared architecture conventions for all SAD projects) and `cloned/CLAUDE.md` (the sample project, for context only — do not copy its domain content).
>
> Read all the documents in `docs/` and produce a `CLAUDE.md` at the root of this folder for my project. It should be self-contained — inline the architecture conventions from PATTERNS.md verbatim, then add my project's domain based on what you read: what the system is, the entities and load order from the class diagram, the use cases we're implementing, and any decisions our group has already made.
>
> Ask me clarifying questions before writing if anything is ambiguous.

**על מה להתמקד בבדיקה:** היקף המימוש, סדר הטעינה, החלטות הקבוצה, ספירות, וסתירות שקטות בין מסמכים. אל תבזבזו זמן על שמות תכונות וערכי enum — הקומפיילר יתפוס אותם בהמשך.

---

> 🔄 **התחילו Session חדש לפני השלב הזה.** מכאן `CLAUDE.md` נטען אוטומטית — פשוט `/clear` והמשיכו.

# שלב 4 — סכמת בסיס הנתונים ו-Stored Procedures

## 4.0 — בסיס הנתונים של הפרויקט

### ☁️ מסלול א׳ — Azure SQL

בסיס הנתונים כבר קיים ו-`.mcp.json` מצביע ישירות עליו — **אין מה ליצור**. רק לאמת:

> Use the mssql MCP tool's execute_sql to run `SELECT DB_NAME()` and `SELECT @@VERSION`. Tell me the current database name and confirm it matches our project database.

ואז הטמיעו ב-`CLAUDE.md`:

> Add a "Database" section to `CLAUDE.md` stating: the project database is `<your_database_name>` on Azure SQL (`<server>.database.windows.net`); the MCP connects directly to it, so no `USE` statement is needed at the start of a batch; Azure SQL does not support `USE`.

### 💻 מסלול ב׳ — SQL Server מקומי

כאן בסיס הנתונים **עדיין לא קיים** — ה-MCP מחובר ל-`master`. צרו אותו:

> Use the mssql MCP tool's execute_sql to create a database named `<your_project_db_name>` (something descriptive — `sharona_pilates`, `bookstore`, etc.). Then verify it exists with `SELECT name FROM sys.databases WHERE name = '<your_project_db_name>'`.

עכשיו הפנו את ה-MCP לבסיס הנתונים החדש:

> Update `MSSQL_DATABASE` in `.mcp.json` from `master` to `<your_project_db_name>`, then tell me to restart Claude Code and wait. After the restart I will type **continue** — when I do, run `SELECT DB_NAME()` through the mssql MCP and confirm it returns our project database and not `master`.

**הפעילו מחדש את Claude Code**, ואז כתבו בשיחה שנפתחת:

> continue

ואז הטמיעו ב-`CLAUDE.md`:

> Add a "Database" section to `CLAUDE.md` stating: the project database is `<your_project_db_name>` on a local SQL Server instance; the MCP connects directly to it, so no `USE` statement is needed at the start of a batch.

> **למה להפנות את ה-MCP לבסיס הנתונים ולא להשתמש ב-`USE`?** כי אז שני המסלולים
> מתנהגים אותו דבר, וכל שאר ה-Prompts במסמך זהים לחלוטין בשניהם.

## 4.1 — יצירת הסכמה

> Read `docs/design/class-diagram.md` and the load-order section of `CLAUDE.md`. Generate `scripts/create_database.sql` with `CREATE TABLE` statements for every entity, with these defaults:
>
> - One `INT NOT NULL PRIMARY KEY` per table named `<entity>_id`. Do not use `IDENTITY(1,1)` — primary keys are assigned in C# per the Primary Key Strategy in `CLAUDE.md`.
> - `NVARCHAR(50)` for short strings; `NVARCHAR(MAX)` only for long free text.
> - `INT` for counts, `DECIMAL(10,2)` for money, `DATETIME2` for timestamps.
> - All columns `NOT NULL` by default. If a column is genuinely optional in the design, mark it nullable and add a one-line comment.
> - Foreign keys with `ON DELETE NO ACTION ON UPDATE NO ACTION` by default.
> - For enum-like attributes, use `NVARCHAR(20)` with a `CHECK` constraint listing the allowed values. No lookup tables.
> - `UNIQUE` only where the class diagram or requirements explicitly say so.
> - Order the `CREATE TABLE` statements so every FK target exists before its referencing table.
>
> Do not invent columns. If a type or nullability is genuinely ambiguous, leave a `-- TODO: <question>` comment. Write the file but do not run it yet.

## 4.2 — בדיקת הסכמה (ידנית, מול דיאגרמת המחלקות)

## 4.3 — הרצת הסכמה

> Run the contents of `scripts/create_database.sql` against the database via the mssql MCP tool. Execute as one batch. If it fails, tell me which statement failed and the error — don't silently retry. After success, use list_tables to confirm every table exists.

## 4.4 — יצירת ה-Stored Procedures

> Generate `scripts/stored_procedures.sql` with basic CRUD stored procedures for every table. For each entity:
>
> - `sp_<entity>_create` — inserts a row. Takes the primary key as the first parameter (`@<entity>_id`). Does not use `SCOPE_IDENTITY()`.
> - `sp_<entity>_update` — updates by primary key.
> - `sp_<entity>_delete` — deletes by primary key.
> - `sp_<entity>_get_all` — returns all rows.
> - `sp_<entity>_get_by_id` — returns one row by PK.
>
> Parameter names match column names. No business logic — mechanical CRUD only. Do not generate report SPs, state-transition SPs, or multi-table procedures.
>
> Write the file but do not run it yet.

ואז:

> Run `scripts/stored_procedures.sql` via the mssql MCP tool.

## 4.6 — בדיקת ישות אחת מקצה לקצה

> Using execute_sql via the mssql MCP, run `sp_<entity>_create` with realistic test values, then `sp_<entity>_get_all` to confirm it landed, then `sp_<entity>_update` on the new row, then `sp_<entity>_delete`. Report each result.

---

> 🔄 **התחילו Session חדש לפני השלב הזה.** פלט ה-SQL כבר לא נחוץ — `/clear`.

# שלב 4.5 — נתוני דמה

> Generate `scripts/seed_data.sql` with realistic test data for every table, in load order. Guidelines:
> - Real-feeling Hebrew names, real-looking emails, plausible dates. No "Test User 1".
> - Cover every role/status/enum value at least once.
> - For multi-actor systems: at least 2 users per role.
> - ~5–10 rows per base entity, ~10–20 for transactional/association tables.
> - Every FK references an existing row. No orphans.
> - Passwords (if any) like `password123` — throwaway test users.
>
> Write the file, then I'll review before we run it.

ואז:

> Execute `scripts/seed_data.sql` against the project DB via the mssql MCP. Report any errors. Afterward, show me row counts per table.

---

> 🔄 **התחילו Session חדש לפני השלב הזה.** `/clear` לפני השלב הארוך ביותר בשיעור.

# שלב 5 — שלד פרויקט ה-C# והמסך הרץ הראשון

## 5.1 — יצירת השלד

> Look at the C# project structure under `cloned/example_project/`. Create a matching scaffold for my project at the root of this folder — a Visual Studio solution + project, same .NET version and references as the sample, same folder layout. Use my project name (read `CLAUDE.md`). Do not copy any of the sample's *.cs files.
>
> Copy `<NoWarn>CA1416</NoWarn>` from the sample's csproj into ours.

## 5.2 — חיבור בסיס הנתונים

### ☁️ מסלול א׳ — Azure SQL

> Look at how the sample project handles the SQL connection (`cloned/example_project/SQL_CON.cs`). Create the equivalent in our project, reading the connection string from `app.config`. Our database is **Azure SQL**, so use this shape and ask me for the four values:
>
>    `Server=tcp:<server>.database.windows.net,1433;Initial Catalog=<database>;User ID=<user>;Password=<password>;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;`
>
> Do NOT use `Trusted_Connection` or `Integrated Security` — Azure SQL does not support Windows Authentication. Then add `app.config` to `.gitignore`, because it will contain our password.

### 💻 מסלול ב׳ — SQL Server מקומי

> Look at how the sample project handles the SQL connection (`cloned/example_project/SQL_CON.cs`). Create the equivalent in our project, reading the connection string from `app.config`. Our database is a **local SQL Server instance**, so use this shape:
>
>    `Data Source=localhost\<instance name>;Initial Catalog=<database>;Integrated Security=True;TrustServerCertificate=True`
>
> Use the instance name we found earlier (e.g. `SQLEXPRESS`). Note this is deliberately different from `.mcp.json`: the C# SqlClient driver resolves named instances and supports Windows Authentication, so it uses the instance name and no port — unlike the MCP's Python driver, which needs `localhost` plus the static port.

> **למה מחרוזת החיבור שונה מ-`.mcp.json`?** במסלול המקומי, ה-C# משתמש בדרייבר של מיקרוסופט שיודע לפענח שמות Instance ולעבוד עם אימות Windows — ולכן שם כותבים `localhost\SQLEXPRESS` בלי פורט. ה-MCP משתמש בדרייבר Python אחר שלא יודע לעשות את זה, ולכן שם כותבים `localhost` + פורט קבוע. שתי ההגדרות נכונות, כל אחת לכלי שלה.

## 5.3 — יצירת כל מחלקות הישויות

> For every entity in the class diagram, generate `<EntityName>.cs` following the entity pattern in `CLAUDE.md` (and the sample's `cloned/example_project/Worker.cs` as a reference). Use column names and types from `scripts/create_database.sql` and SP names from `scripts/stored_procedures.sql`. Add each entity's static list to `Program.cs` and call its `initXyzs()` in `Program.initLists()` in correct load order.
>
> Do not invent fields. Do not invent SP names. If anything is unclear, ask before writing.

## 5.4 — בדיקת הישויות (ידנית)

## 5.5 — עיצוב ויצירת מסך הכניסה

> Read `docs/design/class-diagram.md`, `docs/00e-use-cases.md`, and the UC and actor information in `CLAUDE.md`. Identify the human actors in this project.
>
> **Login is the first screen** for multi-actor projects. Authentication is not in our requirements — derive the login design from the **class diagram**: find every entity that has credential-like fields (email + password); each one is a login source. Map each credential-holding entity to its corresponding role home.
>
> Decide whether the entry flow should be:
> (a) Login → per-role home panels — when multiple human actors with distinct screens.
> (b) Flat menu on `mainForm` — only when single actor or no credentials anywhere.
>
> Tell me which design you've chosen, name every credential-holding entity and the home panel each routes to, and ask for my confirmation before generating any files.
>
> Once I confirm, generate the entry-flow files in one batch:
> - `mainForm.cs` (+ Designer + resx) — matches the sample's pattern, hosts `showPanel(UserControl)`. Entry point loads `LoginPanel` first.
> - If (a):
>   - `LoginPanel.cs` (+ Designer + resx) — email + password, Hebrew labels, multi-table credential check (iterate every credential-holding entity's `Program.Xs` list), route to the matching role home on success, Hebrew error on failure.
>   - Optional dev shortcut button ("כניסת מפתח") to bypass auth and open a debug panel.
>   - One `<Role>HomePanel.cs` (+ Designer + resx) per role. Placeholder buttons for that role's UCs (read from the UC diagram), wired to `MessageBox.Show("TODO")`.
> - If (b): `mainForm` opens directly to a flat menu of placeholder UC buttons.
>
> Add an "Entry Flow" section to `CLAUDE.md` documenting the design.

## 5.6 — יצירת מסך ה-CRUD הראשון

> Pick `<EntityName>` (a Tier 1 base entity) for the first CRUD panel. Generate `<EntityName>Panel.cs` (UserControl + Designer + resx) — full CRUD: list view, view/edit fields, Save / Update / Delete / Back buttons. Hebrew UI text. Wire each button to the entity's `createXyz / updateXyz / deleteXyz` methods.
>
> Then replace the TODO placeholder button in the appropriate role's home panel (per the "Entry Flow" section in `CLAUDE.md`) so it now calls `mainForm.showPanel(new <EntityName>Panel())`. Back button returns to that role's home.
>
> Match the sample's panel patterns: event-handler shape, return-to-home mechanism, Designer.cs structure.

## 5.7 — הרצה

בנו את הפרויקט (Ctrl+Shift+B). לחצו F5. עברו את המסלול: התחברות ← מסך בית לפי תפקיד ← מסך CRUD ← בצעו פעולות CRUD. ואז:

> Use the mssql MCP to run `SELECT * FROM <entity_table>`. Show me what's in the DB right now.

---

> 🔄 **התחילו Session חדש לפני השלב הזה.** `/clear` לפני יצירת המסכים — שלב 5 השאיר הרבה הקשר.

# שלב 6 — יצירת שאר מסכי ה-CRUD

בחרו מסלול אחד.

## אפשרות א׳ — אחד-אחד (זהירה)

> Generate `<EntityName>Panel.cs` (+ Designer + resx) for `<EntityName>`, following the same pattern as `<AlreadyDonePanel>.cs`. Wire it under `<RoleHomePanel>` (replacing the TODO placeholder). Match the entity pattern, RTL settings, and Hebrew labels from existing panels.

## אפשרות ב׳ — הכול בבת אחת (מסלול ה-"וואו")

> Generate CRUD panels for every entity in `CLAUDE.md` that doesn't already have one. For each:
> - Follow the same pattern as `<AlreadyDonePanel>.cs` exactly — entity-method wiring, RTL settings, Hebrew labels, Back-button mechanism.
> - Wire each panel under the role home panel whose actor owns that UC (read the UC diagram and `CLAUDE.md` to decide).
> - Replace each role home's TODO placeholder buttons with real handlers as you go.
>
> Report at the end which panels you generated and which role home each was wired under.

בשני המסלולים: בנו, לחצו F5, ועברו על כל המסכים.

---

# שלב 7 — מכונות מצבים (שיעורי בית — לא נלמד בשיעור)

## 7.1 — איתור ישויות עם מצבים

> Read `docs/design/state-diagram.md` and any other state diagrams. For each entity with a non-trivial state machine, list: the entity, every state, every transition (source, target, trigger, guard, side effects in other entities). Do not implement anything yet — produce the inventory and ask for confirmation.

## 7.3 — יצירת מתודות המעבר וה-Stored Procedures

> For `<EntityName>`, read `docs/design/state-diagram.md` and enumerate the state transitions for this entity. Then generate the transition methods on the entity class and the matching stored procedures.
>
> For each transition:
> - Method on the entity class named after the domain verb (`cancel()`, `lateCancel()`, `promote()`) — NOT `update(...)`. Encode the guard inline; if the guard fails, throw or return false with a Hebrew message.
> - Matching stored procedure (`sp_<entity>_<verb>`) that updates the state column and applies any side effects in other tables — all inside `BEGIN TRAN ... COMMIT TRAN` with `ROLLBACK` on error.
> - In-memory list updates that mirror the DB changes.
>
> Append SPs to `scripts/stored_procedures.sql` and run them via the mssql MCP. Then add the methods to the entity class.
>
> Do not change any existing CRUD methods.

## 7.5 — כפתורי פעולה

> On `<EntityName>Panel.cs`, replace the generic Update button with verb buttons matching the user-triggered state transitions for this entity — one button per transition, Hebrew label describing the verb. Each button calls the corresponding transition method, handles guard failures with a Hebrew MessageBox, and refreshes the list view on success.

---

# שלבים 8-10 — שיעורי בית

לדוחות, לתרחישי UC מורכבים ולליטוש הממשק — ראו את ה-Prompts המלאים ב-`LESSON_STEPS` (שלבים 8 עד 10). כל אחד מהם פועל באותה תבנית: מדביקים Prompt ואז בודקים.

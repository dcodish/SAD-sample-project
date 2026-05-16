# Lesson Steps — From Analysis to Running CRUD

A running checklist of the steps students take during the lesson. We add to this as we figure out what works.

---

## Prerequisites — Install Before Class

**All software setup is in [`PREREQS.md`](./PREREQS.md).** Do it before class — not during.

The short version of what you need installed: Visual Studio 2025, .NET 8 SDK + Windows Desktop Runtime, SQL Server Express + SSMS, Git, VSCode, Claude Code extension (signed in with an active Claude Pro/Max subscription), and `uv`. Full install steps, verification, and troubleshooting are in `PREREQS.md`.

The lesson assumes everything in `PREREQS.md` works on your machine. If it doesn't, fix that first.

---

## Phase 1 — Project Setup

1. **Create a folder for your group's project** (e.g. `sad-<groupname>`). This is your working folder — it's not the sample.
2. **Put your existing analysis and design files in `docs/`** — your Part A and Part B PDFs (org analysis, requirements, UC diagram, class diagram, etc.).
3. **Clone the SAD sample project into `cloned/`** inside your project folder:
   ```
   git clone https://github.com/dcodish/SAD-sample-project.git cloned
   ```
   The C# reference code is then at `cloned/example_project/` and the shared conventions at `cloned/PATTERNS.md`.
4. **Add `.gitignore`** at the project root so the sample isn't committed to your group repo:
   ```
   cloned/
   .mcp.json
   ```
   `.mcp.json` is added now too — it contains your DB connection config, including authentication. It's a credential-class file; never commit it.

5. **Install the MSSQL MCP server** so Claude Code can talk to your local SQL Server.

   Prerequisite: SQL Server installed locally (see `PREREQS.md` — should be done before class).

   Open Claude Code in your project folder and paste this prompt:

   > Set up the MSSQL MCP server for this project so you can talk to my local SQL Server. This setup has several known gotchas — follow every step.
   >
   > 1. **Install `uv` if missing.** Run `uvx --version`. If not found, run `winget install astral-sh.uv`. Note: after install, `uvx` will NOT be on this Claude Code session's PATH (Windows only refreshes PATH for new processes). To work around this, in step 3 you'll write the **absolute path** to `uvx.exe` into `.mcp.json`. Find it with: `Get-ChildItem -Path "$env:LOCALAPPDATA\Microsoft\WinGet\Packages" -Filter "uvx.exe" -Recurse | Select-Object -First 1 -ExpandProperty FullName`.
   >
   > 2. **Ask me my local SQL Server instance name.** Common values: `localhost\SQLEXPRESS`, `localhost`, `.\SQLEXPRESS`. Don't guess.
   >
   > 3. **Verify TCP/IP is enabled on that instance.** SQL Express named instances ship with TCP/IP disabled by default — pymssql (which the MCP server uses) only speaks TCP. Run:
   >    ```powershell
   >    $instKey = (Get-ItemProperty 'HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL').<INSTANCE_NAME>
   >    Get-ItemProperty "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\$instKey\MSSQLServer\SuperSocketNetLib\Tcp" | Select-Object Enabled
   >    ```
   >    If `Enabled=0`, you need to enable TCP/IP and set a static port (e.g. 14330). This requires admin — launch an elevated PowerShell via `Start-Process powershell.exe -Verb RunAs` (will trigger a UAC prompt the user must approve) running:
   >    ```powershell
   >    $tcp = "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\$instKey\MSSQLServer\SuperSocketNetLib\Tcp"
   >    Set-ItemProperty -Path $tcp -Name 'Enabled' -Value 1
   >    Set-ItemProperty -Path "$tcp\IPAll" -Name 'TcpPort' -Value '14330'
   >    Set-ItemProperty -Path "$tcp\IPAll" -Name 'TcpDynamicPorts' -Value ''
   >    Restart-Service -Name 'MSSQL$<INSTANCE_SUFFIX>' -Force
   >    ```
   >    Use the chosen port in step 4. If TCP/IP was already enabled with a fixed port, use that port instead.
   >
   > 4. **Create `.mcp.json` at the project root.** Use this exact shape — note the absolute path to `uvx.exe`, the use of `MSSQL_SERVER=localhost` + separate `MSSQL_PORT` (don't put `\instancename` here — pymssql can't resolve named instances reliably without SQLBrowser), and `MSSQL_ENCRYPT=false` (not `TrustServerCertificate` — this package doesn't recognize that name):
   >    ```json
   >    {
   >      "mcpServers": {
   >        "mssql": {
   >          "command": "<ABSOLUTE_PATH_TO_uvx.exe>",
   >          "args": ["--from", "microsoft_sql_server_mcp", "mssql_mcp_server"],
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
   >
   > 5. **Patch two known bugs in the cached package.** First, run the server once to trigger the install: `& "<uvx path>" --from microsoft_sql_server_mcp mssql_mcp_server --help` (it'll fail with a config error — that's fine, the install succeeded). Then locate the cached `server.py`:
   >    ```powershell
   >    Get-ChildItem -Path "$env:LOCALAPPDATA\uv\cache\archive-v0" -Filter "server.py" -Recurse | Where-Object { $_.FullName -like "*mssql_mcp_server*" } | Select-Object -ExpandProperty FullName
   >    ```
   >    Apply two edits:
   >    - **Bug A — wrong kwarg name.** The package passes `encrypt=` to pymssql, but pymssql's kwarg is `encryption=` (string, not bool). Find the block that sets `config["encrypt"] = encrypt_str.lower() == "true"` for non-Azure connections and replace it with:
   >      ```python
   >      if encrypt_str.lower() == "true":
   >          config["encryption"] = "request"
   >      ```
   >      (i.e. only set it when explicitly requested, and use the right key name).
   >    - **Bug B — DDL fails inside implicit transaction.** Right after `conn = pymssql.connect(**config)` in the `execute_sql` handler, insert `conn.autocommit(True)`. Without this, `CREATE DATABASE` and other DDL fail with "statement not allowed within multi-statement transaction".
   >
   > 6. **Add `.mcp.json` to `.gitignore`** — it's a credential-class file.
   >
   > 7. **Tell me to restart Claude Code** so it picks up the new MCP server.
   >
   > 8. After I restart and we're in a new chat, your first action will be to list your available MCP tools, confirm `mssql.execute_sql` is present, then run `SELECT @@VERSION` to verify the connection actually works. If you get an "Adaptive Server is unavailable" error, TCP/IP or the port is wrong — re-check step 3.

   If anything fails along the way, the troubleshooting table in `MCP_SETUP.md` covers the common issues. Otherwise you're done — move on to Phase 2.

After Phase 1 your folder looks like:
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

The two key files inside the cloned sample:
- **`PATTERNS.md`** — entity pattern, panel navigation, language conventions, "Login is not a UC", etc. Every student project inherits this verbatim.
- **`CLAUDE.md`** — what the sample order-management project is, its document map, its entities. Read for reference; do NOT copy this — your own CLAUDE.md describes *your* domain.

---

## Phase 2 — Extract Your Analysis Into Structured Markdown

Your group's analysis lives inside one or two large PDFs. Claude works much better against structured markdown than against PDFs, and the sample project's `docs/` folder shows the structure to aim for. This phase converts your PDF content into the same shape.

This phase also doubles as a forcing function: extracting cleanly will expose places where your analysis is vague, internally inconsistent, or incomplete. Better to find that now than after you've written code against it.

### Target structure (mirror the sample's `docs/`)

Create these files. Match the sample's layout in `cloned/docs/` and `cloned/docs/org-analysis/`.

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

Keep your PDFs in `docs/` too — they remain the original signed artifact and contain the diagrams.

Diagrams themselves are **not** re-rendered as ASCII or invented text. Only their accompanying textual content (entity descriptions, state transitions, message sequences, group rationale, assumptions) is extracted into the markdown files. The `.png` files you add yourself by exporting from your modeling tool (or by screenshotting).

### Step 2.1 — Run the batch extraction

Open Claude Code in your project folder and paste this prompt:

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
> - `docs/design/class-diagram.md` — every entity with attributes and methods, every relationship including multiplicities, association classes, the rationale text for any mediator classes, and the group's design assumptions. Do not render the diagram as ASCII; leave a placeholder `![class diagram](class-diagram.png)` at the top.
> - `docs/design/state-diagram.md` — states, transitions, guards, and entry actions in text. Placeholder image at top.
> - `docs/design/sequence-diagram.md` — participants, ordered messages with parameters, and any alt/opt fragments in text. Placeholder image at top.
>
> If a category above is not present in our PDFs, skip that file and tell me at the end which ones you skipped. If our PDFs contain content categories not covered above, tell me where you placed them.
>
> Rules:
> - Preserve our team's original wording. Do not paraphrase, summarize, or invent content.
> - Match the section structure of the source documents (headings, ordering).
> - Diagrams stay in the PDFs as the source of truth. Do not re-render diagrams as text or ASCII art — only extract the textual content that accompanies or describes them.
> - The `.png` files I will add myself later by exporting from the modeling tool.
>
> Do all files in this session. Before writing any of them, ask me any clarifying questions about scope or about choices you want me to make.

### Step 2.2 — Review every extracted file. This step is mandatory.

**Do not skip this and do not proceed to Phase 3 until each file is verified.** Whatever errors you let slide here will compound into your CLAUDE.md and from there into your code.

Open each generated file side-by-side with the relevant PDF section and check:

- **Counts.** Number of user stories, NFRs, problems, UCs, entities, states. Verify each list is complete.
- **Tables.** Rows didn't merge, split, or get dropped. Column order preserved. Header row not promoted to body.
- **Wording.** No paraphrasing. Your team's original phrasing is preserved.
- **Language.** Hebrew sections stayed Hebrew. English sections stayed English. No accidental translation.
- **Structure.** Headings match the PDF's section hierarchy. Nothing was renumbered or re-leveled.
- **UC specs.** Preconditions, postconditions, main flow, and extension flows are all present per UC.
- **Class diagram doc.** Every entity has its attributes and methods. Every relationship has its multiplicity. Association classes are noted as such.
- **No invented content.** Anything Claude wrote that you don't recognize is suspect — verify it traces to the PDF, or have Claude remove it.

When you find a problem, do not edit the file yourself yet. Push back at Claude with a specific instruction:

> In `docs/org-analysis/03-problems.md`, the table has [N] rows. The PDF has [M] rows. Re-read the PDF and reconcile.

Or, if you're unsure:

> In `docs/00-requirements.md` you wrote NFR-7 as [text]. I don't recognize that wording. Re-read the requirements section of the PDF and check whether you paraphrased or invented it.

Iterate until each file matches its source. Then commit the file.

### Step 2.3 — Two-layer UC specs

Your group's PDF probably has VP18-exported UC specs already, but most of them are single-layer (behavioral + implementation details mixed together). The target format separates the two:
1. **Formal spec** — behavioral, technology-neutral. No class names, no SP names, no field names.
2. **Implementation Notes** — clearly labelled, maps each step to specific classes, methods, and stored procedures.

If your extracted `00e-use-cases.md` doesn't already follow this pattern, ask Claude to restructure it. Don't fabricate implementation notes that aren't grounded in your design — leave TODOs.

### Step 2.4 — Skip what doesn't fit

Some PDF content has no clear home in the target structure (group choice of a custom UC relationship, screenshots of social media, "added value" prose, etc.). Skip it or fold it into the closest existing file. Don't invent new top-level documents.

---

## Phase 3 — Generate Your Project's CLAUDE.md

The `CLAUDE.md` at the root of your project is the file Claude Code reads automatically every session. It is the single most important document for getting useful output from Claude. Build it carefully.

### Step 3.1 — Open Claude Code in your project folder

Open VSCode on your project folder. Start a fresh Claude Code session.

### Step 3.2 — Paste this prompt

> I have my group's project documents in `docs/` — extracted markdown files (organization, problems, interviews, business processes, requirements, use cases, and design diagrams under `docs/design/`) plus the original PDFs as backup. Prefer the markdown files; consult the PDFs only when the markdown is unclear or when you need to look at a diagram.
>
> The folder `cloned/` is the SAD course's sample project. Read `cloned/PATTERNS.md` (shared architecture conventions for all SAD projects) and `cloned/CLAUDE.md` (the sample project, for context only — do not copy its domain content).
>
> Read all the documents in `docs/` and produce a `CLAUDE.md` at the root of this folder for my project. It should be self-contained — inline the architecture conventions from PATTERNS.md verbatim, then add my project's domain based on what you read: what the system is, the entities and load order from the class diagram, the use cases we're implementing, and any decisions our group has already made.
>
> Ask me clarifying questions before writing if anything is ambiguous.

### Step 3.3 — Answer Claude's questions, then let it write

Claude will likely flag inconsistencies between your documents or ask for scope decisions. Answer them. Then it writes `CLAUDE.md`.

### Step 3.4 — Review the output. Focus on what won't surface later.

Some errors in `CLAUDE.md` will be caught for you by the compiler or by Claude itself when it next reads the file — a wrong attribute name (`reorderQty` instead of `neededQty`) won't compile, an invented enum value fails on first use. These are self-correcting; don't waste your review time on them.

The errors worth catching now are the ones that **won't** surface later — silent errors that lead Claude to confidently produce wrong code while everything still compiles:

- **Implementation scope.** The UCs listed as "in scope" must match what your group actually decided to code. If Claude lists the wrong four UCs, you'll happily spend an afternoon coding the wrong thing.
- **Load order.** Each entity in the load-order list must come after all entities it has FKs into. An association class or a generated artifact (e.g., `Invoice`) placed too early causes a null-reference at `initLists()` time — easy to miss until launch.
- **Assumptions and group decisions.** Each project-specific decision should be one your team actually made — not one Claude inferred from the sample or smoothed over from a conflict in your documents.
- **Counts and lists.** "N use cases", "N user stories", "N problems" — verify N is correct and the listed items are complete and exclusive. If counts drift here, every downstream summary inherits the drift.
- **Document conflicts swept under the rug.** If your Part A says one thing and Part B says another, Claude will often pick one silently. The fact that there was a conflict is more important than the resolution.

Lower-priority (these tend to self-correct later — fix them in CLAUDE.md only if it's quick):
- Attribute names and enum values — the compiler catches these the moment they're referenced.
- Method signatures — same.
- Wording that "feels off" but doesn't change semantics.

### Step 3.5 — Discuss fixes with Claude

When you find an error, do **not** edit the file directly yet. Tell Claude:

> In the CLAUDE.md you wrote, [specific section] says [what's wrong]. According to [source document, page/section], it should be [what's right]. Fix it.

Or, if you're unsure who's right:

> In the CLAUDE.md you wrote, [section] says [claim]. I think the source documents say something different. Can you re-check `docs/PartX.pdf` and confirm or correct?

This forces Claude to re-read the source rather than guess. It also teaches it where the gaps in its first pass were — useful context for the rest of the session.

### Step 3.6 — Iterate until the file is correct

Repeat 2.4 and 2.5 until every claim in `CLAUDE.md` traces back cleanly to a source document or a deliberate decision. Then commit it.

**Why this matters.** From this point on, every prompt you give Claude — "generate this entity", "write this stored procedure", "create this panel" — is bounded by what's in `CLAUDE.md`. Errors here propagate into your code. Twenty minutes spent reviewing now saves hours of debugging the wrong thing later.

---

## Phase 4 — Database Schema and Basic Stored Procedures

By the end of this phase you have a SQL Server database running locally with every table from your class diagram created, plus a basic CRUD stored procedure set per entity. Every SQL operation is driven by Claude Code through the MSSQL MCP server — you stay in Claude Code the whole time. Complex SPs (reports, multi-entity transactions, state-machine transitions) are deferred to Phase 5+ where there's a real call site to drive their shape.

**Prerequisites:** Phase 1 complete (SQL Server installed, MCP wired, Claude verified it can reach `master`).

### Step 4.0 — Create your project database via MCP

The MCP is connected to `master`. Have Claude create the project DB:

> Use the mssql MCP tool's execute_sql to create a database named `<your_project_db_name>` (something descriptive — `sharona_pilates`, `bookstore`, etc.). Then verify it exists with `SELECT name FROM sys.databases WHERE name = '<your_project_db_name>'`.

Then tell Claude the project DB name, so it knows to switch context on every subsequent batch:

> From now on, every execute_sql call you make should start with `USE <your_project_db_name>;` so that statements execute against our project database, not against master. Remember the database name for the rest of this session.

**Better: bake it into `CLAUDE.md` so every new session picks it up automatically.** Add a one-line "Database" section to your `CLAUDE.md`:

> ## Database
> The project database is `<your_project_db_name>`. The MCP connects to `master`, so every `execute_sql` batch must start with `USE <your_project_db_name>;`.

Now you don't need to repeat yourself in future sessions.

### Step 4.1 — Draft the schema as a `.sql` file

Even though the MCP can execute SQL directly, save the schema to a file first. You want a checked-in, re-runnable artifact — not just one-shot tool calls Claude forgets about. Paste:

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

### Step 4.2 — Review the file before executing

Same review discipline as Phase 3 — focus on silent errors:

- **Column count per table** matches the class diagram exactly. No invented columns, none silently dropped.
- **Types** are appropriate for the data — `NVARCHAR(50)` is too small for an email or a long Hebrew name; `INT` is wrong for monetary values; `DATE` loses time-of-day on a class slot.
- **Nullability** is intentional. Claude defaults everything to `NOT NULL` per the prompt; verify the columns it nullable-marked are actually optional and the ones it didn't are actually required.
- **Foreign key direction.** Each FK should point from the *child* (many side) to the *parent* (one side). Association class tables have two FKs, one to each side.
- **Enum CHECK constraints** list exactly the values from the class diagram — no missing, no invented.
- **Table order** matches `initLists()` load order in your `CLAUDE.md`. If it doesn't, fix `CLAUDE.md` or the DDL — they have to agree.
- **TODO comments.** Each `-- TODO` is a real question Claude couldn't answer from the design docs. Resolve each before running.

When you find an issue, push back at Claude with a specific reference: "Table X column Y should be NVARCHAR(200) not NVARCHAR(50), email addresses can be longer than 50 chars."

### Step 4.3 — Execute the schema via MCP

When the file looks right, tell Claude:

> Run the contents of `scripts/create_database.sql` against the database via the mssql MCP tool. Execute it as one batch. If it fails, tell me which statement failed and the error — do not silently retry or modify the script without asking.

Expect failures on the first run — typos, FK ordering, missing semicolons. Each error gets fixed in the `.sql` file, then re-executed. Iterate via Claude (it can both fix the file and re-execute through MCP) until the script runs end-to-end.

When it succeeds, verify still in Claude Code:

> Use list_tables to confirm every table from the class diagram exists. Then use execute_sql with `sp_help <table_name>` on two or three tables to confirm columns and FKs look right.

### Step 4.4 — Generate basic CRUD stored procedures

Same pattern — file first, then execute via MCP. Prompt:

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

Skim-review the file (these are mechanical — full line-by-line review isn't worth it). Then:

> Run `scripts/stored_procedures.sql` via the mssql MCP tool.

### Step 4.5 — Spot-check one entity end-to-end via MCP

Pick one entity. Ask Claude to exercise its full CRUD cycle through MCP:

> Using execute_sql via the mssql MCP, run sp_<entity>_create with realistic test values, then sp_<entity>_get_all to confirm it landed, then sp_<entity>_update on the new row, then sp_<entity>_delete. Report each result.

If one entity's SP set works end-to-end, the others almost certainly do — the generator is mechanical. Move on.

### What's deliberately not in Phase 4

- **Report SPs** (monthly attendance, monthly income) — they encode business logic that's clearer to write when you have an actual call site driving them.
- **State-transition SPs** for `Registration` (cancel, late-cancel, waitlist promotion) — same reason; write them when the UC that calls them gets implemented.
- **Multi-table transactions** — write per UC, not per entity.

These belong to Phase 5+.

---

## Phase 4.5 — Seed Test Data

Before you start writing C# in Phase 5, populate the DB with realistic test data. Reasons:
- Your `LoginPanel` (Phase 5.5) needs at least one user per role to log in as.
- Your CRUD panels (Phase 5.6) are much easier to validate against a list view that already has rows than against an empty table.
- Manually typing rows through the UI to test the UI is circular — you can't trust the UI until you've tested it.

Have Claude generate the seed via MCP:

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

Skim-review:
- Spot-check a few rows for realism (Hebrew text, plausible values, FK pointers).
- Confirm every enum/status value is represented.
- Confirm there's at least one user per role.

Then run via MCP:

> Execute `scripts/seed_data.sql` against the project DB via the mssql MCP. Report any errors per statement.

When it succeeds, verify with a few SELECTs:

> SELECT count(*) FROM each table. Show me the totals.

Counts should match what `seed_data.sql` was supposed to insert. If a table is empty when it shouldn't be, something silently failed and Claude needs to investigate.

### Why this is its own phase

Could be folded into Phase 4 or Phase 5, but kept separate because:
- It's a one-time bootstrap, not a recurring step.
- Skipping it (or doing it badly) breaks Phase 5's validation experience without breaking the build — exactly the kind of issue that needs its own visibility.

---

## Phase 5 — C# Project Scaffold and First Entity End-to-End

By the end of this phase you have a runnable WinForms C# project with:
- The folder/file structure of the sample
- A working DB connection
- One base entity class (e.g. `UserProfile`) following the entity pattern in `PATTERNS.md`
- One panel that does Create / Read / Update / Delete for that entity
- A `MainForm` that opens the panel via `showPanel()`

You'll then repeat the entity → panel cycle for two more entities in Phase 6.

### Step 5.1 — Scaffold the project

In Claude Code:

> Look at the C# project structure under `cloned/example_project/`. Create a matching scaffold for my project at the root of this folder — a Visual Studio solution + project, same .NET version and references as the sample, same folder layout (entities at the root, panels in a Panels folder, etc.). Use my project name (read `CLAUDE.md` if you need it) instead of the sample's. Do not copy any of the sample's *.cs files — just match the structure.
>
> Copy `<NoWarn>CA1416</NoWarn>` from the sample's csproj into ours. (The sample includes it to suppress the .NET 8 platform-compat analyzer, which floods the build output with hundreds of false-positive warnings on every WinForms call. We don't want students seeing those.)

Then open the solution in Visual Studio and confirm it builds. (Empty build, but should succeed.)

### Step 5.2 — Wire the DB connection

The sample has a class that owns the `SqlConnection` (typically `Program.cs` or a `SQL_CON.cs`). Have Claude replicate it:

> Look at how the sample project handles the SQL connection (`cloned/example_project/SQL_CON.cs` or equivalent — find the file that opens the connection and holds it for the app). Create the equivalent in our project, with the connection string pointing at our project database (read `CLAUDE.md` for the DB name). Use `Trusted_Connection=True;TrustServerCertificate=True;`.

Build again. Still nothing to run, but no errors.

### Step 5.3 — Generate the first entity

Pick a base entity (no FKs — `UserProfile`, `Customer`, `Worker`, etc. depending on your domain). In Claude Code:

> Look at `cloned/example_project/Worker.cs` (or another base entity in the sample) and follow its pattern exactly. Generate `<EntityName>.cs` at the root of our project for the `<EntityName>` entity. Use the column names and types from `scripts/create_database.sql` and the stored procedure names from `scripts/stored_procedures.sql`. Follow the entity pattern in `CLAUDE.md` (sections inlined from `PATTERNS.md`): is_new constructor, createXyz/updateXyz/deleteXyz, static initXyzs, static seekXyz. Add to `Program.<EntityName>s` static list.
>
> Do not invent fields. Do not invent SP names. If anything is unclear, ask before writing.

### Step 5.4 — Review the entity

Same review discipline. Focus on:
- Constructor signature matches sample's `is_new` pattern
- Each `createXyz/updateXyz/deleteXyz` uses the matching SP from `stored_procedures.sql`
- Parameter names match SP parameter names exactly
- `initXyzs` calls the `_get_all` SP and rehydrates with `is_new = false`
- `Program.<entity>s` static list is added in `Program.cs`

Build and resolve any compile errors via Claude.

### Step 5.5 — Design and generate the entry flow

The sample project doesn't ship with a polished entry experience — it starts at `mainForm` with a flat list of buttons, no login, no role separation. For most student projects this isn't enough: you have multiple human actors (Customer, Manager, etc.), each with their own UCs and their own screens, and a real first-run experience should reflect that.

This step has Claude inspect your project's actors and design an entry flow appropriate to *your* domain — before the first CRUD panel exists. The first panel in 5.6 then wires into the right place under the right role.

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

Review before moving on:
- The actor identification matches the UC diagram (no actors invented, none missed).
- Each role's home shows only the UCs that role actually performs — verify against your UC diagram.
- Login (if present) authenticates against the right entity (`UserProfile` for customers, `Employee` for staff — or whatever your design says).
- Hebrew labels read correctly.
- "Entry Flow" section was added to `CLAUDE.md`.

Build (Ctrl+Shift+B). Should go green. You can even F5 here to see the login screen / home menu — buttons just show TODO placeholders, but the entry experience is real.

### Step 5.6 — Generate the first CRUD panel, wired under the right role home

Pick a Tier 1 base entity for the first panel — `UserProfile` if Customers manage their own profile, or pick whatever's simplest in your domain. FK hydration isn't an issue at this tier.

> Generate `<EntityName>Panel.cs` (UserControl + Designer + resx) — full CRUD for `<EntityName>`: list view of all rows, fields to view/edit one row, Save / Update / Delete / Back buttons. Hebrew UI text. Wire each button to the entity's `createXyz / updateXyz / deleteXyz` methods.
>
> Then replace the TODO placeholder button in the appropriate role's home panel (per the "Entry Flow" section in `CLAUDE.md`) so it now calls `mainForm.showPanel(new <EntityName>Panel())`. Back button on the CRUD panel returns to that role's home.
>
> Match the sample's panel patterns exactly — event-handler shape, return-to-home mechanism, Designer.cs structure.

Review:
- Panel button event handlers actually call entity methods, not placeholders.
- The home panel button is now wired (not TODO).
- Back button returns to the correct role home (not to login, not to `mainForm` root).
- Hebrew labels correct.

### Step 5.7 — Run it

Build. Then F5. Walk through:
1. Login screen appears (if (a)). Log in as a test user of the relevant role.
2. The right home panel opens for that role.
3. Click the CRUD entity button. Panel opens.
4. Exercise create / read / update / delete through the UI.
5. Back button returns to the role home.

Verify rows actually appeared/disappeared by asking Claude:

> Use the mssql MCP to SELECT * FROM <entity_table>. Show me what's in the DB right now.

If anything fails: Claude has full context (sample, your code, DB). Describe the symptom and let it diagnose.

### What's deliberately not in Phase 5

- **Login / authentication panel** — by the inherited PATTERNS.md, login is an NFR, not a UC. It's a technical artifact added later.
- **Multi-entity panels** (e.g. a panel showing a customer plus their orders) — Phase 6 territory.
- **Reports** — they call the report SPs you'll write in Phase 7 alongside the report panel itself.

---

## Phase 6 — Generate the Remaining CRUD Panels

Phase 5 produced one CRUD panel end-to-end and validated the pattern. Now scale to the rest of your entities — every base entity, every association class, every entity that has a CRUD UC in your scope.

You have two paths. Pick one based on how much you trust the pattern from Phase 5 and how much you enjoy a good demo.

### Option A — One at a time (cautious)

Generate one panel, review, wire it under the right role's home, build. Then the next. Repeat until done.

> Generate `<EntityName>Panel.cs` (+ Designer + resx) for `<EntityName>`, following the same pattern as `<AlreadyDonePanel>.cs`. Wire it under `<RoleHomePanel>` (replacing the TODO placeholder). Match the entity pattern, RTL settings, and Hebrew labels from existing panels.

Pros: catch any pattern drift early. Easy to review each panel against the source.
Cons: slower, more prompts, more context-switching.

### Option B — All at once (the wow path)

One prompt that generates every remaining CRUD panel, wires each under the appropriate role's home, and reports what it built. Watch Claude crank out 8–10 panels in one go.

> Generate CRUD panels for every entity in `CLAUDE.md` that doesn't already have one. For each:
> - Follow the same pattern as `<AlreadyDonePanel>.cs` exactly — entity-method wiring, RTL settings, Hebrew labels, Back-button mechanism.
> - Wire each panel under the role home panel whose actor owns that UC (read the UC diagram and `CLAUDE.md` to decide).
> - Replace each role home's TODO placeholder buttons with real handlers as you go.
>
> Report at the end which panels you generated and which role home each was wired under.

Pros: dramatic, fast, hits Phase 5's "lesson goal" in one shot. Token-cheaper too — context is read once, not 10 times.
Cons: if Claude misread the pattern in Phase 5, the error propagates to every panel. The review step (next) is more important.

### Step 6.1 — Review (regardless of which path)

After Option A or B, build and walk through every panel:

- **Build:** clean compile, no new warnings beyond CA1416 (still suppressed).
- **Visual sweep:** for each panel, F5 → log in as the relevant actor → click into the panel → confirm Hebrew labels look right (RTL alignment, no `?????`), the list view shows real data from your seed, the buttons are present.
- **Wiring spot-check:** create one row, update one row, delete one row, in two or three panels. If all three operations work on a base entity and an association class, the others are very likely fine.
- **Role home coverage:** every role's home panel should have buttons wired to real panels (no TODOs remaining for UCs that are in scope).

When you find a systematic problem (e.g., "every panel uses the wrong stored proc naming convention"), push back at Claude with a single fix-everything prompt:

> Every CRUD panel calls `<wrong>` instead of `<right>`. Fix all of them in one pass and rebuild.

### What's still not done after Phase 6

- **State-transition methods** for entities with state machines (e.g., Registration's cancel / late-cancel / waitlist promotion logic). The CRUD panel handles only basic create/update/delete; the state transitions are separate methods/SPs.
- **Report UCs** (monthly attendance, monthly income, etc.). These call multi-table SPs and have read-only panels — different shape from CRUD.
- **Cross-entity flows** (e.g., "register for a class" touches Registration + ScheduleSlot + CustomerSubscription in one transaction).

These are real student projects' "remaining 30%" — done in their own time after the lesson, with Claude as the agent. The lesson itself ends here.

---

## Phase 7 — State Machines: Bring Your State Diagrams to Life

This is the first thing in your project that **isn't CRUD**. You drew state diagrams in your design — every state, every transition, every guard. This phase translates them into code so the diagrams stop being documentation and become the actual logic.

### Why state machines deserve their own phase

CRUD treats every entity as a bag of fields you can mutate freely. A state-bearing entity isn't like that — its lifecycle has rules:

- **Guards.** Not every transition is always legal. Cancelling a registration <24h before the lesson is allowed but triggers a different path (late cancel + charge) than cancelling earlier.
- **Side effects in other entities.** Cancelling a registration frees a slot on `ScheduleSlot`, refunds a credit on `CustomerSubscription`, and may promote the next person from the waitlist. One verb, multiple tables.
- **Atomicity.** Those side effects must succeed or fail together. Half-cancelled registrations corrupt the domain.
- **Domain verbs, not symmetric CRUD.** `cancel()`, `lateCancel()`, `promoteFromWaitlist()`, `confirm()` — these are the methods, not `update(status='Cancelled')`.

If a student tries to model these as CRUD updates, the app compiles and runs but the data quietly goes wrong. The state diagram is the upstream artifact that prevents this; this phase is where it pays off.

### Step 7.1 — Identify state-bearing entities

In Claude Code:

> Read `docs/design/state-diagram.md` (and any other state diagrams in `docs/design/`). For each entity that has a non-trivial state machine, list:
> - The entity name
> - Every state in the diagram
> - Every transition, including the source state, target state, the trigger (event/method name), the guard (condition for the transition to fire), and any side effects in other entities
>
> Do not implement anything yet. Just produce the inventory and ask for confirmation that it matches my design.

Review against your own state diagram. If Claude missed a transition or invented one, fix it now — same review-and-challenge discipline as Phase 3.

### Step 7.2 — Pick the first state machine to implement

Same logic as picking the first CRUD panel: pick the simplest state-bearing entity first. Validate the pattern, then scale.

If your project has only one state-bearing entity (common), this step is trivial. If it has more than one, start with the one whose transitions touch the fewest other entities.

### Step 7.3 — Generate transition methods + SPs together

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

### Step 7.4 — Review the state-machine code

Focus on the silent errors:

- **Guards present and correct.** Every transition that should have a guard, has one. Late cancellation actually checks the 24-hour boundary; doesn't just trust the caller.
- **Side effects atomic.** Each transition SP uses `BEGIN TRAN ... COMMIT TRAN`. If the slot-free or credit-refund fails, the state change rolls back too.
- **No state changes via `update<Entity>(...)`.** The generic CRUD update should not touch the state column. State changes go through the named transition methods only. (If you don't enforce this, students will eventually do `reg.update(status='Cancelled')` and silently bypass every guard.)
- **In-memory list mirrors DB.** After a transition, the entity's state in `Program.<entity>s` matches what's in the DB. Easy way to check: SELECT the row via MCP after a UI-driven transition.

### Step 7.5 — Wire UI buttons that trigger transitions, not CRUD updates

Open the relevant CRUD panel for the state-bearing entity. Replace the generic "Update" / "Save" button with **verb buttons** for each transition that's user-triggered.

> On `<EntityName>Panel.cs`, replace the generic Update button with verb buttons matching the user-triggered state transitions for this entity (read `docs/design/state-diagram.md` to enumerate them) — one button per transition, Hebrew label describing the verb. Each button calls the corresponding transition method on the entity, handles guard failures by showing the Hebrew error message in a `MessageBox`, and refreshes the list view on success.

Some transitions are system-triggered (`promoteFromWaitlist` runs when a slot frees up, not from a button) — those don't get UI buttons; they're called from other transition methods.

### Step 7.6 — Walk the state paths

For each transition, exercise it end-to-end through the UI:

1. Set up a row in the source state (via seed data or another transition).
2. Click the verb button (or trigger the system event).
3. Verify the row is now in the target state, and that side effects in other tables happened.

For the canonical test path, ask Claude:

> Use the mssql MCP to verify the state machine end-to-end:
> 1. SELECT the relevant rows BEFORE the transition.
> 2. After I trigger the transition through the UI, SELECT them again.
> 3. Compare and confirm both the state change and every documented side effect happened.

If the diff doesn't match the state diagram, the transition is buggy — fix the method or the SP and re-test.

### What's deliberately not in Phase 7

- **Report UCs** (monthly attendance, monthly income) — different shape, gets its own phase if you reach it.
- **Cross-aggregate flows** (e.g., a single user action that traverses multiple state machines on different entities) — rare in student projects; handle case-by-case.

---

## Phase 8 — Reports (Homework)

**Not covered in class.** Same structure as the in-class phases — drafted here so you can follow it on your own.

Reports are a different UI shape from CRUD: read-only, parameterized (date ranges, filters, drop-down selectors), and the data they show is **aggregated** (counts, sums, percentages, groupings) — not raw rows. The SP behind a report uses `JOIN` / `GROUP BY` / window functions, not single-table SELECTs. The panel has filters at the top and a results grid below, no Save/Update/Delete buttons.

### Step 8.1 — Identify report UCs in your scope

> Read `docs/00e-use-cases.md` and `docs/design/sequence-diagram.md` (or wherever your reports are specified). List every UC whose primary actor is a manager/admin and whose behavior is "view summarized data over some range." For each, name: the input parameters (date range, filter values), the columns shown in the output, and any grouping or aggregation rules.

Review the list against your group's intent. Pick the simplest report to implement first.

### Step 8.2 — Generate the report SP

> Generate `sp_<reportName>` and append it to `scripts/stored_procedures.sql`, then run it via the mssql MCP. The SP takes the report's input parameters (use sensible SQL types — `DATETIME2` for dates, `NVARCHAR` for filter strings, etc.) and returns the aggregated rows defined in Step 8.1's inventory. Use `JOIN` / `GROUP BY` as needed. Hebrew column aliases where the output is user-facing.
>
> Before running, show me the SP and let me review the joins and groupings.

### Step 8.3 — Generate the report panel

> Generate `<ReportName>Panel.cs` (+ Designer + resx) — a read-only panel with: filter controls at the top matching the SP's input parameters (DateTimePickers, ComboBoxes, etc.), a "Generate" button, and a DataGridView below. On click, the panel calls the report SP with the user-entered parameters and binds the result set to the grid. RTL layout. Hebrew labels. No Save/Update/Delete buttons — this is read-only.
>
> Wire the panel into the appropriate manager role home (replacing a TODO button if one exists for this report).

### Step 8.4 — Verify the report

Run it through the UI. Spot-check a few rows manually:
- Pick one row from the report output.
- Walk back through the source data via MCP (`SELECT * FROM <table> WHERE <criteria>`) and verify the aggregation matches by hand.

If the numbers are wrong, the bug is almost always in the SP's `GROUP BY` or `JOIN`. Have Claude re-derive the SP from the requirements and compare against the version it generated.

---

## Phase 9 — Complex UC Flows (Homework)

**Not covered in class.** Drafted here so you can follow it on your own.

Some UCs are **orchestrated transactions** — a single user action that touches multiple entities atomically. "Register for a class" is the canonical example: it decrements credits on a `CustomerSubscription`, increments `registrationCount` on a `ScheduleSlot`, creates a `Registration` row, and triggers a notification. All four happen or none of them happen.

The structure is similar to a state-machine transition (Phase 7), but at the UC level rather than tied to a single entity's lifecycle. You'll reuse the same transaction discipline.

### Step 9.1 — Identify the complex flows

> Read `docs/00e-use-cases.md` and list every UC whose main scenario touches more than one entity in a single user action. For each, name: the entities involved, the operation on each, the order they must happen in, and any guards (preconditions that must be true before any side effect fires).
>
> Distinguish these from simple CRUD UCs (touch one entity) and state-machine transitions (touch one entity's state column plus minor side effects).

### Step 9.2 — Pick the most central UC first

The "first runnable end-to-end flow" should be the one that exercises the most of your domain — typically a primary actor's most common action. Once it works, the others follow the same pattern.

### Step 9.3 — Generate the orchestration SP

> Generate `sp_<flowName>` and append to `scripts/stored_procedures.sql`. The SP takes the operation's inputs as parameters, validates every guard (returns an error code or `RAISERROR` if any fails), then performs every entity update inside a single `BEGIN TRAN ... COMMIT TRAN` block with `ROLLBACK` on any error.
>
> Before running, show me the SP. I want to verify: every guard is enforced; the order of operations is correct; nothing is left for the application code to "remember" to do.

### Step 9.4 — Generate the orchestrating method on the originating entity

> Add a domain-verb method to the originating entity (e.g., `UserProfile.registerForClass(slotId)`). The method calls the SP, on success updates the in-memory state of every affected entity (`Program.Registrations` adds the new row, the affected `ScheduleSlot.registrationCount` increments, the affected `CustomerSubscription.remainingCredits` decrements), and on failure surfaces the SP's error message to the caller as a Hebrew string the UI can show.

### Step 9.5 — Wire UI to the flow

> Add a button/control on the relevant panel that triggers this flow. On click, gather user input (the slot to register for, etc.), call the orchestrating method, show a Hebrew success or failure message, and refresh any visible lists so the change is immediately visible.

### Step 9.6 — Walk the flow end-to-end

For each guard case (every "what if this fails" scenario):
1. Set up the precondition (e.g., empty the subscription's credits).
2. Trigger the flow through the UI.
3. Verify: the right Hebrew error appears, and **no partial state change happened in the DB** (the most important check — open SSMS or use MCP to confirm).

For the success case:
1. Set up valid preconditions.
2. Trigger the flow.
3. Verify all expected updates happened in the DB, every affected in-memory list is consistent, and the UI reflects the new state.

---

## Phase 10 — UI Polish With Claude (Homework)

**Not covered in class.** Drafted here so you can follow it on your own.

The panels generated in Phases 5–9 are functional but visually plain — gray controls, default fonts, no spacing discipline, no branding. Claude can read your existing panels and propose richer designs: better layouts, color schemes, fonts, icons, hover/feedback states, real wireframe-grade designs grounded in your project's domain.

This is also the path to **closing the gap between the UC wireframes you drew in design and what your app actually looks like.** If a wireframe specified a tabbed view or a card grid, this is where you bring it to life.

### Step 10.1 — Decide what to polish

Polish is taste-driven. Pick a target: one signature screen (your app's landing/dashboard for the main role), or a consistent visual upgrade across every panel. Don't try to do both in one prompt.

### Step 10.2 — Feed Claude the source material

Claude designs better when it sees what you want, not what you have. Give it:
- A description of your domain in one line ("a pilates studio management app, intimate small-group atmosphere, target users are women, brand colors are warm/feminine").
- A screenshot of the current panel (drag-and-drop into the Claude Code chat — it can read images).
- The matching wireframe from `order_management_use_case_diagram.html` (or your UC diagram modal), if one exists.
- A reference look you like — a screenshot from another app, a Figma frame, a Dribbble link, anything Claude can see or fetch.

### Step 10.3 — Ask for a redesign with constraints

> Read `<EntityName>Panel.cs` and `<EntityName>Panel.Designer.cs`. Propose a redesign of this panel that:
> - Keeps all the existing controls and event handlers wired exactly as they are (no behavior changes)
> - Improves visual hierarchy, spacing, font choices, and color usage
> - Stays RTL and Hebrew per `PATTERNS.md`
> - Targets the look-and-feel of [reference image or description]
>
> Show me the proposed Designer.cs changes first as a diff. Don't write yet — I want to see the design choices and approve them before they land.

### Step 10.4 — Iterate

Visual design rarely lands first try. Common back-and-forth: "the buttons are too close together", "use a lighter shade of pink", "increase the font size on the list view headers". Each round is cheap because Claude already has full context.

### Step 10.5 — Apply consistently

Once one panel looks right, make the design language explicit so other panels inherit it:

> Document the design choices we made for `<EntityName>Panel.cs` — color palette (hex values), font choices, spacing rules, button styling — as a short "Visual Design" section in `CLAUDE.md`. Then apply the same rules to all the other panels in one pass.

After this, future panels Claude generates will pick up the design language without re-asking.

### What to avoid

- Don't break behavior in pursuit of looks. The redesign is Designer.cs (layout + styling), not the panel's .cs (logic).
- Don't switch UI frameworks mid-project (no jumping from WinForms to WPF here — that's a rewrite, see `ROADMAP.md`).
- Don't introduce third-party UI libraries (Telerik, DevExpress) without first checking whether plain WinForms can carry the visual ambition. Most of the time it can.

---

## Phase 11 — Switching to a Shared Database (Homework)

**Not covered in class.** Drafted here so you can follow it on your own.

Throughout the lesson you've been running against your own local SQL Server. That's fine for development, but eventually your group needs to test against a shared DB so everyone sees the same data and the app can be demoed. Two paths.

### Option A — The course's central SQL Server (BGU)

If you have access to the course's central SQL Server, this is the simplest path — same SQL dialect, no extra signup, no cost.

What you'll need from the course (ask your instructor or TA):
- Server name (e.g., `BGUIEDB2\SQL2008` or similar)
- A database name allocated to your group
- A SQL login (username + password) — central servers use **SQL authentication**, not Windows authentication

### Option B — Azure SQL Database (free tier)

If the central server is unreliable or you'd rather have your own shared cloud DB, this section walks the whole setup end-to-end. One member of the group does the Azure-portal steps once; everyone else just receives the credentials.

**Setup time:** ~15 minutes for the first-time setup. Subsequent connections from teammates: under a minute.

#### Step B.1 — One team member: sign up for Azure for Students

Done once per group, by whoever is willing to be the "DB owner."

1. Go to **[azure.microsoft.com/free/students](https://azure.microsoft.com/en-us/free/students/)**.
2. Click **Start free**.
3. Sign in with your institutional account (`.ac.il` email).
4. Microsoft will verify your student status against the email domain. **No credit card is required.** If it asks for one, you're on the wrong signup page — you want the Students path, not the regular Free Trial.
5. After verification, you have a free Azure account with $100 credit (renewable annually while you're a student) and access to the Azure SQL Database free tier.

#### Step B.2 — Create the SQL Database

In the **[Azure Portal](https://portal.azure.com)**:

1. Top search bar → **SQL databases** → **+ Create**.
2. Fill in:
   - **Subscription:** Azure for Students (auto-selected if it's your only one).
   - **Resource group:** click "Create new" → name it `sad-<groupname>-rg`.
   - **Database name:** pick a descriptive name (e.g., `sharona_pilates_shared`). This is what you'll put in `MSSQL_DATABASE`.
   - **Server:** click "Create new":
     - **Server name:** must be globally unique (e.g., `sad-<groupname>-sql`). This becomes `<servername>.database.windows.net`.
     - **Location:** pick the nearest region (e.g., West Europe).
     - **Authentication method:** **"Use SQL authentication"**.
     - **Server admin login:** pick a username (NOT `admin`, NOT `root` — those are blocked). Pick a strong password. **Write both down.** These are the credentials your whole group will use.
     - Click **OK**.
3. **Compute + storage:** click **Configure database** → pick the **General Purpose Serverless** tier with the **free offer** checkbox (it'll be highlighted). The free offer gives you ~100k vCore-seconds/month and 32 GB storage at no cost.
4. **Networking** (tab at the top of the Create wizard):
   - **Connectivity method:** Public endpoint.
   - **Firewall rules:**
     - Set **"Allow Azure services and resources to access this server"** → **Yes**.
     - Set **"Add current client IP address"** → **No** (we'll set a wider rule in the next step, since IPs change constantly).
5. Skip the other tabs (defaults are fine) → **Review + create** → **Create**. Wait ~3 minutes for deployment.

#### Step B.3 — Open the firewall to anywhere

Azure SQL rejects all incoming connections by default. For a teaching project, the simplest workable policy is "allow connections from any IP; security is enforced by the SQL login":

1. Once the database is deployed, click **Go to resource**.
2. In the database overview, click the **server name** at the top (a link, ends in `.database.windows.net`).
3. In the server's left sidebar, click **Networking**.
4. Under **Public network access**, choose **Selected networks**.
5. Under **Firewall rules**, click **+ Add a firewall rule**:
   - **Rule name:** `allow-all-teaching-context`
   - **Start IP:** `0.0.0.0`
   - **End IP:** `255.255.255.255`
6. Click **OK**, then **Save** at the top of the Networking page.

**Why this is acceptable here:** the real access control is the SQL login. Without the username + password, an open firewall still rejects every connection. For a teaching DB with throwaway data, this trade-off — slightly weaker network filtering in exchange for not having to re-add IP rules every time a teammate's IP changes — is the right call. Don't use this pattern for production.

#### Step B.4 — Share credentials with your team

Write down these four values from Steps B.2 and B.3 and share them privately with teammates (Bitwarden, 1Password, your group's private chat — **never in a public channel, never committed to git**):

- **Server:** `<servername>.database.windows.net`
- **Database:** `<database name from B.2>`
- **User:** `<admin login from B.2>`
- **Password:** `<admin password from B.2>`

Each teammate uses these credentials in their own local `.mcp.json` and `app.config`. Same server, same DB, same login — everyone hits the same data.

#### Step B.5 — Hand off to Claude

The Azure portal work is done. From here, everything is in Claude Code. Skip ahead to **Step 11.1** below — the prompt there does the file edits, runs the scripts against Azure, switches `app.config`, and verifies the connection.

### Step 11.1 — Hand the switch off to Claude

Once you have the four credentials from either Option A (BGU server) or Option B (Azure SQL), one prompt does the rest. Open Claude Code, fill in the four placeholders, and paste:

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
> 1. Rename my current `.mcp.json` to `.mcp.json.local` (so I can switch back). Then create a new `.mcp.json` pointing at the shared server above — `MSSQL_SERVER` and `MSSQL_DATABASE` set to the shared values, replace `MSSQL_WINDOWS_AUTH=true` with `MSSQL_USER` and `MSSQL_PASSWORD`, keep `TrustServerCertificate=true`.
>
> 2. Tell me to restart Claude Code so the new MCP config takes effect. After I confirm I've restarted, in your first action verify the new connection by calling `list_tables` against the shared DB — it should be empty.
>
> 3. Once verified, run `scripts/create_database.sql`, then `scripts/stored_procedures.sql`, then `scripts/seed_data.sql` against the shared DB via the mssql MCP, in that order. Report any errors per script.
>
> 4. Rename `<ProjectName>/app.config` to `app.config.local`. Then create a new `app.config` with a connection string for the shared server using SQL authentication. Format: `Server=<server>;Database=<database>;User Id=<username>;Password=<password>;TrustServerCertificate=True;Encrypt=True;`. The `Encrypt=True` is required for Azure SQL and harmless elsewhere.
>
> 5. Update the "Database" section in `CLAUDE.md` to note that the project now has two targets: local (`.mcp.json.local` / `app.config.local`) and shared (the currently active configs). Document the rename-swap convention to switch between them.
>
> 6. Build the C# project. Then tell me to F5 and confirm the panels show data from the shared DB.

That's the whole switch. Claude does the file renames, the MCP-driven script execution, the verification, and the docs update. You only handle the Claude Code restart and one F5.

**If something fails:**
- "Cannot connect" / timeout → firewall. For Azure: re-check the Networking → Firewall rules in the portal; the `0.0.0.0–255.255.255.255` rule should be saved. For BGU: VPN or network policy.
- "Login failed for user" → username/password typo in the prompt. Retry.
- Scripts fail partway → re-run them; they're idempotent.

### Step 11.2 — Switching back and forth

You'll want to keep doing exploratory dev locally and only push to shared periodically. Two options:

- **Two `.mcp.json` files**, swap them as needed (`.mcp.json.local`, `.mcp.json.shared` — rename the active one to `.mcp.json`).
- **Two app.config files**, same pattern.
- Or just edit the active config when you need to switch. For a group of 4-5 students, the manual-edit approach usually works fine.

Whatever you pick, **never commit `.mcp.json` or `app.config`** — they contain credentials. The `.gitignore` already covers them.

### What this does NOT cover

- **Concurrent writes from multiple teammates** — Phase 7's transactions and Phase 9's orchestrated flows handle the per-operation atomicity; nothing in this course addresses pessimistic locking or optimistic concurrency at the table level. For a 4-5 person group doing exploratory testing, conflicts are rare.
- **Migrations** — when one teammate adds a column locally, the shared DB needs the same change. The discipline is the one we already established: schema changes go into `scripts/create_database.sql` first, commit, push, teammates pull and re-run scripts against their own targets (local AND shared, if applicable).
- **Backup of the shared DB** — Azure SQL handles this automatically; BGU's server should too. Don't rely on your teammates' machines as a backup.

---

## What's Genuinely Out of Scope

Beyond Phase 10, the remaining work on a student project is no longer pattern-following — it's project-specific decisions and operational concerns:

- **Deployment** to the central course server (connection-string switch, running scripts against the shared DB, the dev/test/prod separation).
- **Group git workflow** — branches, conflict resolution, PRs. The course explicitly skipped teamwork in class; figure it out as a group.
- **Submission preparation** — README, demo video, walkthrough document.
- **Performance tuning** if you somehow generate enough seed data to need it (unlikely in a teaching project).

Students who internalized Phases 1–7 will move through Phases 8–10 quickly. Students who skipped the review steps will spend the rest of the semester debugging silent errors that compounded.

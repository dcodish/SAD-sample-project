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
> Decide whether the entry flow should be:
> (a) **Login → per-role home panels** — when there are multiple human actors with distinct screens (e.g., Customer vs Manager). Each role gets its own home panel that hosts only the UCs that actor performs.
> (b) **Flat menu on `mainForm`** — when there's only one human actor, or no meaningful role distinction.
>
> Tell me which design you've chosen and why, and ask for my confirmation before generating any files.
>
> Once I confirm, generate the entry-flow files in one batch (so they compile together):
> - `mainForm.cs` (+ Designer + resx) — match the sample's `cloned/example_project/mainForm.cs` pattern: hosts a single content area and a `showPanel(UserControl)` method.
> - If you chose (a):
>   - `LoginPanel.cs` (+ Designer + resx) — username + password fields, login button. On success, authenticate against `UserProfile` / `Employee` (or whichever entities hold credentials in our model), determine the actor's role, and call `mainForm.showPanel(new <Role>Home())`. Per `PATTERNS.md`, login is a technical artifact, not a UC — that's fine here.
>   - One `<Role>HomePanel.cs` (+ Designer + resx) per role identified. Each home panel has placeholder buttons for that role's UCs (read them from the UC diagram). Buttons can be wired to `MessageBox.Show("TODO")` for now — they'll get real handlers as later CRUD panels are added.
>   - `mainForm`'s entry point shows `LoginPanel` first.
> - If you chose (b): `mainForm` opens directly to a flat menu with placeholder buttons for each UC.
>
> Add a short "Entry Flow" section to `CLAUDE.md` documenting the design you chose, so future sessions don't second-guess it.

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
> Then replace the TODO placeholder button in the appropriate role's home panel (per the entry flow you designed in 5.5) so it now calls `mainForm.showPanel(new <EntityName>Panel())`. Back button on the CRUD panel returns to that role's home.
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

## Phase 6 — (next steps to be added as we work them out)

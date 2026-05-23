::: {custom-style="Centered"}
![](bgu-logo.png){width=3.2in}
:::

::: {custom-style="Title"}
Software Analysis and Design
:::

::: {custom-style="Subtitle"}
Prompts Cheat Sheet — In-Class Copy-Paste Guide
:::

::: {custom-style="Author"}
Lecturer: David Codish
:::

# Before You Start

Every prompt you'll paste into Claude Code during the lesson, in order. For the *why* behind each step (review discipline, error categories, troubleshooting), see `LESSON_STEPS.md`. For installation prerequisites, see `PREREQS.md`.

**Before class:** complete every step in `PREREQS.md` and verify all four commands work in a fresh terminal:

```powershell
dotnet --version       # 8.x
git --version
uvx --version
sqlcmd -L              # lists local SQL Server instances
```

# Phase 1 — Project Setup

Do these manually:

1. Create a folder for your group's project.
2. Put your Part A + Part B PDFs in a `docs/` subfolder.
3. Clone the sample as `cloned/`:
   ```
   git clone https://github.com/dcodish/SAD-sample-project.git cloned
   ```
4. Add a `.gitignore` at the project root containing:
   ```
   cloned/
   .mcp.json
   ```

Then open Claude Code in your project folder and paste:

> Set up the MSSQL MCP server for this project so you can talk to my local SQL Server.
>
> 1. Check whether `uv` is installed by running `uvx --version`. If missing, install it via `winget install astral-sh.uv` and tell me to restart my terminal before continuing.
> 2. Ask me my local SQL Server instance name before writing any config. Common values are `localhost\SQLEXPRESS`, `localhost`, or `.\SQLEXPRESS`. Don't guess.
> 3. Create `.mcp.json` at the project root configuring the community `microsoft_sql_server_mcp` package (RichardHan/mssql_mcp_server) under uvx. Connection settings: server = the instance name I gave you, database = `master`, Windows Authentication enabled, TrustServerCertificate enabled.
> 4. Add `.mcp.json` to `.gitignore` so I don't accidentally commit it — it's a credential-class file.
> 5. Tell me to restart Claude Code so it picks up the new MCP server.
> 6. After I restart and we're in a new chat, your first action will be to list your available MCP tools, confirm `mssql.list_tables` and `mssql.execute_sql` are present, then call `list_tables` against master to verify the connection works.

---

# Phase 2 — Extract Analysis Into Structured Markdown

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
> - `docs/design/class-diagram.md` — every entity with attributes and methods, every relationship including multiplicities, association classes, the rationale text for any mediator classes, and the group's design assumptions. Do not render the diagram as ASCII; leave a placeholder `![class diagram](class-diagram.png)` at the top.
> - `docs/design/state-diagram.md` — states, transitions, guards, and entry actions in text. Placeholder image at top.
> - `docs/design/sequence-diagram.md` — participants, ordered messages with parameters, and any alt/opt fragments in text. Placeholder image at top.
>
> If a category above is not present in our PDFs, skip that file and tell me at the end which ones you skipped. If our PDFs contain content categories not covered above, tell me where you placed them.
>
> Rules:
> - Preserve our team's original wording. Do not paraphrase, summarize, or invent content.
> - Match the section structure of the source documents (headings, ordering).
> - Diagrams stay in the PDFs as the source of truth. Do not re-render diagrams as text or ASCII art.
> - The `.png` files I will add myself later by exporting from the modeling tool.
>
> Do all files in this session. Before writing any of them, ask me any clarifying questions about scope or about choices you want me to make.

**Then review each extracted file against the PDF.** Push back on Claude when counts, tables, or wording drift.

---

# Phase 3 — Generate Your Project's CLAUDE.md

> I have my group's project documents in `docs/` — extracted markdown files (organization, problems, interviews, business processes, requirements, use cases, and design diagrams under `docs/design/`) plus the original PDFs as backup. Prefer the markdown files; consult the PDFs only when the markdown is unclear or when you need to look at a diagram.
>
> The folder `cloned/` is the SAD course's sample project. Read `cloned/PATTERNS.md` (shared architecture conventions for all SAD projects) and `cloned/CLAUDE.md` (the sample project, for context only — do not copy its domain content).
>
> Read all the documents in `docs/` and produce a `CLAUDE.md` at the root of this folder for my project. It should be self-contained — inline the architecture conventions from PATTERNS.md verbatim, then add my project's domain based on what you read: what the system is, the entities and load order from the class diagram, the use cases we're implementing, and any decisions our group has already made.
>
> Ask me clarifying questions before writing if anything is ambiguous.

**Review focus:** implementation scope, load order, group decisions, counts, silent conflicts between documents. Don't waste time on attribute names / enum values — the compiler catches those later.

---

# Phase 4 — Database Schema + Stored Procedures

## 4.0 — Create the project database

> Use the mssql MCP tool's execute_sql to create a database named `<your_project_db_name>`. Then verify with `SELECT name FROM sys.databases WHERE name = '<your_project_db_name>'`.
>
> From now on, every execute_sql call must start with `USE <your_project_db_name>;`. Add a "Database" section to `CLAUDE.md` documenting the DB name and this rule.

## 4.1 — Generate the schema

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

## 4.2 — Review the schema (manual, against the class diagram)

## 4.3 — Execute the schema

> Run the contents of `scripts/create_database.sql` against the database via the mssql MCP tool. Execute as one batch. If it fails, tell me which statement failed and the error — don't silently retry. After success, use list_tables to confirm every table exists.

## 4.4 — Generate stored procedures

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

Then:

> Run `scripts/stored_procedures.sql` via the mssql MCP tool.

## 4.5 — Spot-check one entity end-to-end

> Using execute_sql via the mssql MCP, run sp_<entity>_create with realistic test values, then sp_<entity>_get_all to confirm it landed, then sp_<entity>_update on the new row, then sp_<entity>_delete. Report each result.

---

# Phase 4.5 — Seed Test Data

> Generate `scripts/seed_data.sql` with realistic test data for every table, in load order. Guidelines:
> - Real-feeling Hebrew names, real-looking emails, plausible dates. No "Test User 1".
> - Cover every role/status/enum value at least once.
> - For multi-actor systems: at least 2 users per role.
> - ~5–10 rows per base entity, ~10–20 for transactional/association tables.
> - Every FK references an existing row. No orphans.
> - Passwords (if any) like `password123` — throwaway test users.
>
> Write the file, then I'll review before we run it.

Then:

> Execute `scripts/seed_data.sql` against the project DB via the mssql MCP. Report any errors. Afterward, show me row counts per table.

---

# Phase 5 — C# Project Scaffold and First Runnable Panel

## 5.1 — Scaffold

> Look at the C# project structure under `cloned/example_project/`. Create a matching scaffold for my project at the root of this folder — a Visual Studio solution + project, same .NET version and references as the sample, same folder layout. Use my project name (read `CLAUDE.md`). Do not copy any of the sample's *.cs files.
>
> Copy `<NoWarn>CA1416</NoWarn>` from the sample's csproj into ours.

## 5.2 — Wire the DB connection

> Look at how the sample project handles the SQL connection (`cloned/example_project/SQL_CON.cs`). Create the equivalent in our project, reading the connection string from `app.config`. Match the sample's pattern.

## 5.3 — Generate all entity classes

> For every entity in the class diagram, generate `<EntityName>.cs` following the entity pattern in `CLAUDE.md` (and the sample's `cloned/example_project/Worker.cs` as a reference). Use column names and types from `scripts/create_database.sql` and SP names from `scripts/stored_procedures.sql`. Add each entity's static list to `Program.cs` and call its `initXyzs()` in `Program.initLists()` in correct load order.
>
> Do not invent fields. Do not invent SP names. If anything is unclear, ask before writing.

## 5.4 — Review entities (manual)

## 5.5 — Design and generate the entry flow

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

## 5.6 — Generate the first CRUD panel

> Pick `<EntityName>` (a Tier 1 base entity) for the first CRUD panel. Generate `<EntityName>Panel.cs` (UserControl + Designer + resx) — full CRUD: list view, view/edit fields, Save / Update / Delete / Back buttons. Hebrew UI text. Wire each button to the entity's `createXyz / updateXyz / deleteXyz` methods.
>
> Then replace the TODO placeholder button in the appropriate role's home panel (per the "Entry Flow" section in `CLAUDE.md`) so it now calls `mainForm.showPanel(new <EntityName>Panel())`. Back button returns to that role's home.
>
> Match the sample's panel patterns: event-handler shape, return-to-home mechanism, Designer.cs structure.

## 5.7 — Run

Build (Ctrl+Shift+B). F5. Walk through: login → role home → CRUD panel → exercise CRUD. Then:

> Use the mssql MCP to SELECT * FROM <entity_table>. Show me what's in the DB right now.

---

# Phase 6 — Generate Remaining CRUDs

Pick one path.

## Option A — One at a time (cautious)

> Generate `<EntityName>Panel.cs` (+ Designer + resx) for `<EntityName>`, following the same pattern as `<AlreadyDonePanel>.cs`. Wire it under `<RoleHomePanel>` (replacing the TODO placeholder). Match the entity pattern, RTL settings, and Hebrew labels from existing panels.

## Option B — All at once (the wow path)

> Generate CRUD panels for every entity in `CLAUDE.md` that doesn't already have one. For each:
> - Follow the same pattern as `<AlreadyDonePanel>.cs` exactly — entity-method wiring, RTL settings, Hebrew labels, Back-button mechanism.
> - Wire each panel under the role home panel whose actor owns that UC (read the UC diagram and `CLAUDE.md` to decide).
> - Replace each role home's TODO placeholder buttons with real handlers as you go.
>
> Report at the end which panels you generated and which role home each was wired under.

After either path, build, F5, walk through every panel.

---

# Phase 7 — State Machines (in class if time permits, otherwise homework)

## 7.1 — Identify state-bearing entities

> Read `docs/design/state-diagram.md` and any other state diagrams. For each entity with a non-trivial state machine, list: the entity, every state, every transition (source, target, trigger, guard, side effects in other entities). Do not implement anything yet — produce the inventory and ask for confirmation.

## 7.3 — Generate transition methods + SPs

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

## 7.5 — Verb buttons

> On `<EntityName>Panel.cs`, replace the generic Update button with verb buttons matching the user-triggered state transitions for this entity — one button per transition, Hebrew label describing the verb. Each button calls the corresponding transition method, handles guard failures with a Hebrew MessageBox, and refreshes the list view on success.

---

# Phases 8–11 — Homework

For Reports, Complex UC Flows, UI Polish, and Shared DB switching, see the full prompts in `LESSON_STEPS.md` (Phases 8 through 11). Each follows the same paste-prompt-then-review pattern.

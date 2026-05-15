# Roadmap and Rollout Notes

This document captures **why** the lesson is structured the way it is, and **what's deferred** for future iterations. Read it before changing the architecture or the lesson flow — the design decisions here are intentional, and some have non-obvious tradeoffs.

---

## Lesson Rollout — Key Design Decisions

### Separation of patterns from project-specific docs

`PATTERNS.md` holds cross-project architectural conventions (entity pattern, panel navigation, language rules, RTL, primary key strategy, etc.) — inherited verbatim by every student project's `CLAUDE.md` in Phase 3. The sample's own `CLAUDE.md` is slim and project-specific (what the sample order-management project is, its document map, its entities).

**Why:** without this split, students who generated a `CLAUDE.md` either (a) copied the sample's domain by accident, or (b) skipped the conventions. The split forces each project's CLAUDE.md to inline patterns + describe its own domain.

### Students stay in Claude Code

Once the lesson begins, students never leave Claude Code except for one round trip to Visual Studio for F5. All SQL — DDL, SPs, seed data, verification — runs through the MSSQL MCP server.

**Why:** context-switching between SSMS, the terminal, and Claude Code burns time and breaks the flow. Keeping Claude as the operator also lets it observe errors and self-correct without students copy-pasting between tools.

### MCP server choice: community over official

The MSSQL MCP server used in the lesson is RichardHan/mssql_mcp_server (community), not Microsoft's official Azure-Samples MssqlMcp.

**Why:** Microsoft's server exposes only typed atomic tools (`CreateTable`, `InsertData`, etc.) with no arbitrary-SQL escape hatch. It cannot execute `CREATE PROCEDURE`, which our stored-procedure-first architecture requires. The community server exposes `execute_sql`, which runs any DDL or DML statement Claude sends.

### MCP connects to `master`, not the project DB

The project database doesn't exist when the MCP server first boots. Connecting to `master` means the server starts cleanly, and Claude creates the project DB in Phase 4 Step 4.0. Every subsequent batch starts with `USE <project_db>;`.

**Why:** the obvious alternative (have the student create the DB in SSMS first) puts a manual step in the critical path. With this approach, the *only* manual SQL step is one-time and lives in PREREQS.md (install SQL Server), not in the lesson.

### Primary keys assigned in C#, not by the database

DDL uses `INT NOT NULL PRIMARY KEY` (no `IDENTITY`). The entity class owns `static getNextXId() = max(id) + 1`. The `is_new` constructor calls it before insert. Create SPs take the PK as a parameter.

**Why:** this matches the sample's existing entity code. When Phase 4 originally specified `IDENTITY(1,1) + SCOPE_IDENTITY()`, Claude rewrote the schema mid-Phase-5 to match the sample. Documenting the strategy upfront in PATTERNS.md prevents the conflict from re-occurring per group. Concurrency is not a concern in a single-user teaching app.

### Entry flow: login + per-role homes, not flat menu

Step 5.5 has Claude inspect the project's actors (from the UC diagram) and design an entry flow appropriate to the domain — login + per-role home panels for multi-actor projects, flat menu otherwise. This happens before the first CRUD panel, so panels wire under the correct role's home.

**Why:** the sample project doesn't have authentication, but most student projects do (and want it to feel like a real first run). Deferring this to "later" produced apps that opened to a flat list of buttons regardless of the user's role — not what any group wanted.

Per `PATTERNS.md`, Login is still not a UC — it's an NFR precondition. `LoginPanel` is a technical artifact, not a use case.

### RTL layout as a PATTERNS.md rule

Every `Form` and `UserControl` with Hebrew text sets `RightToLeft=Yes` and `RightToLeftLayout=true`. Documented in PATTERNS.md so it's generated into panels from the start.

**Why:** retrofitting RTL onto LTR-built panels is painful — labels overlap, alignment breaks, the designer fights you. Setting it on creation is a one-property change; setting it after is a redesign.

### CA1416 suppression in csproj

The .NET 8 platform-compat analyzer flags every WinForms call (`MessageBox.Show`, `DataGridView.Rows`, `Control.Dock`, etc.) as "only supported on Windows 6.1+". For a project that targets `net8.0-windows`, this is noise — WinForms only runs on Windows. The sample's csproj includes `<NoWarn>CA1416</NoWarn>` so students don't see ~100 warnings per build.

### Phase 4.5 seed data is its own phase

Could be folded into Phase 4 (DB) or Phase 5 (code), but kept separate because (a) it's a one-time bootstrap, not a recurring step, and (b) skipping it silently breaks Phase 5's validation experience.

### Review-and-challenge discipline at every Claude-generated artifact

Every Claude output (extracted markdown, CLAUDE.md, DDL, SPs, entity classes, panels, entry flow) has a review step in LESSON_STEPS.md that names the specific failure modes worth catching and explicitly tells students *not* to catch errors that the compiler will catch for them. The discipline is consistent across phases.

**Why:** Claude moves fast but isn't careful. Students who skip review ship silent errors into downstream artifacts. Students who try to review everything burn out. The compromise is: catch what won't surface later (counts, scope, FK ordering, group decisions); ignore what will surface (attribute names, enum values).

### Tooling stack

- **Visual Studio 2025 Community** for WinForms designer + F5
- **VSCode + Claude Code** for the agentic workflow (where the lesson lives)
- **SSMS** for visual verification only, not on the lesson critical path
- **`uv`/`uvx`** to run the Python-based MCP server without a venv

---

## Future Considerations

Things explicitly **not** in scope for the current course iteration, worth revisiting later:

### Move the project to WPF

WinForms is dated, the designer is fragile on newer .NET versions, and the panel-navigation model we use papers over what WPF would handle natively with `Frame` + `Page`. A WPF version would also be more aligned with what students will see in industry.

Tradeoffs to think through before committing:
- XAML adds a layer for students new to .NET UI; need to compare cognitive load against WinForms' Designer quirks.
- The sample project would need a full rewrite — entity pattern is portable, panel pattern is not.
- MVVM is the WPF idiom; entity-owns-its-DB-methods conflicts with strict MVVM. Need to decide whether to relax the "no service layer" rule.

### Integrate Figma MCP for UI design

[Figma's MCP server](https://www.figma.com/blog/introducing-figmas-dev-mode-mcp-server/) lets Claude read Figma designs directly. For groups that produce wireframes in Figma (instead of inline in the UC diagram modal), Claude could generate panel layouts from the actual design rather than guessing from text descriptions.

Tradeoffs:
- Adds a tool to the student-facing stack — more setup, more failure modes.
- Conflicts with the current "wireframes live in the UC diagram modal" convention. PATTERNS.md would need a "or Figma" alternative.
- Worth piloting with one group before rolling out.

### Create a web version

A Blazor (Server or WASM) or ASP.NET Core MVC version of the project would be more relevant for groups whose target domain is web (most of them). It would also remove the WinForms-runtime install hurdle students hit in Phase 5.

Tradeoffs to think through:
- The single-window panel navigation pattern doesn't translate to web routing — students would have to learn URL-driven navigation.
- The all-in-memory `Program.*` lists pattern doesn't survive multi-request stateless web servers; would need to either (a) load lists per request (kills the pattern's purpose) or (b) accept that the teaching benefits change.
- Could co-exist: keep WinForms as the default, offer Blazor as an alternative for groups whose domain demands it.

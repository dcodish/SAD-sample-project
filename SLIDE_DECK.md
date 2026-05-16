---
title: "From Analysis to Running CRUD"
subtitle: "Building a System with Claude Code"
author: "Software Analysis and Design — BGU"
date: ""
---

# The Goal Today

By the end of this class:

- A working SQL Server database for your project, populated with realistic test data
- A C# WinForms project running on your machine
- 2–3 CRUD screens connected to the database
- A login screen that routes users by role
- Everything driven from Claude Code, end to end

You bring: your group's Part A + Part B analysis, a laptop with the prerequisites installed.

# The Big Picture

| Phase | What | In class? |
|---|---|---|
| 1 | Project setup + MCP | ✅ |
| 2 | Extract analysis to markdown | ✅ |
| 3 | Generate your CLAUDE.md | ✅ |
| 4 | Database schema + stored procedures | ✅ |
| 4.5 | Seed test data | ✅ |
| 5 | C# scaffold + entry flow + first panel | ✅ |
| 6 | Remaining CRUD panels | ✅ |
| 7 | State machines | ✅ if time |
| 8–11 | Reports, complex flows, UI polish, shared DB | Homework |

# Three Things Drive This Lesson

**1. Stay in Claude Code.** Almost everything goes through one tool — file edits, SQL, builds, tests. No bouncing between SSMS, terminal, browser.

**2. Markdown and `.sql` files are the source of truth.** The database is just a cached projection of your scripts. If it's not in a file in git, it doesn't durably exist.

**3. Review what Claude produces.** Claude is fast but not careful. Skipping review now means debugging silent errors for the rest of the semester.

# Prerequisites — Done Before Class

If these don't all succeed in a fresh terminal, you're not ready:

```powershell
dotnet --version       # 8.x
git --version
uvx --version
sqlcmd -L
```

Plus: Claude Code extension signed in (Pro/Max subscription), VS 2025 open, SSMS connects to local SQL Server.

Full details in **`PREREQS.md`**.

# Phase 1 — Project Setup (10 min)

Manual:

- Create your project folder + put PDFs in `docs/`
- Clone the sample: `git clone … cloned`
- `.gitignore` → `cloned/` + `.mcp.json`

In Claude Code: paste the **MCP install prompt** (in cheat sheet).

Claude installs `uv`, writes `.mcp.json`, restarts itself, verifies the connection to your local SQL Server.

# Phase 2 — Extract Analysis to Markdown (20 min)

Paste **one prompt** — Claude reads both PDFs and produces:

```
docs/
├── org-analysis/01-organization.md
├── org-analysis/02-interviews.md
├── org-analysis/03-problems.md
├── org-analysis/04-business-processes.md
├── 00-requirements.md
├── 00e-use-cases.md
└── design/{class,state,sequence}-diagram.md
```

**Then review each file against the PDF.** Counts, tables, wording — Claude drifts here.

# Phase 3 — Generate Your CLAUDE.md (15 min)

The most important file in your project. Claude reads every doc + the shared `PATTERNS.md` and produces a self-contained `CLAUDE.md`.

What to verify:

- Implementation scope matches what you intend to code
- Entity load order is correct (FK targets come before dependents)
- Group decisions are real (not invented from the sample)
- Counts and lists are accurate

Don't fuss about attribute names — the compiler catches those later.

# Phase 4 — Database Schema + Stored Procedures (25 min)

1. **4.0** — Claude creates the project DB via MCP
2. **4.1** — Generate `scripts/create_database.sql`
3. **4.2** — Review the schema (manual)
4. **4.3** — Claude runs the schema via MCP
5. **4.4** — Generate `scripts/stored_procedures.sql` (CRUD only)
6. **4.5** — Spot-check one entity's full CRUD cycle

PK strategy: **app-side IDs (no `IDENTITY`)**. Stored procedures take the PK as a parameter.

# Phase 4.5 — Seed Test Data (10 min)

Generate `scripts/seed_data.sql` covering every role, status, and enum value. Run it via MCP.

Why now (before code): so when Phase 5's `LoginPanel` runs, there are actual users to log in as.

# Phase 5 — C# Project Scaffold + Entry Flow (45 min)

1. **5.1** — Scaffold (`SharonaPilates.sln` + `.csproj` matching the sample)
2. **5.2** — `SQL_CON.cs` reads connection from `app.config`
3. **5.3** — Generate all entity classes + `Program.cs` with load order
4. **5.4** — Review entity hydration of object references
5. **5.5** — **Entry flow:** Claude inspects actors → designs login + per-role homes
6. **5.6** — Generate the first CRUD panel under the right role home
7. **5.7** — **F5.** Login, navigate, CRUD, verify in DB.

# Phase 5.5 — The Entry Flow Decision

Login is **not** a UC, but it **is** the first screen.

Claude reads the class diagram to find credential-holding entities (e.g., `UserProfile`, `Employee`). Each becomes a login source. Each routes to its own home panel.

Claude proposes the design and waits for your confirmation before writing files.

This is where your design diagrams pay off visually — students see actors become routes.

# Phase 6 — Remaining CRUD Panels (20 min)

Two paths, pick one:

**Option A — One at a time.** Cautious. Generate, review, wire, build. Repeat.

**Option B — All at once.** Dramatic. One prompt → 8–10 panels in a single batch. Token-cheaper, more impressive demo, but errors propagate.

Either way, walk through every panel after generation: build clean, F5 sweep, spot-check one create/update/delete per role.

# Phase 7 — State Machines (in class if time)

CRUD treats entities as bags of fields. State machines introduce:

- **Guards** — not every transition is always legal
- **Side effects** — cancelling a registration frees a slot, refunds a credit, may promote a waitlist
- **Atomicity** — `BEGIN TRAN ... COMMIT` so partial failures roll back

Where your state diagrams from design class become running code.

Each user-triggered transition gets a **verb button** ("ביטול הרשמה"), not a generic Update.

# Phases 8–11 — Homework

Documented in `LESSON_STEPS.md` with full prompts. Not covered in class:

- **Phase 8** — Reports: read-only, parameterized, aggregated. Different shape from CRUD.
- **Phase 9** — Complex UC flows: multi-entity orchestrated transactions.
- **Phase 10** — UI polish: feed Claude screenshots + references, get richer designs.
- **Phase 11** — Switching to a shared DB (BGU central or free Azure SQL).

You have the patterns; you have the tools. Phase 8–11 are just "ask Claude to do X following the same shape."

# The Review Discipline

Catch errors that **won't** surface later. Skip ones the compiler will catch.

**Catch now:**
- Implementation scope
- Load order / FK ordering
- Group decisions
- Counts and lists
- Silent conflicts between documents

**Don't bother:**
- Attribute names (compile error first time they're used)
- Enum values (same)
- Method signatures (same)
- Minor wording

# Working With Claude — Principles

1. **Always ask Claude before doing it yourself.** File edits, SQL runs, builds — let Claude do the work, you review.
2. **Push back specifically.** "The problems table has 5 rows, the PDF has 10. Re-read pages 3–4 and reconcile."
3. **Sources of truth are files, not memory.** Persist decisions in `CLAUDE.md` so future sessions inherit them.
4. **Never commit `.mcp.json` or `app.config`.** They contain credentials.
5. **One prompt per concern.** If you're combining "generate this AND change that AND review that AND deploy that," split it up.

# What to Take Home

- `PREREQS.md` — install once, never re-install
- `LESSON_STEPS.md` — the full reference, with the *why* behind each step
- `PROMPTS_CHEATSHEET.md` — every prompt in copy-paste form
- Your group's project, with a working CLAUDE.md and a runnable first panel

After class: pick up at Phase 6 (if you didn't finish), then push through 8–11 over the semester.

# Questions?

Repo: <https://github.com/dcodish/SAD-sample-project>

When stuck: paste the error into Claude Code, it has full context. If Claude can't solve it, the course group chat is your next stop.

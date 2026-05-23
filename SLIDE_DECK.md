---
title: "From Analysis to Running CRUD"
subtitle: "Building a System with Claude Code"
author: "Software Analysis and Design — BGU"
date: ""
---

# The Goal Today

- A working SQL Server database with seed data
- A C# WinForms project running on your machine
- Login screen + 2–3 CRUD screens
- Everything driven from Claude Code

# What You Bring

- Your group's Part A + Part B analysis
- A laptop with all prerequisites installed
- A GitHub account (every group member needs one)
- Your group's project folder, ready to go

# The Big Picture — In Class

| Phase | What |
|---|---|
| 1 | Project setup + MCP |
| 2 | Extract analysis to markdown |
| 3 | Generate your CLAUDE.md |
| 4 | Database schema + stored procedures |
| 4.5 | Seed test data |
| 5 | C# scaffold + entry flow + first panel |
| 6 | Remaining CRUD panels |
| 7 | State machines (if time) |

# The Big Picture — Homework

| Phase | What |
|---|---|
| 8 | Reports |
| 9 | Complex UC flows |
| 10 | UI polish |
| 11 | Switching to a shared DB |

All documented with full prompts in `LESSON_STEPS.md`.

# Three Things Drive This Lesson

**Stay in Claude Code.** One tool, no bouncing between SSMS / terminal / browser.

**Files are the source of truth.** Markdown + `.sql` in git. The database is a cached projection.

**Review what Claude produces.** Fast but not careful. Skipping review = silent errors all semester.

# Prerequisites — Bootstrap First

The smart install order (student-tested):

1. Install **VSCode + Claude Code** by hand (~10 min)
2. **Let Claude install the rest** — Git, uv, .NET, VS, SQL Server

When installs break, Claude reads the error and fixes it — no googling stack traces alone.

# Prerequisites — Verify

In a fresh terminal, all four must succeed:

```
dotnet --version
git --version
uvx --version
sqlcmd -L
```

Plus: Claude Code signed in, VS 2025 open, SSMS connects.

Full details + the install prompt in `PREREQS.md`.

# Phase 1 — Project Setup

Manual (~5 min):

- Create your project folder
- PDFs into `docs/`
- `git clone … cloned`
- `.gitignore` → `cloned/` + `.mcp.json`

Then one Claude prompt installs the MCP, verifies the DB connection.

# Phase 2 — Extract to Markdown

One prompt, Claude produces:

- Organization, interviews, problems, BPMNs
- Requirements + UC specs
- Design diagrams (class, state, sequence)

**Then review each file against the PDF.** Counts, tables, wording — Claude drifts here.

# Phase 3 — Generate CLAUDE.md

Most important file in your project. Claude inlines `PATTERNS.md` + your domain.

What to verify:

- Implementation scope matches what you'll code
- Entity load order is correct
- Group decisions are real (not invented)

Don't fuss over attribute names — compiler catches those later.

# Phase 4 — Schema + SPs

1. Create the project DB (4.0)
2. Generate `create_database.sql` (4.1)
3. Review against class diagram (4.2)
4. Run via MCP (4.3)

Then SPs:

5. Generate `stored_procedures.sql` (4.4)
6. Spot-check one entity's CRUD (4.5)

# Phase 4 — Key Conventions

- **PKs assigned in C#, not by DB.** No `IDENTITY`.
- **`NVARCHAR`, never `VARCHAR`.** Always.
- **Hebrew literals prefixed `N'...'`.**
- **Every SP takes its PK as a parameter.** No `SCOPE_IDENTITY()`.

These live in `PATTERNS.md` and get inlined into your `CLAUDE.md`.

# Phase 4.5 — Seed Data

Generate `seed_data.sql` covering every role, status, and enum.

~5–10 rows per base table, ~10–20 for transactional/association tables.

**Why now (before code):** so when Phase 5's `LoginPanel` runs, there are real users to log in as.

# Phase 5 — C# Scaffold

- 5.1 — `.sln` + `.csproj` mirroring the sample
- 5.2 — `SQL_CON.cs` reading from `app.config`
- 5.3 — All entity classes + `Program.cs` with load order
- 5.4 — Review entity hydration

Build should fail expectedly until 5.5 (no entry point yet).

# Phase 5.5 — The Entry Flow

Login is **not** a UC. But it **is** the first screen.

Claude reads the class diagram → finds credential-holding entities → designs login + per-role homes.

Claude proposes the design and waits for your confirmation before writing files.

Where your design diagrams pay off visually.

# Phase 5.6 + 5.7 — First Panel and Run

5.6 — Generate the first CRUD panel for a Tier 1 base entity. Wire under the right role's home.

5.7 — Build, F5, walk through: login → role home → CRUD → verify in DB via MCP.

This is the "wow" moment. Login screen, real role routing, working CRUD.

# Phase 6 — Remaining CRUDs

Two paths:

**Option A — One at a time.** Cautious. Generate, review, build, repeat.

**Option B — All at once.** Dramatic. One prompt → 8–10 panels in one batch.

Either way, sweep through every panel after. Spot-check create/update/delete on a couple.

# Phase 7 — State Machines

CRUD treats entities as bags of fields. State machines add:

- **Guards** — not every transition is always legal
- **Side effects** — cancel a registration → free slot + refund credit
- **Atomicity** — `BEGIN TRAN … COMMIT`

Each user-triggered transition gets a verb button, not a generic Update.

# Phase 7 — Why It Matters

Your state diagrams from design class become **running code**.

If you only do CRUD, students can manually set `status = 'Cancelled'` and silently skip all guards — broken domain.

State machines are where business logic lives in code, not in screens.

# Phases 8–11 — Homework

Documented with full prompts in `LESSON_STEPS.md`:

- **8** — Reports (read-only, aggregated, parameterized)
- **9** — Complex UC flows (orchestrated multi-entity transactions)
- **10** — UI polish (feed Claude screenshots, get richer designs)
- **11** — Shared DB (BGU central or free Azure SQL)

Same pattern as in-class phases. You have the tools.

# Review Discipline — Catch Now

Errors that **won't** surface later:

- Implementation scope
- Load order / FK ordering
- Group decisions
- Counts and lists
- Silent doc conflicts

# Review Discipline — Don't Bother

Errors the compiler catches the moment code references them:

- Attribute names
- Enum values
- Method signatures
- Minor wording

Save your attention for the silent ones.

# Working With Claude

- **Always ask Claude first.** Let it do the work, you review.
- **Push back specifically.** Cite source, page, count.
- **Persist decisions in files.** `CLAUDE.md` carries forward to every session.
- **One prompt per concern.** Don't combine "generate, change, deploy" in one prompt.
- **Never commit `.mcp.json` or `app.config`.** Credentials.

# Take-Home Materials

- **`PREREQS.md`** — install once
- **`LESSON_STEPS.md`** — full reference with rationale
- **`PROMPTS_CHEATSHEET.md`** — every prompt in copy-paste form
- **Your group's repo** — working scaffold, CLAUDE.md, runnable panel

After class: pick up where you stopped, then 8–11 over the semester.

# Questions

Repo: github.com/dcodish/SAD-sample-project

When stuck:

1. Paste the error into Claude Code — full context
2. Course group chat — second line
3. Office hours — last resort

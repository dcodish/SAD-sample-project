# Session Close-Out — 2026-05-16

State of the SAD lesson rollout at the close of this design session. Read this when picking up next time to remember where things stand.

---

## What's Done and Shipped

### In the sample repo (`github.com/dcodish/SAD-sample-project`)

**Architecture conventions:**
- `PATTERNS.md` — entity pattern, in-memory lists, SP-only DB, panel navigation, language conventions, RTL rule, primary key strategy, decisions
- `CLAUDE.md` (sample-specific) — what this sample is, doc map, sample's entities/load order
- `ROADMAP.md` — design rationale + future considerations (WPF, Figma MCP, web version)

**Lesson materials:**
- `PREREQS.md` + `PREREQS.docx` — pre-class install guide (7 tools)
- `LESSON_STEPS.md` — full lesson plan, Phases 1–11 (in-class 1–7, homework 8–11)
- `PROMPTS_CHEATSHEET.md` + `PROMPTS_CHEATSHEET.docx` — copy-paste prompts
- `SLIDE_DECK.md` + `SLIDE_DECK.pptx` + `SLIDE_DECK.docx` — 24-slide deck for in-class delivery
- `MCP_SETUP.md` — MSSQL MCP server troubleshooting reference

**Sample code:**
- `example_project/` C# WinForms reference implementation (unchanged from before this session except `NoWarn=CA1416` added to csproj)

### Working demo group (`C:\Users\User\Dropbox\projects\sad demo 30`)

A complete Sharona Pilates instance built by running through all of Phases 1–6+ in role-play. Useful as a reference for "what a finished group's output looks like" and as a regression test next time PATTERNS or LESSON_STEPS change.

Folder contains: `CLAUDE.md`, `docs/`, `scripts/`, `SharonaPilates/`, `SharonaPilates.sln`, `cloned/`. Not shared with students.

---

## Loose Ends and Known Issues

1. **`example_project/SQL_CON.cs` is uncommitted in the sample repo.** Pre-existed this session. Decided to leave alone. Run `git diff example_project/SQL_CON.cs` if you want to see what's there before next session.

2. **Slide deck visual styling is pandoc default** (plain, ugly). Open `SLIDE_DECK.pptx` in PowerPoint and apply a Design template — content reflows into branded slides in ~30 seconds. If you want a permanent BGU/IEM theme baked in, drop a reference `.pptx` next to the source and tell me to use it as `--reference-doc`.

3. **No `.gitignore` for the sample repo's generated docx/pptx.** They are currently tracked. If you ever regenerate and don't want a noisy diff, either add them to `.gitignore` (and rely on a separate distribution path) or commit the regenerated versions normally.

4. **Demo group artifacts in `sad demo 30/`** are intact (~10 entity classes + panels + sln + scripts + docs). Decided to keep for in-class demo + regression value. Delete after the semester if folder pressure becomes an issue.

5. **Phase 8–11 prompts have not been empirically validated.** Phases 1–7 were walked through end-to-end with the demo group, so the prompts and review steps are tested. Phases 8–11 are written by analogy and will need real-group validation when a group first attempts them as homework.

---

## What You Need to Do Before Class

In rough priority order:

1. **Send `PREREQS.docx` to students** ~1 week before class. Verify they all run the four-command check in a fresh terminal.

2. **Test your own machine end-to-end.** Walk through Phases 1–7 on a fresh project folder from scratch, using only the cheat sheet. Confirm nothing has rotted since this session. (Pandoc upgrade, Claude Code extension update, Azure changes, etc.)

3. **Apply a Design template to `SLIDE_DECK.pptx`** so it doesn't look pandoc-default in front of students. ~30 seconds in PowerPoint.

4. **Decide on Azure for Students rollout.** Are you recommending the BGU central server or Azure to your groups? If Azure, point at Phase 11 Option B. If both, students choose per group.

5. **Decide whether to share the `PROMPTS_CHEATSHEET.docx` URL or the `.md` version** with students. The `.md` lives on GitHub (clickable, link previews work); the `.docx` lives locally (must be emailed/uploaded). Most students prefer the `.docx`; some prefer the GitHub link. Pick one canonical reference.

6. **Read `ROADMAP.md` once.** If anything in the "Key Design Decisions" section feels stale or wrong by the time you re-read it, update it before students see it.

---

## How to Resume Next Session

When you pick this up again, ask me to:

> Read `SESSION_CLOSE.md` and pick up from there.

I'll re-orient on the state and we'll continue. If something on the "Loose Ends" list has been resolved by then, tell me what changed.

---

## Six Commits This Session, All Pushed

```
f188100  Split overflowing slides into tighter pages
a2c1023  Add prompts cheat sheet + slide deck
2e9e6df  Phase 11: full Azure portal walkthrough + single-prompt handoff
22b6589  Add Phase 11: switching to a shared DB
d3c3129  Add Phases 8-10 as homework-grade content
bc2a55e  Strip step-number references from prompts students paste to Claude
8ef3d95  Step 7.5: drop Sharona-specific example label
9525b84  Add Phase 7: state machines
fe1c28a  Add Phase 6: generate remaining CRUDs, with two paths
2977247  Sharpen Step 5.5: login design comes from class diagram
1986559  Add ROADMAP.md: rollout design decisions + future considerations
283eb24  Add lesson materials: PREREQS, LESSON_STEPS, MCP_SETUP
53ef563  Add RTL layout rule for Hebrew UI to PATTERNS.md
0615a2b  Suppress CA1416 in sample csproj
e9de88c  Add Primary Key Strategy to PATTERNS.md
f33e9d9  Split CLAUDE.md into PATTERNS.md + CLAUDE.md
```

Branch `main` is up to date with `origin/main`. Nothing pending.

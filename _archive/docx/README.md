# Archived Word / PowerPoint build artifacts

Superseded on 2026-09-08, when the student handouts moved from Word to
self-contained HTML.

These files are kept for reference only. **Do not hand them out** — they were
generated before the course moved from a local SQL Server Express instance to
Azure SQL Database, so their setup instructions are wrong.

The current handouts are built by `build-docs.sh` into `dist/`:

| Source | Handout |
|---|---|
| `PREREQS.md` | `dist/PREREQS.html` |
| `PROMPTS_CHEATSHEET.md` | `dist/PROMPTS_CHEATSHEET.html` |
| `LESSON_STEPS.md` | `dist/LESSON_STEPS.html` |
| `GIT_GROUP_WORKFLOW.md` | `dist/GIT_GROUP_WORKFLOW.html` |
| `SLIDE_DECK.md` | `dist/SLIDE_DECK.html` (a self-contained HTML presentation) |

`reference.docx` and `fix_toc.py` were the pandoc Word-styling tooling
(title page, page breaks, TOC placement). The HTML pipeline replaces them with
`html/template.html` (documents) and `html/slides.html` (the deck).

The archived `SLIDE_DECK.pptx` is superseded too: the deck is now HTML. Pandoc
can still emit a .pptx if a room ever demands it — see the commented line at the
end of `build-docs.sh` — but it does not set RTL direction in PowerPoint, so
Hebrew comes out left-aligned.

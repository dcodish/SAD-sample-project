# PATTERNS.md — Shared Architecture Conventions for SAD Student Projects

This file documents the conventions every SAD student project follows. **Inherit these patterns as-is in your own project's `CLAUDE.md` (by reference or by copy).** Your `CLAUDE.md` should describe your own domain — entities, use cases, decisions — and lean on this file for the architectural pieces that don't change between projects.

If you find yourself wanting to violate a rule here, raise it with the instructor first. These decisions are intentional for pedagogical reasons; the cost of deviating is paid by you and by anyone trying to read your code.

---

## The Human vs. AI Distinction — Course-Wide Principle

This distinction is central to how you should approach work in this course. Do not blur it.

**Humans must do this** (AI cannot substitute, even though AI will later use the output):
- Organizational context: who the business is, what problem it has, why it matters
- Stakeholder identification and needs elicitation
- Problem statement and project scope
- Initial domain modeling: what entities exist, what relationships make sense
- Deciding which use cases exist and which don't (e.g., deciding Login is an NFR, not a UC)
- Prioritization and tradeoffs

**AI can accelerate this** (once the human thinking is done):
- Structuring requirements into user story format
- Writing VP18-style UC specs from a brief description
- Generating the UC diagram HTML from the spec data
- Generating entity classes, stored procedures, and panels from UC specs + implementation notes
- Checking consistency between artifacts (traceability)

Project artifacts are produced in this order: organization/problem context → requirements → UC specs → code. Each is input to the next.

---

## Architecture — Key Patterns

### Entity Pattern
Every entity class is self-contained. Each one owns:
- Private fields + getters/setters
- Constructor with `bool is_new` — if `true`, calls `getNextXYZId()` to assign a new PK, then calls `createXYZ()`, then adds to `Program.list`; if `false`, just sets fields (used during loading)
- `createXYZ()`, `updateXYZ()`, `deleteXYZ()` — each builds a `SqlCommand` with a stored procedure
- `static initXYZs()` — loads all records from DB into `Program.XYZs`, always calls constructor with `is_new = false`
- `static seekXYZ(id)` — searches `Program.XYZs` by ID
- `static getNextXYZId()` — returns `max(id) + 1` over `Program.XYZs` (or `1` if the list is empty). See "Primary Key Strategy" below.

### Primary Key Strategy
**Primary keys are assigned in C#, not by the database.**

- DDL: every PK is `INT NOT NULL PRIMARY KEY`. Do **not** use `IDENTITY(1,1)`.
- The entity class's `static getNextXYZId()` returns `max(id) + 1` over the in-memory list.
- The `is_new` constructor calls `getNextXYZId()` before `createXYZ()` to assign the new row's PK.
- Create stored procedures take the PK as the first parameter (`@<entity>_id`). They do **not** use `SCOPE_IDENTITY()` and do **not** return the new ID.

This is deliberate: students can read the full lifecycle of an ID in one place (the C# constructor), and DB writes are deterministic from the entity's state. Concurrency is not a concern in the single-user teaching context.

### In-Memory Lists
All data lives in `Program.*` static lists after startup. No DB calls during normal use except writes.

`initLists()` load order is strict: **base entities first, then entities with FK references, then association classes last.** Each project's CLAUDE.md states its own concrete order.

### DB Operations
All DB access goes through stored procedures. **No ad-hoc SQL strings in application code.** This is an NFR.

### Panel Navigation
Single-window model. All screens are `UserControl` panels. Navigation: `mainForm.showPanel(new XYZPanel())`. Every panel has a Back button. **No additional Forms or dialogs during normal operation.**

### Inheritance — Table-per-Subclass
When an entity has subtypes, use table-per-subclass: a base table for the parent + one table per subclass holding only the subclass's unique fields + a FK to the base table. Load with a LEFT JOIN and check for `DBNull.Value` to determine subtype. *(Sample project example: `Order` base with `DeliveryOrder` / `PickupOrder` subclasses.)*

### Association Class
When a many-to-many relationship has its own attributes, model it as an association class linking the two sides. In the C# class, both sides are stored as **object references, not IDs**. *(Sample project example: `OrderItem` linking `Order` ↔ `Product` with quantity and unit price.)*

### No Service Layer
Entity classes own their own DB methods. One file per entity. This is intentional for teaching — students see the full lifecycle of an entity in one place.

---

## UC Diagram — Conventions

The diagram is generated from inline JavaScript data and rendered by an external shared script. Rules:

- All data globals must use `var` (not `const`/`let`) so they become `window` properties
- Wireframes are embedded as `useCaseDocs[id].wireframe` HTML strings — **not** as separate files
- All wireframe visible text must be in Hebrew; all form fields use `disabled`; no `<script>` tags inside wireframes
- The `[hidden] { display: none !important; }` style override is required in `<head>` for tab switching to work
- **Login/authentication must never appear as a UC.** Note it only in the `assumptions` array.

### Two-Layer UC Spec Format
Each detailed UC has two sections:
1. **Formal spec** (analysis level) — behavioral, technology-neutral. No class names, SP names, or field names.
2. **Implementation Notes** (design level, clearly labelled) — maps behavioral steps to specific classes, methods, and stored procedures.

This separation is intentional and pedagogically important. Do not merge them.

---

## Language Conventions

| Context | Language |
|---|---|
| C# code (classes, methods, variables) | English |
| UI labels, button text, MessageBox text | Hebrew |
| DB text fields | Hebrew — use `NVARCHAR`, never `VARCHAR` |
| Student guide docs (`docs/*.md`) | Hebrew |
| Requirements and UC spec documents | English |
| UC diagram text (actor labels, UC names, flow steps) | Hebrew |

### RTL Layout for Hebrew UI

Every `Form` and `UserControl` with Hebrew visible text **must** be set up for right-to-left rendering:

- `RightToLeft = Yes` on the form/panel (mirrors text direction, button alignment, scrollbar position).
- `RightToLeftLayout = true` on the root form (mirrors the entire layout, including TabControl direction and DataGridView column order).
- Set these on the parent — children inherit unless overridden.

Generate panels with these properties set from the start. Retrofitting RTL onto LTR-built panels is painful — labels overlap, alignment breaks, the designer file fights you.

---

## Decisions Already Made — Do Not Revisit Without Discussion

These apply across all SAD projects:

- **Login is not a UC.** Authentication is an NFR precondition. A `LoginPanel` is a technical artifact. Do not add Login to UC diagrams or UC specs.
- **Wireframes belong inside the UC diagram modal**, not in separate files.
- **No ad-hoc SQL.** All DB operations use stored procedures.
- **No service layer.** Entity classes own their own DB methods.
- **Single window, panel navigation.** No additional forms or dialogs during normal operation.

# CLAUDE.md — SAD Sample Project

## What This Project Is

This is a **teaching aid** for the Software Analysis and Design (SAD) course at Ben-Gurion University, Industrial Engineering and Management. It is not a production system.

The project demonstrates the full analysis → design → development pipeline for a WinForms order management system. It is used in class to show:

1. **How analysis is done** — at the requirements level (user stories, NFRs), the use case level (UC diagram, VP18 specs), and the design level (class diagram, ERD)
2. **Where agentic AI fits** — which parts of the work require human judgment and domain knowledge, and which parts AI can accelerate once the human work is done

## Architecture Conventions

This project follows the shared SAD conventions documented in [`PATTERNS.md`](./PATTERNS.md): entity pattern, in-memory lists, stored-procedure-only DB access, panel navigation, language rules, and the human-vs-AI principle. **Student projects inherit `PATTERNS.md` directly** and only describe their own domain in their `CLAUDE.md`.

The sections below describe the parts that are specific to *this* sample project.

---

## Document Map

| File | Purpose |
|---|---|
| `PATTERNS.md` | Shared architecture conventions — inherited by all student projects |
| `docs/00-requirements.md` | User stories (65) + NFRs (28) + traceability matrix. Primary human-authored artifact. |
| `docs/00e-use-cases.md` | VP18-style UC specs for 5 representative UCs. Two-layer format: behavioral spec + implementation notes. |
| `order_management_use_case_diagram.html` | Interactive UC diagram. Two tabs. Click a UC to see its full spec + wireframe. |
| `docs/01` – `docs/11` | Student guides: setup, DB, architecture, class diagram, CRUD patterns, navigation, Git, AI tools, checklist |
| `example_project/` | C# WinForms source code — the reference implementation |
| `scripts/create_database.sql` | Full DB creation script |

---

## Domain Entities and Load Order

The in-memory list load order for this project is:

```
Workers → Products → Orders (→ DeliveryOrders / PickupOrders) → OrderItems
```

Base entities first, then entities with FK references, then association classes last.

### Inheritance Example in This Project
`Order` is the base; `DeliveryOrder` and `PickupOrder` are table-per-subclass siblings. See `PATTERNS.md § Inheritance` for the rule.

### Association Class Example in This Project
`OrderItem` links `Order` ↔ `Product` with `quantity` and `unitPrice`. See `PATTERNS.md § Association Class` for the rule.

---

## Project-Specific Decisions

Beyond the cross-project decisions in `PATTERNS.md`:

- This project's UC diagram covers order management UCs only. Reporting and admin UCs were intentionally excluded for scope.
- The five UCs detailed in `00e-use-cases.md` were chosen to cover one CRUD, one regular flow, one report, one with an `<<include>>`, and one with an `<<extend>>` — for pedagogical coverage rather than business priority.

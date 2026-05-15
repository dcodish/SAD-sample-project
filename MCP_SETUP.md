# Wiring Claude Code to Your Local SQL Server (MCP)

By the end of this setup, your Claude Code session can read from and write to your local SQL Server directly — no copy-pasting SQL between Claude and SSMS. Setup is once per machine.

**Prerequisite:** You've already installed SQL Server locally and verified it works (see `MSSQL_SETUP.md`).

---

## What you're installing

The **MSSQL MCP server** by Richard Han ([`microsoft_sql_server_mcp`](https://github.com/RichardHan/mssql_mcp_server)) — a small Python program that exposes two MCP tools to Claude:

- `list_tables` — list tables in the connected database
- `execute_sql` — run any SQL (SELECT, INSERT, UPDATE, DELETE, CREATE TABLE, CREATE PROCEDURE, etc.)

It's a community implementation, not Microsoft's. We pick it because Microsoft's official server is locked to a small set of structured operations and can't create stored procedures — and we need stored procedures.

---

## Step 1 — Install `uv` (Python package runner)

The MCP server runs via `uvx`, which is part of [astral-sh/uv](https://github.com/astral-sh/uv). Install it once:

**Windows (PowerShell):**
```powershell
winget install astral-sh.uv
```

Or, if you have Python already and prefer pip:
```powershell
pip install uv
```

Verify:
```powershell
uvx --version
```

If `uvx` is found, you're good. If not, restart your terminal and try again.

---

## Step 2 — Configure Claude Code

Claude Code reads MCP servers from a `.mcp.json` file at the root of your project folder. Create it:

```json
{
  "mcpServers": {
    "mssql": {
      "command": "uvx",
      "args": ["microsoft_sql_server_mcp"],
      "env": {
        "MSSQL_SERVER": "localhost\\SQLEXPRESS",
        "MSSQL_DATABASE": "master",
        "MSSQL_WINDOWS_AUTH": "true",
        "TrustServerCertificate": "true"
      }
    }
  }
}
```

Substitute `localhost\\SQLEXPRESS` with your actual instance name if you used something else. Note the double backslash — it's a JSON escape; the real value passed to SQL is `localhost\SQLEXPRESS`.

**Why `master` and not the project DB?** `master` is the SQL Server system catalog — it's always present. We connect there so the MCP can boot before any project database exists. The project database gets created via Claude (Phase 4), and from then on every SQL batch starts with `USE <project_db>;` to switch context. This way you never have to open SSMS to bootstrap.

If you used **SQL authentication** (not Windows Auth), replace `MSSQL_WINDOWS_AUTH` with `MSSQL_USER` and `MSSQL_PASSWORD` instead. For this course we use Windows Auth.

---

## Step 3 — Restart Claude Code and verify

1. Close Claude Code completely. Reopen it on your project folder.
2. In a new chat, type: *"What MCP tools do you have available?"*
3. Claude should list at least `mssql.list_tables` and `mssql.execute_sql`.
4. Ask Claude: *"Use the mssql tool to list the tables in the master database — just confirm the connection works."*

Expected outcome: Claude calls `list_tables`, the call succeeds, and lists the system tables in `master`. No errors.

---

## Common problems

| Symptom | Fix |
|---|---|
| `uvx: command not found` | Restart your terminal after installing `uv`. If still missing, ensure `%LOCALAPPDATA%\Microsoft\WinGet\Links` (or wherever `uv` installed) is in your PATH. |
| Claude says "no MCP server named mssql" | Restart Claude Code after editing `.mcp.json`. Verify the file is at the **root** of the folder Claude Code opened. |
| First `execute_sql` call hangs | First-run `uvx` downloads the package — give it 30 seconds. Subsequent calls are instant. |
| "Login failed for user" | If using Windows Auth, ensure `MSSQL_WINDOWS_AUTH` is set to `"true"` (string, not boolean) and no `MSSQL_USER`/`MSSQL_PASSWORD` are also set. |
| "Cannot open database ... requested by login. The login failed." | The database in `MSSQL_DATABASE` doesn't exist. Go back to Step 2 and create it. |
| "SSL certificate" / "encryption" error | `TrustServerCertificate: "true"` should handle this. If still failing, check that the SQL Server service supports the protocol your client is requesting. |

---

## Security note

`execute_sql` accepts any SQL, including `DROP DATABASE`. Treat your `.mcp.json` like a credential file:

- **Add `.mcp.json` to `.gitignore`.** Do not commit it to your group repo.
- **Use a project-specific local database**, not your only SQL Server instance.
- **Don't enable this MCP against a production server.**

For coursework against a local machine, the risk is bounded — at worst you drop the wrong test database and re-run your DDL.

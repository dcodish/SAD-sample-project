::: {custom-style="Centered"}
![](bgu-logo.png){width=3.2in}
:::

::: {custom-style="Title"}
Software Analysis and Design
:::

::: {custom-style="Subtitle"}
Pre-Class Setup Guide
:::

::: {custom-style="Author"}
Lecturer: David Codish
:::

# Prerequisites — SAD Course Pre-Class Setup

Everything in this document must be installed and working **before** the lesson. Doing this in class wastes everyone's time and blocks the rest of your group.

**Time budget:** ~45 minutes if nothing fails. Plan for an hour to be safe. Reboot before you start.

## The approach: bootstrap Claude first, then let it do the rest

Installing dev tools always throws surprises — PATH problems, missing dependencies, version conflicts, cryptic installer errors. Rather than fight those alone, you'll install **just two things by hand** (VSCode and Claude Code), and then **let Claude install and troubleshoot everything else.** When something fails, Claude is right there to read the error and fix it, instead of you googling stack traces.

So the flow is:

1. **Part A — Bootstrap (manual):** VSCode + Claude Code + your Claude subscription. ~10 minutes.
2. **Part B — Claude-assisted install:** paste one prompt; Claude installs and verifies the rest (Git, uv, .NET, Visual Studio, SQL Server, SSMS), troubleshooting failures as they come up.
3. **Part C — Manual finish:** the few GUI steps Claude can't click for you (mainly the first SQL Server connection).

If you'd rather install everything by hand, the **Manual Install Reference** at the bottom has the full per-tool steps.

---

# Part A — Bootstrap (Do These by Hand)

## A1. VSCode

Download: <https://code.visualstudio.com> — pick the Windows installer (user version is fine).

Install with defaults. When the installer asks about "Additional Tasks", check:
- ✅ **Add "Open with Code" action to Windows Explorer file/directory context menu**
- ✅ **Add to PATH**

Open VSCode once to confirm it launches.

## A2. Claude Pro or Max subscription

Claude Code is included in both Pro and Max — no separate purchase. There is **no free tier** for the agent; without a subscription, nothing in this course works.

1. Go to <https://claude.ai>.
2. Sign up or sign in.
3. Profile → **Upgrade** → **Pro** (sufficient) or **Max** (higher limits).
4. Complete checkout.

## A3. Claude Code extension + sign in

1. In VSCode, open Extensions (Ctrl+Shift+X).
2. Search **"Claude Code"** (published by Anthropic) → **Install**.
3. Open the Claude Code panel (Claude icon in the sidebar, or Ctrl+Shift+P → "Claude Code: Sign In").
4. Sign in with the **same account** as your Pro/Max subscription. Authorize in the browser when prompted.

**Verify:** type "hello, are you connected?" in Claude Code and send. You should get a reply. If you see "no active subscription," wait 1–2 minutes after subscribing, then sign out and back in.

Once Claude replies, the bootstrap is done. Everything else, Claude helps with.

---

# Part B — Let Claude Install the Rest

Open a folder in VSCode (any empty folder is fine for now — you'll make your real project folder during the lesson). In the Claude Code panel, paste this prompt:

> Help me install and verify the development tools for a course. I'm on Windows. For each tool, first check whether it's already installed (and the right version); if not, install it — prefer `winget` and run the command for me, then verify. If a tool needs a GUI installer you can't drive, give me the exact steps and wait while I do it, then help me verify and troubleshoot. Work through them one at a time and tell me the status of each.
>
> The tools:
> 1. **Git** — any recent version.
> 2. **uv** (Astral's Python runner — package id `astral-sh.uv`). Verify `uvx --version` works.
> 3. **.NET 8 SDK** — verify `dotnet --version` reports 8.x.
> 4. **.NET 8 Windows Desktop Runtime** — separate from the SDK. Verify `dotnet --list-runtimes` shows `Microsoft.WindowsDesktop.App 8.x`.
> 5. **Visual Studio 2025 Community** with the ".NET desktop development" workload. This is a large GUI install — guide me and help me pick the right workload.
> 6. **SQL Server Express** (2019 or newer). If I already have a local instance, help me confirm it's running instead of reinstalling.
> 7. **SQL Server Management Studio (SSMS)**.
>
> After each install, run the verification command and tell me if it passed. If anything fails, diagnose it before moving on. At the end, give me a summary table of what's installed and what (if anything) still needs my attention.

Claude will work through the list, running `winget` commands, checking versions, and troubleshooting whatever breaks. Let it drive. Answer its questions (e.g., your SQL Server instance name) when it asks.

**What Claude can and can't do here:**
- ✅ Install Git, uv, .NET SDK, .NET Desktop Runtime via `winget` and verify them.
- ✅ Read error messages and fix PATH / version / dependency problems.
- ⚠️ Visual Studio and SQL Server have large GUI installers. Claude can launch them via `winget` or guide you through the download, but you'll click through the wizard. Claude helps verify the result.

---

# Part C — Manual Finish

A couple of steps need your eyes and clicks. Claude can guide you, but you drive.

## C1. Confirm SQL Server is running

```powershell
Get-Service MSSQL*
```
`MSSQL$SQLEXPRESS` (or `MSSQLSERVER`) should be **Running**. If stopped, ask Claude to start it, or run `Start-Service MSSQL$SQLEXPRESS` in an Administrator PowerShell.

## C2. First connection in SSMS + smoke test

1. Open SSMS. Skip the Microsoft account sign-in ("Not now, maybe later").
2. Connect:
   - **Server Name:** `localhost\SQLEXPRESS` (or your instance name)
   - **Authentication:** Windows Authentication
   - ✅ **Trust Server Certificate**
3. New query (Ctrl+N), run:

```sql
SELECT @@VERSION;
GO
CREATE DATABASE sad_smoketest;
GO
USE sad_smoketest;
GO
CREATE TABLE ping (id INT, msg NVARCHAR(50));
INSERT INTO ping VALUES (1, N'שלום');
SELECT * FROM ping;
GO
USE master;
DROP DATABASE sad_smoketest;
GO
```

Expected: version info, a row showing `שלום` (not `?????`), no errors. Paste any error into Claude Code — it'll diagnose.

## C3. GitHub account (required for your group)

You work in groups, and your group needs a shared place to version the project — the schema scripts, the C# code, the docs. GitHub is the default. So **every group member needs a GitHub account.**

- Cloning the course sample repo needs **no** account (it's public).
- But pushing to your group's own repo, or being added as a collaborator on it, **does** need an account.
- The "source of truth lives in git" workflow (schema changes go into `.sql` files, commit, teammates pull and re-run) only works once you have a shared remote.

Steps:
1. Each member signs up at <https://github.com> (free).
2. One member creates the group repo and adds the others as collaborators (Settings → Collaborators).
3. Each member sets up authentication for pushing. Ask Claude: *"Help me set up GitHub authentication so I can push — I'm on Windows."* (GitHub CLI is the easiest path; Claude can install and configure it.)

If your group has decided to host elsewhere (GitLab, BGU's internal Git), that's fine — the lesson doesn't depend on GitHub specifically, but you still each need an account on whatever host you chose.

---

# Final Verification

Open a **fresh** PowerShell window. All four must succeed:

```powershell
dotnet --version          # 8.x.x
git --version             # git version 2.x.x
uvx --version             # uv 0.x.x
sqlcmd -L                 # lists local SQL Server instances
```

If any fail, paste the result into Claude Code and let it fix the issue — that's exactly why we installed Claude first.

---

# Bring-to-Class Checklist

The day of class:

- [ ] Claude Code is signed in (test message returns a reply)
- [ ] VS 2025 opens
- [ ] SSMS connects to your local instance with Windows Authentication
- [ ] `dotnet --version`, `git --version`, `uvx --version`, `sqlcmd -L` all work in a fresh terminal
- [ ] You have a GitHub account and can push (or your group's chosen Git host)
- [ ] Your group's Part A + Part B PDFs are accessible

If anything fails the morning of class, message the group chat *immediately*.

---

# Manual Install Reference (Fallback)

If you prefer to install by hand, or if Claude's assisted install hit a wall, here are the full per-tool steps.

## Visual Studio 2025 (Community)

<https://visualstudio.microsoft.com/downloads/> → **Community 2025**. On the Workloads screen, check **.NET desktop development** (includes the .NET 8 SDK and WinForms tooling). Verify: `dotnet --version` reports `8.x.x`.

## .NET 8 Windows Desktop Runtime

Separate from the SDK; the SDK builds apps, the Desktop Runtime runs them. Without it: "You must install or update .NET to run this application" at launch.

<https://dotnet.microsoft.com/download/dotnet/8.0> → under **"Run desktop apps"** → **"Windows Desktop Runtime x64"**. Verify: `dotnet --list-runtimes` shows `Microsoft.WindowsDesktop.App 8.x.x`.

## SQL Server Express

Any version 2019+. <https://www.microsoft.com/en-us/sql-server/sql-server-downloads> → **Express** → **Download now**. Choose **Basic** install. Note the instance name (`SQLEXPRESS`). If you already have an instance from a previous course, skip — just confirm it runs (Part C1).

## SQL Server Management Studio (SSMS)

<https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms>. Install with defaults. First connection + smoke test in Part C2.

## Git

<https://git-scm.com/download/win>. Defaults are fine; accept "Checkout Windows-style, commit Unix-style" for line endings; pick VSCode as the default editor if offered. Verify: `git --version`.

## uv (Python runner for the MSSQL MCP)

```powershell
winget install astral-sh.uv
```
Or `pip install uv` if you have Python. Verify: `uvx --version`. If "command not found", reopen the terminal.

## VSCode extensions

From Extensions (Ctrl+Shift+X):
- **C# Dev Kit** (Microsoft) — syntax highlighting + IntelliSense for the `.cs` files Claude writes. Without it, C# looks like plain text.
- **C#** (Microsoft) — auto-installed with C# Dev Kit.
- **Claude Code** (Anthropic) — already installed in Part A.

Don't install random "popular" extensions — they can interfere with the lesson tooling.

---

# Common Problems

| Symptom | Likely cause | Fix |
|---|---|---|
| `dotnet` reports 7.x or "not found" | .NET 8 SDK not installed | Re-run VS Installer → Modify → check ".NET desktop development", or `winget install Microsoft.DotNet.SDK.8` |
| Built app says "You must install or update .NET" at launch | Desktop Runtime missing (only SDK installed) | Install .NET 8 Windows Desktop Runtime |
| SSMS: "Cannot connect to localhost\SQLEXPRESS" | SQL Server service stopped | `Start-Service MSSQL$SQLEXPRESS` in Admin PowerShell; set startup type Automatic |
| SSMS: "A network-related or instance-specific error" | Wrong instance name | `sqlcmd -L` to list local instances; use whichever appears |
| Hebrew text comes back as `?????` | Column is `VARCHAR` not `NVARCHAR` | Always use `NVARCHAR` for Hebrew |
| SSMS: "Login failed for user" | Trying SQL auth instead of Windows auth | Use **Windows Authentication** |
| `uvx: command not found` after install | Terminal hasn't refreshed PATH | Reopen the terminal |
| Claude Code: "no active subscription" | Subscription not propagated, or wrong account | Wait 2 min, sign out + back in, confirm the subscribed account |
| Visual Studio designer broken / missing | Outdated VS | Help → Check for Updates; VS 2025 has the most reliable .NET 8 WinForms designer |

**For any of these:** paste the symptom into Claude Code. It has the context to diagnose and usually fixes it faster than the table.

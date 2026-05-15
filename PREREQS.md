# Prerequisites — SAD Course Pre-Class Setup

Everything in this document must be installed and working **before** the lesson. Doing this in class wastes everyone's time and blocks the rest of your group.

**Time budget:** ~45 minutes if nothing fails. Plan for an hour to be safe. Reboot before you start.

**What you'll have at the end:** a Windows machine ready to clone the sample, open it in Claude Code, and start the lesson immediately.

If something breaks, post the exact error message in the course group chat — most problems are 30-second fixes once we see the error.

---

## Install in this order

The order matters. Some installers depend on others being present.

| # | Tool | Why |
|---|---|---|
| 1 | Visual Studio 2025 (Community) | Builds and runs the WinForms project. Also pulls in the .NET 8 SDK. |
| 2 | .NET 8 Windows Desktop Runtime | **Separate from the SDK.** Without it, your built app won't launch. |
| 3 | SQL Server Express (2019 or newer) | The local database. If you already have it from a previous course, skip the install. |
| 4 | SQL Server Management Studio (SSMS) | GUI for verifying the database visually. |
| 5 | Git | Clone the sample, version your group's project. |
| 6 | VSCode | Your primary IDE for the lesson. |
| 7 | Claude Code (extension) + Claude Pro/Max account | The AI agent that drives most of the lesson. |
| 8 | `uv` (Python runner) | Powers the MSSQL MCP server that connects Claude to your database. |

---

## 1. Visual Studio 2025 (Community)

Download: <https://visualstudio.microsoft.com/downloads/> — pick **Visual Studio Community 2025** (free).

During install, on the "Workloads" screen, check:
- **.NET desktop development** (required — includes .NET 8 SDK and WinForms tooling)

You can uncheck everything else if you want a small install (~3 GB). Otherwise leave defaults.

**Verify:**
```powershell
dotnet --version
```
Should report `8.x.x`. If it reports `7.x` or "not found," your VS install didn't pull in the .NET 8 SDK. Run the VS Installer → Modify → ensure ".NET desktop development" is checked.

---

## 2. .NET 8 Windows Desktop Runtime

This is **separate from the SDK** and easy to miss. The SDK builds apps; the Desktop Runtime *runs* them. Without it, you'll see this error when launching the project:

> "You must install or update .NET to run this application."

Download: <https://dotnet.microsoft.com/download/dotnet/8.0>

On that page, under **"Run desktop apps"**, download **"Windows Desktop Runtime x64"** (about 57 MB). Run the installer, accept defaults.

**Verify:**
```powershell
dotnet --list-runtimes
```
You should see at least one line containing `Microsoft.WindowsDesktop.App 8.x.x`. If you only see `Microsoft.NETCore.App`, the Desktop Runtime didn't install — re-run the installer.

---

## 3. SQL Server Express

Any version from **2019 onward** works. If you already have SQL Server Express installed from the previous course (or anywhere else), skip this step — confirm it runs by jumping to the verification at the bottom of this section.

Download: <https://www.microsoft.com/en-us/sql-server/sql-server-downloads> → scroll to **Express** → **Download now**. (Microsoft's current download is 2022; 2019 is fine too, just no longer linked from the main page.)

Run the installer:
1. Choose **Basic** installation type.
2. Accept the license terms.
3. Leave the install location at the default.
4. Click **Install** and wait (~5 minutes).

When it finishes, the summary screen shows:
- **Instance Name:** `SQLEXPRESS` (write this down)
- **Connection String:** `Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;`

Click **Install SSMS** at the bottom of the summary, or move to step 4.

> If you've installed SQL Server before and have a default instance (`MSSQLSERVER`) instead of `SQLEXPRESS`, that's fine — just use whichever name you actually have, everywhere.

**Verify:** SQL Server's service must be running for anything to connect:
```powershell
Get-Service MSSQL*
```
The status of `MSSQL$SQLEXPRESS` (or `MSSQLSERVER`) should be **Running**. If it's stopped, run `Start-Service MSSQL$SQLEXPRESS` in an Administrator PowerShell.

---

## 4. SQL Server Management Studio (SSMS)

Download: <https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms>

Install with defaults. Reboot if it asks.

**Verify by connecting:**
1. Open SSMS.
2. On first launch, **skip** the Microsoft account sign-in ("Not now, maybe later"). It's for syncing settings only.
3. In the "Connect" dialog:
   - **Server Name:** `localhost\SQLEXPRESS`
   - **Authentication:** Windows Authentication
   - **Encrypt:** Mandatory
   - **Check** ✅ **Trust Server Certificate** (required for local servers — otherwise you'll get an SSL error)
4. Click **Connect**.

You should see Object Explorer on the left with your server expanded. To prove it really works, open a new query (Ctrl+N) and run:

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

Expected: version info, a row with `שלום` (not `?????`), no errors. If you see `?????`, the column was `VARCHAR` not `NVARCHAR` — your test failed, but the install is fine; remember to always use `NVARCHAR` for Hebrew.

---

## 5. Git

Download: <https://git-scm.com/download/win>

Install with defaults. **Important during install:** when asked about line endings, accept the default ("Checkout Windows-style, commit Unix-style"). Pick **"Use Visual Studio Code as Git's default editor"** if it's offered.

**Verify:**
```powershell
git --version
```
Should report `git version 2.x.x`.

### GitHub account (optional but recommended)

Your group will probably want to push your project to GitHub for collaboration and submission.

- The course's sample repo is **public**, so you can `git clone` it without an account.
- For your own group's repo, sign up at <https://github.com> (free).
- Once you have an account, set up authentication for pushing — easiest path is the **GitHub CLI** (<https://cli.github.com>) which handles login interactively. Alternatively, generate a Personal Access Token from your GitHub settings and use it as your password when Git asks.

Skip this section if your group has already decided to host elsewhere (GitLab, BGU's internal Git, a shared zip, etc.) — the lesson doesn't depend on GitHub specifically.

---

## 6. VSCode

Download: <https://code.visualstudio.com> — pick the Windows installer (user version is fine).

Install with defaults. When the installer asks about "Additional Tasks", check:
- ✅ **Add "Open with Code" action to Windows Explorer file/directory context menu** (right-click a folder → "Open with Code")
- ✅ **Add to PATH** (so `code` works from any terminal)

After install, open VSCode once to make sure it launches.

### Recommended extensions

Install these from the Extensions panel (Ctrl+Shift+X):

| Extension | Publisher | Why |
|---|---|---|
| **C# Dev Kit** | Microsoft | Syntax highlighting, IntelliSense, and project navigation for the `.cs` files Claude writes. Without it, C# files look like plain text. |
| **C#** | Microsoft | Auto-installed with C# Dev Kit. The underlying language server. |
| **Claude Code** | Anthropic | The AI agent. Covered in Step 7. |

(That's it. Don't install random "popular" extensions until you know what they do — they can interfere with the lesson tooling.)

### Workspace trust

The first time you open your project folder in VSCode, you'll see a banner asking whether you trust the authors of files in this folder. **Click "Yes, I trust the authors"** — without this, several features (including some Claude Code tool calls) are disabled.

**Verify:**
```powershell
code --version
```
Should report a version number on the first line. If "command not found", reopen your terminal — PATH refreshes on new sessions.

---

## 7. Claude Code + Claude Pro/Max Account

This course uses Claude Code as the primary AI agent. You need both:
- **A Claude Pro or Max subscription** (Claude Code is included in both, no separate purchase needed).
- **The Claude Code VSCode extension** installed.

### 7a. Subscribe to Claude Pro or Max

1. Go to <https://claude.ai>.
2. Sign up or sign in.
3. Click your profile → **Upgrade** → pick **Pro** (sufficient for this course) or **Max** (if you want higher limits).
4. Complete checkout.

Without a subscription, Claude Code will not work — there is no free tier for the agent.

### 7b. Install the Claude Code extension

1. Open VSCode.
2. Open Extensions panel (Ctrl+Shift+X).
3. Search **"Claude Code"** (published by Anthropic).
4. Click **Install**.

### 7c. Sign in

1. Open the Claude Code panel from the VSCode sidebar (look for the Claude icon, or `Ctrl+Shift+P` → "Claude Code: Sign In").
2. Sign in with the same Anthropic account you used for your Pro/Max subscription.
3. Authorize the extension in your browser when prompted.

**Verify:** Open Claude Code in VSCode, type "hello, are you connected?" and send. You should get a response. If you see "no active subscription" or a sign-in loop, your subscription isn't active yet — wait 1-2 minutes after subscribing, then retry.

---

## 8. `uv` (Python runner for the MSSQL MCP)

The MSSQL MCP server is a Python program that runs via `uv`'s `uvx` command. It connects Claude Code to your local SQL Server during the lesson.

**Install:**
```powershell
winget install astral-sh.uv
```

If `winget` isn't available, use pip (requires Python already installed):
```powershell
pip install uv
```

**Verify:**
```powershell
uvx --version
```
Should report a version number. If "command not found", restart your terminal — PATH refreshes on new sessions.

---

## Final verification — run this before you call it done

Open a **fresh** PowerShell window and run each command. **All four should succeed.**

```powershell
dotnet --version          # 8.x.x
git --version             # git version 2.x.x
uvx --version             # uv 0.x.x
sqlcmd -L                 # lists local SQL Server instances, including yours
```

If any of these fail, fix that one before class — they're all required, and Claude can't help with broken installs.

---

## Bring-to-class checklist

The day of class, verify in this order:

- [ ] VS 2025 opens and shows the dotnet version
- [ ] SSMS connects to `localhost\SQLEXPRESS` with Windows Authentication
- [ ] VSCode opens
- [ ] Claude Code extension is signed in (test message returns a response)
- [ ] `dotnet --version`, `git --version`, `uvx --version`, `sqlcmd -L` all work in a fresh terminal
- [ ] You have your group's Part A + Part B PDFs accessible

If anything in this list fails the morning of class, message the group chat *immediately* — sooner is cheaper to fix than during the first 10 minutes of class.

---

## Common problems

| Symptom | Likely cause | Fix |
|---|---|---|
| `dotnet` reports 7.x or "not found" | .NET 8 SDK not installed | Re-run VS Installer → Modify → check ".NET desktop development" |
| Built app says "You must install or update .NET" at launch | Desktop Runtime missing (only SDK installed) | Install .NET 8 Windows Desktop Runtime (Step 2 above) |
| SSMS: "Cannot connect to localhost\SQLEXPRESS" | SQL Server service stopped | `Start-Service MSSQL$SQLEXPRESS` in Admin PowerShell. Set startup type to Automatic in `services.msc`. |
| SSMS: "A network-related or instance-specific error" | Wrong instance name | Run `sqlcmd -L` to list local instances. Use whichever name appears. |
| Hebrew text comes back as `?????` | Column is `VARCHAR` not `NVARCHAR` | Always use `NVARCHAR` for Hebrew text. Recreate the table. |
| SSMS: "Login failed for user" | Trying SQL auth instead of Windows auth | For this course, always use **Windows Authentication**. |
| `uvx: command not found` after install | Terminal hasn't refreshed PATH | Close and reopen the terminal. If still missing, check that `winget` actually placed `uv` in PATH. |
| Claude Code: "no active subscription" | Subscription not yet propagated, or signed into the wrong Anthropic account | Wait 2 minutes after subscribing, then sign out + sign back in. Confirm the account is the one with the subscription. |
| Visual Studio designer is broken / missing | Outdated VS install | Update VS via Help → Check for Updates. If you're on VS 2022, upgrade to VS 2025 — the WinForms designer for .NET 8 is more reliable on 2025. |

# SAD Sample Project — פרויקט לדוגמה בקורס ניתוח ועיצוב מערכות מידע - הנדסת תעשיה וניהול - אוניברסיטת בן-גוריון

פרויקט דוגמה לקורס **Software Analysis and Design** (SAD).
הפרויקט מדגים מערכת WinForms מלאה עם חיבור ל-SQL Server, כולל דוגמאות לירושה, Enum, מחלקת קישור (Association Class), וניווט מבוסס Panels.

> **הערה:** בקורס אפשר לעבוד מול **Azure SQL Database** בענן (מומלץ — בסיס נתונים
> אחד משותף לכל הקבוצה) או מול **SQL Server Express מקומי**. בחרו מסלול אחד לכל
> הקבוצה. מדריך ההתקנה המלא, כולל השוואה בין המסלולים, נמצא ב-[`PREREQS.md`](PREREQS.md).

## טכנולוגיות

- **C# / .NET 8** — Windows Forms
- **Azure SQL Database** או **SQL Server Express** — Stored Procedures
- **Visual Studio 2022 / 2025**

## חומרי הקורס (Handouts)

כל חומרי השיעור — כולל המצגת — מופצים כקבצי **HTML עצמאיים** בעברית (RTL): נפתחים בכל
דפדפן, עובדים גם בלי אינטרנט, ואפשר להדפיס אותם ל-PDF (Ctrl+P ← Save as PDF).

| קובץ | תוכן |
|---|---|
| [`dist/PREREQS.html`](dist/PREREQS.html) | התקנות והכנות **לפני** השיעור, כולל יצירת בסיס הנתונים ב-Azure |
| [`dist/PROMPTS_CHEATSHEET.html`](dist/PROMPTS_CHEATSHEET.html) | כל ה-Prompts של השיעור להעתקה־הדבקה (יש כפתור Copy על כל אחד) |
| [`dist/LESSON_STEPS.html`](dist/LESSON_STEPS.html) | המדריך המלא של השיעור, עם ההסברים |
| [`dist/GIT_GROUP_WORKFLOW.html`](dist/GIT_GROUP_WORKFLOW.html) | עבודה עם git בקבוצה |
| [`dist/SLIDE_DECK.html`](dist/SLIDE_DECK.html) | מצגת השיעור — נפתחת בדפדפן. ניווט בחיצים או בשלט מצגות, `O` לתצוגת סקירה, `F` למסך מלא |

המקור של כל אלה הוא קובצי ה-Markdown בשורש הפרויקט. אחרי עריכה, הריצו `bash build-docs.sh`
כדי לייצר מחדש את התיקייה `dist/`.

## איך להתחיל

1. לחצו על **"Use this template"** כדי ליצור עותק לצוות שלכם
2. עקבו אחר המדריכים בתיקיית `docs/`

## מדריכים

| # | מדריך | תיאור |
|---|-------|-------|
| 🏢 | [תיאור ארגון](docs/org-analysis/01-organization.md) | פרופיל קדם אספקה בע"מ, מבנה ארגוני, מערכות קיימות |
| 🎤 | [ראיונות בעלי עניין](docs/org-analysis/02-interviews.md) | סיכומי ראיונות עם מנהלת משמרת, עובד שטח ומנכ"ל |
| ⚠️ | [טבלת בעיות](docs/org-analysis/03-problems.md) | בעיות עסקיות, גורמים, ומיפוי לדרישות ולהחלטות עיצוב |
| 00 | [מסמך דרישות](docs/00-requirements.md) | User Stories, דרישות פונקציונליות ולא-פונקציונליות, Traceability Matrix |
| 00e | [מפרטי Use Cases](docs/00e-use-cases.md) | מפרטי VP18 ל-5 UC מפורטים — קלט ישיר לפיתוח ול-AI agents |
| 00e 🖼 | [דיאגרמת Use Cases (HTML)](order_management_use_case_diagram.html) | דיאגרמה אינטראקטיבית — לחיצה על UC מציגה מפרט מלא |
| 01 | [התקנה](docs/01-installation.md) | Visual Studio 2022/2025, .NET 8, SSMS, Azure SQL או מקומי |
| 02 | [הקמת DB](docs/02-database-setup.md) | הרצת הסקריפט מול Azure, אימות טבלאות ו-SPs |
| 03 | [סקירת הפרויקט](docs/03-project-overview.md) | מבנה, ארכיטקטורה, מחלקות |
| 04 | [Class Diagram → DB](docs/04-class-diagram-to-database.md) | מיפוי טיפוסים, Enum, ירושה, Association Class |
| 05 | [חיבור DB](docs/05-database-connection.md) | SQL_CON — איך עובד החיבור |
| 06 | [סדר פיתוח](docs/06-development-order.md) | הצעדים הראשונים + תבנית Entity |
| 07 | [ניווט מסכים](docs/07-form-navigation.md) | Panel Navigation — חלון יחיד |
| 08 | [Git לצוות](docs/08-git-teamwork.md) | עבודה בצוות, חלוקת עבודה, merge conflicts |
| 09 | [דיאגרמות](docs/09-diagrams.md) | Class Diagram, ERD, ניווט (Mermaid) |
| 10 | [כלי AI](docs/10-ai-tools-guide.md) | שימוש מתודי ב-AI בפרויקט |
| 11 | [צ'קליסט מימוש](docs/11-implementation-checklist.md) | צ'קליסט שלב-אחר-שלב עם הפניות לדוגמאות |

## מבנה הפרויקט

```
├── docs/                    ← מדריכים לסטודנטים
├── scripts/
│   └── create_database.sql  ← סקריפט יצירת DB
└── example_project/         ← קוד המקור
    ├── Program.cs           ← נקודת התחלה + רשימות
    ├── SQL_CON.cs           ← חיבור DB
    ├── Worker.cs            ← ישות + CRUD + init + seek
    ├── Order.cs             ← ישות אב + ירושה
    ├── DeliveryOrder.cs     ← ירושה מ-Order
    ├── PickupOrder.cs       ← ירושה מ-Order
    ├── Product.cs           ← ישות בסיסית
    ├── OrderItem.cs         ← Association Class
    ├── Title.cs             ← Enum + TitleHelper
    ├── mainForm.cs          ← חלון ראשי + showPanel
    └── *Panel.cs            ← מסכי UI (UserControls)
```

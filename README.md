# SAD Sample Project — פרויקט לדוגמה בקורס ניתוח ועיצוב מערכות מידע - הנדסת תעשיה וניהול - אוניברסיטת בן-גוריון

פרויקט דוגמה לקורס **Software Analysis and Design** (SAD).
הפרויקט מדגים מערכת WinForms מלאה עם חיבור ל-SQL Server, כולל דוגמאות לירושה, Enum, מחלקת קישור (Association Class), וניווט מבוסס Panels.

## טכנולוגיות

- **C# / .NET 8** — Windows Forms
- **SQL Server** — Stored Procedures
- **Visual Studio 2022**

## איך להתחיל

1. לחצו על **"Use this template"** כדי ליצור עותק לצוות שלכם
2. עקבו אחר המדריכים בתיקיית `docs/`

## מדריכים

| # | מדריך | תיאור |
|---|-------|-------|
| 01 | [התקנה](docs/01-installation.md) | Visual Studio, .NET 8, SQL Server, SSMS |
| 02 | [הקמת DB](docs/02-database-setup.md) | הרצת סקריפט, אימות טבלאות ו-SPs |
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

---
title: "עבודה עם Git בקבוצה"
subtitle: "תיאום פרויקט קבוצתי בלי לאבד עבודה"
course: "ניתוח ועיצוב מערכות מידע — אוניברסיטת בן-גוריון, הנדסת תעשייה וניהול"
author: "מרצה: דוד קודיש"
lang: he
dir: rtl
---

המדריך הזה מיועד לקבוצות של 2–5 סטודנטים שעובדים על אותו פרויקט C#. הוא מכסה את מינימום העבודה עם git שאתם צריכים כדי לא לאבד עבודה וכדי להסתנכרן עם חברי הקבוצה.

---

## למה git חשוב לקבוצה שלכם

בלי git קורה אחד משניים: או שרק אדם אחד יכול לערוך קבצים בכל רגע נתון, או ששני אנשים עורכים את אותו קובץ והעבודה של אחד מהם נמחקת. git פותר את זה — כל אחד עובד על עותק משלו, ו-git ממזג את השינויים.

הקובץ `CLAUDE.md` ש-Claude Code קורא, תיקיית `scripts/`, וכל קובצי המקור ב-C# — כל אלה דברים שכמה חברי קבוצה עשויים לגעת בהם. שמרו אותם ב-git, וכולם יישארו מסונכרנים.

---

## הקמה: אדם אחד יוצר את ה-Repository

עושים את זה פעם אחת, מוקדם בשלב 1. חבר הקבוצה שיוצר את תיקיית הפרויקט מבצע את השלב הזה.

> **אם כבר הקמתם את ה-repository לפני השיעור** (ראו [`PREREQS`](./PREREQS.md) חלק ג4) — דלגו לסעיף הבא.
> שם מפורט גם תהליך ההזמנה של חברי הקבוצה כ-collaborators, כולל **אישור ההזמנה** —
> השלב שהכי הרבה אנשים מפספסים.

1. היכנסו ל-[github.com](https://github.com) → **New repository**
2. קראו לו `sad-groupname`. הגדירו אותו כ-**Private**. **אל** תאתחלו אותו עם README (כבר יש לכם קבצים).
3. אחרי היצירה, GitHub יציג כתובת בסגנון `https://github.com/username/sad-groupname.git` — העתיקו אותה.
4. ב-Claude Code, בקשו:
   > Initialize a git repository here if one doesn't exist, then add the remote: `git remote add origin https://github.com/username/sad-groupname.git`

5. בצעו את ה-commit הראשון (אם עוד לא נעשה) ו-push:
   > Commit all current files and push to origin main: `git push -u origin main`

6. שתפו את כתובת ה-repository עם כל חברי הקבוצה, והזמינו אותם כ-collaborators:
   ה-repository ← **Settings** ← **Collaborators** ← **Add people** ← שם המשתמש שלהם ב-GitHub.

> **כל מוזמן חייב לאשר את ההזמנה** — במייל שאיתו נרשם ל-GitHub, או ב-<https://github.com/notifications>.
> בלי אישור אין לו גישה, וה-`git clone` ייכשל. השלבים המלאים ב-[`PREREQS`](./PREREQS.md) חלק ג4.

---

## כל חבר קבוצה: שכפול ה-Repository

כל שאר חברי הצוות (חוץ מזה שיצר אותו) משכפלים את ה-repository:

ב-Claude Code, פתחו תיקייה ריקה חדשה, ואז:
> Clone our group repo: `git clone https://github.com/username/sad-groupname.git .`

(הנקודה `.` בסוף משכפלת לתוך התיקייה הנוכחית.)

---

## שגרת עבודה יומית: Pull ← עבודה ← Commit ← Push

בכל פעם שאתם מתיישבים לעבוד:

**1. קודם כל Pull.**
> Pull the latest changes from the group repo: `git pull origin main`

עשו את זה **לפני** שאתם מבצעים שינויים. אם תדלגו על השלב הזה, אתם עובדים על עותק מיושן ותצטרכו לפתור קונפליקטים בהמשך.

**2. עבדו.** בצעו את השינויים שלכם ב-Claude Code.

**3. בצעו commit כשסיימתם יחידת עבודה משמעותית.**

נקודות ה-commit המסומנות ב-`LESSON_STEPS` הן המינימום. אפשר לבצע commit גם לעתים קרובות יותר — commit הוא זול וגם ניתן לביטול.

> Commit the files I changed. Write a short message describing what changed.

**4. בצעו Push.**
> Push to origin main: `git push origin main`

---

## חלוקת עבודה בתוך הקבוצה

כל שלב ב-`LESSON_STEPS` מייצר קבצים מסוימים. חלקו את השלבים בין חברי הקבוצה כדי למזער את הסיכוי ששני אנשים יערכו את אותו קובץ באותו זמן:

| מי | מה |
|-----|------|
| חבר א׳ | שלב 2 (חילוץ המסמכים), שלב 3 (`CLAUDE.md`) |
| חבר ב׳ | שלב 4 (סכמת בסיס הנתונים וה-Stored Procedures) |
| חבר א׳ + ב׳ | שלב 5 ביחד — הישות הראשונה ומסך הכניסה צריכים להתקמפל יחד |
| חלוקה לפי ישות | שלב 6 — הקצו מסכי CRUD ספציפיים לכל חבר |

זה לא כלל נוקשה, אבל זה מקטין קונפליקטים. כששני אנשים חייבים לערוך את אותו קובץ (למשל שניהם מוסיפים מסכים לאותו מסך בית), תאמו לפי זמן — אחד מסיים ומבצע push לפני שהשני מתחיל.

---

## פתרון קונפליקטים

קונפליקט קורה כששני אנשים ערכו את אותו קובץ לפני שאחד מהם ביצע push. git ידווח על כך ויסמן את הקובץ בסימני קונפליקט (`<<<<`, `====`, `>>>>`). זה נראה מפחיד אבל זה בר-טיפול.

בקשו מ-Claude Code:
> I have a git conflict in `<filename>`. Show me the conflict markers and help me resolve it — keep the correct version of each section.

Claude יכול לקרוא את סימני הקונפליקט ולעזור לכם לבחור אילו שינויים לשמור. אחרי הפתרון, בצעו commit:
> Mark the conflict as resolved and commit: `git add <filename>` then `git commit`

---

## מה **לא** להעלות ל-git

קובץ ה-`.gitignore` שלכם כבר מחריג את שני הדברים החשובים ביותר:

- **`.mcp.json`** — מכיל את פרטי הגישה לבסיס הנתונים. לעולם אל תעלו אותו.
- **`cloned/`** — פרויקט הדוגמה המשוכפל. יש לו repository משלו; אל תכלילו אותו בשלכם.

בנוסף, לעולם אל תעלו:

- את `app.config.local` או `app.config` אם הוא מכיל סיסמה אמיתית
- קבצים מקומפלים (`bin/`, `obj/`) — הם נוצרים אוטומטית; חברי הצוות בונים אותם מקומית

**אם העליתם בטעות פרטי גישה:** שנו את הסיסמה מיד (הסיסמה נמצאת עכשיו בהיסטוריית git), ואז בקשו מ-Claude לעזור להסיר אותה מההיסטוריה.

---

## נקודות Commit מומלצות (מתוך `LESSON_STEPS`)

| שלב | מה לשמור ב-commit |
|-------|----------------|
| שלב 1 | `.gitignore`, ה-commit הראשון |
| שלב 2 | כל קובצי ה-markdown ב-`docs/` |
| שלב 3 | `CLAUDE.md` |
| שלב 4 | `scripts/create_database.sql`, `scripts/stored_procedures.sql`, `scripts/seed_data.sql` |
| שלב 5 | שלד הפרויקט ב-C#, `SQL_CON.cs`, הישות הראשונה, המסך הראשון, `mainForm`, `LoginPanel` |
| שלב 6 | כל שאר המסכים ומסכי הבית המחוברים |

אחרי כל push: כל חברי הצוות צריכים לבצע `git pull` לפני שהם ממשיכים בעבודה שלהם.

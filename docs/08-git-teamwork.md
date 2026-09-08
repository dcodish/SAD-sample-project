# עבודה בצוות עם Git

מדריך זה מסביר כיצד להשתמש ב-Git כדי לעבוד בצוות על הפרויקט.
Git מאפשר לכל חברי הצוות לעבוד על הקוד במקביל, בלי לדרוס אחד את השני.

## 1. מה זה Git?

Git הוא כלי לניהול גרסאות — הוא עוקב אחרי כל שינוי שנעשה בקוד.

```
                    ┌──────────────┐
                    │   GitHub     │  ← המאגר המרוחק (משותף לכל הצוות)
                    │   (Remote)   │
                    └──────┬───────┘
                           │
           ┌───────────────┼───────────────┐
           │               │               │
     ┌─────┴─────┐  ┌─────┴─────┐  ┌─────┴─────┐
     │ סטודנט 1  │  │ סטודנט 2  │  │ סטודנט 3  │
     │ (Local)   │  │ (Local)   │  │ (Local)   │
     └───────────┘  └───────────┘  └───────────┘
```

**מושגים חשובים:**
| מושג | הסבר |
|------|-------|
| **Repository (Repo)** | תיקיית הפרויקט + כל היסטוריית השינויים |
| **Remote** | המאגר ב-GitHub (משותף לכולם) |
| **Local** | העותק במחשב שלכם |
| **Commit** | "תמונת מצב" של השינויים שעשיתם |
| **Push** | שליחת ה-commits שלכם ל-GitHub |
| **Pull** | קבלת שינויים חדשים מ-GitHub |
| **Merge Conflict** | כששני אנשים שינו את אותו קובץ |

## 2. הכנה חד-פעמית

### התקנת Git
1. הורידו מ: https://git-scm.com/downloads
2. התקינו עם ההגדרות ברירת מחדל
3. פתחו Command Prompt ובדקו:
```
git --version
```

### הגדרת שם ומייל
הריצו פעם אחת (עם הפרטים שלכם):
```
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

### יצירת חשבון GitHub
1. היכנסו ל: https://github.com
2. צרו חשבון חינמי
3. **כל חברי הצוות** צריכים חשבון GitHub

## 3. יצירת ה-Repository של הצוות

> **⚠️ ה-repository של הצוות מתחיל ריק — לא מעותק של פרויקט הדוגמה.**
> אל תשתמשו בכפתור **"Use this template"** של `SAD-sample-project`. הפרויקט שלכם
> מתאר את **התחום שלכם**, ואתם בונים אותו מאפס לאורך השיעור. פרויקט הדוגמה משוכפל
> בשלב 1 לתת-תיקייה `cloned/` **לעיון בלבד**, והיא נכנסת ל-`.gitignore` כדי שלא
> תיכנס ל-repository שלכם.

התהליך המלא מתואר ב-[`GIT_GROUP_WORKFLOW`](../GIT_GROUP_WORKFLOW.md); זה התקציר.

### א. חבר צוות אחד יוצר Repository ריק
1. היכנסו ל-<https://github.com> ← **New repository**
2. תנו שם (למשל: `sad-groupname`)
3. סמנו **Private** (פרטי — רק הצוות רואה)
4. **אל תאתחלו אותו עם README** — כבר יש לכם קבצים בתיקייה המקומית
5. לחצו **Create repository** והעתיקו את הכתובת שמוצגת

### ב. חיבור התיקייה המקומית ל-Repository
אותו חבר צוות, מתוך תיקיית הפרויקט (זו שיצרתם בשלב 0 ועבדתם בה בשלב 1), מבקש מ-Claude Code:

> Initialize a git repository here if one doesn't exist, then add the remote:
> `git remote add origin https://github.com/username/sad-groupname.git`
> Then commit all current files and push: `git push -u origin main`

### ג. הוספת חברי הצוות
1. ב-GitHub: **Settings > Collaborators > Add people**
2. הוסיפו את שאר חברי הצוות לפי שם המשתמש שלהם ב-GitHub

> **כל מוזמן חייב לאשר את ההזמנה** — במייל שאיתו נרשם ל-GitHub, או ב-<https://github.com/notifications>.
> בלי אישור אין לו גישה וה-`git clone` ייכשל. זה השלב שהכי הרבה קבוצות מפספסות.

### ד. שאר חברי הצוות משכפלים
כל אחד פותח **תיקייה ריקה חדשה** ומבקש מ-Claude Code:

> Clone our group repo: `git clone https://github.com/username/sad-groupname.git .`

(הנקודה `.` בסוף משכפלת לתוך התיקייה הנוכחית.)

פתחו את ה-`.sln` ב-Visual Studio — אבל שימו לב: הוא קיים רק אחרי ששלב 5 הושלם.
לפני כן ה-repository מכיל את המסמכים והסקריפטים בלבד.

## 4. ה-Commit הראשון שלכם

לפני שמתחילים לעבוד, כל חבר צוות צריך לוודא שהוא יכול לעשות push:

1. שנו משהו קטן (למשל הוסיפו הערה בקובץ)
2. הריצו:
```
git add .
git commit -m "בדיקת חיבור - [השם שלכם]"
git push
```
3. אם ה-push הצליח — הכל עובד!

## 5. העבודה היום-יומית — 4 פקודות

### לפני שמתחילים לעבוד — תמיד תמשכו שינויים:
```
git pull
```
> **זה הכלל הכי חשוב!** תמיד תעשו pull לפני שאתם מתחילים לכתוב קוד.

### אחרי שסיימתם שינוי — שמרו ושלחו:
```
git add .
git commit -m "הוספת מחלקת Customer"
git push
```

> **אפשר להריץ את הפקודות האלה גם דרך Claude Code** — פשוט בקשו ממנו ("commit and push
> my changes with the message ..."). זה שימושי במיוחד אם `git` לא נמצא ב-PATH של הטרמינל
> שלכם, תקלה נפוצה שגורמת ל-`command not found`.

### סיכום הזרימה:
```
┌─────────────────────────────────────────────┐
│  1. git pull          ← קבלו שינויים חדשים │
│  2. עבודה על הקוד     ← כתבו קוד           │
│  3. git add .         ← סמנו מה להעלות     │
│  4. git commit -m "..." ← שמרו תמונת מצב   │
│  5. git push          ← שלחו ל-GitHub       │
└─────────────────────────────────────────────┘
```

## 6. הודעות Commit — איך לכתוב

הודעת commit טובה מסבירה **מה** עשיתם ו**למה**:

### ✅ הודעות טובות:
```
git commit -m "הוספת מחלקת Customer עם CRUD ו-initCustomers"
git commit -m "תיקון באג בטעינת הזמנות - שדה תאריך היה null"
git commit -m "הוספת פאנל יצירת לקוח חדש"
```

### ❌ הודעות גרועות:
```
git commit -m "fix"
git commit -m "changes"
git commit -m "asdfjkl"
```

## 7. חלוקת עבודה — איך להימנע מקונפליקטים

הדרך הטובה ביותר להימנע מקונפליקטים: **כל אחד עובד על קבצים שונים.**

### חלוקה מומלצת:

```
סטודנט 1: Customer.cs + CustomerPanel.cs (+ טבלה ו-SPs ב-DB)
סטודנט 2: Product.cs + ProductPanel.cs (+ טבלה ו-SPs ב-DB)
סטודנט 3: Order.cs + OrderPanel.cs (+ טבלה ו-SPs ב-DB)
סטודנט 4: Invoice.cs + InvoicePanel.cs (+ טבלה ו-SPs ב-DB)
```

**כללי זהב:**
1. כל סטודנט עובד על **ישות אחת בכל פעם** (Entity + Panel)
2. **אל תעבדו על אותו קובץ** במקביל
3. את `Program.cs` משנים **בזהירות** — רק הוספת רשימה וקריאה ב-initLists
4. עשו **pull לפני push** תמיד
5. עשו **commit קטנים ותכופים** — לא commit ענק בסוף

### דוגמה לזרימת עבודה:

```
בוקר:
  כולם עושים git pull

סטודנט 1 עובד על Customer.cs:
  git add Customer.cs CustomerPanel.cs CustomerPanel.Designer.cs
  git commit -m "הוספת מחלקת Customer עם createCustomer ו-initCustomers"
  git push

סטודנט 2 עובד על Product.cs:
  git pull    ← קודם מושכים את השינויים של סטודנט 1
  git add Product.cs ProductPanel.cs ProductPanel.Designer.cs
  git commit -m "הוספת מחלקת Product"
  git push
```

## 8. Merge Conflicts — מה לעשות כשיש קונפליקט

קונפליקט קורה כשנושאים **שני אנשים שינו את אותה שורה** באותו קובץ.

### איך זה נראה:
```
<<<<<<< HEAD
// הקוד שלכם
Worker.initWorkers();
Customer.initCustomers();
=======
// הקוד של חבר הצוות
Worker.initWorkers();
Product.initProducts();
>>>>>>> origin/main
```

### איך לפתור:
1. פתחו את הקובץ ב-Visual Studio
2. תראו את שני הגרסאות מסומנות
3. מחקו את סימני הקונפליקט (`<<<<`, `====`, `>>>>`)
4. שמרו את הקוד הנכון — **שילוב של שני השינויים**:
```csharp
Worker.initWorkers();
Customer.initCustomers();
Product.initProducts();
```
5. שמרו, ואז:
```
git add .
git commit -m "פתרון קונפליקט ב-Program.cs"
git push
```

### איך להימנע מקונפליקטים:
- עשו **pull תכוף** — לא רק בבוקר
- **אל תעבדו על Program.cs במקביל** — דברו בינכם לפני שמשנים אותו
- עשו **push מיד** אחרי commit — אל תצברו שינויים

## 9. פקודות שימושיות

| פקודה | מתי |
|-------|------|
| `git status` | לראות מה השתנה |
| `git log --oneline` | לראות היסטוריית commits |
| `git diff` | לראות בדיוק מה שיניתם |
| `git pull` | למשוך שינויים מ-GitHub |
| `git add .` | לסמן את כל השינויים |
| `git commit -m "..."` | לשמור תמונת מצב |
| `git push` | לשלוח ל-GitHub |

## 10. Git ב-Visual Studio

אפשר גם להשתמש ב-Git ישירות מתוך Visual Studio:

1. **View > Git Changes** — חלון שמראה את כל השינויים
2. כתבו הודעת commit בשדה למעלה
3. לחצו **Commit All** לשמירה
4. לחצו **Push** לשליחה ל-GitHub
5. לחצו **Pull** לקבלת שינויים

> **טיפ:** גם אם משתמשים ב-Visual Studio, כדאי להכיר את הפקודות ב-Command Prompt — הן עובדות תמיד.

## 11. סיכום — 5 כללי הזהב

1. **Pull לפני שמתחילים** — תמיד
2. **כל אחד עובד על קבצים שונים** — חלוקה לפי ישויות
3. **Commit קטנים ותכופים** — עם הודעות ברורות
4. **Push מיד** — אל תצברו commits
5. **דברו בינכם** — לפני שמשנים קובץ משותף כמו Program.cs

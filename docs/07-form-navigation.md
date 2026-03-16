# ניווט בין מסכים — Panel Navigation

מדריך זה מסביר כיצד עובד הניווט בין מסכים בפרויקט.

## 1. העיקרון: חלון אחד, תוכן מתחלף

במקום לפתוח חלונות חדשים לכל מסך, יש לנו **חלון ראשי אחד** (`mainForm`) עם **Panel** שמחליף את התוכן שבתוכו.

```
┌─────────────────────────────────┐
│  mainForm (חלון יחיד)           │
│  ┌───────────────────────────┐  │
│  │                           │  │
│  │   panelMain               │  │
│  │                           │  │
│  │   כאן נטען תוכן מתחלף:   │  │
│  │   LoginPanel              │  │
│  │   CRUDPanel               │  │
│  │   CreateWorkerPanel       │  │
│  │   ...                     │  │
│  │                           │  │
│  └───────────────────────────┘  │
└─────────────────────────────────┘
```

## 2. איך זה עובד

### mainForm — המעטפת
```csharp
public partial class mainForm : Form
{
    private static mainForm instance;

    public mainForm()
    {
        InitializeComponent();
        instance = this;
        showPanel(new LoginPanel());  // מסך ראשון
    }

    // זו הדרך היחידה לנווט בין מסכים!
    public static void showPanel(UserControl panel)
    {
        instance.panelMain.Controls.Clear();  // ניקוי המסך הקודם
        panel.Dock = DockStyle.Fill;          // מילוי כל השטח
        instance.panelMain.Controls.Add(panel);
    }
}
```

### כל מסך — UserControl
```csharp
public partial class CRUDPanel : UserControl  // לא Form!
{
    // ...
}
```

## 3. ניווט — שורה אחת

כדי לעבור למסך אחר, שורה אחת:

```csharp
mainForm.showPanel(new CRUDPanel());          // מעבר למסך CRUD
mainForm.showPanel(new LoginPanel());         // חזרה למסך כניסה
mainForm.showPanel(new CreateWorkerPanel());  // מעבר ליצירת עובד
```

### דוגמה — כפתור ניווט:
```csharp
private void button_new_Click(object sender, EventArgs e)
{
    mainForm.showPanel(new CreateWorkerPanel());
}
```

### דוגמה — חזרה אחרי פעולה:
```csharp
private void Add_button_Click(object sender, EventArgs e)
{
    Worker W = new Worker(..., true);         // ביצוע הפעולה
    mainForm.showPanel(new CRUDPanel());      // חזרה למסך הניהול
}
```

## 4. UserControl vs Form — מה ההבדל?

| | Form | UserControl |
|---|------|-------------|
| **מה זה** | חלון עצמאי | פאנל תוכן |
| **יצירה ב-VS** | Add > Windows Form | Add > User Control |
| **Designer** | זהה! אותו drag & drop | זהה! |
| **סרגל כותרת** | יש (title bar, X) | אין |
| **שימוש** | `mainForm` בלבד | כל שאר המסכים |

> **בפרויקט שלנו:** רק `mainForm` הוא Form. כל שאר המסכים הם UserControl.

## 5. זרימת הניווט בדוגמה

```
LoginPanel
  │
  ├── כניסה כמנהל (1111) ──→ CRUDPanel
  │                            ├── יצירת עובד ──→ CreateWorkerPanel ──→ חזרה ל-CRUDPanel
  │                            ├── עדכון/מחיקה ──→ UpdateDeletePanel ──→ חזרה ל-CRUDPanel
  │                            ├── הזמנת משלוח ──→ CreateDeliveryOrderPanel ──→ חזרה ל-CRUDPanel
  │                            ├── הזמנת איסוף ──→ CreatePickupOrderPanel ──→ חזרה ל-CRUDPanel
  │                            └── חזור ──→ LoginPanel
  │
  ├── כניסה כעובד ──→ WatchOrdersPanel ──→ חזרה ל-LoginPanel
  │
```

## 6. איך להוסיף מסך חדש

1. Right Click על הפרויקט > **Add > User Control**
2. תנו שם שמסתיים ב-`Panel` (למשל `CustomerListPanel`)
3. עצבו את המסך ב-Designer (drag & drop)
4. הוסיפו כפתור חזרה שמנווט בחזרה:
```csharp
private void back_Click(object sender, EventArgs e)
{
    mainForm.showPanel(new CRUDPanel());  // או למסך שממנו הגעתם
}
```
5. מהמסך שממנו רוצים להגיע — הוסיפו כפתור:
```csharp
private void button_customers_Click(object sender, EventArgs e)
{
    mainForm.showPanel(new CustomerListPanel());
}
```

## 7. העברת נתונים בין מסכים

אם מסך צריך לקבל נתונים (למשל: מסך הזמנות צריך לדעת איזה עובד), העבירו דרך הבנאי:

```csharp
// ניווט עם העברת נתונים
mainForm.showPanel(new WatchOrdersPanel(emp));  // מעבירים את העובד

// במסך המקבל
public partial class WatchOrdersPanel : UserControl
{
    Worker w;
    public WatchOrdersPanel(Worker w)  // מקבלים בבנאי
    {
        InitializeComponent();
        this.w = w;
    }
}
```

## 8. סיכום — חוקי הניווט

1. **לנווט למסך** → `mainForm.showPanel(new MyPanel())`
2. **לחזור** → `mainForm.showPanel(new PreviousPanel())`
3. **להעביר נתונים** → דרך הבנאי של ה-Panel
4. **להוסיף מסך** → Add > User Control, שם שמסתיים ב-Panel
5. **לעולם לא** → `Show()`, `Hide()`, `Close()`, `ShowDialog()`

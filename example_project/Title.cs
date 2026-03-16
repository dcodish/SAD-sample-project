using System;

namespace Example_Project
{
    /// <summary>
    /// Enum לתפקידי עובדים.
    /// הערכים מוגדרים כאן וגם בטבלת Lookup (Titles) בבסיס הנתונים.
    /// ב-DB נשמרים כטקסט עם רווחים ("מנהל משמרת").
    /// ב-Enum משתמשים בקו תחתון כי רווחים אסורים ("מנהל_משמרת").
    /// TitleHelper ממיר בין שני הפורמטים.
    /// </summary>
    public enum Title
    {
        מנהל_משמרת,
        ראש_צוות,
        עובד_חדש
    }

    /// <summary>
    /// מחלקת עזר להמרה בין Enum לטקסט תצוגה.
    /// קו תחתון (C#) ↔ רווחים (DB / תצוגה)
    /// </summary>
    public static class TitleHelper
    {
        //המרה מ-enum לטקסט תצוגה (עם רווחים)
        public static string ToDisplayString(Title title)
        {
            return title.ToString().Replace('_', ' ');
        }

        //המרה מטקסט תצוגה (עם רווחים) ל-enum
        public static Title FromDisplayString(string displayString)
        {
            string enumString = displayString.Replace(' ', '_');
            return (Title)Enum.Parse(typeof(Title), enumString);
        }
    }
}

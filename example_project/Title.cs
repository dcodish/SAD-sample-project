using System;

namespace Example_Project
{
    public enum Title
    {
        מנהל_משמרת,
        ראש_צוות,
        עובד_חדש
    }

    //מחלקת עזר להמרה בין enum לטקסט תצוגה
    //ב-enum אי אפשר להשתמש ברווחים, לכן משתמשים בקו תחתון
    //ובהמרה לתצוגה מחליפים קו תחתון ברווח
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

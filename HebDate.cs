using System;
using System.Globalization;

namespace DateConv
{
    internal class HebDate
    {
        public static int[] ConvertToHebrewDate(DateTime gregorianDate)
        {
            HebrewCalendar hebrewCalendar = new HebrewCalendar();

            int year = (hebrewCalendar.GetYear(gregorianDate)) % 1000;
            int month = hebrewCalendar.GetMonth(gregorianDate);
            int day = hebrewCalendar.GetDayOfMonth(gregorianDate);
            int elef = (hebrewCalendar.GetYear(gregorianDate)) / 1000;
            // Return an array with day, month, year, and a placeholder value (e.g., 0)
            return new int[] { day, month, year, elef };
        }

        public static DateTime ConvertIntToGregorianDate(int hebrewYear, int hebrewMonth, int hebrewDay)
        {
            HebrewCalendar hebrewCalendar = new HebrewCalendar();

            try
            {
                // יצירת תאריך לועזי מתוך תאריך עברי
                DateTime gregorianDate = hebrewCalendar.ToDateTime(hebrewYear, hebrewMonth, hebrewDay, 0, 0, 0, 0);
                return gregorianDate;
            }
            catch (ArgumentOutOfRangeException)
            {
                // טיפול במקרה של תאריך עברי לא תקין
                if (hebrewDay == 30)
                {
                    try
                    {                         // אם התאריך הוא ה-30 בחודש, ננסה להמיר אותו ל-29
                        DateTime gregorianDate = hebrewCalendar.ToDateTime(hebrewYear, hebrewMonth, 29, 0, 0, 0, 0);
                        throw new ArgumentException("בחודש שנבחר אין יום ל'.");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                    }
                }
                throw new ArgumentException("התאריך העברי שסופק אינו תקין.");
            }
        }
        public static DateTime convertStrToGregDate(string str)
        {
            string[] parts = str.Split(new[] { ' ', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int[] hebDate = new int[3];
            if (parts.Length == 3)
            {
                hebDate[0] = Gimatria.ConvertToGimatria(parts[0]); // day
                hebDate[2] = Gimatria.ConvertToGimatria(parts[2]); // year
                hebDate[1] = Gimatria.HebCodeshNameToNumber(parts[1], hebDate[2]); // month
            }
            else if (parts.Length == 4)
            {
                hebDate[0] = Gimatria.ConvertToGimatria(parts[0]); // day
                hebDate[2] = Gimatria.ConvertToGimatria(parts[3]); // year
                hebDate[1] = Gimatria.HebCodeshNameToNumber(parts[1] + " " + parts[2], hebDate[2]); // month
            }

            DateTime GregDate = ConvertIntToGregorianDate(hebDate[2], hebDate[1], hebDate[0]);
            return GregDate;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DateConv
{
    internal class Parse
    {
        /// <summary>
        /// Takes a date from the calendar and returns it in DateTime format.
        /// </summary>
        /// <param name="isHebrew">Indicates whether the input date is Hebrew or Gregorian.</param>
        /// <param name="day">The day of the month.</param>
        /// <param name="month">The month (name or number).</param>
        /// <param name="year">The year.</param>
        /// <returns>A DateTime object representing the date.</returns>
        public static DateTime GetDateFromCalendar(bool isHebrew, string day, string month, string year)
        {
            if (isHebrew)
            {
                // Convert Hebrew date to Gregorian date
                int hebDay = Gimatria.ConvertToGimatria(day);
                int hebYear = Gimatria.ConvertToGimatria(year);
                int hebMonth = Gimatria.HebCodeshNameToNumber(month, hebYear);
                return HebDate.ConvertIntToGregorianDate(hebYear, hebMonth, hebDay);
            }
            else
            {
                // Convert Gregorian date components to DateTime
                int gregDay = int.Parse(day);
                int gregMonth = int.Parse(month);
                int gregYear = int.Parse(year);
                return new DateTime(gregYear, gregMonth, gregDay);
            }
        }
    }
}

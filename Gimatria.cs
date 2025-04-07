using System;
using System.Globalization;

namespace DateConv
{
    internal class Gimatria
    {

        public static int ConvertToGimatria(string hebrewText)
        {
            int total = 0;
            for (int i = 0; i < hebrewText.Length; i++)
            {
                char c = hebrewText[i];

                // Check if the character is a Hebrew letter
                if (c >= 'א' && c <= 'ת')
                {
                    int value = GetGimatriaValue(c);

                    // Check if the next character is a geresh (׳)
                    if (i + 1 < hebrewText.Length && hebrewText[i + 1] == '\'')
                    {
                        value *= 1000; // Multiply the value by 1,000
                        
                    }

                    total += value;
                }
            }
            return total;
        }
        public static int GetGimatriaValue(char hebrewChar)
        {
            // Convert the Hebrew character to its Gimatria value
            switch (hebrewChar)
            {
                case 'א': return 1;
                case 'ב': return 2;
                case 'ג': return 3;
                case 'ד': return 4;
                case 'ה': return 5;
                case 'ו': return 6;
                case 'ז': return 7;
                case 'ח': return 8;
                case 'ט': return 9;
                case 'י': return 10;
                case 'כ': return 20;
                case 'ל': return 30;
                case 'מ': return 40;
                case 'נ': return 50;
                case 'ס': return 60;
                case 'ע': return 70;
                case 'פ': return 80;
                case 'צ': return 90;
                case 'ק': return 100;
                case 'ר': return 200;
                case 'ש': return 300;
                case 'ת': return 400;
                default: return 0; // Non-Hebrew characters are ignored
            }
        }
        public static string GimatriaToHebrewLetters(int number)
        {
            string strTemp = "";
            if (number > 900)
            {
                strTemp += "תתק";
            }
            else if (number > 800)
            {
                strTemp += "תת";
            }
            else if (number > 700)
            {
                strTemp += "תש";
            }
            else if (number > 600)
            {
                strTemp += "תר";
            }
            else if (number > 500)
            {
                strTemp += "תק";
            }
            else if (number > 400)
            {
                strTemp += "ת";
            }
            else if (number > 300)
            {
                strTemp += "ש";
            }
            else if (number > 200)
            {
                strTemp += "ר";
            }
            else if (number > 100)
            {
                strTemp += "ק";
            }
            int Digit = (number % 100) / 10;

            if (Digit > 0)
            {
                switch (Digit)
                {
                    case 1: strTemp += "י"; break;
                    case 2: strTemp += "כ"; break;
                    case 3: strTemp += "ל"; break;
                    case 4: strTemp += "מ"; break;
                    case 5: strTemp += "נ"; break;
                    case 6: strTemp += "ס"; break;
                    case 7: strTemp += "ע"; break;
                    case 8: strTemp += "פ"; break;
                    case 9: strTemp += "צ"; break;
                }
            }
            int Digit2 = number % 10;
            if (Digit2 > 0)
            {
                switch (Digit2)
                {
                    case 1: strTemp += "א"; break;
                    case 2: strTemp += "ב"; break;
                    case 3: strTemp += "ג"; break;
                    case 4: strTemp += "ד"; break;
                    case 5: strTemp += "ה"; break;
                    case 6: strTemp += "ו"; break;
                    case 7: strTemp += "ז"; break;
                    case 8: strTemp += "ח"; break;
                    case 9: strTemp += "ט"; break;
                }
            }
            //בדיקה אם קיים במשתנה STRTEMP הצירוף "יה" וא"כ להחליף ל"טו" ואת "יו" ל"טז" ו"רעה" ל"ערה" ו"רצח" ל"רחצ" ו"רע" ל"ער" ו"שד ל"דש" ושמד" לשדמ" 
            if (strTemp.Contains("יה"))
            {
                strTemp = strTemp.Replace("יה", "טו");
            }
            else if (strTemp.Contains("יו"))
            {
                strTemp = strTemp.Replace("יו", "טז");
            }
            else if (strTemp.Contains("רעה"))
            {
                strTemp = strTemp.Replace("רעה", "ערה");
            }
            else if (strTemp.Contains("רצח"))
            {
                strTemp = strTemp.Replace("רצח", "רחצ");
            }
            else if (strTemp.Contains("רע"))
            {
                strTemp = strTemp.Replace("רע", "ער");
            }
            else if (strTemp.Contains("שד"))
            {
                strTemp = strTemp.Replace("שד", "דש");
            }
            else if (strTemp.Contains("שמד"))
            {
                strTemp = strTemp.Replace("שמד", "שדמ");
            }
            return strTemp;
        }
        public static string NumberToHebChodesName(int hebrewMonth, int Shana)
        {
            HebrewCalendar hebrewCalendar = new HebrewCalendar(); // מאתחל את לוח השנה העברי
            string[] hebrewMonths = new string[] {
            "","תשרי", "חשוון", "כסלו", "טבת", "שבט", "אדר", "אדר א'", "אדר ב'","ניסן", "אייר", "סיון", "תמוז", "אב", "אלול"
            };

            string monthName = ""; // משתנה לאחסון שם החודש העברי

            if (hebrewMonth < 6)
            {
                monthName = hebrewMonths[hebrewMonth];
            }
            else if (hebrewCalendar.IsLeapYear(Shana))
            {
                hebrewMonth++;
                monthName = hebrewMonths[hebrewMonth];
            }
            else if (hebrewMonth == 6)
            {
                monthName = hebrewMonths[hebrewMonth];
            }
            else
            {
                hebrewMonth += 2;
                monthName = hebrewMonths[hebrewMonth];
            }
            return monthName;

        }
        public static int HebCodeshNameToNumber(string monthName, int YearNumber)
        {
            HebrewCalendar hebrewCalendar = new HebrewCalendar(); // מאתחל את לוח השנה העברי

            // Convert Hebrew month name to its corresponding number
            switch (monthName)
            {
                case "תשרי": return 1;
                case "חשוון":
                case "מרחשון": // Added for compatibility
                case "חשון":  // Added for compatibility
                case "מר-חשוון":
                case "מרחשוון": return 2; // Added for compatibility
                case "כסלו":
                case "כסליו": return 3; // Added for compatibility
                case "טבת": return 4;
                case "שבט": return 5;
                
                //case "אדר א": return 6;
                //case "אדר ב": return 7;
                default:
                    if (hebrewCalendar.IsLeapYear(YearNumber))
                    {
                        switch (monthName)
                        {
                            case "אדר": throw new ArgumentException("השנה שנבחרה הינה שנה מעוברת יש לבחור אדר א' או ב'");
                            case "אדר א": return 6;
                            case "אדר ב": return 7;
                            case "ניסן": return 8;
                            case "אייר": return 9;
                            case "סיון": return 10;
                            case "תמוז": return 11;
                            case "אב": return 12;
                            case "אלול": return 13;
                        }
                    }
                    else
                    {
                        switch (monthName)
                        {
                            case "אדר": return 6;
                            case "אדר א": throw new ArgumentException("השנה שנבחרה אינה מעוברת");
                            case "אדר ב": throw new ArgumentException("השנה שנבחרה אינה מעוברת");
                            case "ניסן": return 7;
                            case "אייר": return 8;
                            case "סיון": return 9;
                            case "תמוז": return 10;
                            case "אב": return 11;
                            case "אלול": return 12;
                        }
                    }
                    break; // Ensure control does not fall out of the switch
            }

            // Return a default value if no match is found
            return 0;
        }
    }
}

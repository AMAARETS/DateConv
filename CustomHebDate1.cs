using System;

namespace DateConv
{
    internal class CustomHebDate
    {
        private string _day;
        private string _month;
        private string _year;

        public string Day
        {
            get { return _day; }
            set { _day = value; }
        }

        public string Month
        {
            get { return _month; }
            set { _month = value; }
        }

        public string Year
        {
            get { return _year; }
            set { _year = value; }
        }

        public string GetHebDate
        {
            return $"{_day} {_month} {_year}";
        }
        public void setHebDate (DataTime Date)
        {
                // Convert the input Gregorian date to a Hebrew date
                
                if (DateTime.TryParse(value, out Date))
                {
                    string hebrewDate = HebDate.ConvertToHebrewDate(Date);
                    var parts = hebrewDate.Split(' ');

                    if (parts.Length == 3)
                    {
                        _day = parts[0];
                        _month = parts[1];
                        _year = parts[2];
                    }
                    else
                    {
                        throw new ArgumentException("Invalid Hebrew date format.");
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid Gregorian date format.");
                }
            }
        }
    }
}

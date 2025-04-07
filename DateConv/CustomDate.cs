using System;

namespace DateConv
{
    internal class CustomDate
    {
        private int _day;
        private int _month;
        private int _year;

        public int Day
        {
            get { return _day; }
            set { _day = value; }
        }

        public int Month
        {
            get { return _month; }
            set { _month = value; }
        }

        public int Year
        {
            get { return _year; }
            set { _year = value; }
        }

        public void SetDate(DateTime date)
        {
            _day = date.Day;
            _month = date.Month;
            _year = date.Year;
        }

        public DateTime GetDate()
        {
            try
            {
                return new DateTime(_year, _month, _day);
            }
            catch (ArgumentOutOfRangeException)
            {
                if (_day > 28)
                {
                        for (int i = 1; i < 4; i++)
                        {
                        try
                        {
                            DateTime dateNisuy = new DateTime(_year, _month, _day - i);
                            throw new ArgumentException($"בחודש זה ישנם רק {_day-i} ימים");
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                        }
                    }
                }
                // Handle the case where the date is invalid
                throw new ArgumentException("הערך לא תקין");
            }
        }
    }
}
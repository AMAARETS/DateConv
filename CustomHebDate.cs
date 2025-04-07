using System;

namespace DateConv
{
    internal class CustomHebDate
    {
        private string _day;
        private string _month;
        private string _year;
        private string _elef;
        //private HebDate hebDate;
        //private Gimatria gimatria;
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

        public string Elef
        {
            get { return _elef; }
            set { _elef = value; }
        }

        public string GetHebDate()
        {
            return $"{_day} {_month} {_elef}'{_year}";
        }
        public void setHebDate(DateTime Date)
        {
            // Convert the input Gregorian date to a Hebrew date

            try
            {
                int[] hebrewDate = HebDate.ConvertToHebrewDate(Date);
                //hebDate = new HebDate();
                //gimatria = new Gimatria();
                _day = Gimatria.GimatriaToHebrewLetters(hebrewDate[0]);
                _month = Gimatria.NumberToHebChodesName(hebrewDate[1], hebrewDate[2] + (hebrewDate[3])*1000);
                _year = Gimatria.GimatriaToHebrewLetters(hebrewDate[2]);
                _elef = Gimatria.GimatriaToHebrewLetters(hebrewDate[3]);

            }
            catch (ArgumentException)
            {
                //throw new ArgumentException("Invalid Gregorian date format.");
                _day = "kkkkk";
            }
        }
    }
}


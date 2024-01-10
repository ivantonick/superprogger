using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    internal class CalendarService
    {
        private CalendarStorage storage;
        public CalendarService() {
            storage = new CalendarStorage() { Dates = new List<DateTimeBox>() };
        }

        public CalendarService(CalendarStorage storage)
        {
            this.storage = storage;
        }

        public bool IsLeapYear(int year)
        {
            DateTime yearObj = new DateTime(year, 1, 1);
            SaveDate(yearObj);
            return DateTime.IsLeapYear(year);
        }

        public int CalcIntervalLength(DateTime from, DateTime to)
        {
            SaveDate(from);
            SaveDate(to);
            return (to - from).Days;
        }

        public string GetDayOfWeek(DateTime date)
        {
            SaveDate(date);
            return date.ToString("dddd");
        }

        public IEnumerable<DateTime> Dates
        {
            get
            {
                return storage.Dates.Select((d) => d.Value);
            }
        }

        public CalendarStorage Storage { get { return storage; } }

        private void SaveDate(DateTime date)
        {
            storage.Dates.Add(new DateTimeBox() { Value = date });
        }
    }
}

using System;
using System.Globalization;
using System.Linq;

namespace iSy.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToBecomTimestamp(this DateTime timestamp, bool withPlus1 = true)
        {
            string ret = $"{timestamp.Year:D4}-{timestamp.Month:D2}-{timestamp.Day:D2}";
            ret += $"-{timestamp.Hour:D2}.{timestamp.Minute:D2}.{timestamp.Second:D2}";
            ret += $".{timestamp.Millisecond.ToString().PadRight(6, '0')}";

            if (withPlus1) ret += "+01:00";

            return ret;
        }

        public static DateTime FromBecomTimestamp(this string becomTimestamp)
        {
            var dateItems = becomTimestamp.Split('-');
            string parseString = $"{dateItems[2]}.{dateItems[1]}.{dateItems[0]}";
            var timeString = dateItems.Last().Split('+').First();
            var timeItems = timeString.Split('.');
            parseString += $" {timeItems[0]}:{timeItems[1]}:{timeItems[2]}.{timeItems[3]}";

            return DateTime.Parse(parseString, new CultureInfo("de"));
        }

        public static string ToIBMTimestampString(this DateTime timestamp)
        {
            string ret = $"{timestamp.Year:D4}-{timestamp.Month:D2}-{timestamp.Day:D2} {timestamp.Hour:D2}:{timestamp.Minute:D2}:00";
            return ret;
        }

        public static (DateTime first, DateTime last) GetTimestampsForADay(this DateTime adate)
        {
            var f = DateTime.Parse($"{adate.ToShortDateString()}");
            var l = DateTime.Parse($"{adate.AddDays(1).ToShortDateString()}");
            return (f, l);
        }
        public static bool IsSaturday(this DateTime dateTime)
        {
            if (dateTime.DayOfWeek == DayOfWeek.Saturday) return true;
            else return false;
        }

        public static bool IsSunday(this DateTime dateTime)
        {
            if (dateTime.DayOfWeek == DayOfWeek.Sunday) return true;
            else return false;
        }

        public static DateTime NormalizeAsOnlyDate(this DateTime date) => DateTime.Parse(date.ToShortDateString());

        public static DateTime NormalizeDateTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
        }

        public static DateTime NormalizeFullHour(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
        }

        public static DateTime FirstDayOfMonth(this DateTime dayOfMonth)
        {
            return new DateTime(dayOfMonth.Year, dayOfMonth.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime dayOfMonth)
        {
            return dayOfMonth.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        public static DateTime FirstDayOfYear(this DateTime dayOfMonth)
        {
            return new DateTime(dayOfMonth.Year, 1, 1);
        }

        public static DateTime LastDayOfYear(this DateTime dayOfMonth)
        {
            return dayOfMonth.FirstDayOfYear().AddYears(1).AddDays(-1);
        }

        public static string ToMonthView(this DateTime date)
        {
            return date.ToString("MMMM yyyy", new CultureInfo("de-AT"));
        }

        public static int GetYears(this TimeSpan timespan)
        {
            return (int)(timespan.Days / 365.2425);
        }
        public static int GetMonths(this TimeSpan timespan)
        {
            return (int)(timespan.Days / 30.436875);
        }

    }
}

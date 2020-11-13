using Becom.EDI.PersonalDataExchange.Model.Enums;
using System;
using System.Xml.Linq;

namespace Becom.EDI.PersonalDataExchange.Extensions
{
    public static class ZeiterfassungsServiceExtensions
    {
        public static DateTime ToDateShort(this XElement element, string key)
        {
            var strSource = element.Element(element.Name.Namespace + key).Value;
            var strYear = strSource.Substring(strSource.Length - 2);
            var strMonth = strSource.Substring(strSource.Length - 4, 2);
            var strDay = strSource.Length > 5 ? strSource.Substring(0, 2) : strSource.Substring(0, 1);

            var strFullYear = Convert.ToInt32(strYear) > 80 ? $"19{strYear}" : $"20{strYear}";

            var year = Convert.ToInt32(strFullYear);
            var month = Convert.ToInt32(strMonth);
            var day = Convert.ToInt32(strDay);

            return new DateTime(year, month, day);
        }

        public static DateTime ToDate(this XElement element, string key)
        {
            var value = element.Element(element.Name.Namespace + key).Value;

            var strYear = value.Substring(0, 4);
            var strMonth = value.Substring(4, 2);
            var strDay = value.Substring(6, 2);

            return new DateTime(Convert.ToInt32(strYear), Convert.ToInt32(strMonth), Convert.ToInt32(strDay));
        }

        public static DateTime ToDate2(this XElement element, string key)
        {
            var strYear = "";
            var strMonth = "";
            var strDay = "";

            var value = element.Element(element.Name.Namespace + key).Value;

            if (value.Length > 7)
            {
                strYear = value.Substring(4, 4);
                strMonth = value.Substring(2, 2);
                strDay = value.Substring(0, 2);
            }
            else
            {
                strYear = value.Substring(3, 4);
                strMonth = value.Substring(1, 2);
                strDay = value.Substring(0, 1);
            }

            return new DateTime(Convert.ToInt32(strYear), Convert.ToInt32(strMonth), Convert.ToInt32(strDay));
        }

        public static string FromDate(this DateTime source)
        {
            var year = source.ToString("yyyy");
            var month = source.ToString("MM");
            var day = source.ToString("dd");
            if (day.StartsWith("0")) day = day.Substring(1, 1);
            return $"{day}{month}{year}";
        }

        public static int ToInt(this XElement element, string key)
        {
            var value = element.Element(element.Name.Namespace + key).Value;
            return Convert.ToInt32(value);
        }

        public static CompanyEnum ToCompany(this XElement element, string key)
        {
            var value = element.Element(element.Name.Namespace + key).Value;
            return (CompanyEnum)Convert.ToInt32(value);
        }

        public static int ToEmployeeId(this XElement element, string key)
        {
            return element.ToInt(key);
        }

        public static TimeSpan ToMinutesTimeSpan(this XElement element, string key)
        {
            var value = element.Element(element.Name.Namespace + key).Value;
            var dbl = double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            return TimeSpan.FromMinutes(dbl);
        }

        public static TimeSpan ToHourTimeSpan(this XElement element, string key)
        {
            var value = element.Element(element.Name.Namespace + key).Value;
            var dbl = double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            return TimeSpan.FromHours(dbl);
        }

        public static DateTime ToTime(this XElement element, string key, DateTime date)
        {
            var value = element.Element(element.Name.Namespace + key).Value;

            var hour = value.Substring(0, 2);
            var min = value.Substring(3, 2);
            var hourDbl = double.Parse(hour, System.Globalization.CultureInfo.InvariantCulture);
            var minDbl = double.Parse(min, System.Globalization.CultureInfo.InvariantCulture);

            return date.AddHours(hourDbl).AddMinutes(minDbl);
        }
    }
}

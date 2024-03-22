using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Mhazami.Utility
{
    public static class DateTimeUtil
    {
        public static List<int> GetShamsiYearsList(int from, int? to = null)
        {
            var list = new List<int>();
            if (from == 0 || from.ToString().Length != 4) return list;
            var persianCalendar = new PersianCalendar();
            if (to == null) to = persianCalendar.GetYear(DateTime.Now);
            for (int i = from; i <= to; i++)
                list.Add(i);
            return list;

        }
        public static string GetDateTimeDescription(DateTime dateTime)
        {
            string str = string.Empty;
            switch (GetDayOfWeek(dateTime))
            {
                case (int)DayOfWeek.Sunday:
                    str = "Sunday";
                    break;
                case (int)DayOfWeek.Monday:
                    str = "Monday";
                    break;
                case (int)DayOfWeek.Tuesday:
                    str = "Tuesday";
                    break;
                case (int)DayOfWeek.Wednesday:
                    str = "Wednesday";
                    break;
                case (int)DayOfWeek.Thursday:
                    str = "Thursday";
                    break;
                case (int)DayOfWeek.Friday:
                    str = "Friday";
                    break;
                case (int)DayOfWeek.Saturday:
                    str = "Saturday";
                    break;
            }
            int dayOfMonth = GetDayOfMonth(dateTime);
            string monthString = DateTimeUtil.GetMonthString((short)GetMonth(dateTime));
            int year = GetYear(dateTime);
            return string.Format("{0} {1} {2} {3} ", (object)str, (object)dayOfMonth, (object)monthString, (object)year);

        }
        public static int GetShamsiYearNow()
        {
            return new PersianCalendar().GetYear(DateTime.Now);
        }
        public static string GetStartDate(DateTime dateTime)
        {
            if (Utils.IsPersianCulture)
                return dateTime.ShamsiDate().Substring(0, 4) + "/01" + "/01";
            if (Utils.IsHijriCulture)
                return dateTime.HijriDate().Substring(0, 4) + "/01" + "/01";
            return dateTime.Year + "/01" + "/01";
        }
        public static string GetFinishDate(DateTime dateTime)
        {
            if (Utils.IsPersianCulture)
                return dateTime.ShamsiDate().Substring(0, 4) + "/12" + "/30";
            if (Utils.IsHijriCulture)
                return dateTime.HijriDate().Substring(0, 4) + "/12" + "/30";
            return dateTime.Year + "/12" + "/31";
        }

        public static DateTime GetStartDateToGregorianDate(DateTime dateTime)
        {
            return GetDateToGregorianDate(GetStartDate(dateTime));
        }
        public static DateTime GetFinishDateToGregorianDate(DateTime dateTime)
        {
            return GetDateToGregorianDate(GetFinishDate(dateTime));
        }
        public static int GetMonthDayCount(int month)
        {
            if (Utils.IsPersianCulture)
            {
                return month > 6 ? 30 : 31;
            }
            if (Utils.IsHijriCulture)
            {
                if (month == 12)
                    return 30;
                return month % 2 == 0 ? 29 : 30;
            }
            switch (month)
            {
                case 2:
                    return 29;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                default:
                    return 31;
            }
        }
        public static int GetYearNow()
        {
            if (Utils.IsPersianCulture)
                return new PersianCalendar().GetYear(DateTime.Now);
            if (Utils.IsHijriCulture)
                return new HijriCalendar().GetYear(DateTime.Now);
            return DateTime.Now.Year;
        }
        public static int GetYear(string date)
        {
            if (string.IsNullOrEmpty(date)) throw new Exception("the date is null or empty");
            string[] parts = date.Split('/');
            if (parts.Length != 3)
                throw new Exception("Incorrect format in  date.");
            return parts[0].ToInt();
        }
        public static int GetMonth(string date)
        {
            if (string.IsNullOrEmpty(date)) throw new Exception("the date is null or empty");
            string[] parts = date.Split('/');
            if (parts.Length != 3)
                throw new Exception("Incorrect format in  date.");
            return parts[1].ToInt();
        }
        public static int GetDayOfMonth(string date)
        {
            if (string.IsNullOrEmpty(date)) throw new Exception("the date is null or empty");
            string[] parts = date.Split('/');
            if (parts.Length != 3)
                throw new Exception("Incorrect format in  date.");
            return parts[2].ToInt();
        }
        public static int GetYear(DateTime dateTime)
        {
            if (Utils.IsPersianCulture)
                return new PersianCalendar().GetYear(dateTime);
            if (Utils.IsHijriCulture)
                return new HijriCalendar().GetYear(dateTime);
            return dateTime.Year;
        }
        public static int GetMonth(DateTime dateTime)
        {
            if (Utils.IsPersianCulture)
                return new PersianCalendar().GetMonth(dateTime);
            if (Utils.IsHijriCulture)
                return new HijriCalendar().GetMonth(dateTime);
            return dateTime.Month;


        }
        public static int GetDayOfMonth(DateTime dateTime)
        {
            if (Utils.IsPersianCulture)
                return new PersianCalendar().GetDayOfMonth(dateTime);
            if (Utils.IsHijriCulture)
                return new HijriCalendar().GetDayOfMonth(dateTime);
            return dateTime.Day;

        }



        public static class PersianDate
        {

            public static CultureInfo PersianCulture()
            {
                var result = new CultureInfo("fa-IR")
                {
                    DateTimeFormat =
                    {
                        FirstDayOfWeek = DayOfWeek.Saturday,
                        DayNames =
                            new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" },
                        MonthNames =
                            new[]
                                                 {
                                                     "فروردین", "اردیبهشت", "خرداد","تیر", "مرداد", "شهریور","مهر",
                                                    "آبان", "آذر", "دی", "بهمن", "اسفند", ""
                                                 }
                    }
                };
                return result;
            }

            public static string FormatTime(string time)
            {
                if (string.IsNullOrEmpty(time)) return time;
                var parts = time.Split(':');
                if (parts.Length != 2)
                    throw new Exception("Time string was not in correct format.");
                return string.Format("{0:D2}:{1:D2}", parts[0].ToInt(), parts[1].ToInt());
            }
        }

        public static string GetCultureDate(this DateTime date, string culture)
        {
            switch (culture)
            {
                case "fa-IR":
                    return date.ShamsiDate();
                default:
                    return date.GregorianDate();
            }
        }

        public static string GetCultureDate(this string date, string culture)
        {
            switch (culture)
            {
                case "fa-IR":
                    return date;
                default:
                    return GetDateToGregorianDate(date).GregorianDate();
            }
        }
        public static string GetCultureDate(this DateTime date)
        {
            if (Utils.IsPersianCulture)
                return date.ShamsiDate();
            if (Utils.IsHijriCulture)
                return date.HijriDate();
            return date.GregorianDate();
        }
        public static string GetTime(this DateTime date)
        {
            return string.Format("{0:D2}:{1:D2}", date.Hour, date.Minute);
        }

        public static string GetTimewithSecond(this DateTime date)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", date.Hour, date.Minute, date.Second);
        }

        public static string ShamsiDate(this DateTime date)
        {
            var pc = new PersianCalendar();
            return string.Format("{0:D4}/{1:D2}/{2:D2}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date));
        }

        public static string HijriDate(this DateTime date)
        {
            var hc = new HijriCalendar();
            return string.Format("{0:D4}/{1:D2}/{2:D2}", hc.GetYear(date), hc.GetMonth(date), hc.GetDayOfMonth(date));
        }
        public static string GregorianDate(this DateTime date)
        {

            return string.Format("{0:D4}/{1:D2}/{2:D2}", date.Year, date.Month, date.Day);
        }
        public static DateTime GetDateToGregorianDate(string date)
        {
            if (string.IsNullOrEmpty(date)) throw new Exception("the date is null or empty");
            string[] parts = date.Split('/');
            if (parts.Length != 3)
                throw new Exception("Incorrect format in  date.");
            if (Utils.IsPersianCulture)
                return new PersianCalendar().ToDateTime(parts[0].ToInt(), parts[1].ToInt(), parts[2].ToInt(), 0, 0, 0, 0);
            if (Utils.IsHijriCulture)
                return new HijriCalendar().ToDateTime(parts[0].ToInt(), parts[1].ToInt(), parts[2].ToInt(), 0, 0, 0, 0);
            return new DateTime(parts[0].ToInt(), parts[1].ToInt(), parts[2].ToInt(), 0, 0, 0, 0);
        }
        public static DateTime GetStartOfWeek(DateTime input)
        {

            int dayOfWeek = (((int)input.DayOfWeek) + 6) % 7;
            return input.Date.AddDays(-dayOfWeek);
        }

        public static int GetWeeks(DateTime start, DateTime end, bool roundtodown = true)
        {
            start = GetStartOfWeek(start);
            end = GetStartOfWeek(end);
            int days = (int)(end - start).TotalDays;
            double value = (days / 7);
            if (!roundtodown)
            {
                var round = Math.Round(value, 0, MidpointRounding.ToEven);
                return (int)(round + 1);
            }
            return (int)Math.Floor(value);

        }
        public static int GetDays(DateTime start, DateTime end)
        {
            TimeSpan ts = end.Subtract(start);
            return ts.Days;
        }
        public static int GetYears(DateTime start, DateTime end)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan span = end - start;
            return (zeroTime + span).Year - 1;
        }
        public static List<int> GetYearsList(DateTime start, DateTime? end = null)
        {
            var list = new List<int>();
            if (end == null) end = DateTime.Now;
            var startyear = GetYear(start);
            var endyear = GetYear((DateTime)end);
            for (int i = startyear; i <= endyear; i++)
                list.Add(i);
            return list;

        }

        public static double GetMonths(DateTime start, DateTime end)
        {
            var value = end.Subtract(start).Days / (365.25 / 12);
            var months = Math.Round(value, 0, MidpointRounding.ToEven);
            return months;
        }
        public static string ShamsiDateToHijriDate(string date)
        {
            if (string.IsNullOrEmpty(date)) return date;
            return GetDateToGregorianDate(date).HijriDate();
        }

        public static int ComputeBetweenDate(string StartDate, string EndDate)
        {
            if (string.IsNullOrEmpty(StartDate) || string.IsNullOrEmpty(EndDate))
                return 0;


            var sDate = GetDateToGregorianDate(StartDate);
            var eDate = GetDateToGregorianDate(EndDate);
            return GetDays(sDate, eDate);

        }

        public static void ParseDate(string Date, ref int Year, ref int Month, ref int Day)
        {
            if (string.IsNullOrEmpty(Date)) return;
            if (Date.Length != 10)
                throw new ArgumentException("The date format is not in correct format.");
            try
            {
                Year = Date.Substring(0, 4).ToInt();
                Month = Date.Substring(5, 2).ToInt();
                Day = Date.Substring(8, 2).ToInt();
            }
            catch
            {
                throw new ArgumentException("The date format is not in correct format.");
            }
        }

        public static string AddToDate(string date, int year, int month, int day)
        {
            if (string.IsNullOrEmpty(date)) return date;
            if (date.Length == 10)
            {
                try
                {
                    var gregorianDate = GetDateToGregorianDate(date);
                    var timeSpan = gregorianDate.AddMonths(month).AddYears(year).AddDays(day);
                    return GetCultureDate(timeSpan);

                }
                catch
                {
                    return "The date format is not in correct format.";
                }
            }
            return "The date format is not in correct format.";
        }

        public static string GetMonthString(short monthId)
        {
            string str = "";
            switch (monthId)
            {
                case 1:
                    str = "فروردین";
                    break;
                case 2:
                    str = "اردیبهشت";
                    break;
                case 3:
                    str = "خرداد";
                    break;
                case 4:
                    str = "تیر";
                    break;
                case 5:
                    str = "مرداد";
                    break;
                case 6:
                    str = "شهریور";
                    break;
                case 7:
                    str = "مهر";
                    break;
                case 8:
                    str = "آبان";
                    break;
                case 9:
                    str = "آذر";
                    break;
                case 10:
                    str = "دی";
                    break;
                case 11:
                    str = "بهمن";
                    break;
                case 12:
                    str = "اسفند";
                    break;
            }
            return str;
        }

        public static string ComputeBetweenDateTime(string greaterDate, string greaterTime, string lowerDate, string lowerTime)
        {
            var dateTime = GetDifferenceBetweenDateTime(greaterDate, greaterTime, lowerDate, lowerTime);
            return GetDifferenceBetweenTextValue(dateTime);


        }

        public static string GetDifferenceBetweenTextValue(TimeSpan timeSpan)
        {
            string result;
            var dayDiff = timeSpan.Days;
            var hourDiff = timeSpan.Hours;
            var minuteDiff = timeSpan.Minutes;

            if (dayDiff == 0)
            {
                if (hourDiff == 0)
                {
                    if (minuteDiff == 0) minuteDiff++;
                    result = string.Format("{0} Minute(s) ", minuteDiff);
                }
                else
                {
                    result = string.Format("{0} Hour(s) ", hourDiff);
                    if (minuteDiff > 0)
                        result += " And " + minuteDiff + " Minute(s) ";
                }
            }
            else
            {
                result = string.Format("{0} Day(s) ", dayDiff);
                if (hourDiff > 0 && dayDiff < 5)
                    result += " And " + hourDiff + " Hour(s) ";
            }

            return (result.Contains("-") ? result.Replace("-", "") + "remained" : result + "past");
        }
        public static TimeSpan GetDifferenceBetweenDateTime(string greaterDate, string greaterTime, string lowerDate, string lowerTime)
        {

            var greater = new TimeSpan(greaterTime.Split(':')[0].ToInt(), greaterTime.Split(':')[1].ToInt(), 0);
            var lower = new TimeSpan(lowerTime.Split(':')[0].ToInt(), lowerTime.Split(':')[1].ToInt(), 0);
            var fromDate = GetDateToGregorianDate(greaterDate);
            var toDate = GetDateToGregorianDate(lowerDate);
            fromDate = fromDate.AddHours(greater.Hours);
            fromDate = fromDate.AddMinutes(greater.Minutes);
            toDate = toDate.AddHours(lower.Hours);
            toDate = toDate.AddMinutes(lower.Minutes);
            return fromDate.Subtract(toDate);



        }
        public static int GetDayOfWeek(DateTime dateTime)
        {
            if (Utils.IsPersianCulture)
                return (int)new PersianCalendar().GetDayOfWeek(dateTime);
            if (Utils.IsHijriCulture)
                return (int)new HijriCalendar().GetDayOfWeek(dateTime);
            return (int)dateTime.DayOfWeek;
        }
        public static string GetDayOfWeekString(short dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1:
                    return "Saturday";
                case 2:
                    return "Sunday";
                case 3:
                    return "Monday";
                case 4:
                    return "Tuesday";
                case 5:
                    return "Wednesday";
                case 6:
                    return "Thursday";
                case 7:
                    return "Friday";
                default:
                    return string.Empty;
            }
        }
        public static string GetDateMonth(this string Date)
        {
            var year = 0;
            var month = 0;
            var day = 0;
            ParseDate(Date, ref year, ref month, ref day);
            var monthString = GetMonthString((short)month);
            Date = string.Format("{0} {1} {2}", day, monthString, year);
            return Date;
        }
        public static bool ValidateParsianDate(string date)
        {
            bool status = true;

            try
            {
                PersianCalendar persianCalendar = new PersianCalendar();
                var dateParts = date.Split(new char[] { '/' }).Select(d => int.Parse(d)).ToArray();
                var pdate = persianCalendar.ToDateTime(dateParts[0], dateParts[1], dateParts[2], 0, 0, 0, 0 /*8 era of year here **/);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                status = false;
            }

            return status;
        }
        public struct DateRange
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
        }

        public static DateRange TodayRange()
        {
            DateTime date = DateTime.Now;
            DateRange range = new DateRange
            {
                Start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0),
                End = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59)
            };

            return range;
        }
        public static DateRange YesterdayRange()
        {
            DateTime date = DateTime.Now.AddDays(-1);
            DateRange range = new DateRange();
            range.Start = new DateTime(date.Year, date.Month, date.Day);
            range.End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddSeconds(-1);

            return range;
        }
        public static DateRange PreviousDayRange()
        {
            DateTime date = DateTime.Now.AddDays(-2);
            DateRange range = new DateRange
            {
                Start = new DateTime(date.Year, date.Month, date.Day)
            };
            range.End = range.Start.AddDays(1).AddSeconds(-1);

            return range;
        }
        public static DateRange ThisYearRange()
        {
            DateTime date = DateTime.Now;
            DateRange range = new DateRange();

            range.Start = new DateTime(date.Year, 1, 1);
            range.End = range.Start.AddYears(1).AddSeconds(-1);

            return range;



        }

        public static DateRange LastYearRange()
        {
            DateTime date = DateTime.Now;
            DateRange range = new DateRange();

            range.Start = new DateTime(date.Year - 1, 1, 1);
            range.End = range.Start.AddYears(1).AddSeconds(-1);

            return range;

        }

        public static DateRange ThisMonthRange()
        {
            DateTime date = DateTime.Now;
            DateRange range = new DateRange();

            range.Start = new DateTime(date.Year, date.Month, 1);
            range.End = range.Start.AddMonths(1).AddSeconds(-1);

            return range;



        }
        public static DateRange LastCustomeDayRange(int day)
        {
            DateTime date = DateTime.Now;
            DateRange range = new DateRange
            {
                Start = (new DateTime(date.Year, date.Month, date.Day)).AddDays(-(day - 1)),
                End = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59)
            };
            return range;
        }
        public static DateRange NextCustomeDayRange(int day)
        {
            DateTime date = DateTime.Now;
            DateRange range = new DateRange
            {
                Start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0),
                End = (new DateTime(date.Year, date.Month, date.Day, 23, 59, 59)).AddDays((day - 1))
            };
            return range;
        }
        public static DateRange TomorrowDayRange()
        {
            DateTime date = DateTime.Now.AddDays(1);
            DateRange range = new DateRange
            {
                Start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0),
                End = (new DateTime(date.Year, date.Month, date.Day, 23, 59, 59))
            };
            return range;
        }
        public static DateRange ThedayaftertomorrowDayRange()
        {
            DateTime date = DateTime.Now.AddDays(2);
            DateRange range = new DateRange
            {
                Start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0),
                End = (new DateTime(date.Year, date.Month, date.Day, 23, 59, 59))
            };
            return range;
        }


        public static DateRange LastWeekRange()
        {

            DateRange range = ThisWeekRange();
            range.Start = range.Start.AddDays(-7);
            range.End = range.End.AddDays(-7);
            return range;
        }
        public static DateRange LastMonthRange()
        {
            DateTime date = DateTime.Now;
            DateRange range = new DateRange
            {
                Start = (new DateTime(date.Year, date.Month, 1)).AddMonths(-1)
            };
            range.End = range.Start.AddMonths(1).AddSeconds(-1);

            return range;


        }

        public static DateRange ThisWeekRange()
        {
            DateTime date = DateTime.Now;
            DayOfWeek weekStart = Utils.IsPersianCulture ? DayOfWeek.Saturday : DayOfWeek.Monday;
            if (date.DayOfWeek == weekStart)
                return LastCustomeDayRange(-7);
            DateRange range = new DateRange
            {
                Start = date.Date.AddDays(-(int)date.DayOfWeek)
            };
            range.End = range.Start.AddDays(7).AddSeconds(-1);
            return range;



        }
    }
}

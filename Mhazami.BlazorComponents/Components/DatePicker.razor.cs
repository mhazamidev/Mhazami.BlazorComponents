using Mhazami.BlazorComponents.Models;
using Mhazami.BlazorComponents.Utility;
using Mhazami.Utility;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Mhazami.BlazorComponents.Components;

public partial class DatePicker
{
    [Parameter] public string Id { get; set; }
    [Parameter] public string Value { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public DateTime? StartDate { get; set; }
    [Parameter] public DateTime? ToDate { get; set; }
    [Parameter] public EventCallback<DateTime> OnChangeAction { get; set; }
    [Parameter] public EventCallback<KeyValuePair<string, DateTime>> OnChangeActionById { get; set; }
    [Parameter] public DatePickerType DatePickerType { get; set; } = DatePickerType.DateTime;
    [Parameter] public CalendarType CalendarType { get; set; } = CalendarType.Gregorian;
    [Parameter] public bool AutoClose { get; set; } = true;

    private DatePickerInfo dateInfo = new DatePickerInfo();
    private bool hide = true;
    private int Year = DateTime.Now.Year;
    private int MonthNum = 1;

    private string MonthNameShort = "";
    private string MonthName = "";
    private string DayNum = "";
    private string DayName = "";
    private string DayNameShort = "";
    private List<DateTime> DateOfMonth = new List<DateTime>();
    private KeyValuePair<int, int> EmptyDates = new KeyValuePair<int, int>();
    private DateTime CurrentValue;
    private DateTime Today = DateTime.Now;
    private List<KeyValuePair<int, string>> Years = new();
    private int Hour = 0;
    private int Minute = 0;


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!string.IsNullOrEmpty(Value))
        {
            var dateOfValue = DateTime.Parse(Value);
            Value = dateOfValue.Date.ToShortDateString();
            CurrentValue = dateOfValue;
            PrepareDate(dateOfValue);
        }
        else
        {
            CurrentValue = DateTime.Now;
            PrepareDate(CurrentValue);
        }

        StateHasChanged();
    }

    void SetFormat(DateTime date)
    {
        string datetime = "";
        string year = "";
        string dates = "";
        switch (CalendarType)
        {
            case CalendarType.Hijri:
                HijriCalendar hc = new();
                datetime = $"{hc.GetYear(date)}/{hc.GetMonth(date)}/{hc.GetDayOfMonth(date)} {hc.GetHour(date)}:{hc.GetMinute(date)}:{hc.GetSecond(date)}";
                year = $"{hc.GetYear(date)}";
                dates = $"{hc.GetYear(date)}/{hc.GetMonth(date)}/{hc.GetDayOfMonth(date)}";
                break;
            case CalendarType.Shamsi:
                PersianCalendar pc = new();
                datetime = $"{pc.GetYear(date)}/{pc.GetMonth(date)}/{pc.GetDayOfMonth(date)} {pc.GetHour(date)}:{pc.GetMinute(date)}:{pc.GetSecond(date)}";
                year = $"{pc.GetYear(date)}";
                dates = $"{pc.GetYear(date)}/{pc.GetMonth(date)}/{pc.GetDayOfMonth(date)}";
                break;
            case CalendarType.Turkish:
            case CalendarType.German:
            case CalendarType.Gregorian:
                datetime = date.ToString();
                year = $"{date.Year}";
                dates = date.ToShortDateString();
                break;
        }

        switch (DatePickerType)
        {
            case DatePickerType.DateTime:
                Value = datetime;
                break;
            case DatePickerType.Date:
                Value = dates;
                break;
            case DatePickerType.Day:
                Value = DayName;
                break;
            case DatePickerType.Time:
                Value = date.ToShortTimeString();
                break;
            case DatePickerType.Month:
                Value = $"{GetMonthName(date)} {year}";
                break;
            case DatePickerType.Year:
                Value = year;
                break;

        }
    }
    void Open() => hide = !hide;
    private void PrepareDate(DateTime dateOfValue, bool applyAutoClose = true)
    {
        Year = dateOfValue.Year;
        MonthNum = dateOfValue.Month;
        DayNum = GetDayByCalendarType(dateOfValue);
        GetDayInfo(dateOfValue);
        GetMonthInfo(dateOfValue);
        Years = DatePickerInfo.BuildYears(CalendarType);
        SetFormat(dateOfValue);
        if (AutoClose && applyAutoClose)
            hide = true;
        StateHasChanged();
    }

    private void GetMonthInfo(DateTime date)
    {
        var month = date.Month;
        switch (CalendarType)
        {
            case CalendarType.Hijri:
                HijriCalendar hc = new HijriCalendar();
                var hy = hc.GetYear(date);
                var hm = hc.GetMonth(date);
                var hh = hc.GetHour(date);
                var hmin = hc.GetMinute(date);
                var hs = hc.GetSecond(date);
                DateOfMonth = Enumerable.Range(1, hc.GetDaysInMonth(date.Year, month))
             .Select(day => hc.ToDateTime(hy, hm, day, hh, hmin, hs, 0))
             .ToList();
                MonthNameShort = GetMonthNameShort(hm);
                break;
            case CalendarType.Shamsi:
                PersianCalendar pc = new PersianCalendar();
                var y = pc.GetYear(date);
                var m = pc.GetMonth(date);
                var h = pc.GetHour(date);
                var min = pc.GetMinute(date);
                var s = pc.GetSecond(date);
                DateOfMonth = Enumerable.Range(1, pc.GetDaysInMonth(date.Year, month))
                .Select(day => pc.ToDateTime(y, m, day, h, min, s, 0))
                .ToList();
                MonthNameShort = GetMonthNameShort(m);
                break;
            case CalendarType.Turkish:
            case CalendarType.German:
            case CalendarType.Gregorian:
                DateOfMonth = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, month))
                   .Select(day => new DateTime(date.Year, month, day))
                   .ToList();
                MonthNameShort = GetMonthNameShort(month);
                break;
        }

        var firstDay = DateOfMonth.First();
        var skipDays = 0;
        var emptyDate = new DateTime(1, 1, 1);
        if (IsRTLCalendar())
        {
            switch (firstDay.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    skipDays = 7 - ((DateOfMonth.Count() + 2) % 7);
                    EmptyDates = new KeyValuePair<int, int>(2, skipDays);
                    break;
                case DayOfWeek.Tuesday:
                    skipDays = 7 - ((DateOfMonth.Count() + 3) % 7);
                    EmptyDates = new KeyValuePair<int, int>(3, skipDays);
                    break;
                case DayOfWeek.Wednesday:
                    skipDays = 7 - ((DateOfMonth.Count() + 4) % 7);
                    EmptyDates = new KeyValuePair<int, int>(4, skipDays);
                    break;
                case DayOfWeek.Thursday:
                    skipDays = 7 - ((DateOfMonth.Count() + 5) % 7);
                    EmptyDates = new KeyValuePair<int, int>(5, skipDays);
                    break;
                case DayOfWeek.Friday:
                    skipDays = 7 - ((DateOfMonth.Count() + 6) % 7);
                    EmptyDates = new KeyValuePair<int, int>(6, skipDays);
                    break;
                case DayOfWeek.Saturday:
                    skipDays = 7 - (DateOfMonth.Count() % 7);
                    EmptyDates = new KeyValuePair<int, int>(0, skipDays);
                    break;
                case DayOfWeek.Sunday:
                    skipDays = 7 - ((DateOfMonth.Count() + 1) % 7);
                    EmptyDates = new KeyValuePair<int, int>(1, skipDays);
                    break;
            }
        }
        else
        {
            switch (firstDay.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    skipDays = 7 - (DateOfMonth.Count() % 7);
                    EmptyDates = new KeyValuePair<int, int>(0, skipDays);
                    break;
                case DayOfWeek.Tuesday:
                    skipDays = 7 - ((DateOfMonth.Count() + 1) % 7);
                    EmptyDates = new KeyValuePair<int, int>(1, skipDays);
                    break;
                case DayOfWeek.Wednesday:
                    skipDays = 7 - ((DateOfMonth.Count() + 2) % 7);
                    EmptyDates = new KeyValuePair<int, int>(2, skipDays);
                    break;
                case DayOfWeek.Thursday:
                    skipDays = 7 - ((DateOfMonth.Count() + 3) % 7);
                    EmptyDates = new KeyValuePair<int, int>(3, skipDays);
                    break;
                case DayOfWeek.Friday:
                    skipDays = 7 - ((DateOfMonth.Count() + 4) % 7);
                    EmptyDates = new KeyValuePair<int, int>(4, skipDays);
                    break;
                case DayOfWeek.Saturday:
                    skipDays = 7 - ((DateOfMonth.Count() + 5) % 7);
                    EmptyDates = new KeyValuePair<int, int>(5, skipDays);
                    break;
                case DayOfWeek.Sunday:
                    skipDays = 7 - ((DateOfMonth.Count() + 6) % 7);
                    EmptyDates = new KeyValuePair<int, int>(6, skipDays);
                    break;
            }
        }

        for (int i = 0; i < EmptyDates.Key; i++)
        {
            DateOfMonth.Insert(0, emptyDate);
        }
        for (int i = 0; i < EmptyDates.Value; i++)
        {
            DateOfMonth.Add(emptyDate);
        }
    }

    private void GetDayInfo(DateTime date)
    {
        var dayOfWeek = (int)date.DayOfWeek == 0 ? 7 : (int)date.DayOfWeek;
        DayNameShort = GetDaysShortName(dayOfWeek);
        DayName = GetDayName(dayOfWeek);
    }

    void ChangeMonth(int num)
    {
        CurrentValue = CurrentValue.AddMonths(num);
        PrepareDate(CurrentValue);
    }
    async Task ChangeMonthForOnlyMonthMode(short num)
    {
        CurrentValue = DateTimeUtils.CreateDateFromTime(CurrentValue.Year, num, CurrentValue.Day, CurrentValue.Hour, CurrentValue.Minute, CurrentValue.Second);
        PrepareDate(CurrentValue);
        await OnChangeAction.InvokeAsync(CurrentValue);
        await OnChangeActionById.InvokeAsync(new KeyValuePair<string, DateTime>(Id, CurrentValue));
    }
    async Task SelectDate(DateTime date)
    {
        Value = date.Date.ToShortDateString();
        CurrentValue = date;
        await OnChangeAction.InvokeAsync(date);
        await OnChangeActionById.InvokeAsync(new KeyValuePair<string, DateTime>(Id, date));
        PrepareDate(CurrentValue);
    }

    async Task ChangeYear(ChangeEventArgs args)
    {
        var dif = int.Parse(args.Value.ToString()) - CurrentValue.Year;
        CurrentValue = CurrentValue.AddYears(dif);
        await OnChangeAction.InvokeAsync(CurrentValue);
        await OnChangeActionById.InvokeAsync(new KeyValuePair<string, DateTime>(Id, CurrentValue));
        PrepareDate(CurrentValue);
    }
    async Task ChangeHour(ChangeEventArgs args)
    {
        var hour = int.Parse(args.Value.ToString());
        CurrentValue = DateTimeUtils.CreateDateFromTime(CurrentValue.Year, CurrentValue.Month, CurrentValue.Day, hour, CurrentValue.Minute, CurrentValue.Second);
        await OnChangeAction.InvokeAsync(CurrentValue);
        await OnChangeActionById.InvokeAsync(new KeyValuePair<string, DateTime>(Id, CurrentValue));
        PrepareDate(CurrentValue, false);
    }
    async Task ChangeMinute(ChangeEventArgs args)
    {
        var minute = int.Parse(args.Value.ToString());
        CurrentValue = DateTimeUtils.CreateDateFromTime(CurrentValue.Year, CurrentValue.Month, CurrentValue.Day, CurrentValue.Hour, minute, CurrentValue.Second);
        await OnChangeAction.InvokeAsync(CurrentValue);
        await OnChangeActionById.InvokeAsync(new KeyValuePair<string, DateTime>(Id, CurrentValue));
        PrepareDate(CurrentValue);
    }

    string GetMonthNameShort(int m)
    {
        switch (CalendarType)
        {
            case CalendarType.Hijri:
                var ht = (HijriMonthsShort)m;
                return ht.GetDescription();
            case CalendarType.Shamsi:
                var t = (ShamsiMonthsShort)m;
                return t.GetDescription();
            case CalendarType.Turkish:
                var tt = (TurkishMonthsShort)m;
                return tt.GetDescription();
            case CalendarType.German:
                var gr = (GermanMonthsShort)m;
                return gr.GetDescription();
            case CalendarType.Gregorian:
            default:
                var gt = (GregorianMonthsShort)m;
                return gt.GetDescription();
        }
    }
    string GetDaysShortName(int m)
    {
        switch (CalendarType)
        {
            case CalendarType.Hijri:
                var ht = (HijriDaysShort)m;
                return ht.GetDescription();
            case CalendarType.Shamsi:
                var t = (ShamsiDaysShort)m;
                return t.GetDescription();
            case CalendarType.Turkish:
                var tt = (TurkishDaysShort)m;
                return tt.GetDescription();
            case CalendarType.German:
                var gr = (GermanDaysShort)m;
                return gr.GetDescription();
            case CalendarType.Gregorian:
            default:
                var gt = (GregorianDaysShort)m;
                return gt.GetDescription();
        }
    }

    string GetDayName(int m)
    {
        switch (CalendarType)
        {
            case CalendarType.Hijri:
                var ht = (HijriDays)m;
                return ht.GetDescription();
            case CalendarType.Shamsi:
                var t = (ShamsiDays)m;
                return t.GetDescription();
            case CalendarType.Turkish:
                var tt = (TurkishDays)m;
                return tt.GetDescription();
            case CalendarType.German:
                var gr = (GermanDays)m;
                return gr.GetDescription();
            case CalendarType.Gregorian:
            default:
                var gt = (GregorianDays)m;
                return gt.GetDescription();
        }
    }
    string GetMonthName(DateTime date)
    {
        switch (CalendarType)
        {
            case CalendarType.Hijri:
                HijriCalendar hc = new HijriCalendar();
                var ht = (HijriMonths)hc.GetMonth(date);
                return ht.GetDescription();
            case CalendarType.Shamsi:
                PersianCalendar pc = new PersianCalendar();
                var t = (ShamsiMonths)pc.GetMonth(date);
                return t.GetDescription();
            case CalendarType.Turkish:
                var tt = (TurkishMonths)date.Month;
                return tt.GetDescription();
            case CalendarType.German:
                var grt = (GermanMonths)date.Month;
                return grt.GetDescription();
            case CalendarType.Gregorian:
            default:
                var gt = (GregorianMonths)date.Month;
                return gt.GetDescription();
        }
    }

    string GetDayByCalendarType(DateTime date)
    {
        switch (CalendarType)
        {
            case CalendarType.Hijri:
                HijriCalendar hc = new HijriCalendar();
                return $"{hc.GetDayOfMonth(date)}".ConvertNumLa2Fa();
            case CalendarType.Shamsi:
                PersianCalendar pc = new PersianCalendar();
                return $"{pc.GetDayOfMonth(date)}".ConvertNumLa2Fa();
            case CalendarType.Turkish:
            case CalendarType.Gregorian:
            case CalendarType.German:
            default:
                return date.Day.ToString();
        }
    }

    bool IsRTLCalendar()
    {
        switch (CalendarType)
        {
            case CalendarType.Hijri:
            case CalendarType.Shamsi:
                return true;
            case CalendarType.Turkish:
            case CalendarType.Gregorian:
            case CalendarType.German:
            default:
                return false;
        }
    }
}
internal class DatePickerInfo
{
    internal static List<KeyValuePair<int, string>> BuildYears(CalendarType cType, uint start = 0, uint end = 0)
    {
        if (end == 0)
            end = (uint)DateTime.Now.Year;
        if (start == 0)
            start = end - 100;

        List<KeyValuePair<int, string>> result = new();
        for (uint i = start; i <= end; i++)
        {
            var sDate = new DateTime((int)i, DateTime.Now.Month, DateTime.Now.Day);
            switch (cType)
            {
                case CalendarType.Hijri:
                    HijriCalendar hc = new();
                    var yearHijri = hc.GetYear(sDate);
                    if (result.Any())
                    {
                        var last = result.Last().Value;
                        yearHijri = yearHijri != int.Parse(last) + 1 ? int.Parse(last) + 1 : yearHijri;
                    }
                    result.Add(new KeyValuePair<int, string>((int)i, yearHijri.ToString()));
                    break;
                case CalendarType.Shamsi:
                    PersianCalendar pc = new();
                    var yearPersian = pc.GetYear(sDate);
                    if (result.Any())
                    {
                        var last = result.Last().Value;
                        yearPersian = yearPersian != int.Parse(last) + 1 ? int.Parse(last) + 1 : yearPersian;
                    }
                    result.Add(new KeyValuePair<int, string>((int)i, yearPersian.ToString()));
                    break;
                case CalendarType.Turkish:
                case CalendarType.Gregorian:
                case CalendarType.German:
                default:
                    result.Add(new KeyValuePair<int, string>((int)i, i.ToString()));
                    break;
            }
        }


        return result;
    }
}
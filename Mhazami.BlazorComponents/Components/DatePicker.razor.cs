using Mhazami.BlazorComponents.Models;
using Mhazami.BlazorComponents.Utility;
using Mhazami.Utility;
using Microsoft.AspNetCore.Components;
using System;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    [Parameter] public bool Disable { get; set; } = false;
    [Parameter] public DateTime[] DisabledDates { get; set; } = Array.Empty<DateTime>();

    private bool hide = true;
    private int Year = DateTime.Now.Year;

    private string MonthNameShort = "";
    private string DayNum = "";
    private string DayName = "";
    private string DayNameShort = "";
    private List<DateTime> DateOfMonth = new List<DateTime>();
    List<int> DisabledMonth = new();


    void GenerateDisabledMonth()
    {
        if (DatePickerType != DatePickerType.Month)
            return;

        var months = new int[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }.ToList();

        if (StartDate is null && ToDate is null)
            return;

        if (StartDate is not null && CurrentValue.Year == StartDate?.Year && ToDate is null)
            DisabledMonth = months.Where(x => x < StartDate.Value.Month).ToList();
        else if (StartDate is null && ToDate is not null && CurrentValue.Year == ToDate?.Year)
            DisabledMonth = months.Where(x => x >= ToDate.Value.Month).ToList();
        else if (StartDate is not null && ToDate is not null && CurrentValue.Year == StartDate?.Year && CurrentValue.Year == ToDate?.Year)
            DisabledMonth = months.Where(x => x < StartDate.Value.Month || x >= ToDate.Value.Month).ToList();
        else
            DisabledMonth.Clear();
    }
    private List<DateTime> _disabledOfDates;
    private List<DateTime> DisabledOfDates
    {
        get
        {
            if (StartDate is null && ToDate is null && DisabledDates.Length <= 0)
                return default!;

            _disabledOfDates = new List<DateTime>();

            _disabledOfDates.AddRange(DisabledDates);


            if (StartDate is not null && ToDate is null)
                _disabledOfDates.AddRange(DateOfMonth.Where(x => x <= StartDate).ToList());

            if (StartDate is null && ToDate is not null)
                _disabledOfDates.AddRange(DateOfMonth.Where(x => x > ToDate).ToList());

            if (StartDate is not null && ToDate is not null)
                _disabledOfDates.AddRange(DateOfMonth.Where(x => x <= StartDate || x > ToDate).ToList());

            return _disabledOfDates;
        }
    }
    private KeyValuePair<int, int> EmptyDates = new KeyValuePair<int, int>();
    private DateTime CurrentValue;
    private DateTime Today = DateTime.Now;
    private List<KeyValuePair<int, string>> Years = new();
    private int Hour = 0;
    private int Minute = 0;
    private string MinuteTitle = "";
    private string HourTitle = "";
    private string Result = "";
    private string OldDate = "";

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        StartProcess();
    }


    void StartProcess()
    {
        if (!string.IsNullOrEmpty(Value))
        {
            var dateOfValue = DateTime.Parse(Value);
            Result = dateOfValue.Date.ToShortDateString();
            CurrentValue = dateOfValue;
            PrepareDate(dateOfValue);
        }
        else
        {
            CurrentValue = DateTime.Now;
            PrepareDate(CurrentValue);
        }
        OldDate = Value;
        GenerateDisabledMonth();
        StateHasChanged();
    }

    protected override void OnParametersSet()
    {
        if (OldDate != Value)
            StartProcess();
    }
    void SetTitles()
    {
        switch (CalendarType)
        {
            case CalendarType.Hijri:
                MinuteTitle = "دقيقة";
                HourTitle = "ساعة";
                break;
            case CalendarType.Shamsi:
                MinuteTitle = "دقیقه";
                HourTitle = "ساعت";
                break;
            case CalendarType.Turkish:
                MinuteTitle = "Dakika";
                HourTitle = "Saat";
                break;
            case CalendarType.German:
                MinuteTitle = "Hour";
                HourTitle = "Stunde";
                break;
            case CalendarType.Gregorian:
                MinuteTitle = "Minute";
                HourTitle = "Minute";
                break;
        }
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
                Result = datetime;
                break;
            case DatePickerType.Date:
                Result = dates;
                break;
            case DatePickerType.Day:
                Result = DayName;
                break;
            case DatePickerType.Time:
                Result = $"{date.Hour}:{date.Minute}";
                Hour = date.Hour;
                Minute = date.Minute;
                break;
            case DatePickerType.Month:
                Result = $"{GetMonthName(date)} {year}";
                break;
            case DatePickerType.Year:
                Result = year;
                break;

        }
        SetTitles();
    }
    void Open() => hide = !hide;
    private void PrepareDate(DateTime dateOfValue, bool applyAutoClose = true)
    {
        if (StartDate is not null && ToDate is not null && StartDate > ToDate)
            throw new Exception("ToDate can not be greater than StartDate");

        Year = dateOfValue.Year;
        DayNum = GetDayByCalendarType(dateOfValue);
        GetDayInfo(dateOfValue);
        GetMonthInfo(dateOfValue);
        Years = DatePickerInfo.BuildYears(CalendarType, StartDate, ToDate);
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
                var test = Enumerable.Range(1, hc.GetDaysInMonth(date.Year, month));
                try
                {
                    DateOfMonth = Enumerable.Range(1, hc.GetDaysInMonth(date.Year, month))
                     .Select(day => hc.ToDateTime(hy, hm, day, hh, hmin, hs, 0))
                     .ToList();
                }
                catch (Exception)
                {
                    DateOfMonth = Enumerable.Range(1, hc.GetDaysInMonth(date.Year, month)).SkipLast(1)
                                       .Select(day => hc.ToDateTime(hy, hm, day, hh, hmin, hs, 0))
                                       .ToList();
                }
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
        GenerateDisabledMonth();
        PrepareDate(CurrentValue, false);
    }
    async Task ChangeMonthForOnlyMonthMode(short num)
    {
        var days = Enumerable.Range(1, DateTime.DaysInMonth(CurrentValue.Year, num));
        var day = days.Last() < CurrentValue.Day ? days.Last() : CurrentValue.Day;
        CurrentValue = DateTimeUtils.CreateDateFromTime(CurrentValue.Year, num, day, CurrentValue.Hour, CurrentValue.Minute, CurrentValue.Second);
        GenerateDisabledMonth();
        PrepareDate(CurrentValue);
        await OnChangeAction.InvokeAsync(CurrentValue);
        await OnChangeActionById.InvokeAsync(new KeyValuePair<string, DateTime>(Id, CurrentValue));
    }
    async Task SelectDate(DateTime date)
    {
        Result = date.Date.ToShortDateString();
        CurrentValue = date;
        GenerateDisabledMonth();
        await OnChangeAction.InvokeAsync(date);
        await OnChangeActionById.InvokeAsync(new KeyValuePair<string, DateTime>(Id, date));
        PrepareDate(CurrentValue);
    }

    async Task ChangeYear(ChangeEventArgs args)
    {
        var dif = int.Parse(args.Value.ToString()) - CurrentValue.Year;

        CurrentValue = CurrentValue.AddYears(dif);
        GenerateDisabledMonth();
        await OnChangeAction.InvokeAsync(CurrentValue);
        await OnChangeActionById.InvokeAsync(new KeyValuePair<string, DateTime>(Id, CurrentValue));
        PrepareDate(CurrentValue, DatePickerType == DatePickerType.Year);
    }
    async Task ChangeHour(ChangeEventArgs args)
    {
        var hour = int.Parse(args.Value.ToString());
        CurrentValue = DateTimeUtils.CreateDateFromTime(CurrentValue.Year, CurrentValue.Month, CurrentValue.Day, hour, CurrentValue.Minute, CurrentValue.Second);
        await OnChangeAction.InvokeAsync(CurrentValue);
        await OnChangeActionById.InvokeAsync(new KeyValuePair<string, DateTime>(Id, CurrentValue));
        Result = $"{CurrentValue.Hour}:{CurrentValue.Minute}";
        Hour = CurrentValue.Hour;
    }
    async Task ChangeMinute(ChangeEventArgs args)
    {
        var minute = int.Parse(args.Value.ToString());
        CurrentValue = DateTimeUtils.CreateDateFromTime(CurrentValue.Year, CurrentValue.Month, CurrentValue.Day, CurrentValue.Hour, minute, CurrentValue.Second);
        await OnChangeAction.InvokeAsync(CurrentValue);
        await OnChangeActionById.InvokeAsync(new KeyValuePair<string, DateTime>(Id, CurrentValue));
        Result = CurrentValue.ToShortTimeString();
        Minute = CurrentValue.Minute;
        //PrepareDate(CurrentValue);
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
                m = m <= 5 ? m + 2 : m == 6 ? 1 : 2;
                var ht = (HijriDaysShort)m;
                return ht.GetDescription();
            case CalendarType.Shamsi:
                m = m <= 5 ? m + 2 : m == 6 ? 1 : 2;
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
    internal static List<KeyValuePair<int, string>> BuildYears(CalendarType cType, DateTime? startdate, DateTime? enddate, uint start = 0, uint end = 0)
    {
        if (enddate is not null)
            end = (uint)enddate.Value.Year;
        else if (end == 0)
            end = (uint)DateTime.Now.Year;

        if (startdate is not null)
            start = (uint)startdate.Value.Year;
        else if (start == 0)
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
namespace Mhazami.BlazorComponents.Utility;

internal static class DateTimeUtils
{
    public static DateTime CreateDateFromTime(int year, int month, int day, int hour, int minute, int second)
    {
        return new DateTime(year, month, day, hour, minute, second);
    }
}

using Mhazami.BlazorComponents.Models;
using System.Collections;
using System.Reflection.Metadata;

namespace Mhazami.BlazorComponents.Utility;

internal static class DateTimeUtils
{
    public static DateTime CreateDateFromTime(int year, int month, int day, int hour, int minute, int second)
    {
        return new DateTime(year, month, day, hour, minute, second);
    }

    public static bool EqualsSelectedItemType<T1, T2>(this IEnumerable<T1> t1, IEnumerable<T2> t2)
        where T1 : class
        where T2 : class
    {
        try
        {
            var result1 = (IEnumerable<SelectListItem>)t1;
            var result2 = (IEnumerable<SelectListItem>)t2;

            return true;
        }
        catch
        {
            return false;
        }
    }
}

namespace Mhazami.Utility;

public static class Convert
{
    public static long ToLong(this string data)
    {
        long value;
        long.TryParse(data.ValidateDigitValue().ConvertNumFa2La(), out value);
        return value;
    }

    public static int ToInt(this string data)
    {
        int value;
        int.TryParse(data.ValidateDigitValue().ConvertNumFa2La(), out value);
        return value;
    }

    public static short ToShort(this string data)
    {
        short value;
        short.TryParse(data.ValidateDigitValue().ConvertNumFa2La(), out value);
        return value;
    }

    public static byte ToByte(this string data)
    {
        byte value;
        byte.TryParse(data.ValidateDigitValue().ConvertNumFa2La(), out value);
        return value;
    }

    public static double ToDouble(this string data)
    {
        double value;
        double.TryParse(data.ValidateDigitValue().ConvertNumFa2La(), out value);
        return value;
    }
    public static double ToSingle(this string data)
    {
        Single value;
        Single.TryParse(data.ValidateDigitValue().ConvertNumFa2La(), out value);
        return value;
    }

    public static decimal ToDecimal(this string data)
    {
        decimal value;
        decimal.TryParse(data.ValidateDigitValue().ConvertNumFa2La(), out value);
        return value;
    }

    public static float ToFloat(this string data)
    {
        float value;
        float.TryParse(data.ValidateDigitValue().ConvertNumFa2La(), out value);
        return value;
    }

    public static bool ToBool(this string data)
    {
        bool value;
        bool.TryParse(data, out value);
        return value;
    }

    public static Guid ToGuid(this string data)
    {
        Guid value;
        Guid.TryParse(data, out value);
        return value;
    }

    public static DateTime ToDateTime(this string data)
    {
        DateTime value;
        DateTime.TryParse(data.Replace("=", ":"), out value);
        return value;
    }

    public static T ToEnum<T>(this string data)
    {
        return (T)Enum.Parse(typeof(T), data);
    }



}
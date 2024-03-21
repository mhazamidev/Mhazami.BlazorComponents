using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace Mhazami.Utility;

public static class Utils
{
    public static byte[] Compress(byte[] data)
    {
        using (var compressedStream = new MemoryStream())
        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
        {
            zipStream.Write(data, 0, data.Length);
            zipStream.Close();
            return compressedStream.ToArray();
        }
    }

    public static byte[] Decompress(byte[] data)
    {
        using (var compressedStream = new MemoryStream(data))
        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
        using (var resultStream = new MemoryStream())
        {
            zipStream.CopyTo(resultStream);
            return resultStream.ToArray();
        }
    }
    public static async Task<byte[]> CompressAsync(byte[] data)
    {
        using (var compressedStream = new MemoryStream())
        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
        {
            await zipStream.WriteAsync(data, 0, data.Length);
            zipStream.Close();
            return compressedStream.ToArray();
        }
    }

    public static async Task<byte[]> DecompressAsync(byte[] data)
    {
        using (var compressedStream = new MemoryStream(data))
        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
        using (var resultStream = new MemoryStream())
        {
            await zipStream.CopyToAsync(resultStream);
            return resultStream.ToArray();
        }
    }
    public static bool IsPersianCulture
    {
        get
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "fa-IR";
        }
    }
    public static bool IsHijriCulture
    {
        get
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "ar-SA";
        }
    }
    public static T CreateInstance<T>()
    {
        var type = typeof(T);
        return (T)CreateInstance(type);
    }
    public static object CreateInstance(Type type)
    {

        var constructor = type.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null, Type.EmptyTypes, null);
        return constructor?.Invoke(new object[] { });

    }
    public static long GenerateRandomNumber(int size)
    {
        var random = new Random(DateTime.Now.Millisecond);
        const string chars = "123456789";
        var builder = new string(Enumerable.Repeat(chars, size)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        return builder.ToLong();
    }


    public static void FillObject(Object toObj, Object fromObj, bool fillHasValueProp = true)
    {
        if (toObj == null || fromObj == null) return;
        var propertyInfos = fromObj.GetType().GetProperties();
        foreach (var propertyInfo in propertyInfos)
        {

            var property = toObj.GetType().GetProperty(propertyInfo.Name);
            if (property == null || !property.CanWrite) continue;
            var sourcevalue = property.GetValue(toObj, null);
            if (!fillHasValueProp && sourcevalue != null) continue;
            var value = propertyInfo.GetValue(fromObj, null);
            if (property.PropertyType.IsEnum && value != null && !string.IsNullOrEmpty(value.ToString().Trim()))
                property.SetValue(toObj, Enum.Parse(property.PropertyType, value.ToString()), null);
            else
            {
                if (property.PropertyType != propertyInfo.PropertyType) continue;
                property.SetValue(toObj, value, null);
            }


        }
    }


    public static bool IsValueType<T>()
    {

        return IsValueType(typeof(T));
    }
    public static bool IsValueType(Type type)
    {



        type = type.GetTypeValidValue();
        if (type.IsEnum)
            return true;
        if (type == typeof(Guid))
            return true;
        switch (Type.GetTypeCode(type))
        {

            case TypeCode.Boolean:
            case TypeCode.Char:
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
            case TypeCode.DateTime:
            case TypeCode.String:
                return true;
            default:
                return false;
        }

    }
    public static T MapData<T>(NameValueCollection collection, T obj) where T : class
    {
        if (obj == null)
            obj = (T)Utils.CreateInstance<T>();
        foreach (var property in obj.GetType().GetProperties())
        {
            var val = collection.Get(property.Name);
            if (val != null)
            {
                obj.SetValue(property, collection[property.Name]);

            }
        }
        return obj;
    }
    public static NameValueCollection GetQuerys(Uri uri)
    {
        var result = new NameValueCollection();
        if (!string.IsNullOrEmpty(uri.Query))
        {
            var arr = uri.Query.Substring(1).Split('&');

            foreach (var part in arr)
            {
                if (part.Split('=').Length > 2)
                {
                    var parts = part.Split('=');
                    result.Add(parts[0], part.Substring(part.IndexOf('=') + 1, part.Length - part.IndexOf('=') - 1));
                }
                else
                {
                    var parts = part.Split('=');
                    result.Add(parts[0], parts[1]);
                }
            }
        }
        return result;
    }
    public enum PasswordStrength : byte
    {
        None = 0,
        [MhazamiDescriptionAttribute("Blank", Type = typeof(Resources.ProjectsFoundation))]
        Blank = 1,
        [MhazamiDescriptionAttribute("VeryWeak", Type = typeof(Resources.ProjectsFoundation))]
        VeryWeak = 2,
        [MhazamiDescriptionAttribute("Weak", Type = typeof(Resources.ProjectsFoundation))]
        Weak = 3,
        [MhazamiDescriptionAttribute("Medium", Type = typeof(Resources.ProjectsFoundation))]
        Medium = 4,
        [MhazamiDescriptionAttribute("Strong", Type = typeof(Resources.ProjectsFoundation))]
        Strong = 5,
        [MhazamiDescriptionAttribute("VeryStrong", Type = typeof(Resources.ProjectsFoundation))]
        VeryStrong = 6
    }

    /// <summary>
    /// Generic method to retrieve password strength: use this for general purpose scenarios,
    /// i.e. when you don't have a strict policy to follow.
    /// </summary>
    ///  /// <param name="password"></param>
    /// <returns></returns>
    public static PasswordStrength GetPasswordStrength(string password)
    {
        var score = 0;
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password.Trim())) return PasswordStrength.Blank;
        if (!(password.Length >= 5)) return PasswordStrength.VeryWeak;
        if (!(password.Length >= 5)) score++;
        // Returns TRUE if the password has at least one uppercase letter And lowercase letter
        if (password.Any(char.IsUpper) && password.Any(char.IsLower)) score++;
        // Returns TRUE if the password has at least one digit
        if (password.Any(char.IsDigit)) score++;
        if (password.Distinct().Count() >= 5) score++;
        // Returns TRUE if the password has at least one special character
        if (password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1) score++;
        return (PasswordStrength)score;
    }
    public static DataTable ObjectToTable(List<object> obj)
    {
        var table = new DataTable();
        if (obj != null && obj.Count > 0)
        {
            var item = obj[0];
            foreach (var property in item.GetType().GetProperties())
            {
                var column = new DataColumn();
                var propertyType = property.PropertyType;
                if (propertyType.IsGenericType &&
                    propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                    column.AllowDBNull = true;
                }
                column.DataType = propertyType;
                column.ColumnName = property.Name;
                table.Columns.Add(column);
            }

            foreach (var value in obj)
            {
                var row = table.NewRow();
                foreach (var property in item.GetType().GetProperties())
                {
                    var propertyType = property.PropertyType.GetTypeValidValue();
                    if (property.GetValue(value, null) != null)
                        row[property.Name] = System.Convert.ChangeType(property.GetValue(value, null), propertyType);
                }
                table.Rows.Add(row);
            }
        }
        return table;
    }
    public static string ValidateDigitValue(this string propertyValue)
    {
        return string.IsNullOrEmpty(propertyValue) ? "" : propertyValue.Replace("/", ".").Replace("،", "").Replace(",", "").Replace("٫", ".");
    }

    public static Type GetTypeValidValue(this Type type)
    {

        if (type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(Nullable<>))
            type = type.GetGenericArguments()[0];
        return type;
    }



    public static bool In<T>(this T obj, IEnumerable<T> items)
    {
        return items.Contains(obj);
    }
    public static IEnumerable<T> Union<T>(IEnumerable<T> source, IEnumerable<T> target)
    {
        return source.Union(target);
    }
    public static bool NotIn<T>(this T obj, IEnumerable<T> items)
    {
        return !items.Contains(obj);
    }
    [Obsolete]
    public static double GetObjectSize(this object obj)
    {
        double size = 0;
        using (Stream s = new MemoryStream())
        {

            var formatter = new BinaryFormatter();
            formatter.Serialize(s, obj);
            size += ((double)s.Length / 8);

        }
        return size;
    }
    [Obsolete]
    public static double GetObjectSize(this IEnumerable<object> objlist)
    {
        double size = 0;
        var formatter = new BinaryFormatter();
        var cacheInfos = objlist.ToList();
        foreach (var cacheInfo in cacheInfos)
        {
            Stream s = new MemoryStream();
            formatter.Serialize(s, cacheInfo);
            size += ((double)s.Length / 8);

        }
        return size;
    }


    public static string GetNumberByteSize(this double number)
    {
        var names = Enum.GetNames(typeof(FileSizeType));
        if (number == 0) return string.Format("{0:n1} {1}", 0, names[0]);
        var mag = (int)Math.Log(number, 1024);
        double adjustedSize = number / (1L << (mag * 10));
        return string.Format("{0:n1} {1}", adjustedSize, names[mag]);
    }

    public static FileSizeType GetNumberByteSizeType(this double number)
    {
        var names = Enum.GetNames(typeof(FileSizeType));
        if (number == 0) return names[0].ToEnum<FileSizeType>();
        var mag = (int)Math.Log(number, 1024);
        return names[mag].ToEnum<FileSizeType>();
    }

    public enum FileSizeType
    {
        bytes, KB, MB, GB, TB, PB, EB, ZB, YB
    }



    public static bool isNumeric(this string val, NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle, CultureInfo.CurrentCulture, out result);
    }
    public static bool IsNumericType(this object o)
    {
        var propertyType = o.GetType().GetTypeValidValue();
        switch (Type.GetTypeCode(propertyType))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }
    public static string ConvertStringToHTML(string str)
    {
        str = str.Replace("\"", "&quot;");
        str = str.Replace("'", "&#39;");
        str = str.Replace("&", "&amp;");
        str = str.Replace("<", "&lt;");
        str = str.Replace(">", "&gt;");
        return str;
    }

    public static string EncodeString(string m_enc)
    {
        byte[] toEncodeAsBytes =
        System.Text.Encoding.UTF8.GetBytes(m_enc);
        string returnValue =
        System.Convert.ToBase64String(toEncodeAsBytes);
        return returnValue;
    }

    public static string DecodeString(string m_enc)
    {
        byte[] encodedDataAsBytes =
        System.Convert.FromBase64String(m_enc);
        string returnValue =
        System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
        return returnValue;
    }


    public static string ConvertHtmlToString(string str)
    {
        if (String.IsNullOrEmpty(str)) return "";

        str = System.Net.WebUtility.HtmlDecode(str);
        str = str.Replace("&quot;", "\"");
        str = str.Replace("&#39;", "'");
        str = str.Replace("&amp;", "&");
        str = str.Replace("&lt;", "<");
        str = str.Replace("&gt;", ">");


        str = str.Replace("&#1740;", "ی");
        str = str.Replace("&#1728;", "ه");
        str = str.Replace("&#8204;", " ");
        str = str.Replace("&#171;", "«");
        str = str.Replace("&#187;", "»");
        str = str.Replace("&#8211;", "-");
        str = str.Replace("&#160;", " ");
        str = str.Replace("\r\n", "<br>");
        return str;
    }

    public static bool IsEmail(string inputEmail)
    {
        var regex = new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
        var match = regex.Match(inputEmail);
        return match.Success;

    }


    public static string GetSimpleConnectionString(string entityConnection)
    {
        const string pattern = "provider\\s.+";
        entityConnection = entityConnection.ToLowerInvariant();
        var regex = new Regex(pattern);
        var match = regex.Match(entityConnection);
        if (match.Success)
        {
            var value = match.Value;
            var index = value.IndexOf('\"');
            var str = value.Substring(index + 1, value.Length - index - 1);
            str = str.Substring(0, str.IndexOf('\"'));
            return str;
        }
        return String.Empty;
    }


    #region Number

    private static readonly string[] yekan = { Resources.ProjectsFoundation.zero, Resources.ProjectsFoundation.One, Resources.ProjectsFoundation.Two, Resources.ProjectsFoundation.Three, Resources.ProjectsFoundation.Four, Resources.ProjectsFoundation.Five, Resources.ProjectsFoundation.Six, Resources.ProjectsFoundation.Seven, Resources.ProjectsFoundation.eight, Resources.ProjectsFoundation.Nine };
    private static readonly string[] dahgan = { "", "", Resources.ProjectsFoundation.twenty, Resources.ProjectsFoundation.thirty, Resources.ProjectsFoundation.Forty, Resources.ProjectsFoundation.Fifty, Resources.ProjectsFoundation.sixty, Resources.ProjectsFoundation.Seventy, Resources.ProjectsFoundation.Eighty, Resources.ProjectsFoundation.ninety };
    private static readonly string[] dahyek =
    {
        Resources.ProjectsFoundation.ten, Resources.ProjectsFoundation.Eleven, Resources.ProjectsFoundation.twelve, Resources.ProjectsFoundation.Thirteen, Resources.ProjectsFoundation.fourteen, Resources.ProjectsFoundation.Fifteen, Resources.ProjectsFoundation.Sixteen,
        Resources.ProjectsFoundation.Seventeen, Resources.ProjectsFoundation.Eighteen,Resources.ProjectsFoundation.Nineteen
    };
    private static readonly string[] sadgan =
    {
        "", Resources.ProjectsFoundation.ahundred, Resources.ProjectsFoundation.twohundred,Resources.ProjectsFoundation.threehundred, Resources.ProjectsFoundation.fourhundred, Resources.ProjectsFoundation.fivehundred,Resources.ProjectsFoundation.sixhundred, Resources.ProjectsFoundation.sevenhundred,
        Resources.ProjectsFoundation.eighthundred, Resources.ProjectsFoundation.ninehundred
    };
    private static readonly string[] basex = { "", Resources.ProjectsFoundation.Thousand, Resources.ProjectsFoundation.Million, Resources.ProjectsFoundation.billion, Resources.ProjectsFoundation.Trillion };
    private static string GetNumberName(int num)
    {
        string s = "";
        int d12 = num % 100;
        int d3 = num / 100;
        if (d3 != 0)
            s = sadgan[d3] + " " + Resources.ProjectsFoundation.And + " ";
        if ((d12 >= 10) && (d12 <= 19))
        {
            s = s + dahyek[d12 - 10];
        }
        else
        {
            int d2 = d12 / 10;
            if (d2 != 0)
                s = s + dahgan[d2] + " " + Resources.ProjectsFoundation.And + " ";
            int d1 = d12 % 10;
            if (d1 != 0)
                s = s + yekan[d1] + " " + Resources.ProjectsFoundation.And + " ";
            s = s.Substring(0, s.Length - 3);
        }
        return s;
    }
    public static string ConvertNumberToChar(this int num)
    {
        return ConvertNumberToCharWithoutSpliter(num);
    }
    private static string ConvertNumberToCharWithoutSpliter(this double num)
    {

        bool isnegative = false;
        if (num < 0)
        {
            isnegative = true;
            num = Math.Abs(num);
        }
        var snum = num.ToString();
        string stotal = "";
        if (snum == "0")
        {
            return yekan[0];
        }
        snum = snum.PadLeft(((snum.Length - 1) / 3 + 1) * 3, '0');
        int L = snum.Length / 3 - 1;
        for (int i = 0; i <= L; i++)
        {
            int b = Int32.Parse(snum.Substring(i * 3, 3));
            if (b != 0)
                stotal = stotal + GetNumberName(b) + " " + basex[L - i] + " " + Resources.ProjectsFoundation.And + " ";
        }
        stotal = stotal.Substring(0, stotal.Length - 3);
        return isnegative ? " " + Resources.ProjectsFoundation.Negative + " " + stotal : stotal;
    }
    public static string ConvertNumberToChar(this double num)
    {
        var snum = num.ToString().ValidateDigitValue().ConvertNumFa2La();
        if (!snum.Contains('.'))
        {
            return num.ConvertNumberToCharWithoutSpliter();
        }
        var isnegative = false;
        if (num < 0)
        {
            isnegative = true;
            snum = Math.Abs(num).ToString().ValidateDigitValue().ConvertNumFa2La();

        }
        var result = string.Empty;
        var patrs = snum.Split('.');
        result = patrs[0].ToInt().ConvertNumberToChar();
        result += " " + Resources.ProjectsFoundation.Point + " ";
        result += patrs[1].ToInt().ConvertNumberToChar();
        result += Math.Pow(10, patrs[1].Length).ConvertNumberToChar().Replace(Resources.ProjectsFoundation.One, "").Trim() + Resources.ProjectsFoundation.M;
        return isnegative ? (" " + Resources.ProjectsFoundation.Negative + " " + result) : result;
    }

    #endregion

    #region NationId

    public static bool ValidAlienCode(string aliencode)
    {
        if (aliencode == null || string.IsNullOrEmpty(aliencode.Trim())) return false;
        aliencode = aliencode.Trim();
        return aliencode.Length >= 9 && aliencode.Length <= 13;
    }
    private static Random rng = new Random();


    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static bool ValidNationalID(string nationalID)
    {
        if (nationalID == null || string.IsNullOrEmpty(nationalID.Trim())) return false;
        nationalID = nationalID.Trim();
        if (nationalID.Length != 10) return false;

        var nid = string.Format("{0:D10}", nationalID);

        var nidPart1 = "";
        var nidPart2 = "";
        var nidPart3 = "";
        NationalIDStep(nid, ref nidPart1, ref nidPart2, ref nidPart3);

        if (nidPart1 == "000")
            return false;

        if (nidPart2 == "000000")
            return false;
        switch (nid)
        {
            case "1111111111":
            case "2222222222":
            case "3333333333":
            case "4444444444":
            case "5555555555":
            case "6666666666":
            case "7777777777":
            case "8888888888":
            case "9999999999":
                return false;
        }

        var total = 0;
        for (var i = 1; i < 10; i++)
            total += i * nid.Substring(i - 1, 1).ToInt();

        double reminder = total % 11;
        return reminder.ToString().Substring(0, 1) == nidPart3;
    }

    private static void NationalIDStep(string NID, ref string NationalIDPart1, ref string NationalIDPart2, ref string NationalIDPart3)
    {
        var strZero = "";
        if (NID.Length < 10 && NID.Length > 7)
        {
            for (var i = 10; i > NID.Length; i--)
                strZero += "0";
            NID = strZero + NID;
        }
        NationalIDPart1 = NID.Substring(0, 3);
        NationalIDPart2 = NID.Substring(3, 6);
        NationalIDPart3 = NID.Substring(9, 1);
    }

    #endregion
}
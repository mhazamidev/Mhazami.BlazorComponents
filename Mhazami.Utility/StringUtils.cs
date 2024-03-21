using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;

namespace Mhazami.Utility;

public static class StringUtils
{
    static Semaphore _semaphore = new Semaphore(1, 1);
    public static string Trancate(this string txt, int lentgth, bool addContinueDot = true)
    {
        if (!string.IsNullOrEmpty(txt) && txt.Length >= lentgth)
            txt = txt.Substring(0, lentgth - 3) + (addContinueDot ? "..." : "");
        return txt;
    }
    public static string generateRandomString(int size)
    {
        var random = new Random(DateTime.Now.Millisecond);
        const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz";
        var builder = new string(Enumerable.Repeat(chars, size)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        return builder.ToUpperInvariant();
    }
    public static string RemoveHtml(this string strHtml)
    {
        var input = Regex.Replace(strHtml, "<style>(.|\n)*?</style>", string.Empty);
        input = Regex.Replace(input, @"<xml>(.|\n)*?</xml>", string.Empty);
        return Regex.Replace(input, @"<(.|\n)*?>", string.Empty).Replace("&nbsp;", " ");

    }
    public static bool ValidateUserName(this string username)
    {
        if (string.IsNullOrEmpty(username)) return false;
        Regex regex = new Regex(@"^(?=[a-zA-Z])[-\w.]{0,23}([a-zA-Z\d]|(?<![-.])_)$");
        return regex.IsMatch(username);



    }
    public static byte[] Zip(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
                CopyTo(msi, gs);
            return mso.ToArray();
        }
    }
    public static async Task<byte[]> ZipAsync(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
                await CopyToAsync(msi, gs);
            return mso.ToArray();
        }
    }

    public static string GeneratePasswordSalt()
    {
        string result = "";
        try
        {
            _semaphore.WaitOne();
            result = Guid.NewGuid().ToString();
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            _semaphore.Release();
        }


        return result;
    }
    private static void CopyTo(Stream src, Stream dest)
    {

        byte[] bytes = new byte[16 * 1024];
        int cnt;
        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            dest.Write(bytes, 0, cnt);
    }
    private static async Task CopyToAsync(Stream src, Stream dest)
    {

        byte[] bytes = new byte[16 * 1024];
        int cnt;
        while ((cnt = await src.ReadAsync(bytes, 0, bytes.Length)) != 0)
            await dest.WriteAsync(bytes, 0, cnt);
    }

    public static string Unzip(byte[] bytes)
    {
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                CopyTo(gs, mso);
            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }
    public static async Task<string> UnzipAsync(byte[] bytes)
    {
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                await CopyToAsync(gs, mso);
            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }
    public static string ConvertNumLa2Fa(this string num)
    {
        if (string.IsNullOrEmpty(num)) return "";
        var result = string.Empty;
        foreach (var c in num)
        {
            if (c.Equals('0'))
                result += "٠";
            else if (c.Equals('1'))
                result += "١";
            else if (c.Equals('2'))
                result += "٢";
            else if (c.Equals('3'))
                result += "٣";
            else if (c.Equals('4'))
                result += "۴";
            else if (c.Equals('5'))
                result += "۵";
            else if (c.Equals('6'))
                result += "۶";
            else if (c.Equals('7'))
                result += "٧";
            else if (c.Equals('8'))
                result += "٨";
            else if (c.Equals('9'))
                result += "٩";
            else
                result += c;

        }
        return result;

    }
    public static string ConvertNumFa2La(this string num)
    {
        if (string.IsNullOrEmpty(num)) return "";
        var result = string.Empty;
        foreach (var c in num)
        {
            if (c.Equals('٠'))
                result += "0";
            else if (c.Equals('١'))
                result += "1";
            else if (c.Equals('٢'))
                result += "2";
            else if (c.Equals('٣'))
                result += "3";
            else if (c.Equals('۴'))
                result += "4";
            else if (c.Equals('۵'))
                result += "5";
            else if (c.Equals('۶'))
                result += "6";
            else if (c.Equals('٧'))
                result += "7";
            else if (c.Equals('٨'))
                result += "8";
            else if (c.Equals('٩'))
                result += "9";
            else
                result += c;

        }
        return result;



    }
    public static string InversePersianDate(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        var data = str.Trim().Split('/');
        return string.Format("{0}/{1}/{2}", data[2], data[1], data[0]);
    }
    public static string InverseDraftNUmber(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        if (str.IndexOf('/') != -1)
        {
            var data = str.Trim().Split('/');
            return string.Format("{0}/{1}", data[1], data[0]);
        }
        return str.Trim();
    }
    public static string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password)) return password;
        return System.Convert.ToBase64String(
           new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(password)));
    }

    public static string Encrypt(string data)
    {
        const string key = "rdn(!)";
        var RijndaelCipher = new RijndaelManaged();
        var PlainText = Encoding.Unicode.GetBytes(data);
        var Salt = Encoding.ASCII.GetBytes(key.Length.ToString());
        var SecretKey = new PasswordDeriveBytes(key, Salt);
        var Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(0x20), SecretKey.GetBytes(0x10));
        var memoryStream = new MemoryStream();
        var cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(PlainText, 0, PlainText.Length);
        cryptoStream.FlushFinalBlock();
        var CipherBytes = memoryStream.ToArray();
        memoryStream.Close();
        cryptoStream.Close();
        return System.Convert.ToBase64String(CipherBytes);
    }
    public static async Task<string> EncryptAsync(string data)
    {
        const string key = "rdn(!)";
        var RijndaelCipher = new RijndaelManaged();
        var PlainText = Encoding.Unicode.GetBytes(data);
        var Salt = Encoding.ASCII.GetBytes(key.Length.ToString());
        var SecretKey = new PasswordDeriveBytes(key, Salt);
        var Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(0x20), SecretKey.GetBytes(0x10));
        var memoryStream = new MemoryStream();
        var cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
        await cryptoStream.WriteAsync(PlainText, 0, PlainText.Length);
        cryptoStream.FlushFinalBlock();
        var CipherBytes = memoryStream.ToArray();
        memoryStream.Close();
        cryptoStream.Close();
        return System.Convert.ToBase64String(CipherBytes);
    }

    public static byte[] Encrypt(byte[] bytesToEncrypt, string password)
    {
        byte[] ivSeed = Guid.NewGuid().ToByteArray();

        var rfc = new Rfc2898DeriveBytes(password, ivSeed);
        byte[] Key = rfc.GetBytes(16);
        byte[] IV = rfc.GetBytes(16);

        byte[] encrypted;
        using (MemoryStream mstream = new MemoryStream())
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                using (CryptoStream cryptoStream = new CryptoStream(mstream, aesProvider.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                }
            }
            encrypted = mstream.ToArray();
        }

        var messageLengthAs32Bits = bytesToEncrypt.Length;
        var messageLength = BitConverter.GetBytes(messageLengthAs32Bits);

        encrypted = encrypted.Prepend(ivSeed);
        encrypted = encrypted.Prepend(messageLength);

        return encrypted;
    }
    public static async Task<byte[]> EncryptAsync(byte[] bytesToEncrypt, string password)
    {
        byte[] ivSeed = Guid.NewGuid().ToByteArray();

        var rfc = new Rfc2898DeriveBytes(password, ivSeed);
        byte[] Key = rfc.GetBytes(16);
        byte[] IV = rfc.GetBytes(16);

        byte[] encrypted;
        using (MemoryStream mstream = new MemoryStream())
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                using (CryptoStream cryptoStream = new CryptoStream(mstream, aesProvider.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                {
                    await cryptoStream.WriteAsync(bytesToEncrypt, 0, bytesToEncrypt.Length);
                }
            }
            encrypted = mstream.ToArray();
        }

        var messageLengthAs32Bits = bytesToEncrypt.Length;
        var messageLength = BitConverter.GetBytes(messageLengthAs32Bits);

        encrypted = encrypted.Prepend(ivSeed);
        encrypted = encrypted.Prepend(messageLength);

        return encrypted;
    }
    public static byte[] Prepend(this byte[] bytes, byte[] bytesToPrepend)
    {
        var tmp = new byte[bytes.Length + bytesToPrepend.Length];
        bytesToPrepend.CopyTo(tmp, 0);
        bytes.CopyTo(tmp, bytesToPrepend.Length);
        return tmp;
    }
    public static (byte[] left, byte[] right) Shift(this byte[] bytes, int size)
    {
        var left = new byte[size];
        var right = new byte[bytes.Length - size];

        Array.Copy(bytes, 0, left, 0, left.Length);
        Array.Copy(bytes, left.Length, right, 0, right.Length);

        return (left, right);
    }

    public static string Decrypt(string TextToBeDecrypted, bool forurl = false)
    {
        if (TextToBeDecrypted.IndexOf(" ") >= 0 && forurl)
        {
            TextToBeDecrypted = TextToBeDecrypted.Replace(" ", "+");
        }
        var RijndaelCipher = new RijndaelManaged();
        const string key = "rdn(!)";

        try
        {
            var EncryptedData = System.Convert.FromBase64String(TextToBeDecrypted);
            var Salt = Encoding.ASCII.GetBytes(key.Length.ToString());
            var SecretKey = new PasswordDeriveBytes(key, Salt);
            var Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(0x20), SecretKey.GetBytes(0x10));
            var memoryStream = new MemoryStream(EncryptedData);
            var cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
            var PlainText = new byte[EncryptedData.Length];
            var DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
        }
        catch
        {
            return TextToBeDecrypted;
        }
    }
    public static async Task<string> DecryptAsync(string TextToBeDecrypted, bool forurl = false)
    {
        if (TextToBeDecrypted.IndexOf(" ") >= 0 && forurl)
        {
            TextToBeDecrypted = TextToBeDecrypted.Replace(" ", "+");
        }
        var RijndaelCipher = new RijndaelManaged();
        const string key = "rdn(!)";

        try
        {
            var EncryptedData = System.Convert.FromBase64String(TextToBeDecrypted);
            var Salt = Encoding.ASCII.GetBytes(key.Length.ToString());
            var SecretKey = new PasswordDeriveBytes(key, Salt);
            var Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(0x20), SecretKey.GetBytes(0x10));
            var memoryStream = new MemoryStream(EncryptedData);
            var cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
            var PlainText = new byte[EncryptedData.Length];
            var DecryptedCount = await cryptoStream.ReadAsync(PlainText, 0, PlainText.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
        }
        catch
        {
            return TextToBeDecrypted;
        }
    }


    public static byte[] Decrypt(byte[] bytesToDecrypt, string password)
    {
        (byte[] messageLengthAs32Bits, byte[] bytesWithIv) = bytesToDecrypt.Shift(4);
        (byte[] ivSeed, byte[] encrypted) = bytesWithIv.Shift(16);

        var length = BitConverter.ToInt32(messageLengthAs32Bits, 0);

        var rfc = new Rfc2898DeriveBytes(password, ivSeed);
        byte[] Key = rfc.GetBytes(16);
        byte[] IV = rfc.GetBytes(16);

        byte[] decrypted;

        using (MemoryStream mStream = new MemoryStream(encrypted))
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Padding = PaddingMode.None;
                using (CryptoStream cryptoStream = new CryptoStream(mStream, aesProvider.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                {
                    cryptoStream.Read(encrypted, 0, length);
                }
            }
            decrypted = mStream.ToArray().Take(length).ToArray();
        }
        return decrypted;
    }
    public static async Task<byte[]> DecryptAsync(byte[] bytesToDecrypt, string password)
    {
        (byte[] messageLengthAs32Bits, byte[] bytesWithIv) = bytesToDecrypt.Shift(4);
        (byte[] ivSeed, byte[] encrypted) = bytesWithIv.Shift(16);

        var length = BitConverter.ToInt32(messageLengthAs32Bits, 0);

        var rfc = new Rfc2898DeriveBytes(password, ivSeed);
        byte[] Key = rfc.GetBytes(16);
        byte[] IV = rfc.GetBytes(16);

        byte[] decrypted;

        using (MemoryStream mStream = new MemoryStream(encrypted))
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Padding = PaddingMode.None;
                using (CryptoStream cryptoStream = new CryptoStream(mStream, aesProvider.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                {
                    await cryptoStream.ReadAsync(encrypted, 0, length);
                }
            }
            decrypted = mStream.ToArray().Take(length).ToArray();
        }
        return decrypted;
    }
    public static string FixUrlCatchall(this string txt)
    {
        txt = txt.Replace("/", " ");
        txt = txt.Replace("&", " ");
        txt = txt.Replace(":", " ");
        txt = txt.Replace(".", " ");
        txt = txt.Replace("\"", " ");
        txt = txt.Replace("+", " ");
        txt = txt.Replace("ç", "c");
        txt = txt.Replace("ı", "i");
        txt = txt.Replace("ğ", "g");
        txt = txt.Replace("ö", "o");
        txt = txt.Replace("ü", "u");
        txt = txt.Replace("ş", "s");
        txt = txt.Replace("Ç", "C");
        txt = txt.Replace("İ", "I");
        txt = txt.Replace("Ğ", "G");
        txt = txt.Replace("Ö", "O");
        txt = txt.Replace("Ü", "U");
        txt = txt.Replace("Ş", "S");
        if (txt.StartsWith("/"))
            txt = txt.Substring(1, txt.Length - 1);
        return txt;
    }


    public static string SetDirection(this string Txt, string direction = "", string aligment = "")
    {
        if (direction == "") direction = "rtl";
        if (aligment == "") aligment = "right";
        return string.Format("<div style=\"text-align:{1};direction:{2};\" >{0}</div>", Txt, aligment, direction);
    }



    public static string FixString(this string str)
    {
        str = str.Replace('ك', 'ک');
        str = str.Replace('ي', 'ی');
        return str;
    }
    public static string ToCurrencyFormat(this double? value)
    {
        if (value == null) return String.Empty;
        return ToCurrencyFormat((double)value);


    }
    public static string ToCurrencyFormat(this decimal? value)
    {
        if (value == null) return String.Empty;
        return ToCurrencyFormat((decimal)value);

    }
    public static string ToCurrencyFormat(this float? value)
    {
        if (value == null) return String.Empty;
        return ToCurrencyFormat((float)value);


    }
    public static string ToCurrencyFormat(this long? value)
    {
        if (value == null) return String.Empty;
        return ToCurrencyFormat((long)value);

    }
    public static string ToCurrencyFormat(this int? value)
    {
        if (value == null) return String.Empty;
        return ToCurrencyFormat((int)value);

    }
    public static string ToCurrencyFormat(this double value)
    {
        return value.ToString(value % 1 == 0 ? "n0" : "n3");


    }
    public static string ToCurrencyFormat(this decimal value)
    {
        return value.ToString(value % 1 == 0 ? "n0" : "n3");

    }
    public static string ToCurrencyFormat(this float value)
    {
        return value.ToString(value % 1 == 0 ? "n0" : "n3");

    }
    public static string ToCurrencyFormat(this long value)
    {
        return value.ToString(value % 1 == 0 ? "n0" : "n3");

    }
    public static string ToCurrencyFormat(this int value)
    {
        return value.ToString(value % 1 == 0 ? "n0" : "n3");

    }
    public static string ToCurrencyFormat(dynamic value)
    {
        if (value == null) return String.Empty;
        switch (value)
        {
            case decimal value1:
                return ToCurrencyFormat(value1);
            case double d:
                return ToCurrencyFormat(d);
            case long l:
                return ToCurrencyFormat(l);
            case float f:
                return ToCurrencyFormat(f);
            case int i:
                return ToCurrencyFormat(i);
            default:
                return String.Empty;
        }
    }
    public static string Compress(this string instance)
    {
        if (string.IsNullOrEmpty(instance)) return string.Empty;
        var bytes = Encoding.UTF8.GetBytes(instance);
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                msi.CopyTo(gs);
            }
            return System.Convert.ToBase64String(mso.ToArray());
        }
    }


    public static string Decompress(this string instance)
    {
        if (string.IsNullOrEmpty(instance)) return string.Empty;
        var bytes = System.Convert.FromBase64String(instance);
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                gs.CopyTo(mso);
            }
            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }

    public static bool IsCaseSensitiveEqual(this string instance, string comparing)
    {
        return string.CompareOrdinal(instance, comparing) == 0;
    }

    public static bool IsCaseInsensitiveEqual(this string instance, string comparing)
    {
        return string.Compare(instance, comparing, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        if (email.Trim().EndsWith("."))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    public static bool IsDigitsOnly(this string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return false;
        foreach (char c in str)
            if (c < '0' || c > '9')
                return false;
        return true;
    }
    public static bool IsValidMobile(this string mobile)
    {
        if (string.IsNullOrWhiteSpace(mobile)) return false;
        bool validphone = true;
        if (mobile.StartsWith("+"))
        {
            if (mobile.Length != 13)
                validphone = false;
        }
        else if (mobile.StartsWith("00"))
        {
            if (mobile.Length != 14)
                validphone = false;
        }
        else
        {
            if (mobile.StartsWith("0"))
            {
                if (mobile.Length != 11)
                    validphone = false;
            }
            else
            {
                if (mobile.Length != 10)
                    validphone = false;
            }

        }
        if (!validphone) return false;
        return !mobile.StartsWith("+") ? mobile.IsDigitsOnly() : mobile.Split('+')[1].IsDigitsOnly();
    }
}

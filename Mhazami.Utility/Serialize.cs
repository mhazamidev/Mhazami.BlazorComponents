using System.Runtime.Serialization.Json;
using System.Text;

namespace Mhazami.Utility;

public static class Serialize
{
    public static string JsonSerializer<T>(this T t)
    {
        var ser = new DataContractJsonSerializer(typeof(T));
        var ms = new MemoryStream();
        ser.WriteObject(ms, t);
        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        ms.Close();
        return jsonString;
    }
    public static string JsonSerializer<T>(this T t, DataContractJsonSerializerSettings jsonSerializerSettings)
    {
        var ser = new DataContractJsonSerializer(typeof(T), jsonSerializerSettings);
        var ms = new MemoryStream();
        ser.WriteObject(ms, t);
        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        ms.Close();
        return jsonString;
    }

    public static string JsonSerializer<T>(this T t, Type[] KnownTypes)
    {
        var ser = new DataContractJsonSerializer(typeof(T), KnownTypes);
        var ms = new MemoryStream();
        ser.WriteObject(ms, t);
        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        ms.Close();
        return jsonString;
    }
    /// <summary>
    /// JSON Deserialization
    /// </summary>
    public static T JsonDeserialize<T>(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString)) return default(T);
        var ser = new DataContractJsonSerializer(typeof(T));
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        T obj = (T)ser.ReadObject(ms);
        return obj;
    }
    public static T JsonDeserialize<T>(string jsonString, Type[] KnownTypes)
    {
        if (string.IsNullOrEmpty(jsonString)) return default(T);
        var ser = new DataContractJsonSerializer(typeof(T), KnownTypes);
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        T obj = (T)ser.ReadObject(ms);
        return obj;
    }


    public static string XmlSerialize<T>(this T model)
    {
        var xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using (StringWriter textWriter = new StringWriter())
        {
            xs.Serialize(textWriter, model);
            return textWriter.ToString();

        }
    }

    public static T XmlDeserialize<T>(string xmlDoc)
    {
        var xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using (TextReader sr = new StringReader(xmlDoc))
        {
            return (T)xs.Deserialize(sr);

        }

    }

}
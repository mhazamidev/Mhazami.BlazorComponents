using System.Reflection;

namespace Mhazami.Utility;

public static class PropertyUtils
{
    public static object GetPropertyValue(object obj, PropertyInfo property)
    {
        return property.GetValue(obj, null);
    }
    public static Object GetPropertyValue<T>(object value)
    {
        var type = typeof(T);
        return GetPropertyValue(type, value);



    }
    public static Object GetPropertyValue(Type type, object value)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()) || value.ToString().ToLowerInvariant() == "null") return null;
        type = type.GetTypeValidValue();
        var propertyValue = value.ToString();
        if (type.IsEnum) return Enum.Parse(type, propertyValue);
        if (type == typeof(Guid))
        {
            if (Guid.Parse(propertyValue.ToString()).Equals(Guid.Empty))
                return null;
            return propertyValue.ToGuid();
        }
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Boolean:
                return propertyValue.ToBool();
            case TypeCode.Char:
                return propertyValue;
            case TypeCode.Byte:
            case TypeCode.SByte:
                return propertyValue.ToByte();
            case TypeCode.Int16:
            case TypeCode.UInt16:
                return propertyValue.ToShort();
            case TypeCode.Int32:
            case TypeCode.UInt32:
                return propertyValue.ToInt();
            case TypeCode.Int64:
            case TypeCode.UInt64:
                return propertyValue.ToLong();
            case TypeCode.Double:
                return propertyValue.ToDouble();
            case TypeCode.Single:
                return propertyValue.ToSingle();
            case TypeCode.Decimal:
                return propertyValue.ToDecimal();
            case TypeCode.DateTime:
                return propertyValue.ToDateTime();
            case TypeCode.String:
                return propertyValue;
            default:
                return System.Convert.ChangeType(propertyValue, type);
        }

        return null;

    }


    public static dynamic GetValidTypeValue<T>(object value)
    {
        var type = typeof(T);
        if (value == DBNull.Value) return default(T);
        if (type.IsEnum) return Enum.Parse(type, value.ToString());
        try
        {
            return (T)value;
        }
        catch
        {

            return (T)System.Convert.ChangeType(value, type);
        }

    }
    public static void SetValue(this PropertyInfo propertyInfo, object val, object result)
    {
        try
        {
            if (val is DBNull)
                propertyInfo.SetValue(result, null, null);
            else
            {
                if (propertyInfo.PropertyType.IsEnum)
                {
                    propertyInfo.SetValue(result, Enum.Parse(propertyInfo.PropertyType, val.ToString()), null);
                    return;
                }

                try
                {
                    propertyInfo.SetValue(result, val, null);
                }
                catch
                {
                    var changeType = System.Convert.ChangeType(val, propertyInfo.PropertyType);
                    propertyInfo.SetValue(result, changeType, null);
                }


            }
        }
        catch (Exception ex)
        {
        }
    }
    public static void SetValue<TModel>(this TModel model, PropertyInfo propertyInfo, string value)
    {

        var propertyInfoPropertyType = propertyInfo.PropertyType.GetTypeValidValue();
        if (propertyInfoPropertyType.IsEnum)
        {
            if (value != "null" && !string.IsNullOrEmpty(value))
                propertyInfo.SetValue(model, Enum.Parse(propertyInfoPropertyType, value), null);
            return;
        }
        if (string.IsNullOrEmpty(value) && propertyInfo.PropertyType.Name.ToLowerInvariant().IndexOf("nullable") >= 0)
        {
            propertyInfo.SetValue(model, null, null);
            return;
        }
        if (propertyInfoPropertyType == typeof(Guid))
        {
            if (string.IsNullOrEmpty(value))
                propertyInfo.SetValue(model, null, null);
            else
                propertyInfo.SetValue(model, value.ToGuid(), null);
            return;
        }
        switch (Type.GetTypeCode(propertyInfoPropertyType))
        {

            case TypeCode.Boolean:
                if (string.IsNullOrEmpty(value))
                {
                    if (propertyInfo.PropertyType.Name.ToLowerInvariant().IndexOf("nullable") >= 0)
                        propertyInfo.SetValue(model, null, null);
                    else propertyInfo.SetValue(model, false, null);
                }
                else
                {
                    var val = value.ToLowerInvariant().Equals("on") || value.ToLowerInvariant().Contains("true");
                    propertyInfo.SetValue(model, val, null);
                }
                return;
            case TypeCode.SByte:
            case TypeCode.Byte:
                if (string.IsNullOrEmpty(value))
                    propertyInfo.SetValue(model, null, null);
                else
                    propertyInfo.SetValue(model, value.ToByte(), null);
                return;
            case TypeCode.Int16:
            case TypeCode.UInt16:
                if (string.IsNullOrEmpty(value))
                    propertyInfo.SetValue(model, null, null);
                else
                    propertyInfo.SetValue(model, value.ToShort(), null);
                return;
            case TypeCode.Int32:
            case TypeCode.UInt32:
                if (string.IsNullOrEmpty(value))
                    propertyInfo.SetValue(model, null, null);
                else
                    propertyInfo.SetValue(model, value.ToInt(), null);
                return;
            case TypeCode.Int64:
            case TypeCode.UInt64:
                if (string.IsNullOrEmpty(value))
                    propertyInfo.SetValue(model, null, null);
                else
                    propertyInfo.SetValue(model, value.ToLong(), null);
                return;
            case TypeCode.Single:
                if (string.IsNullOrEmpty(value))
                    propertyInfo.SetValue(model, null, null);
                else
                    propertyInfo.SetValue(model, value.ToSingle(), null);
                return;
            case TypeCode.Double:
                if (string.IsNullOrEmpty(value))
                    propertyInfo.SetValue(model, null, null);
                else
                    propertyInfo.SetValue(model, value.ToDouble(), null);
                return;
            case TypeCode.Decimal:
                if (string.IsNullOrEmpty(value))
                    propertyInfo.SetValue(model, null, null);
                else
                    propertyInfo.SetValue(model, value.ToDecimal(), null);
                return;
            case TypeCode.DateTime:
                if (string.IsNullOrEmpty(value))
                    propertyInfo.SetValue(model, null, null);
                else
                    propertyInfo.SetValue(model, value.ToDateTime(), null);
                break;
            case TypeCode.String:
            case TypeCode.Char:
                propertyInfo.SetValue(model, value, null);
                return;

        }



    }
}

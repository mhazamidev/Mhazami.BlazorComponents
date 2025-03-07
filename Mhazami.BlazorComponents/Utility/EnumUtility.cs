using Mhazami.BlazorComponents.Components;
using Mhazami.Utility;
using System;
using System.ComponentModel;

namespace Mhazami.BlazorComponents.Utility;

public static class EnumUtility
{
    internal static IEnumerable<DropdownListItem> ToDropdownListItem(this IEnumerable<KeyValuePair<string, string>> items)
    {
        return items.Select(item => new DropdownListItem(item.Key, item.Value));
    }

    public static IEnumerable<DropdownListItem> ToDropdownListItem<TEnum>()
    {
        var result = new List<KeyValuePair<string, string>>();
        var fieldInfos = typeof(TEnum).GetFields().Where(f => f.IsLiteral).ToList();
        for (var i = 0; i < fieldInfos.Count; i++)
        {
            var value = "";
            var attributes = fieldInfos[i].GetCustomAttributes(typeof(DescriptionAttribute), false);
            var key = fieldInfos[i].Name;
            if (attributes != null && attributes.Length > 0)
                value = ((DescriptionAttribute)attributes[0]).Description;
            if (!string.IsNullOrEmpty(value))
                result.Add(new KeyValuePair<string, string>(key, value));

        }
        return result.ToDropdownListItem();
    }
}

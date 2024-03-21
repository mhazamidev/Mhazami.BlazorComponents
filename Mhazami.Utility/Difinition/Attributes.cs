namespace Mhazami.Utility;

[AttributeUsage(AttributeTargets.All)]
[Serializable]
public class MhazamiDescriptionAttribute : Attribute
{
    private string _layoutDescription;
    public MhazamiDescriptionAttribute()
    {

    }
    public MhazamiDescriptionAttribute(string layoutDescription)
    {
        this.LayoutDescription = layoutDescription;
    }

    private string description = "";


    public string LayoutDescription
    {
        get
        {
            GetValue();
            return description;
        }
        set { _layoutDescription = value; }
    }

    public Type Type { get; set; }

    private void GetValue()
    {
        if (Type == null)
        {
            description = _layoutDescription;
            return;
        }

        var propertyInfos = Type.GetProperty(_layoutDescription);
        if (propertyInfos != null)
        {
            var value = Type.GetProperty(_layoutDescription).GetValue(Type, null);
            if (value != null)
                description = value.ToString();
        }
    }
}

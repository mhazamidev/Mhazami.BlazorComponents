namespace Mhazami.BlazorComponents.Models;

public record class SelectList
{
    public List<SelectListItem> Items { get; init; }
    public SelectListItem SelectedValue { get; init; }

    public SelectList(IEnumerable<object> items, string valuefield, string textfield, object? selectedvalue = null)
    {
        var result = new List<SelectListItem>();
        foreach (var item in items)
        {
            var value_field = item.GetType().GetProperty(valuefield).GetValue(item)?.ToString();
            var text_field = item.GetType().GetProperty(textfield).GetValue(item)?.ToString();
            result.Add(new SelectListItem
            {
                Value = value_field,
                Text = text_field
            });
        }
        Items = result;

        if (selectedvalue is not null)
        {
            var value = selectedvalue.GetType().GetProperty(valuefield).GetValue(selectedvalue)?.ToString();
            var text = selectedvalue.GetType().GetProperty(textfield).GetValue(selectedvalue)?.ToString();
            SelectedValue = new SelectListItem
            {
                Value = value,
                Text = text
            };
        }
    }
}

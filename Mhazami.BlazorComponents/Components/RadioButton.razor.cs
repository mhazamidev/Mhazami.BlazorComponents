using Mhazami.BlazorComponents.Models;
using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class RadioButton
{
    [Parameter] public IEnumerable<object> Items { get; set; }
    [Parameter] public string CssClass { get; set; }
    [Parameter] public string GroupName { get; set; }
    [Parameter] public string Value { get; set; }
    [Parameter] public string TextField { get; set; }
    [Parameter] public string ValueField { get; set; }
    [Parameter] public string Id { get; set; }
    [Parameter] public EventCallback<string> OnChange { get; set; }
    /// <summary>
    /// This event is triggered when the value of the radio button changes. KeyValuePair<string, string>("Id","Value")
    /// </summary>
    [Parameter] public EventCallback<KeyValuePair<string, string>> OnChangeById { get; set; }
    [Parameter] public DirectionOrder DirectionOrder { get; set; } = DirectionOrder.Horizontal;
    private SelectList _items = new();
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (ValidateParameters())
        {
            _items = new SelectList(Items, ValueField, TextField);
        }
    }

    async Task OnSelect(string val)
    {
        Value = val;
        await OnChange.InvokeAsync(val);
        if (!string.IsNullOrEmpty(Id))
            await OnChangeById.InvokeAsync(new KeyValuePair<string, string>(Id, val));
        StateHasChanged();
    }

    bool ValidateParameters()
    {
        if (string.IsNullOrEmpty(TextField) || string.IsNullOrEmpty(ValueField) || string.IsNullOrEmpty(GroupName))
            return false;
        return Items is not null && Items.Any();
    }
}

public class RadioButtonItem
{
    public string Key { get; set; }
    public string Value { get; set; }
}



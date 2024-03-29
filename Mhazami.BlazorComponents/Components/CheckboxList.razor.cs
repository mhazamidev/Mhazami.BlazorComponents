using Mhazami.BlazorComponents.Models;
using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class CheckboxList
{
    [Parameter] public IEnumerable<object> Items { get; set; }
    [Parameter] public string ValueField { get; set; }
    [Parameter] public string TextField { get; set; }
    [Parameter] public IEnumerable<object> SelectedList { get; set; }
    [Parameter] public IEnumerable<object> DisabledList { get; set; }
    [Parameter] public string CssClass { get; set; }
    [Parameter] public EventCallback<List<string>> OnSelectList { get; set; }
    [Parameter] public EventCallback<KeyValuePair<string, bool>> OnSelect { get; set; }
    private SelectList _items = new();
    private SelectList _selectedItems = new();
    private SelectList _disabledItems = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (ValidateParameters())
        {
            _items = new SelectList(Items, ValueField, TextField);

            if (SelectedList is not null)
                _selectedItems = new SelectList(SelectedList, ValueField, TextField);
            if (DisabledList is not null)
                _disabledItems = new SelectList(DisabledList, ValueField, TextField);
        }
    }

    async Task OnSelectCheck(SelectListItem item)
    {
        var oldIdem = _selectedItems.Items.FirstOrDefault(x => x.Value == item.Value);
        if (oldIdem is null)
            _selectedItems.Items.Add(item);
        else
            _selectedItems.Items.Remove(oldIdem);

        var keys = _selectedItems.Items.Select(x => x.Value).ToList();
        await OnSelectList.InvokeAsync(keys);
        await OnSelect.InvokeAsync(new KeyValuePair<string, bool>(item.Value, (oldIdem is null)));
    }

    bool ValidateParameters()
    {
        if (string.IsNullOrEmpty(TextField) || string.IsNullOrEmpty(ValueField))
            return false;
        return Items is not null && Items.Any();
    }
}

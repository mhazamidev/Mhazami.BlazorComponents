using Mhazami.BlazorComponents.Models;
using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class CheckboxList
{
    [Parameter] public IEnumerable<object> Items { get; set; }
    [Parameter] public string ValueField { get; set; }
    [Parameter] public string TextField { get; set; }
    [Parameter] public IEnumerable<object> SeletedList { get; set; }
    [Parameter] public IEnumerable<object> DisabledList { get; set; }
    [Parameter] public string CssClass { get; set; }
    [Parameter] public EventCallback<List<string>> OnSelectList { get; set; }
    [Parameter] public EventCallback<string> OnSelect { get; set; }
    private SelectList _items = new();
    private SelectList _selectedItems = new();
    private SelectList _disabledItems = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (Items is not null)
            _items = new SelectList(Items, ValueField, TextField);
        if (SeletedList is not null)
            _selectedItems = new SelectList(SeletedList, ValueField, TextField);
        if (DisabledList is not null)
            _disabledItems = new SelectList(DisabledList, ValueField, TextField);
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
        await OnSelect.InvokeAsync(item.Value);
    }
}

using Mhazami.BlazorComponents.Models;
using Mhazami.BlazorComponents.Utility;
using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class DropDownList
{
    [Parameter] public IEnumerable<object> Items { get; set; }
    [Parameter] public IEnumerable<object> SelectedItems { get; set; }
    [Parameter] public IEnumerable<object> DisabledItems { get; set; }
    [Parameter] public string TextField { get; set; }
    [Parameter] public string ValueField { get; set; }
    [Parameter] public EventCallback<object> onChangeAction { get; set; }
    [Parameter] public EventCallback<IEnumerable<object>> onChangeMultiAction { get; set; }
    [Parameter] public string Id { get; set; }
    [Parameter] public string CustomClass { get; set; }
    [Parameter] public object Value { get; set; }
    [Parameter] public bool MuliSelect { get; set; } = false;
    private SelectList _items = new();
    private bool hide = true;
    private List<SelectListItem> SelectedList = new();
    private List<SelectListItem> SelectableList = new();
    private string SelectedValue = "";

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (ValidateParameters())
        {
            _items = new SelectList(Items, ValueField, TextField);
            if (!MuliSelect && Value is not null)
                SelectedValue = _items.SelectedValue.Text;
            else if (SelectedItems is not null && SelectedItems.Any())
            {
                SelectedList = new SelectList(SelectedItems, ValueField, TextField).Items
                    .Select(x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Text
                    }).ToList();

                SelectedValue = string.Join(',', SelectedList.Select(x => x.Text));

                if (DisabledItems is not null && DisabledItems.Any())
                {
                    SelectableList = new SelectList(DisabledItems, ValueField, TextField).Items
                    .Select(x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Text
                    }).ToList();
                }
            }
        }
    }
    void Toggle() => hide = !hide;

    async Task SelectItem(SelectListItem item)
    {
        if (SelectableList.Any(x => x.Value == item.Value))
            return;
        if (!MuliSelect)
        {
            SelectedValue = item.Text;
            hide = true;
            await onChangeAction.InvokeAsync(item.Value);
        }
        else
        {
            var target = SelectedList.FirstOrDefault(x => x.Value == item.Value);
            if (target is null)
                SelectedList.Add(item);
            else
                SelectedList.Remove(target);
            SelectedValue = string.Join(',', SelectedList.Select(x => x.Text));
            SelectedItems = SelectedList;
            await onChangeMultiAction.InvokeAsync(SelectedList.Select(x => x.Value));
        }
    }

    bool ValidateParameters()
    {
        if (string.IsNullOrEmpty(TextField) || string.IsNullOrEmpty(ValueField))
            return false;
        return Items is not null && Items.Any();
    }
}

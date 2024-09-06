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
    [Parameter] public string DefaultTitle { get; set; }
    [Parameter] public EventCallback<object> OnChangeAction { get; set; }
    [Parameter] public EventCallback<IEnumerable<object>> OnChangeMultiAction { get; set; }
    [Parameter] public string Id { get; set; }
    [Parameter] public string CustomClass { get; set; }
    [Parameter] public object Value { get; set; }
    private object? InternalValue;
    [Parameter] public bool MultiSelect { get; set; } = false;
    private SelectList _items = new();
    private bool hide = true;
    private List<SelectListItem> SelectedList = new();
    private List<SelectListItem> SelectableList = new();
    private string SelectedValue = "";

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Prepare();
    }

    protected override void OnParametersSet()
    {
        Prepare();
    }
    void Prepare()
    {
        if (ValidateParameters())
        {
            if (InternalValue is not null)
                Value = InternalValue;
            _items = new SelectList(Items, ValueField, TextField, Value);
            if (!MultiSelect && Value is not null && _items.SelectedValue is not null)
                SelectedValue = _items.SelectedValue.Text;
            else if (SelectedItems is not null && SelectedItems.Any())
            {
                if (SelectedItems.EqualsSelectedItemType(new List<SelectListItem>()))
                    SelectedList = (List<SelectListItem>)SelectedItems;
                else
                    SelectedList = new SelectList(SelectedItems, ValueField, TextField).Items
                    .Select(x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = x.Text
                    }).ToList();

                SelectedValue = string.Join(',', SelectedList.Select(x => x.Text));
            }
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
        StateHasChanged();
    }
    void Toggle() => hide = !hide;

    async Task SelectItem(SelectListItem item)
    {
        if (SelectableList.Any(x => x.Value == item.Value))
            return;
        if (!MultiSelect)
        {
            SelectedValue = item.Text;
            InternalValue = item.Value;
            hide = true;
            await OnChangeAction.InvokeAsync(item.Value);
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
            var values = SelectedList.Select(x => x.Value).ToList();
            await OnChangeMultiAction.InvokeAsync(values);
        }
    }
    async Task SelectNull()
    {
        SelectedValue = string.Empty;
        InternalValue = string.Empty;
        await OnChangeAction.InvokeAsync("");
        hide = true;
    }
    bool ValidateParameters()
    {
        if (string.IsNullOrEmpty(TextField) || string.IsNullOrEmpty(ValueField))
            return false;
        return Items is not null && Items.Any();
    }
}

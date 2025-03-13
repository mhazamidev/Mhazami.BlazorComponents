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
    [Parameter] public EventCallback<string> OnChangeAction { get; set; }
    [Parameter] public EventCallback<IEnumerable<string>> OnChangeMultiAction { get; set; }
    [Parameter] public string Id { get; set; }
    [Parameter] public string CustomClass { get; set; }
    [Parameter] public object Value { get; set; }
    [Parameter] public bool HasSearch { get; set; } = false;
    private object? InternalValue;
    [Parameter] public bool MultiSelect { get; set; } = false;
    private SelectList _items = new();
    private bool hide = true;
    private List<SelectListItem> SelectedList = new();
    private List<SelectListItem> SelectableList = new();
    private string SelectedValue = "";
    private string SearchText = "";
    List<SelectListItem> Model = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Prepare();
    }

    protected override void OnParametersSet()
    {
        InternalValue = Value;
        Prepare();
    }

    void Prepare()
    {
        if (ValidateParameters())
        {
            if (InternalValue is not null)
                Value = InternalValue;
            _items = new SelectList(Items, ValueField, TextField, Value);
            if (!MultiSelect && _items.SelectedValue is null)
                SelectedValue = string.Empty;
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
            Model.Clear();
            Model.AddRange(_items.Items);
        }
        else
            SelectedValue = string.Empty;
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
        => !(Items is null || !Items.Any() || string.IsNullOrEmpty(TextField) || string.IsNullOrEmpty(ValueField));
    private void Search(ChangeEventArgs e)
    {
        if (e is null)
            return;

        SearchText = e.Value as string;

        Model.Clear();
        List<SelectListItem> result = default!;
        if (!string.IsNullOrEmpty(SearchText))
            result = _items.Items.Where(x => x.Text.ToLower().Contains(SearchText.ToLower().Trim())).ToList();
        else
            result = _items.Items;
        Model.AddRange(result);
        StateHasChanged();
    }

    public void Refresh(object? value = null)
    {
        if (value is not null)
            InternalValue = value;
    }
    public void Reset()
    {
        SelectedValue = string.Empty;
        InternalValue = string.Empty;
        StateHasChanged();
    }
}

public record DropdownListItem(string Key, string Value);

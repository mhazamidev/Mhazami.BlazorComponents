using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class Checkbox
{
    [Parameter] public string DisplayName { get; set; }
    [Parameter] public bool IsChecked { get; set; }
    [Parameter] public EventCallback<bool> OnChecked { get; set; }
    [Parameter] public bool IsDisabled { get; set; }
    [Parameter] public string CssClass { get; set; }

    async Task OnChange(ChangeEventArgs e)
    {
        if (e.Value is not null)
        {
            var value = (bool)e.Value;
            await OnChecked.InvokeAsync(value);
        }
    }

    async Task OnChangeLable()
    {
        IsChecked = !IsChecked;
        await OnChecked.InvokeAsync(IsChecked);
    }
}

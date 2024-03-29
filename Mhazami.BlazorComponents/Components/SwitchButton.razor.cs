using Mhazami.BlazorComponents.Models;
using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class SwitchButton
{
    [Parameter] public string CssClass { get; set; }
    [Parameter] public bool Value { get; set; } = false;
    [Parameter] public string Id { get; set; } = string.Empty;
    [Parameter] public SwitchShape Style { get; set; } = SwitchShape.Square;
    [Parameter] public EventCallback<bool> OnChange { get; set; }
    [Parameter] public EventCallback<KeyValuePair<string, bool>> OnChangeById { get; set; }

    async Task OnChangeCheck(ChangeEventArgs e)
    {
        if (e is not null)
        {
            var val = (bool)e.Value;
            Value = val;
            await OnChange.InvokeAsync(val);
            await OnChangeById.InvokeAsync(new KeyValuePair<string, bool>(Id, val));
        }
    }
}

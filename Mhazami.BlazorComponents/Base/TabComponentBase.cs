using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Base;

public class TabComponentBase : ComponentBase
{
    [CascadingParameter]
    public Dictionary<string, object> Items { get; set; } = new();
}

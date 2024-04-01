using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class ExtendedPanel
{
    [Parameter] public string CssClass { get; set; }
    [Parameter] public string Id { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public RenderFragment Body { get; set; }
    [Parameter] public RenderFragment Header { get; set; }
    [Parameter] public bool IsExtended { get; set; } = false;
}

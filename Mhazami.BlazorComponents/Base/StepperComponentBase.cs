using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Base;

public class StepperComponentBase : ComponentBase
{
    [CascadingParameter]
    public Dictionary<string, object> Items { get; set; } = new();
}

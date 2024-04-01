using Mhazami.BlazorComponents.Base;
using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class Tab : TabComponentBase
{
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public string SelectedTab { get; set; }
    private List<string> Tabs = new();


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Tabs = Items.Keys.ToList();
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}

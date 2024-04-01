using Mhazami.BlazorComponents.Base;
using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class TabItem : TabComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (string.IsNullOrEmpty(Title))
            Title = string.Empty;

        if (ChildContent is not null)
            Items.Add(Title, ChildContent);
        else
            Items.Add(Title, "");
    }
}

using Mhazami.Utility;
using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class ProgressBar
{
	[Parameter] public string CssClass { get; set; }
	[Parameter] public int ProgressBarWidth { get; set; } = 0;

    protected override void OnParametersSet()
    {
        if (ProgressBarWidth < 0 || ProgressBarWidth > 100)
            throw new ArgumentOutOfRangeException(nameof(ProgressBarWidth));
    }
}

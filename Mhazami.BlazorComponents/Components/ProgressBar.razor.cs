using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class ProgressBar
{
	[Parameter] public string CssClass { get; set; }
	[Parameter] public int ProgressBarWidth { get; set; } = 0;
}

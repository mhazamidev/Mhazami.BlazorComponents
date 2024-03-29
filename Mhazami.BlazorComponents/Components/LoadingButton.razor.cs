﻿using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class LoadingButton
{
    [Parameter] public string Text { get; set; }
    [Parameter] public string CssClass { get; set; }
    [Parameter] public bool EnableLoading { get; set; } = false;
}

using Microsoft.AspNetCore.Components;

namespace Mhazami.BlazorComponents.Components;

public partial class Pagination
{
    [Parameter] public int TotalPages { get; set; }
    [Parameter] public int CurrentPage { get; set; } = 1;
    [Parameter] public EventCallback<int> OnPageChange { get; set; }
    [Parameter] public int PaseSize { get; set; } = 10;
    private int FirstPage = 1;
    private int LastPage = 1;


    async Task SelectPage(int page)
    {
        CurrentPage = page;
        FirstPage = CurrentPage - 2 > 0 ? CurrentPage - 2 : 1;
        LastPage = CurrentPage + 2 < TotalPages ? CurrentPage + 2 : CurrentPage + 1 < TotalPages ? CurrentPage + 1 : 0;
        await OnPageChange.InvokeAsync(CurrentPage);
    }

    async Task ChangePage(int page)
    {
        if (page > 0 && CurrentPage + 1 > TotalPages)
            return;
        if (page < 0 && CurrentPage == 1)
            return;
        CurrentPage += page;
        await SelectPage(CurrentPage);
    }
}

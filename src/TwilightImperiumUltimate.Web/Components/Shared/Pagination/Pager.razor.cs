namespace TwilightImperiumUltimate.Web.Components.Shared.Pagination;

public partial class Pager
{
    [Parameter]
    public int TotalCount { get; set; }

    [Parameter]
    public int PageSize { get; set; } = 50;

    [Parameter]
    public int CurrentPage { get; set; } = 1; // 1-based

    [Parameter]
    public EventCallback<int> CurrentPageChanged { get; set; }

    [Parameter]
    public int MaxPageButtons { get; set; } = 5;

    /// <summary>
    /// Gets or sets additional CSS classes applied to the pager's outer container.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional inline styles applied to the pager's outer container. Useful for
    /// page-specific adjustments, for example compensating for a Page with asymmetric horizontal
    /// padding so the pager still appears centered the same way it does on a default 10%/10% page
    /// (see the equivalent comment next to ResponsiveHeading usages on such pages).
    /// </summary>
    [Parameter]
    public string Style { get; set; } = string.Empty;

    private int TotalPages => PageSize <= 0 ? 1 : (int)Math.Ceiling((double)TotalCount / PageSize);

    private bool IsFirstPage => CurrentPage <= 1;

    private bool IsLastPage => CurrentPage >= TotalPages;

    private IEnumerable<int> VisiblePages
    {
        get
        {
            if (TotalPages <= 0)
                yield break;

            var half = MaxPageButtons / 2;
            var start = Math.Max(1, CurrentPage - half);
            var end = Math.Min(TotalPages, start + MaxPageButtons - 1);

            // adjust start if not enough pages at the end
            start = Math.Max(1, end - MaxPageButtons + 1);

            for (var i = start; i <= end; i++)
            {
                yield return i;
            }
        }
    }

    private async Task ChangePage(int page)
    {
        if (page < 1 || page > TotalPages || page == CurrentPage)
            return;

        CurrentPage = page;
        await CurrentPageChanged.InvokeAsync(page);
    }

    private Task MoveFirst() => ChangePage(1);

    private Task MoveLast() => ChangePage(TotalPages);

    private Task MovePrevious() => ChangePage(CurrentPage - 1);

    private Task MoveNext() => ChangePage(CurrentPage + 1);
}

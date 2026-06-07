using Microsoft.AspNetCore.Components.Routing;

namespace TwilightImperiumUltimate.Web.Pages.Rules;

public sealed partial class Rules : IDisposable
{
    private static readonly char[] UriSeparators = ['?', '#'];

    [Parameter]
    [SupplyParameterFromQuery(Name = "search")]
    public string SearchWord { get; set; } = string.Empty;

    [Parameter]
    [SupplyParameterFromQuery(Name = "letter")]
    public string? Letter { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "category")]
    public string? Category { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "source")]
    public string? Source { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "version")]
    public string? Version { get; set; }

    [Parameter]
    public int? RuleId { get; set; }

    [Parameter]
    public string? ItemKey { get; set; }

    private string _section = "index";

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        _section = GetSection(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        var section = GetSection(args.Location);
        if (section == _section)
            return;

        _section = section;
        _ = InvokeAsync(StateHasChanged);
    }

    private string GetSection(string uri)
    {
        var relativePath = NavigationManager.ToBaseRelativePath(uri);
        var separatorIndex = relativePath.IndexOfAny(UriSeparators);
        if (separatorIndex >= 0)
            relativePath = relativePath[..separatorIndex];
        relativePath = relativePath.Trim('/');

        if (relativePath.StartsWith("rules/factions", StringComparison.OrdinalIgnoreCase))
            return "factions";

        if (relativePath.StartsWith("rules/components", StringComparison.OrdinalIgnoreCase))
            return "components";

        return relativePath == "rules" ? "index" : "rules";
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
        GC.SuppressFinalize(this);
    }
}

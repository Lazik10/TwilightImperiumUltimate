using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class GameVersionFilterDropdown
{
    private sealed record GameVersionDropdownItem(string Value, string Label);

    [Parameter]
    public IEnumerable<GameVersion> GameVersions { get; set; } = Enumerable.Empty<GameVersion>();

    [Parameter]
    public GameVersion? SelectedGameVersion { get; set; }

    [Parameter]
    public EventCallback<GameVersion?> SelectedGameVersionChanged { get; set; }

    [Parameter]
    public string AllLabel { get; set; } = "All";

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    private string GetStyle() => $"{Style}";

    private string GetCssClass() => $"game-version-filter {CssClass}";

    private IEnumerable<GameVersion> GetOrderedVersions() =>
        GameVersions
            .Distinct()
            .OrderBy(x => x);

    private IEnumerable<GameVersionDropdownItem> GetDropdownItems()
    {
        var items = new List<GameVersionDropdownItem>
        {
            new(string.Empty, AllLabel),
        };

        items.AddRange(GetOrderedVersions().Select(x => new GameVersionDropdownItem(x.ToString(), x.GetDisplayName())));

        return items;
    }

    private string GetSelectedValue() => SelectedGameVersion?.ToString() ?? string.Empty;

    private async Task OnRadzenSelectionChanged(object value)
    {
        var selectedValue = value?.ToString();

        if (string.IsNullOrWhiteSpace(selectedValue))
        {
            await SelectedGameVersionChanged.InvokeAsync(null);
            return;
        }

        if (Enum.TryParse<GameVersion>(selectedValue, out var parsedVersion))
        {
            await SelectedGameVersionChanged.InvokeAsync(parsedVersion);
            return;
        }

        await SelectedGameVersionChanged.InvokeAsync(null);
    }
}

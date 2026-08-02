namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionSystemStats
{
    private string _title = string.Empty;

    private List<(string Label, string Value)> _stats = new();

    [Parameter]
    public FactionName FactionName { get; set; } = default!;

    protected override void OnParametersSet()
    {
        ParseSystemStats();
    }

    private void ParseSystemStats()
    {
        _title = string.Empty;
        _stats = new List<(string Label, string Value)>();

        var lines = FactionName.GetFactionUIText(FactionResourceType.SystemStats)
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var line in lines)
        {
            var separatorIndex = line.IndexOf('.', StringComparison.Ordinal);

            if (separatorIndex < 0)
            {
                _title = line;
                continue;
            }

            var label = line[..separatorIndex].Trim();
            var value = line[(separatorIndex + 1)..].Trim();
            _stats.Add((label, value));
        }
    }
}

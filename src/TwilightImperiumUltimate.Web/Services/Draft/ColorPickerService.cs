using System.Net.Http.Json;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.Drafts;
using TwilightImperiumUltimate.Web.Models.Factions;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Settings.Drafts;

namespace TwilightImperiumUltimate.Web.Services.Draft;

public class ColorPickerService : IColorPickerService
{
    private static readonly Random _random = new();
    private readonly HttpClient _http;
    private IReadOnlyCollection<FactionModel> _selectedFactions = new List<FactionModel>();
    private Dictionary<PlayerColor, bool> _colors = new Dictionary<PlayerColor, bool>();
    private IReadOnlyCollection<FactionColorDraftResult>? _results = new List<FactionColorDraftResult>();

    public ColorPickerService(HttpClient http)
    {
        _http = http;
        InitializeColors();
        ResetSelectedFactions();
    }

    public event EventHandler? OnFactionUpdate;

    public IDictionary<PlayerColor, bool> Colors => _colors;

    public IReadOnlyCollection<FactionModel> SelectedFactions => _selectedFactions;

    public IReadOnlyCollection<FactionColorDraftResult>? Results => _results;

    public bool IsDraftPossible() => Colors.Count(x => !x.Value) >= SelectedFactions.Count;

    public PlayerColor GetRandomColor() => Colors.Keys.ToList()[_random.Next(Colors.Count)];

    public void ResetSelectedFactions()
    {
        _selectedFactions = new List<FactionModel>();
        _results = new List<FactionColorDraftResult>();
    }

    public void ResetBannedColors() => InitializeColors();

    public void UpdateColorBanStatus(PlayerColor color)
    {
        _colors[color] = !_colors[color];
    }

    public void UpdateSelectedFactions(IReadOnlyCollection<FactionModel>? factions, FactionModel faction)
    {
        if (factions?.Count(x => !x.Banned) > ColorDraftSettings.MaxNumberOfFactions
            && faction is not null)
        {
            faction.Banned = true;
            return;
        }

        _selectedFactions = factions != null
                ? factions.Where(x => !x.Banned).ToArray()
                : new List<FactionModel>();
    }

    public async Task PerformDraft()
    {
        await GetRandomResultsAsync();
        await GetColorDraftResults();
    }

    private async Task GetRandomResultsAsync()
    {
        for (int i = 0; i < FactionDraftSettings.DefaultNumberOfAssignments; i++)
        {
            _results = GenerateFakeColorDraftResult();
            OnFactionUpdate?.Invoke(this, EventArgs.Empty);
            await Task.Delay(FactionDraftSettings.DefaultDelayInMilliseconds);
        }
    }

    private List<FactionColorDraftResult> GenerateFakeColorDraftResult()
    {
        var availableColors = _colors
            .Where(kvp => !kvp.Value)
            .Select(kvp => kvp.Key)
            .OrderBy(x => _random.Next())
            .ToList();

        return _selectedFactions.Select((x, index) => new FactionColorDraftResult()
        {
            FactionName = x.FactionName,
            Color = availableColors[index],
        }).ToList();
    }

    private void InitializeColors()
    {
        _colors = Enum.GetValues<PlayerColor>()
            .ToDictionary(x => x, x => false);
    }

    private async Task GetColorDraftResults()
    {
        Uri uri = new(Paths.ApiPath_ColorDraft);

        ColorDraftRequest request = new()
        {
            Factions = _selectedFactions.Select(x => x.FactionName).ToList(),
            Colors = _colors.Where(x => !x.Value).Select(x => x.Key).ToList(),
        };

        var response = await _http.PostAsJsonAsync(uri, request);

        var results = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<FactionColorDraftResult>>();
        _results = results!.OrderBy(results => results.FactionName).ToList();
    }
}

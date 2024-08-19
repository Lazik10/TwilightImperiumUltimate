using TwilightImperiumUltimate.Contracts.DTOs.Draft;
using TwilightImperiumUltimate.Web.Options.Drafts;

namespace TwilightImperiumUltimate.Web.Services.Draft;

public class ColorPickerService : IColorPickerService
{
    private static readonly Random _random = new();
    private readonly ITwilightImperiumApiHttpClient _httpClient;
    private IReadOnlyCollection<FactionModel> _selectedFactions = [];
    private Dictionary<PlayerColor, bool> _colors = [];
    private IReadOnlyCollection<FactionColorDraftResult>? _factionColorDraftResults = [];

    public ColorPickerService(ITwilightImperiumApiHttpClient httpClient)
    {
        _httpClient = httpClient;
        InitializeColors();
        ResetSelectedFactions();
    }

    public event EventHandler? OnFactionUpdate;

    public IDictionary<PlayerColor, bool> Colors => _colors;

    public IReadOnlyCollection<FactionModel> SelectedFactions => _selectedFactions;

    public IReadOnlyCollection<FactionColorDraftResult>? FactionColorDraftResults => _factionColorDraftResults;

    public bool IsDraftPossible() => Colors.Count(x => !x.Value) >= SelectedFactions.Count;

    public PlayerColor GetRandomColor() => Colors.Keys.ToList()[_random.Next(Colors.Count)];

    public void ResetSelectedFactions()
    {
        _selectedFactions = [];
        _factionColorDraftResults = [];
    }

    public void ResetBannedColors() => InitializeColors();

    public void UpdateColorBanStatus(PlayerColor color)
    {
        _colors[color] = !_colors[color];
    }

    public void UpdateSelectedFactions(IReadOnlyCollection<FactionModel>? factions, FactionModel faction)
    {
        if (factions?.Count(x => !x.Banned) > ColorDraftOptions.MaxNumberOfFactions
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
        for (int i = 0; i < FactionDraftOptions.DefaultNumberOfAssignments; i++)
        {
            _factionColorDraftResults = GenerateFakeColorDraftResult();
            OnFactionUpdate?.Invoke(this, EventArgs.Empty);
            await Task.Delay(FactionDraftOptions.DefaultDelayInMilliseconds);
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
        ColorDraftRequest request = new()
        {
            Factions = _selectedFactions.Select(x => x.FactionName).ToList(),
            Colors = _colors.Where(x => !x.Value).Select(x => x.Key).ToList(),
        };

        var (response, statusCode) = await _httpClient.PostAsync<ColorDraftRequest, ApiResponse<FactionColorDraftResultDto>>(Paths.ApiPath_ColorDraft, request);

        if (statusCode == HttpStatusCode.OK)
        {
            var factionColorDraftResults = response!.Data!.FactionColorDraftResults;

            _factionColorDraftResults = factionColorDraftResults
                .Select(factionColorDraftResult => new FactionColorDraftResult
                {
                    FactionName = factionColorDraftResult.Key,
                    Color = factionColorDraftResult.Value,
                })
                .OrderBy(x => x.FactionName)
                .ToList();
        }
        else
        {
            _factionColorDraftResults = new List<FactionColorDraftResult>();
        }
    }
}

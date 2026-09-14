using System.Globalization;
using TwilightImperiumUltimate.Contracts.DTOs.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Players;

public partial class AsyncPlayersList
{
    private List<AsyncPlayerProfileDto>? _filteredPlayerProfiles = new();
    private IReadOnlyCollection<AsyncPlayerProfileDto>? _cachedPlayerProfileNames;
    private List<AsyncPlayerProfileDto> _orderedPlayerProfiles = [];
    private Dictionary<char, List<AsyncPlayerProfileDto>> _playerGroups = [];
    private AsyncPlayerSearchbar? _searchbar;
    private char? _selectedLetter = 'A';
    private char _digitGroup = '1';
    private char _othersGroup = '*';

    [CascadingParameter(Name = "Letter")]
    public string Letter { get; set; } = string.Empty;

    [CascadingParameter(Name = "AsyncPlayerProfileNames")]
    public IReadOnlyCollection<AsyncPlayerProfileDto> PlayerProfileNames { get; set; } = new List<AsyncPlayerProfileDto>();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnParametersSet()
    {
        CachePlayerProfiles();

        if (!string.IsNullOrEmpty(Letter))
        {
            _selectedLetter = Letter.ToUpper(CultureInfo.InvariantCulture)[0];
        }

        _playerGroups.TryGetValue(_selectedLetter ?? char.MinValue, out var playerGroup);

        _filteredPlayerProfiles = playerGroup is null ? [.. _orderedPlayerProfiles] : [.. playerGroup];

        _filteredPlayerProfiles = _filteredPlayerProfiles.OrderBy(x => x.DiscordUsername).ToList();
    }

    private void CachePlayerProfiles()
    {
        if (ReferenceEquals(_cachedPlayerProfileNames, PlayerProfileNames))
            return;

        _cachedPlayerProfileNames = PlayerProfileNames;
        _orderedPlayerProfiles = PlayerProfileNames.OrderBy(x => x.DiscordUsername).ToList();
        _playerGroups = PlayerProfileNames
            .GroupBy(x =>
            {
                var firstChar = x.DiscordUsername.ToUpperInvariant().First();
                if (char.IsLetter(firstChar))
                {
                    return firstChar;
                }
                else if (char.IsDigit(firstChar))
                {
                    return _digitGroup;
                }
                else
                {
                    return _othersGroup;
                }
            })
            .ToDictionary(group => group.Key, group => group.OrderBy(x => x.DiscordUsername).ToList());
    }

    private async Task SearchPlayerGroup(char letter)
    {
        _selectedLetter = letter;
        await (_searchbar?.ResetSearchAsync() ?? Task.CompletedTask);

        _playerGroups.TryGetValue(letter, out var playerGroup);
        _filteredPlayerProfiles = playerGroup is null ? [] : [.. playerGroup];
    }

    private void SearchPlayerGroup(string search)
    {
        _selectedLetter = 'A';

        if (search.Length < 3)
        {
            _playerGroups.TryGetValue(_selectedLetter.Value, out var playerGroup);
            _filteredPlayerProfiles = playerGroup is null ? [] : [.. playerGroup];
            return;
        }

        _selectedLetter = null;
        _filteredPlayerProfiles = _orderedPlayerProfiles
            .Where(profile => profile.DiscordUsername.Contains(search, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private void RedirectToPlayerProfile(int id)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={id}");
    }
}

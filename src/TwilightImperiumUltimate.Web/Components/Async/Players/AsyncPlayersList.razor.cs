using System.Globalization;
using TwilightImperiumUltimate.Contracts.DTOs.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Players;

public partial class AsyncPlayersList
{
    private List<AsyncPlayerProfileDto>? _filteredPlayerProfiles = new();
    private char _selectedLetter = 'A';
    private char _digitGroup = '1';
    private char _othersGroup = '*';

    [CascadingParameter(Name = "Letter")]
    public string Letter { get; set; } = string.Empty;

    [CascadingParameter(Name = "AsyncPlayerProfileNames")]
    public IReadOnlyCollection<AsyncPlayerProfileDto> PlayerProfileNames { get; set; } = new List<AsyncPlayerProfileDto>();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private List<IGrouping<char, AsyncPlayerProfileDto>> GroupedPlayers =>
        PlayerProfileNames.GroupBy(x =>
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
        .ToList();

    protected override void OnParametersSet()
    {
        if (!string.IsNullOrEmpty(Letter))
        {
            _selectedLetter = Letter.ToUpper(CultureInfo.InvariantCulture)[0];
        }

        var playerGroup = GroupedPlayers
            .Find(group => group.Key == _selectedLetter)
            ?.ToList();

        _filteredPlayerProfiles = playerGroup is null ? PlayerProfileNames.ToList() : playerGroup;

        OrderPlayerList();
    }

    private void SearchPlayerGroup(char letter)
    {
        _selectedLetter = letter;

        _filteredPlayerProfiles = GroupedPlayers
            .Find(group => group.Key == letter)
            ?.ToList();

        OrderPlayerList();
    }

    private void SearchPlayerGroup(string search)
    {
        if (search.Length == 0)
        {
            _filteredPlayerProfiles = GroupedPlayers
                .Find(group => group.Key == _selectedLetter)
                ?.ToList();
        }
        else if (_selectedLetter != search[0])
        {
            _selectedLetter = search[0];
        }

        var group = GroupedPlayers
            .Find(group => group.Key == _selectedLetter)
            ?.ToList();

        if (group is null)
        {
            _filteredPlayerProfiles = PlayerProfileNames.ToList();
            return;
        }

        _filteredPlayerProfiles = group
            .Where(profile => profile.DiscordUsername.Contains(search, StringComparison.OrdinalIgnoreCase))
            .ToList();

        OrderPlayerList();
    }

    private void OrderPlayerList()
    {
        _filteredPlayerProfiles = _filteredPlayerProfiles?.OrderBy(x => x.DiscordUsername).ToList();
    }

    private void RedirectToPlayerProfile(int id)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={id}");
    }
}
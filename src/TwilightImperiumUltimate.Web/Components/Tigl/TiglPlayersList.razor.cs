using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglPlayersList
{
    private readonly char _digitGroup = '1';
    private readonly char _othersGroup = '*';
    private List<TiglUserLiteDto>? _filteredUsers = new();
    private char? _selectedLetter = 'A';
    private TiglPlayerSearchbar? _searchbar;

    [Parameter]
    public IReadOnlyCollection<TiglUserLiteDto> Users { get; set; } = new List<TiglUserLiteDto>();

    [Parameter]
    public bool Loading { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnParametersSet()
    {
        var group = GroupedUsers().Find(g => g.Key == _selectedLetter);
        _filteredUsers = group is null ? Users.ToList() : group.ToList();
        OrderList();
    }

    private static string GetUserName(TiglUserLiteDto player)
    {
        if (player.TiglUserName == player.DiscordUserName)
            return player.TiglUserName;

        return $"{player.TiglUserName} ({player.DiscordUserName})";
    }

    private List<IGrouping<char, TiglUserLiteDto>> GroupedUsers() =>
        Users.GroupBy(x =>
        {
            if (string.IsNullOrWhiteSpace(x.TiglUserName))
                return _othersGroup;

            var firstChar = x.TiglUserName.ToUpperInvariant().First();

            if (char.IsLetter(firstChar))
                return firstChar;

            if (char.IsDigit(firstChar))
                return _digitGroup;

            return _othersGroup;
        })
        .ToList();

    private async Task FilterByLetter(char letter)
    {
        _selectedLetter = letter;
        await (_searchbar?.ResetSearchAsync() ?? Task.CompletedTask);

        _filteredUsers = GroupedUsers().Find(g => g.Key == letter)?.ToList() ?? new List<TiglUserLiteDto>();
        OrderList();
    }

    private void SearchPlayers(string search)
    {
        _selectedLetter = 'A';

        if (search.Length < 3)
        {
            _filteredUsers = GroupedUsers().Find(g => g.Key == _selectedLetter)?.ToList() ?? new List<TiglUserLiteDto>();
            OrderList();
            return;
        }

        _selectedLetter = null;
        _filteredUsers = Users
            .Where(u => u.TiglUserName.Contains(search, StringComparison.OrdinalIgnoreCase))
            .ToList();
        OrderList();
    }

    private void OrderList()
    {
        _filteredUsers = _filteredUsers?.OrderBy(u => u.TiglUserName).ToList();
        StateHasChanged();
    }

    private void RedirectToPlayer(TiglUserLiteDto player)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.TiglPlayerProfile}?playerId={player.Id}");
    }
}

using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglPlayersList
{
    private List<TiglUserLiteDto>? _filteredUsers = new();
    private char _selectedLetter = 'A';
    private readonly char _digitGroup = '1';
    private readonly char _othersGroup = '*';

    [Parameter]
    public IReadOnlyCollection<TiglUserLiteDto> Users { get; set; } = new List<TiglUserLiteDto>();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private List<IGrouping<char, TiglUserLiteDto>> GroupedUsers =>
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

    protected override void OnParametersSet()
    {
        var group = GroupedUsers.Find(g => g.Key == _selectedLetter)?.ToList();
        _filteredUsers = group is null ? Users.ToList() : group;
        OrderList();
    }

    private void FilterByLetter(char letter)
    {
        _selectedLetter = letter;
        _filteredUsers = GroupedUsers.Find(g => g.Key == letter)?.ToList() ?? new List<TiglUserLiteDto>();
        OrderList();
    }

    private void SearchPlayers(string search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            _filteredUsers = GroupedUsers.Find(g => g.Key == _selectedLetter)?.ToList() ?? Users.ToList();
            OrderList();
            return;
        }

        _selectedLetter = char.ToUpperInvariant(search[0]);
        var group = GroupedUsers.Find(g => g.Key == _selectedLetter)?.ToList() ?? Users.ToList();
        _filteredUsers = group
            .Where(u => u.TiglUserName.Contains(search, StringComparison.OrdinalIgnoreCase))
            .ToList();
        OrderList();
    }

    private void OrderList()
    {
        _filteredUsers = _filteredUsers?.OrderBy(u => u.TiglUserName).ToList();
        StateHasChanged();
    }

    private static string GetUserName(TiglUserLiteDto player)
    {
        if (player.TiglUserName == player.DiscordUserName)
            return player.TiglUserName;

        return $"{player.TiglUserName} ({player.DiscordUserName})";
    }

    private void RedirectToPlayer(int id)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.TiglPlayerProfile}?playerId={id}");
    }
}

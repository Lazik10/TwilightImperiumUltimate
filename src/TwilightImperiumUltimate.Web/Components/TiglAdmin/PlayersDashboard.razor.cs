using TwilightImperiumUltimate.Contracts.ApiContracts.Rankings;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Prestige;
using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.TiglAdmin;

public partial class PlayersDashboard
{
    private bool _loading;
    private IReadOnlyCollection<TiglUserLiteDto> _users = Array.Empty<TiglUserLiteDto>();
    private TiglUserLiteDto? _selectedUser;

    private RankingsUserDto? _userOverview;
    private IReadOnlyCollection<RankHistoryDto> _rankHistory = Array.Empty<RankHistoryDto>();
    private IReadOnlyCollection<PrestigeRankHistoryDto> _prestigeHistory = Array.Empty<PrestigeRankHistoryDto>();

    private TiglLeague _selectedLeague = TiglLeague.ThundersEdge;
    private TiglRankName _newRankName = TiglRankName.Agent;
    private DateTime _newRankDate = DateTime.UtcNow;

    private TiglPrestigeRank _newPrestigeRank = TiglPrestigeRank.GalacticThreat;
    private int _newPrestigeLevel;
    private DateTime _newPrestigeRankDate = DateTime.UtcNow;

    private IReadOnlyList<PrestigeRankOption> _prestigeRankOptions = Array.Empty<PrestigeRankOption>();

    private List<TiglRankName> ExcludedRanksForLeague => GetExcludedRanksForLeague();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _prestigeRankOptions = Enum.GetValues<TiglPrestigeRank>()
            .Select(r => new PrestigeRankOption(r, r.GetDisplayName()))
            .ToList();

        await LoadUsersAsync();
    }

    private List<TiglRankName> GetExcludedRanksForLeague()
    {
        if (_selectedLeague == TiglLeague.ProphecyOfKings || _selectedLeague == TiglLeague.ThundersEdge)
        {
            return new List<TiglRankName>()
            {
                TiglRankName.Thrall,
                TiglRankName.Acolyte,
                TiglRankName.Legionnaire,
                TiglRankName.Starlancer,
                TiglRankName.GeneSorcerer,
                TiglRankName.IxthLord,
                TiglRankName.Archon,
            };
        }

        if (_selectedLeague == TiglLeague.Fractured)
        {
            return new List<TiglRankName>()
            {
                TiglRankName.Minister,
                TiglRankName.Agent,
                TiglRankName.Commander,
                TiglRankName.Hero,
            };
        }

        return new List<TiglRankName>();
    }

    private async Task LoadUsersAsync()
    {
        _loading = true;
        var usersResult = await HttpClient.GetAsync<ApiResponse<ItemListDto<TiglUserLiteDto>>>(Paths.ApiPath_TiglUsers);
        var usersStatus = usersResult.StatusCode;
        var usersResponse = usersResult.Response;
        if (usersStatus == HttpStatusCode.OK && usersResponse?.Data?.Items is not null)
            _users = usersResponse.Data.Items;
        else
            _users = Array.Empty<TiglUserLiteDto>();
        _loading = false;
    }

    private async Task OnUserSelected(int tiglUserId)
    {
        _selectedUser = _users.FirstOrDefault(u => u.Id == tiglUserId);
        _userOverview = null;
        _rankHistory = Array.Empty<RankHistoryDto>();
        _prestigeHistory = Array.Empty<PrestigeRankHistoryDto>();

        if (_selectedUser is null)
            return;

        await LoadUserRanksAsync(_selectedUser.Id);
        await LoadUserRankHistoryAsync(_selectedUser.Id);
        await LoadUserPrestigeHistoryAsync(_selectedUser.Id);
    }

    private async Task LoadUserRanksAsync(int tiglUserId)
    {
        var ranksResult = await HttpClient.GetAsync<ApiResponse<RankingsUserDto>>(string.Concat(Paths.ApiPath_RankingsUserOverview, tiglUserId));
        if (ranksResult.StatusCode == HttpStatusCode.OK && ranksResult.Response?.Data is not null)
            _userOverview = ranksResult.Response.Data;
    }

    private async Task LoadUserRankHistoryAsync(int tiglUserId)
    {
        var rankHistoryResult = await HttpClient.GetAsync<ApiResponse<ItemListDto<RankHistoryDto>>>(string.Concat(Paths.ApiPath_RankingsUserHistory, tiglUserId));
        if (rankHistoryResult.StatusCode == HttpStatusCode.OK && rankHistoryResult.Response?.Data?.Items is not null)
            _rankHistory = rankHistoryResult.Response.Data.Items;
    }

    private async Task LoadUserPrestigeHistoryAsync(int tiglUserId)
    {
        var prestigeHistoryResult = await HttpClient.GetAsync<ApiResponse<ItemListDto<PrestigeRankHistoryDto>>>(string.Concat(Paths.ApiPath_RankingsUserPrestigeHistory, tiglUserId));
        if (prestigeHistoryResult.StatusCode == HttpStatusCode.OK && prestigeHistoryResult.Response?.Data?.Items is not null)
            _prestigeHistory = prestigeHistoryResult.Response.Data.Items;
    }

    private static string GetLeagueRank(RankingsUserDto overview, TiglLeague league)
    {
        var l = overview.Leagues.FirstOrDefault(x => x.League == league);
        return l is null ? TiglRankName.Unranked.GetDisplayName() : l.CurrentRank.GetDisplayName();
    }

    private TextColor GetLeagueRankTextColor(RankingsUserDto overview, TiglLeague league)
    {
        var l = overview.Leagues.FirstOrDefault(x => x.League == league);
        return l?.CurrentRank.GetRankColor() ?? TextColor.White;
    }

    private static string FormatTimestamp(long ts)
    {
        if (ts <= 0)
            return "-";
        var dt = DateTimeOffset.FromUnixTimeMilliseconds(ts).ToLocalTime().DateTime;
        return dt.ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
    }

    private void ChangeLeague(TiglLeague league)
    {
        _selectedLeague = league;
        StateHasChanged();
    }

    private async Task AwardPrestigeRankAsync(TiglPrestigeRank rank, TiglFactionName faction = TiglFactionName.None, int level = 0, long achievedAt = 0)
    {
        if (_selectedUser is null)
            return;

        var req = new AwardPrestigeRankRequest
        {
            TiglUserId = _selectedUser.Id,
            PrestigeRank = rank,
            League = _selectedLeague,
            Faction = faction,
            Level = level,
            AchievedAt = achievedAt,
        };

        var awardResult = await HttpClient.PostAsync<AwardPrestigeRankRequest, ApiResponse<AwardPrestigeRankResponse>>(Paths.ApiPath_AwardPrestigeRank, req);
        if (awardResult.StatusCode == HttpStatusCode.OK && awardResult.Response?.Data?.Success == true)
        {
            await LoadUserPrestigeHistoryAsync(_selectedUser.Id);
            StateHasChanged();
        }
    }

    private async Task AddPrestigeRankAsync()
    {
        if (_selectedUser is null)
            return;

        var achievedAt = new DateTimeOffset(_newPrestigeRankDate).ToUnixTimeMilliseconds();
        await AwardPrestigeRankAsync(_newPrestigeRank, TiglFactionName.None, _newPrestigeLevel, achievedAt);
    }

    private async Task RemovePrestigeRankAsync(PrestigeRankHistoryDto prestigeEntry)
    {
        if (_selectedUser is null)
            return;

        var req = new RemovePrestigeRankRequest
        {
            TiglUserId = _selectedUser.Id,
            PrestigeRank = prestigeEntry.PrestigeRank,
            League = prestigeEntry.League,
            Faction = prestigeEntry.Faction,
            Level = prestigeEntry.Level,
        };
        var removePrestigeResult = await HttpClient.DeleteAsync<RemovePrestigeRankRequest, ApiResponse<RemovePrestigeRankResponse>>(Paths.ApiPath_RemovePrestigeRank, req);
        if (removePrestigeResult.StatusCode == HttpStatusCode.OK && removePrestigeResult.Response?.Data?.Success == true)
        {
            await LoadUserPrestigeHistoryAsync(_selectedUser.Id);
            StateHasChanged();
        }
    }

    private async Task AddRankHistoryAsync()
    {
        if (_selectedUser is null)
            return;

        var request = new AddRankHistoryRequest
        {
            TiglUserId = _selectedUser.Id,
            League = _selectedLeague,
            Rank = _newRankName,
            AchievedAt = new DateTimeOffset(_newRankDate).ToUnixTimeMilliseconds(),
        };

        var addRankResult = await HttpClient.PostAsync<AddRankHistoryRequest, ApiResponse<AddRankHistoryResponse>>(Paths.ApiPath_AddRankHistory, request);
        if (addRankResult.StatusCode == HttpStatusCode.OK && addRankResult.Response?.Data?.Success == true)
        {
            await LoadUserRankHistoryAsync(_selectedUser.Id);
            await LoadUserRanksAsync(_selectedUser.Id);
            StateHasChanged();
        }
    }

    private async Task RemoveRankHistoryAsync(RankHistoryDto entry)
    {
        if (entry is null || _selectedUser is null)
            return;

        var request = new RemoveRankHistoryRequest { RankHistoryId = entry.Id };
        var removeRankResult = await HttpClient.DeleteAsync<RemoveRankHistoryRequest, ApiResponse<RemoveRankHistoryResponse>>(Paths.ApiPath_RemoveRankHistory, request);
        if (removeRankResult.StatusCode == HttpStatusCode.OK && removeRankResult.Response?.Data?.Success == true)
        {
            await LoadUserRankHistoryAsync(_selectedUser.Id);
            await LoadUserRanksAsync(_selectedUser.Id);
            StateHasChanged();
        }
    }

    private sealed record PrestigeRankOption(TiglPrestigeRank Value, string Text);
}

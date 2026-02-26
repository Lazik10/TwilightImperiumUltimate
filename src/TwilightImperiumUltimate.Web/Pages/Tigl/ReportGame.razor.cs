using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Helpers.TiglFactions;

namespace TwilightImperiumUltimate.Web.Pages.Tigl;

public partial class ReportGame
{
    private const int MinNumberOfPlayers = 6;
    private const int MaxNumberOfPlayers = 8;
    private const int MinVictoryPoints = 10;
    private const int MaxStandardLeagueVictoryPoints = 14;
    private const int MaxVictoryPoints = 20;
    private const int VictoryPointStandardStep = 2;
    private const int VictoryPointFracturedStep = 1;
    private int _selectedVpCount = MinVictoryPoints;
    private TiglLeague _selectedLeague = TiglLeague.ThundersEdge;
    private int _round = 5;
    private int _minRound = 1;
    private int _maxRound = 9;
    private GameReport gameReportRequest = new();
    private List<PlayerResult> playerResults = new();
    private bool isSubmitting;
    private string errorMessage = string.Empty;
    private string errorDetails = string.Empty;
    private List<string> validationErrors = new List<string>();
    private string successMessage = string.Empty;
    private DateTime startGame;
    private DateTime endGame;
    private string? _clientTimeZoneId;
    private IJSObjectReference? _module;

    private int _currentPlayerIndex;
    private List<RowViewMode> _rowModes = new();

    private bool _loading = true;
    private IList<TiglUserLiteDto> _users = Array.Empty<TiglUserLiteDto>();
    private Dictionary<long, TiglUserLiteDto> _usersById = new();
    private List<string> _availableFactionNames = new();

    private enum RowViewMode
    {
        Search,
        Edit,
    }

    // Keep names in sync with TiglGalacticEventConverter map keys
    private static readonly List<string> _allGalacticEvents = new()
    {
        "Age of Commerce",
        "Call of the Void",
        "Dangerous Wilds",
        "Hidden Agenda",
        "Age of Exploration",
        "Civilized Society",
        "Minor Factions",
        "Stellar Atomics",
        "Age of Fighters",
        "Total War",
        "Wild Wild Galaxy",
        "Weird Wormholes",
        "Monuments to the Ages",
        "Cosmic Phenomenae",
        "Cultural Exchange Program",
        "Mercenaries for Hire",
        "Rapid Mobilization",
        "Zealous Orthodoxy",
        "Advent of the War Sun",
        "Conventions of War Abandoned",
    };

    private readonly HashSet<string> _selectedEvents = new(StringComparer.OrdinalIgnoreCase);

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        InitializeForm();
        UpdateFactionList();
        await LoadUsersAsync();
        _loading = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Tigl/ReportGame.razor.js");

                _clientTimeZoneId = await _module.InvokeAsync<string>("getTimeZone");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load timezone: {ex.Message}");
                _clientTimeZoneId = null;
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void InitializeForm()
    {
        gameReportRequest = new GameReport
        {
            Source = ResultSource.Async,
            Score = 10,
            EndTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
            League = TiglLeague.ThundersEdge,
            PlayerCount = MinNumberOfPlayers,
        };

        // Keep local state in sync with the bound model values right away
        _selectedLeague = gameReportRequest.League;
        _selectedVpCount = gameReportRequest.Score;
        _round = 5;

        // Initialize dates to avoid invalid DateTime conversions
        startGame = DateTime.Now;
        endGame = DateTime.Now;

        playerResults = new List<PlayerResult>();
        _rowModes = new List<RowViewMode>();
        for (int i = 0; i < MinNumberOfPlayers; i++)
        {
            playerResults.Add(new PlayerResult());
            _rowModes.Add(RowViewMode.Search);
        }

        ClearMessages();
    }

    private async Task LoadUsersAsync()
    {
        var (response, status) = await HttpClient.GetAsync<ApiResponse<ItemListDto<TiglUserLiteDto>>>(Paths.ApiPath_TiglUsers);
        if (status == HttpStatusCode.OK && response?.Data?.Items is not null)
        {
            _users = response.Data.Items as IList<TiglUserLiteDto> ?? response.Data.Items.ToList();
            _usersById = _users.ToDictionary(u => u.DiscordUserId);
        }
        else
        {
            _users = Array.Empty<TiglUserLiteDto>();
            _usersById = new();
        }
    }

    private void UpdateFactionList()
    {
        var all = Enum.GetValues<TiglFactionName>()
            .Where(f => f != TiglFactionName.None && f != TiglFactionName.TwilightsFall);

        if (gameReportRequest.League == TiglLeague.ThundersEdge)
        {
            all = all.Where(f => f <= TiglFactionName.TheRalNelConsortium);
        }

        _availableFactionNames = all
            .OrderBy(x => x)
            .Select(f => TiglFactionParser.ToFactionString(f))
            .Distinct()
            .ToList();
    }

    private void ChangeLeague(TiglLeague league)
    {
        // Update both local state and the bound model immediately, so other logic uses correct value before any subsequent change.
        _selectedLeague = league;
        gameReportRequest.League = league;

        // Reset VP to minimum when league changes
        _selectedVpCount = MinVictoryPoints;

        if (league != TiglLeague.Fractured)
            _selectedEvents.Clear();

        UpdateFactionList();
    }

    private TiglUserLiteDto? FindUser(long discordUserId)
        => _usersById.GetValueOrDefault(discordUserId);

    private string GetDiscordTag(long discordUserId)
    {
        var user = FindUser(discordUserId);
        return user?.DiscordUserName ?? string.Empty;
    }

    private string GetTiglUserName(long discordUserId)
    {
        var user = FindUser(discordUserId);
        return user?.TiglUserName ?? string.Empty;
    }

    private void ClearPlayer(int index)
    {
        playerResults[index] = new PlayerResult();
        if (index >= 0 && index < _rowModes.Count)
            _rowModes[index] = RowViewMode.Search;
        StateHasChanged();
    }

    private void OnUserSelected(int index, long discordUserId)
    {
        if (index < 0 || index >= playerResults.Count)
            return;

        playerResults[index].DiscordId = discordUserId;
        if (index >= 0 && index < _rowModes.Count)
            _rowModes[index] = RowViewMode.Edit;
    }

    private void ToggleEvent(string name, bool enabled)
    {
        if (enabled)
            _selectedEvents.Add(name);
        else
            _selectedEvents.Remove(name);
    }

    private void AddPlayer()
    {
        // Standard league supports only 6 players
        if (_selectedLeague == TiglLeague.ThundersEdge)
            return;

        if (playerResults.Count < MaxNumberOfPlayers)
        {
            playerResults.Add(new PlayerResult());
            _rowModes.Add(RowViewMode.Search);
        }
    }

    private void RemovePlayer()
    {
        if (playerResults.Count > MinNumberOfPlayers)
        {
            playerResults.RemoveAt(playerResults.Count - 1);
            if (_rowModes.Count > 0)
                _rowModes.RemoveAt(_rowModes.Count - 1);
            if (_currentPlayerIndex >= playerResults.Count)
                _currentPlayerIndex = playerResults.Count - 1;
        }
    }

    private void ResetForm()
    {
        InitializeForm();
        _currentPlayerIndex = 0;
        StateHasChanged();
    }

    private void ClearMessages()
    {
        validationErrors = new List<string>();
        errorMessage = string.Empty;
        errorDetails = string.Empty;
        successMessage = string.Empty;
    }

    private async Task SubmitGameReport()
    {
        ClearMessages();
        isSubmitting = true;

        try
        {
            // Use currently selected values
            gameReportRequest.GameId = gameReportRequest.GameId.ToLowerInvariant();
            gameReportRequest.PlayerResults = playerResults.ToList();
            gameReportRequest.PlayerCount = playerResults.Count;
            gameReportRequest.Events = _selectedEvents.ToList();
            gameReportRequest.Score = _selectedVpCount;
            gameReportRequest.Round = _round;
            gameReportRequest.League = _selectedLeague;

            // Safely set timestamps (validate first so we don't convert invalid dates)
            bool validationResult = ValidateInputs();
            if (!validationResult)
            {
                StateHasChanged();
                return;
            }

            SetGameTimestampsFromClientTimeZone();

            (var response, HttpStatusCode code) = await HttpClient.PostApiAsync<GameReport, ApiResponse<GameReportResult>>(Paths.ApiPath_ManualReportGame, gameReportRequest);

            if (code == HttpStatusCode.OK && response is not null && response.Success)
            {
                successMessage = $"Game report for '{gameReportRequest.GameId}' has been successfully submitted and processed!";
                ResetForm();
                await Task.Delay(1000);
                NavigationManager.NavigateTo(Pages.TiglGames);
            }
            else
            {
                errorMessage = response?.ProblemDetails.Title ?? "Unknown error.";
                errorDetails = response?.ProblemDetails.Detail ?? string.Empty;
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Failed to submit game report: {ex.Message}";
        }
        finally
        {
            isSubmitting = false;
        }
    }

    private bool ValidateInputs()
    {
        // 1) GameId cannot be blank / whitespace
        if (string.IsNullOrWhiteSpace(gameReportRequest.GameId))
            validationErrors.Add("Game ID is required.");

        // 2) League is bound via enum picker; ensure it's one of supported leagues
        if (_selectedLeague != TiglLeague.ThundersEdge && _selectedLeague != TiglLeague.Fractured)
            validationErrors.Add("Invalid league selected.");

        // 3) Result source is bound via enum picker (already bound into model). No extra validation needed here.

        // 4) VP validation depending on league
        if (_selectedLeague == TiglLeague.ThundersEdge)
        {
            var allowed = new HashSet<int> { 10, 12, 14 };
            if (!allowed.Contains(_selectedVpCount))
                validationErrors.Add("Standard league supports VP of 10, 12 or 14 only.");
        }
        else if (_selectedLeague == TiglLeague.Fractured)
        {
            if (_selectedVpCount < MinVictoryPoints || _selectedVpCount > MaxVictoryPoints)
                validationErrors.Add("Fractured league supports VP between 10 and 20.");
        }

        // 5) Round between 1 and 9
        if (_round < _minRound || _round > _maxRound)
            validationErrors.Add("Round must be between 1 and 9.");

        // 6) Dates must be bound (basic sanity: end >= start)
        if (startGame == default)
            validationErrors.Add("Start date must be selected.");
        if (endGame == default)
            validationErrors.Add("End date must be selected.");
        if (startGame != default && endGame != default && endGame < startGame)
            validationErrors.Add("End date cannot be before start date.");

        // 10) Player count restrictions per league
        if (_selectedLeague == TiglLeague.ThundersEdge && playerResults.Count != 6)
            validationErrors.Add("Exactly 6 players are required for Standard league.");

        if (_selectedLeague == TiglLeague.Fractured && (playerResults.Count < 6 || playerResults.Count > 8))
            validationErrors.Add("Fractured league supports 6 to 8 players.");

        // Players and uniqueness
        if (playerResults.Any(p => p.DiscordId <= 0))
            validationErrors.Add("All players must be selected from database (with TIGL name or DiscordTag).");

        if (playerResults.Select(x => x.DiscordId).Distinct().Count() != playerResults.Count)
            validationErrors.Add($"Report needs {playerResults.Count} unique players.");

        // 7) Only one winner can be selected
        var winners = playerResults.Count(p => p.IsWinner);
        if (winners != 1)
            validationErrors.Add("Exactly one winner must be selected.");

        // 8) Each player score must be between 0 and game score
        if (playerResults.Any(p => p.Score < 0 || p.Score > _selectedVpCount))
            validationErrors.Add("Each player's score must be between 0 and the game score.");

        // Ensure at least one player reached the winning score
        if (!playerResults.Any(p => p.Score == _selectedVpCount))
            validationErrors.Add($"At least one player must reach the winning score of {_selectedVpCount}.");

        // 9) Each player must have faction selected
        if (playerResults.Any(p => string.IsNullOrWhiteSpace(p.Faction)))
            validationErrors.Add("All players must have a faction assigned.");

        // Faction validity per league (Standard limited set)
        if (_selectedLeague == TiglLeague.ThundersEdge)
        {
            var allowed = Enum.GetValues<TiglFactionName>()
                .Where(f => f != TiglFactionName.None && f <= TiglFactionName.TheRalNelConsortium)
                .Select(TiglFactionParser.ToFactionString)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var invalidPlayers = playerResults
                .Where(p => !string.IsNullOrWhiteSpace(p.Faction) && !allowed.Contains(p.Faction))
                .Select(p =>
                {
                    var user = FindUser(p.DiscordId);
                    var name = !string.IsNullOrWhiteSpace(user?.TiglUserName) ? user!.TiglUserName : GetDiscordTag(p.DiscordId);
                    return $"{name} ({p.Faction})";
                })
                .ToList();

            if (invalidPlayers.Count > 0)
                validationErrors.Add($"Standard league disallows factions for: {string.Join(", ", invalidPlayers)}");
        }

        return validationErrors.Count == 0;
    }

    private void OnVpIncrease(int vp)
    {
        if (_selectedLeague == TiglLeague.ThundersEdge)
        {
            if (vp + VictoryPointStandardStep <= MaxStandardLeagueVictoryPoints)
                _selectedVpCount = vp + VictoryPointStandardStep;
        }
        else
        {
            if (vp + VictoryPointFracturedStep <= MaxVictoryPoints)
                _selectedVpCount = vp + VictoryPointFracturedStep;
        }
    }

    private void OnVpDecrease(int vp)
    {
        if (_selectedLeague == TiglLeague.ThundersEdge)
        {
            if (vp - VictoryPointStandardStep >= MinVictoryPoints)
                _selectedVpCount = vp - VictoryPointStandardStep;
        }
        else
        {
            if (vp - VictoryPointFracturedStep >= MinVictoryPoints)
                _selectedVpCount = vp - VictoryPointFracturedStep;
        }
    }

    private void OnRoundIncrease()
    {
        if (_round < _maxRound)
            _round++;
    }

    private void OnRoundDecrease()
    {
        if (_round > _minRound)
            _round--;
    }

    private void RedirectBack() => NavigationManager.NavigateTo(Pages.TiglLeaderboard);

    private void ChangeSource(ResultSource source)
    {
        gameReportRequest.Source = source;
    }

    private void SetGameTimestampsFromClientTimeZone()
    {
        // We assume ValidateInputs() has already ensured these are not default and end >= start.
        var startLocal = DateTime.SpecifyKind(startGame, DateTimeKind.Unspecified);
        var endLocal = DateTime.SpecifyKind(endGame, DateTimeKind.Unspecified);

        DateTime startUtc;
        DateTime endUtc;

        if (!string.IsNullOrEmpty(_clientTimeZoneId))
        {
            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(_clientTimeZoneId);

                // Convert local (in that TZ) -> UTC (handles DST etc.)
                startUtc = TimeZoneInfo.ConvertTimeToUtc(startLocal, tz);
                endUtc = TimeZoneInfo.ConvertTimeToUtc(endLocal, tz);
            }
            catch (TimeZoneNotFoundException)
            {
                // Fallback: treat as local to environment and convert
                startUtc = DateTime.SpecifyKind(startLocal, DateTimeKind.Local).ToUniversalTime();
                endUtc = DateTime.SpecifyKind(endLocal, DateTimeKind.Local).ToUniversalTime();
            }
            catch (InvalidTimeZoneException)
            {
                // Same fallback for corrupted TZ info
                startUtc = DateTime.SpecifyKind(startLocal, DateTimeKind.Local).ToUniversalTime();
                endUtc = DateTime.SpecifyKind(endLocal, DateTimeKind.Local).ToUniversalTime();
            }
        }
        else
        {
            // No timezone from browser: last-resort fallback
            startUtc = DateTime.SpecifyKind(startLocal, DateTimeKind.Local).ToUniversalTime();
            endUtc = DateTime.SpecifyKind(endLocal, DateTimeKind.Local).ToUniversalTime();
        }

        gameReportRequest.StartTimestamp = new DateTimeOffset(startUtc).ToUnixTimeMilliseconds();
        gameReportRequest.EndTimestamp = new DateTimeOffset(endUtc).ToUnixTimeMilliseconds();
    }
}
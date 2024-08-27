using TwilightImperiumUltimate.Web.Options.MiltyDraft;

namespace TwilightImperiumUltimate.Web.Services.MiltyDraft;

public class MiltyDraftSettingsService : IMiltyDraftSettingsService
{
    public int NumberOfPlayers { get; set; } = MiltyDraftOptions.NumberOfPlayers;

    public int NumberOfSlices { get; set; } = MiltyDraftOptions.NumberOfSlices;

    public int NumberOfFactions { get; set; } = MiltyDraftOptions.NumberOfFactions;

    public int NumberOfLegendaryPlanets { get; set; } = MiltyDraftOptions.NumberOfLegendaryPlanets;

    public bool EnablePlayerNames { get; set; } = MiltyDraftOptions.EnablePlayerNames;

    public bool ImportSlices { get; set; } = MiltyDraftOptions.ImportSlices;

    public IReadOnlyCollection<GameVersion> GameVersions { get; set; } = MiltyDraftOptions.GameVersions.ToList();

    public IReadOnlyCollection<MiltyDraftPlayerModel> Players { get; set; } = MiltyDraftOptions.MiltyDraftPlayers;

    public WormholeDensity WormholeDensity { get; set; } = MiltyDraftOptions.WormholeDensity;

    public MapTemplate MapTemplate { get; set; } = MapTemplate.SixPlayersMediumMap;

    public string ImportedSlicesString { get; set; } = string.Empty;

    public Task DecreaseNumberOfFactions()
    {
        if (NumberOfFactions > NumberOfPlayers)
        {
            NumberOfFactions--;
        }

        return Task.CompletedTask;
    }

    public Task DecreaseNumberOfLegendaryPlanets()
    {
        if (NumberOfLegendaryPlanets > 0)
        {
            NumberOfLegendaryPlanets--;
        }

        return Task.CompletedTask;
    }

    public Task DecreaseNumberOfPlayers()
    {
        var players = Players.ToList();

        if (NumberOfPlayers > MiltyDraftOptions.MinNumberOfPlayers)
        {
            players.RemoveAt(players.Count - 1);
            NumberOfPlayers--;
        }

        Players = players;

        return Task.CompletedTask;
    }

    public Task DecreaseNumberOfSlices()
    {
        if (NumberOfSlices > NumberOfPlayers)
        {
            NumberOfSlices--;
        }

        return Task.CompletedTask;
    }

    public Task<MapTemplate> GetMapTemplateForSpecificPlayerCount()
    {
        return NumberOfPlayers switch
        {
            5 => Task.FromResult(MapTemplate.FivePlayersMediumHyperlineMap),
            6 => Task.FromResult(MapTemplate.SixPlayersMediumMap),
            7 => Task.FromResult(MapTemplate.SevenPlayersLargeWarpMap),
            8 => Task.FromResult(MapTemplate.EightPlayersLargeWarpMap),
            _ => Task.FromResult(MapTemplate.CustomMap),
        };
    }

    public Task IncreaseNumberOfFactions()
    {
        var numberOfAvailableFactions = 0;
        numberOfAvailableFactions += GameVersions.Contains(GameVersion.BaseGame) ? 17 : 0;
        numberOfAvailableFactions += GameVersions.Contains(GameVersion.ProphecyOfKings) ? 7 : 0;
        numberOfAvailableFactions += GameVersions.Contains(GameVersion.CodexVigil) ? 1 : 0;
        numberOfAvailableFactions += GameVersions.Contains(GameVersion.DiscordantStars) ? 34 : 0;

        if (NumberOfFactions < numberOfAvailableFactions)
        {
            NumberOfFactions++;
        }

        return Task.CompletedTask;
    }

    public Task IncreaseNumberOfLegendaryPlanets()
    {
        var numberOfAvailableLegendaryPlanets = 0;
        numberOfAvailableLegendaryPlanets += GameVersions.Contains(GameVersion.ProphecyOfKings) ? 2 : 0;
        numberOfAvailableLegendaryPlanets += GameVersions.Contains(GameVersion.UnchartedSpace) ? 5 : 0;
        numberOfAvailableLegendaryPlanets += GameVersions.Contains(GameVersion.AscendantSun) ? 12 : 0;

        if (NumberOfLegendaryPlanets < numberOfAvailableLegendaryPlanets)
        {
            NumberOfLegendaryPlanets++;
        }

        return Task.CompletedTask;
    }

    public Task IncreaseNumberOfPlayers()
    {
        var players = Players.ToList();

        if (NumberOfPlayers < MiltyDraftOptions.MaxNumberOfPlayers)
        {
            NumberOfPlayers++;
            players.Add(new MiltyDraftPlayerModel() { PlayerId = NumberOfPlayers, PlayerName = $"Player {NumberOfPlayers}", PlayerDefaultName = $"Player {NumberOfPlayers}" });
        }

        Players = players;

        if (NumberOfSlices < NumberOfPlayers)
        {
            NumberOfSlices = NumberOfPlayers;
        }

        if (NumberOfFactions < NumberOfPlayers)
        {
            NumberOfFactions = NumberOfPlayers;
        }

        return Task.CompletedTask;
    }

    public Task IncreaseNumberOfSlices()
    {
        if (NumberOfSlices < MiltyDraftOptions.MaxNumberOfSlices)
        {
            NumberOfSlices++;
        }

        return Task.CompletedTask;
    }

    public Task SetPlayerNamesOption(bool enablePlayerNames)
    {
        EnablePlayerNames = enablePlayerNames;
        return Task.CompletedTask;
    }

    public Task UpdateGameVersion(GameVersion gameVersion)
    {
        var gameVersions = GameVersions.ToList();

        if (!gameVersions.Remove(gameVersion))
            gameVersions.Add(gameVersion);

        int maxNumberOfLegendaryPlanets = 0;
        maxNumberOfLegendaryPlanets += GameVersions.Contains(GameVersion.ProphecyOfKings) ? 2 : 0;
        maxNumberOfLegendaryPlanets += GameVersions.Contains(GameVersion.UnchartedSpace) ? 5 : 0;
        maxNumberOfLegendaryPlanets += GameVersions.Contains(GameVersion.AscendantSun) ? 12 : 0;

        if (NumberOfLegendaryPlanets > maxNumberOfLegendaryPlanets)
            NumberOfLegendaryPlanets = maxNumberOfLegendaryPlanets;

        GameVersions = gameVersions;

        return Task.CompletedTask;
    }

    public Task UpdateWormholeDensity(WormholeDensity wormholeDensity)
    {
        WormholeDensity = wormholeDensity;
        return Task.CompletedTask;
    }
}

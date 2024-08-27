namespace TwilightImperiumUltimate.Web.Services.MiltyDraft;

public interface IMiltyDraftSettingsService
{
    int NumberOfPlayers { get; }

    int NumberOfSlices { get; }

    int NumberOfFactions { get; }

    int NumberOfLegendaryPlanets { get; }

    bool EnablePlayerNames { get; set; }

    bool ImportSlices { get; set; }

    string ImportedSlicesString { get; set; }

    public MapTemplate MapTemplate { get; set; }

    IReadOnlyCollection<MiltyDraftPlayerModel> Players { get; }

    WormholeDensity WormholeDensity { get; }

    IReadOnlyCollection<GameVersion> GameVersions { get; }

    Task UpdateGameVersion(GameVersion gameVersion);

    Task UpdateWormholeDensity(WormholeDensity wormholeDensity);

    Task IncreaseNumberOfLegendaryPlanets();

    Task DecreaseNumberOfLegendaryPlanets();

    Task IncreaseNumberOfFactions();

    Task DecreaseNumberOfFactions();

    Task IncreaseNumberOfSlices();

    Task DecreaseNumberOfSlices();

    Task IncreaseNumberOfPlayers();

    Task DecreaseNumberOfPlayers();

    Task SetPlayerNamesOption(bool enablePlayerNames);

    Task<MapTemplate> GetMapTemplateForSpecificPlayerCount();
}

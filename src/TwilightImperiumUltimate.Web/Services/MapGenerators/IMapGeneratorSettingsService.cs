namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapGeneratorSettingsService
{
    int MapScale { get; set; }

    MapTemplate MapTemplate { get; set; }

    PlacementStyle PlacementStyle { get; set; }

    SystemWeight SystemWeight { get; set; }

    List<GameVersion> GameVersions { get; set; }

    List<FactionModel> FactionsForMapGenerator { get; set; }

    List<MapGeneratorPlayerModel> Players { get; set; }

    SystemTileOverlay MapOverlay { get; set; }

    SystemTileOverlay MenuOverlay { get; set; }

    WormholeDensity WormholeDensity { get; set; }

    int NumberOfLegendaryPlanets { get; set; }

    bool LegendaryPriorityInEquidistant { get; set; }

    bool EnableFactionPick { get; set; }

    bool EnablePlayerNames { get; set; }

    void IncreaseMapScale();

    void DecreaseMapScale();

    void UpdateGameVersion(GameVersion gameVersion);

    void UpdateWormholeDensity(WormholeDensity wormholeDensity);

    void UpdateFactionBanStatus(FactionModel factionModel);

    void GameVersionGlobalEnableDisable(GameVersion gameVersion);

    Task InitializeFactionsForMapGenerator();

    Task InitializePlayersForMapGenerator();

    int GetMapTemplatePlayerCount();

    IReadOnlyCollection<string> GetPlayerNames();
}

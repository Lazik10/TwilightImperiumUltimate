namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapGeneratorSettingsService
{
    public int MapScale { get; set; }

    public MapTemplate MapTemplate { get; set; }

    public PlacementStyle PlacementStyle { get; set; }

    public SystemWeight SystemWeight { get; set; }

    public List<GameVersion> GameVersions { get; set; }

    public List<FactionModel> FactionsForMapGenerator { get; set; }

    public List<MapGeneratorPlayerModel> Players { get; set; }

    public SystemTileOverlay SystemTileOverlay { get; set; }

    public WormholeDensity WormholeDensity { get; set; }

    public int NumberOfLegendaryPlanets { get; set; }

    public bool LegendaryPriorityInEquidistant { get; set; }

    public bool EnableFactionPick { get; set; }

    public bool EnablePlayerNames { get; set; }

    public void IncreaseMapScale();

    public void DecreaseMapScale();

    public void UpdateGameVersion(GameVersion gameVersion);

    public void UpdateWormholeDensity(WormholeDensity wormholeDensity);

    public void UpdateFactionBanStatus(FactionModel factionModel);

    public void GameVersionGlobalEnableDisable(GameVersion gameVersion);

    public Task InitializeFactionsForMapGenerator();

    public Task InitializePlayersForMapGenerator();

    public int GetMapTemplatePlayerCount();

    public IReadOnlyCollection<string> GetPlayerNames();
}

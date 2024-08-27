using TwilightImperiumUltimate.Contracts.ApiContracts.Draft;
using TwilightImperiumUltimate.Web.Options.MiltyDraft;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Services.MiltyDraft;

public class MiltyDraftService(
    IMiltyDraftSettingsService miltyDraftSettingsService,
    IMapGeneratorService mapGeneratorService,
    ITwilightImperiumApiHttpClient twilightImperiumApiHttpClient,
    IMiltyDraftMapPositionSetter miltyDraftMapPositionSetter,
    IMapper mapper)
    : IMiltyDraftService
{
    private readonly IMiltyDraftSettingsService _miltyDraftSettingsService = miltyDraftSettingsService;
    private readonly IMapGeneratorService _mapGeneratorService = mapGeneratorService;
    private readonly ITwilightImperiumApiHttpClient _httpClient = twilightImperiumApiHttpClient;
    private readonly IMiltyDraftMapPositionSetter _miltyDraftMapPositionSetter = miltyDraftMapPositionSetter;
    private readonly IMapper _mapper = mapper;

    private List<SliceModel> _slices = new List<SliceModel>();

    private List<MiltyDraftPlayerModel> _playersOrderOfPicks = new List<MiltyDraftPlayerModel>();

    private List<MiltyDraftPlayerModel> _players = new List<MiltyDraftPlayerModel>();

    private List<MiltyDraftInitiativeModel> _initiatives = MiltyDraftOptions.Initiatives.ToList();

    private List<FactionModel> _factionsForMiltyDraftMenu = new List<FactionModel>();

    private List<MiltyDraftFactionModel> _draftedFactions = new List<MiltyDraftFactionModel>();

    private Dictionary<int, SystemTileModel> _generatedPositionsWithSystemTiles = new Dictionary<int, SystemTileModel>();

    private List<SystemTileModel> _systemTiles = new List<SystemTileModel>();

    public IReadOnlyCollection<MiltyDraftFactionModel> DraftedFactions => _draftedFactions;

    public IReadOnlyCollection<FactionModel> FactionsForMiltyDraftMenu => _factionsForMiltyDraftMenu;

    public IReadOnlyList<SliceModel> Slices => GetOrderedSlices();

    public IReadOnlyCollection<MiltyDraftInitiativeModel> Initiatives => _initiatives;

    public MiltyDraftState State { get; set; } = MiltyDraftState.Initialized;

    public MiltyDraftPlayerModel ActivePlayer => PlayersOrderOfPicks.FirstOrDefault() ?? new MiltyDraftPlayerModel();

    public IReadOnlyCollection<MiltyDraftPlayerModel> PlayersOrderOfPicks => _playersOrderOfPicks;

    public IReadOnlyCollection<MiltyDraftPlayerModel> Players => _players;

    public bool NotEnoughFactionsPicked => _factionsForMiltyDraftMenu.Count(x => !x.Banned) < _miltyDraftSettingsService.NumberOfFactions;

    public IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles => _generatedPositionsWithSystemTiles;

    public MapTemplate CurrentDraftMapTemplate { get; set; } = MapTemplate.SixPlayersMediumMap;

    public IReadOnlyCollection<SystemTileModel> SystemTiles => _systemTiles;

    public bool FirstPickAssigned { get; set; }

    public async Task InitializeFactions()
    {
        if (_factionsForMiltyDraftMenu.Count == 0)
        {
            var (response, statusCode) = await _httpClient.GetAsync<ApiResponse<ItemListDto<FactionDto>>>(Paths.ApiPath_Factions);
            if (statusCode == HttpStatusCode.OK)
            {
                _factionsForMiltyDraftMenu = _mapper.Map<List<FactionModel>>(response!.Data!.Items);
                foreach (var faction in _factionsForMiltyDraftMenu)
                {
                    if (faction.GameVersion == GameVersion.DiscordantStars)
                    {
                        faction.Banned = true;
                    }
                }
            }
        }
    }

    public async Task GeneratePreviewSlices()
    {
        await GenerateSlices(true);
    }

    public async Task GenerateSlices(bool previewSlices)
    {
        var request = new SliceDraftRequest(
            previewSlices,
            _miltyDraftSettingsService.NumberOfSlices,
            _miltyDraftSettingsService.GameVersions,
            _miltyDraftSettingsService.WormholeDensity,
            _miltyDraftSettingsService.NumberOfLegendaryPlanets);

        var (response, statusCode) = await _httpClient.PostAsync<SliceDraftRequest, ApiResponse<ItemListDto<SliceDto>>>(Paths.ApiPath_GenerateSlices, request);

        if (statusCode == HttpStatusCode.OK)
        {
            _slices = _mapper.Map<List<SliceModel>>(response!.Data!.Items);
        }
    }

    public Task GenerateImportedSlices()
    {
        var importedSlices = _miltyDraftSettingsService.ImportedSlicesString
            .Trim()
            .Split('\n')
            .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
            .ToList();

        var slices = new List<SliceModel>();

        for (var i = 0; i < importedSlices.Count; i++)
        {
            var slice = new SliceModel()
            {
                Id = i,
                SystemTiles = new List<SystemTileModel>(),
            };

            var homePlaceholder = _systemTiles.Find(x => x.SystemTileName == SystemTileName.TileHome);
            if (homePlaceholder is not null)
            {
                slice.SystemTiles.Add(homePlaceholder);
            }

            var systemTileCodes = importedSlices[i]
                .Trim()
                .Split(',')
                .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (systemTileCodes.Count == 5)
            {
                foreach (var code in systemTileCodes)
                {
                    var systemTile = _systemTiles.Find(x => x.SystemTileCode == code);
                    if (systemTile is not null)
                    {
                        slice.SystemTiles.Add(systemTile);
                    }
                }
            }

            if (slice.SystemTiles.Count == 6)
                slices.Add(slice);
        }

        _slices = slices;
        return Task.CompletedTask;
    }

    public Task AddSlice()
    {
        if (_slices.Count < 9)
        {
            _slices.Add(new SliceModel()
            {
                Id = _miltyDraftSettingsService.NumberOfSlices - 1,
                SystemTiles = new List<SystemTileModel>
                {
                    new SystemTileModel() { SystemTileName = SystemTileName.TileHome },
                    new SystemTileModel() { SystemTileName = SystemTileName.TileEmpty },
                    new SystemTileModel() { SystemTileName = SystemTileName.TileEmpty },
                    new SystemTileModel() { SystemTileName = SystemTileName.TileEmpty },
                    new SystemTileModel() { SystemTileName = SystemTileName.TileEmpty },
                    new SystemTileModel() { SystemTileName = SystemTileName.TileEmpty },
                },
            });
        }

        return Task.CompletedTask;
    }

    public Task RemoveSlice()
    {
        if (_slices.Count > 0)
        {
            var slice = _slices.OrderBy(x => x.Id).Last();
            _slices.Remove(slice);
        }

        return Task.CompletedTask;
    }

    public Task UpdateMiltyDraftState(MiltyDraftState state)
    {
        State = state;
        return Task.CompletedTask;
    }

    public async Task MoveToNextPlayer()
    {
        if (_playersOrderOfPicks.Count > 0)
            _playersOrderOfPicks.RemoveAt(0);

        if (_playersOrderOfPicks.Count == 0)
            await UpdateMiltyDraftState(MiltyDraftState.Finished);
    }

    public Task RandomizePlayerOrder()
    {
        AssignRandomPlayerColors();
        var shuffledPlayers = _miltyDraftSettingsService.Players.OrderBy(x => Guid.NewGuid()).ToList();

        var snakeOrder = new List<MiltyDraftPlayerModel>(shuffledPlayers);
        shuffledPlayers.Reverse();
        snakeOrder.AddRange(shuffledPlayers);
        shuffledPlayers.Reverse();
        snakeOrder.AddRange(shuffledPlayers);

        _playersOrderOfPicks = snakeOrder;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<MiltyDraftPlayerModel>> NextPicks()
    {
        if (_playersOrderOfPicks.Count == 0)
        {
            return Task.FromResult(Array.Empty<MiltyDraftPlayerModel>() as IReadOnlyCollection<MiltyDraftPlayerModel>);
        }

        return Task.FromResult(_playersOrderOfPicks
            .Skip(1)
            .Take(Math.Min(_miltyDraftSettingsService.NumberOfPlayers, _playersOrderOfPicks.Count))
            .ToList() as IReadOnlyCollection<MiltyDraftPlayerModel>);
    }

    public async Task StartMiltyDraft()
    {
        State = MiltyDraftState.Started;
        FirstPickAssigned = false;
        await InitializeSystemTiles();
        await InitializeInitiativePicks();
        await InitializeDraftPlayers();

        if (_miltyDraftSettingsService.ImportSlices && await IsValidImportSlicesString())
        {
            await GenerateImportedSlices();
        }
        else
        {
            await GenerateSlices(false);
        }

        await RandomizePlayerOrder();
        await PickFactions();
        await InitializePositionsWithSystemTiles();
    }

    public async Task ResetMiltyDraft()
    {
        State = MiltyDraftState.Initialized;
        FirstPickAssigned = false;

        await GeneratePreviewSlices();
        await InitializeInitiativePicks();
        await InitializePositionsWithSystemTiles();

        _playersOrderOfPicks = new List<MiltyDraftPlayerModel>();
        _players = new List<MiltyDraftPlayerModel>();
        _draftedFactions = new List<MiltyDraftFactionModel>();
    }

    public async Task FactionPicked(FactionName faction)
    {
        var activePlayer = GetActivePlayer();

        if (activePlayer is not null)
        {
            activePlayer.Faction = faction;

            var factionToUpdate = DraftedFactions.FirstOrDefault(x => x.FactionName == faction);
            if (factionToUpdate is not null)
            {
                factionToUpdate.IsPicked = true;
                factionToUpdate.Color = activePlayer.PlayerColor;

                if (activePlayer.Initiative != MiltyDraftInitiative.None)
                {
                    SystemTileModel homeSystemTile;

                    if (faction == FactionName.TheCouncilKeleres)
                    {
                        homeSystemTile = _systemTiles
                            .Where(x => x.FactionName == faction && x.SystemTileCategory == SystemTileCategory.Green)
                            .OrderBy(x => Guid.NewGuid())
                            .First();
                    }
                    else
                    {
                        homeSystemTile = _systemTiles.First(x => x.FactionName == faction && x.SystemTileCategory == SystemTileCategory.Green);
                    }

                    await _miltyDraftMapPositionSetter.SetHomePositionWithSpecificFactionSystemTile(
                        homeSystemTile,
                        CurrentDraftMapTemplate,
                        activePlayer.Initiative,
                        _generatedPositionsWithSystemTiles);
                }
            }
        }

        FirstPickAssigned = true;
        await MoveToNextPlayer();
    }

    public async Task InitiativePicked(MiltyDraftInitiative initiative)
    {
        var activePlayer = GetActivePlayer();

        if (activePlayer is not null)
        {
            activePlayer.Initiative = initiative;

            var initiativeToUpdate = Initiatives.FirstOrDefault(x => x.Initiative == activePlayer.Initiative);
            if (initiativeToUpdate is not null)
            {
                initiativeToUpdate.Picked = true;
                initiativeToUpdate.DraftColor = activePlayer.PlayerColor;

                if (activePlayer.Faction != FactionName.None)
                {
                    await _miltyDraftMapPositionSetter.SetHomePositionWithSpecificFactionSystemTile(
                        _systemTiles.First(x => x.FactionName == activePlayer.Faction && x.SystemTileCategory == SystemTileCategory.Green),
                        CurrentDraftMapTemplate,
                        activePlayer.Initiative,
                        _generatedPositionsWithSystemTiles);
                }

                if (activePlayer.Slice is not null && activePlayer.Slice.IsPicked)
                {
                    await _miltyDraftMapPositionSetter.SetSliceToMapPositions(
                    activePlayer.Slice,
                    CurrentDraftMapTemplate,
                    activePlayer.Initiative,
                    _generatedPositionsWithSystemTiles);
                }
            }
        }

        FirstPickAssigned = true;
        await MoveToNextPlayer();
    }

    public Task SetFactionBanStatus(FactionModel faction)
    {
        var factionToUpdate = _factionsForMiltyDraftMenu.Find(x => x.FactionName == faction.FactionName);
        if (factionToUpdate is not null && faction is not null)
        {
            factionToUpdate.Banned = faction.Banned;
        }

        return Task.CompletedTask;
    }

    public Task UpdateGameVersionFactions(GameVersion gameVersion)
    {
        if (_factionsForMiltyDraftMenu.Exists(x => x.GameVersion == gameVersion && !x.Banned))
        {
            _factionsForMiltyDraftMenu.ForEach(x =>
            {
                if (x.GameVersion == gameVersion)
                    x.Banned = true;
            });
        }
        else if (_factionsForMiltyDraftMenu.Where(x => x.GameVersion == gameVersion).All(x => x.Banned))
        {
            _factionsForMiltyDraftMenu.ForEach(x =>
            {
                if (x.GameVersion == gameVersion)
                    x.Banned = false;
            });
        }

        return Task.CompletedTask;
    }

    public async Task SlicePicked(SliceModel slice)
    {
        var activePlayer = GetActivePlayer();

        if (activePlayer is not null)
        {
            activePlayer.Slice = slice;

            var sliceToUpdate = Slices.FirstOrDefault(x => x.Id == activePlayer.Slice.Id);
            if (sliceToUpdate is not null)
            {
                sliceToUpdate.IsPicked = true;
                sliceToUpdate.Color = activePlayer.PlayerColor;

                if (activePlayer.Initiative != MiltyDraftInitiative.None)
                {
                    await _miltyDraftMapPositionSetter.SetSliceToMapPositions(
                    slice,
                    CurrentDraftMapTemplate,
                    activePlayer.Initiative,
                    _generatedPositionsWithSystemTiles);
                }
            }
        }

        FirstPickAssigned = true;
        await MoveToNextPlayer();
    }

    public Task<bool> IsValidImportSlicesString()
    {
        var numberOfRequiredSlices = _miltyDraftSettingsService.NumberOfSlices;
        var importedSlices = _miltyDraftSettingsService.ImportedSlicesString.Trim().Split('\n').ToList();
        var importedSlicesCount = importedSlices.Count(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x));

        var validSlices = importedSlices
            .Count(x =>
                x.Trim()
                .Split(',')
                .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
                .ToList().Count == 5);

        return Task.FromResult(importedSlicesCount == numberOfRequiredSlices && validSlices == numberOfRequiredSlices);
    }

    public async Task RegenerateSlices()
    {
        if (_miltyDraftSettingsService.ImportSlices && await IsValidImportSlicesString())
        {
            await GenerateImportedSlices();
        }
        else
        {
            await GenerateSlices(false);
        }
    }

    public async Task RegeneratePlayerOrder() => await RandomizePlayerOrder();

    public async Task RegenerateFactions() => await PickFactions();

    private Task PickFactions()
    {
        var shuffledFactions = _factionsForMiltyDraftMenu.OrderBy(x => Guid.NewGuid()).ToList();
        var draftedFactions = shuffledFactions.Where(x => !x.Banned).Take(_miltyDraftSettingsService.NumberOfFactions).ToList();

        _draftedFactions = draftedFactions.Select(x => new MiltyDraftFactionModel
        {
            FactionName = x.FactionName,
            IsPicked = false,
            Color = DraftColor.None,
        }).ToList();

        return Task.CompletedTask;
    }

    private List<SliceModel> GetOrderedSlices()
    {
        return _slices.OrderBy(x => x.Id).ToList();
    }

    private void AssignRandomPlayerColors()
    {
        var colors = Enum.GetValues(typeof(DraftColor)).Cast<DraftColor>().ToList();
        colors.Remove(DraftColor.None);

        foreach (var player in _miltyDraftSettingsService.Players)
        {
            player.PlayerColor = colors.OrderBy(x => Guid.NewGuid()).First();
            colors.Remove(player.PlayerColor);
        }
    }

    private Task InitializeDraftPlayers()
    {
        _players = _miltyDraftSettingsService.Players.ToList();
        _players.ForEach(x => x.Slice = new SliceModel());
        _players.ForEach(x => x.Faction = FactionName.None);
        _players.ForEach(x => x.Initiative = MiltyDraftInitiative.None);
        return Task.CompletedTask;
    }

    private MiltyDraftPlayerModel? GetActivePlayer()
    {
        return _players.Find(x => x.PlayerId == ActivePlayer.PlayerId);
    }

    private Task InitializeInitiativePicks()
    {
        _initiatives = MiltyDraftOptions.Initiatives.ToList();
        _initiatives.ForEach(x => x.Picked = false);
        _initiatives.ForEach(x => x.DraftColor = DraftColor.None);
        return Task.CompletedTask;
    }

    private async Task InitializePositionsWithSystemTiles()
    {
        var mapTemplate = await _miltyDraftSettingsService.GetMapTemplateForSpecificPlayerCount();
        CurrentDraftMapTemplate = mapTemplate;
        _generatedPositionsWithSystemTiles = await _mapGeneratorService.GenerateMapLayoutPreview(mapTemplate);
    }

    private async Task InitializeSystemTiles()
    {
        if (_systemTiles.Count > 0)
            return;

        var (response, statusCode) = await _httpClient.GetAsync<ApiResponse<ItemListDto<SystemTileDto>>>(Paths.ApiPath_SystemTiles);
        if (statusCode == HttpStatusCode.OK)
        {
            _systemTiles = _mapper.Map<List<SystemTileModel>>(response!.Data!.Items);
        }
    }
}

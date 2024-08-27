namespace TwilightImperiumUltimate.Web.Services.MiltyDraft;

public interface IMiltyDraftService
{
    bool FirstPickAssigned { get; }

    MapTemplate CurrentDraftMapTemplate { get; }

    MiltyDraftState State { get; }

    MiltyDraftPlayerModel ActivePlayer { get; }

    IReadOnlyCollection<MiltyDraftPlayerModel> PlayersOrderOfPicks { get; }

    IReadOnlyCollection<MiltyDraftPlayerModel> Players { get; }

    IReadOnlyList<SliceModel> Slices { get; }

    IReadOnlyCollection<MiltyDraftInitiativeModel> Initiatives { get; }

    IReadOnlyCollection<MiltyDraftFactionModel> DraftedFactions { get; }

    IReadOnlyCollection<FactionModel> FactionsForMiltyDraftMenu { get; }

    IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles { get; }

    IReadOnlyCollection<SystemTileModel> SystemTiles { get; }

    bool NotEnoughFactionsPicked { get; }

    Task StartMiltyDraft();

    Task ResetMiltyDraft();

    Task GeneratePreviewSlices();

    Task UpdateMiltyDraftState(MiltyDraftState state);

    Task MoveToNextPlayer();

    Task RandomizePlayerOrder();

    Task<IReadOnlyCollection<MiltyDraftPlayerModel>> NextPicks();

    Task FactionPicked(FactionName faction);

    Task InitiativePicked(MiltyDraftInitiative initiative);

    Task SlicePicked(SliceModel slice);

    Task InitializeFactions();

    Task SetFactionBanStatus(FactionModel faction);

    Task UpdateGameVersionFactions(GameVersion gameVersion);

    Task RegenerateSlices();

    Task RegeneratePlayerOrder();

    Task RegenerateFactions();

    public Task<bool> IsValidImportSlicesString();
}

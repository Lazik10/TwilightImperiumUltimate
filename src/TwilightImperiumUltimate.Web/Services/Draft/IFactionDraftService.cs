using TwilightImperiumUltimate.Web.Models.Drafts;
using TwilightImperiumUltimate.Web.Models.Factions;

namespace TwilightImperiumUltimate.Web.Services.Draft;

public interface IFactionDraftService
{
    event EventHandler? OnFactionUpdate;

    bool ToManyBans { get; }

    IReadOnlyCollection<FactionDraftPlayerModel> Players { get; }

    IReadOnlyCollection<FactionModel> FactionsWithUpdatedBanStatus { get; }

    int NumberOfPlayers { get; set; }

    int NumberOfDraftFactions { get; set; }

    void InitializePlayers();

    void IncreaseFactionCount();

    void DecreaseFactionCount();

    void IncreasePlayerCount();

    void DecreasePlayerCount();

    void ResetBans();

    void ResetPlayerFactions();

    void UpdateBanFactions(IReadOnlyCollection<FactionModel>? factionsWithBanStatus);

    void GameVersionGlobalEnableDisable(GameVersion version);

    Task PerformDraft();
}

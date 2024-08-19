namespace TwilightImperiumUltimate.Web.Services.Draft;

public interface IColorPickerService
{
    event EventHandler? OnFactionUpdate;

    IDictionary<PlayerColor, bool> Colors { get; }

    IReadOnlyCollection<FactionModel> SelectedFactions { get; }

    IReadOnlyCollection<FactionColorDraftResult>? FactionColorDraftResults { get; }

    void ResetBannedColors();

    void ResetSelectedFactions();

    Task PerformDraft();

    void UpdateSelectedFactions(IReadOnlyCollection<FactionModel>? factions, FactionModel faction);

    void UpdateColorBanStatus(PlayerColor color);

    bool IsDraftPossible();

    PlayerColor GetRandomColor();
}

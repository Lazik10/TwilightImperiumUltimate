using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Components.DraftRooms.FactionPick;

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <inheritdoc />
public sealed class DraftTypeUiRegistry : IDraftTypeUiRegistry
{
    /// <inheritdoc />
    public Type GetSettingsEditor(DraftTypeId type) => type switch
    {
        DraftTypeId.FactionPick => typeof(FactionPickSettingsEditor),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "No settings component is registered for this draft type."),
    };

    /// <inheritdoc />
    public Type GetBoard(DraftTypeId type) => type switch
    {
        DraftTypeId.FactionPick => typeof(FactionPickBoard),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "No board is registered for this draft type."),
    };

    /// <inheritdoc />
    public Type GetResultView(DraftTypeId type) => type switch
    {
        DraftTypeId.FactionPick => typeof(FactionPickResultView),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "No result view is registered for this draft type."),
    };
}

using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>Resolves a draft engine's project-owned settings component.</summary>
public interface IDraftTypeUiRegistry
{
    /// <summary>Gets the component type for editing or displaying the selected engine's rules.</summary>
    Type GetSettingsEditor(DraftTypeId type);

    /// <summary>Gets the component type that displays a personalized draft projection.</summary>
    Type GetBoard(DraftTypeId type);

    /// <summary>Gets the component that renders the engine's public result payload.</summary>
    Type GetResultView(DraftTypeId type);
}

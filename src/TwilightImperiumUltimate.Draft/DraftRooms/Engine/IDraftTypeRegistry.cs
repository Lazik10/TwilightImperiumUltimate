using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Resolves registered draft engines without coupling room commands to a particular draft type.
/// </summary>
public interface IDraftTypeRegistry
{
    /// <summary>
    /// Gets all registered modules in identifier order.
    /// </summary>
    IReadOnlyList<IDraftTypeModule> All { get; }

    /// <summary>
    /// Resolves a registered draft type or throws when it is unavailable.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Naming",
        "CA1716:Identifiers should not match keywords",
        Justification = "Get is the existing .NET-only registry contract; changing it would break callers.")]
    IDraftTypeModule Get(DraftTypeId id);
}

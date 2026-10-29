using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

/// <summary>
/// Tracks Faction Pick changes in the caller's room transaction. Only the unit of work saves them.
/// </summary>
public interface IFactionPickRepository
{
    /// <summary>Loads tracked settings for the room.</summary>
    Task<FactionPickSettings?> LoadSettingsAsync(DraftRoomScope scope, CancellationToken cancellationToken);

    /// <summary>Loads tracked runtime state for the room.</summary>
    Task<FactionPickState?> LoadStateAsync(DraftRoomScope scope, CancellationToken cancellationToken);

    /// <summary>Loads the tracked initial pick order in position order.</summary>
    Task<IReadOnlyList<FactionPickTurnEntry>> LoadTurnOrderAsync(DraftRoomScope scope, CancellationToken cancellationToken);

    /// <summary>Loads tracked choices for use only by the server-side engine.</summary>
    Task<IReadOnlyList<FactionPickSelection>> LoadSelectionsAsync(DraftRoomScope scope, CancellationToken cancellationToken);

    /// <summary>Synchronizes choice rows in the current transaction, preserving timestamps of unchanged choices.</summary>
    Task SaveSelectionsAsync(DraftRoomScope scope, IReadOnlyList<FactionPickSelection> selections, CancellationToken cancellationToken);

    /// <summary>Stages settings for insertion or updates the tracked settings.</summary>
    Task SaveSettingsAsync(DraftRoomScope scope, FactionPickSettings settings, CancellationToken cancellationToken);

    /// <summary>Stages runtime state for insertion or updates the tracked state.</summary>
    Task SaveStateAsync(DraftRoomScope scope, FactionPickState state, CancellationToken cancellationToken);

    /// <summary>Stages the initial order for an unstarted or reset draft; existing order must be cleared first.</summary>
    void AddTurnOrder(DraftRoomScope scope, IReadOnlyList<Guid> seatIds);

    /// <summary>Stages removal of runtime state and turn order, preserving settings.</summary>
    Task DeleteStateAsync(DraftRoomScope scope, CancellationToken cancellationToken);

    /// <summary>Stages removal of settings and all runtime state when changing draft type.</summary>
    Task DeleteAllAsync(DraftRoomScope scope, CancellationToken cancellationToken);
}

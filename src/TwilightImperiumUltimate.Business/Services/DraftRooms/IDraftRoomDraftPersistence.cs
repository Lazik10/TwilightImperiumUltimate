using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Routes type-specific storage operations through the existing room transaction, outside the pure engine.
/// </summary>
public interface IDraftRoomDraftPersistence
{
    /// <summary>Stages default settings for the chosen type in a new or reset room.</summary>
    void InitializeDefaults(DraftRoomScope scope, DraftTypeId type);

    /// <summary>Stages removal of the current type's settings and runtime rows.</summary>
    Task DeleteAllAsync(DraftRoomScope scope, CancellationToken cancellationToken);

    /// <summary>Loads the type's settings, pool and plain state without projecting any hidden information.</summary>
    Task<(DraftSettingsDto Settings, IDraftState State, int PoolSize)> LoadAsync(
        DraftRoomScope scope,
        CancellationToken cancellationToken);

    /// <summary>Stages an initialized engine state and its initial turn order.</summary>
    Task SaveStartedStateAsync(DraftRoomScope scope, IDraftState state, CancellationToken cancellationToken);

    /// <summary>Stages runtime cursor, locks and choices after a successful engine action.</summary>
    Task SaveRuntimeStateAsync(DraftRoomScope scope, IDraftState state, CancellationToken cancellationToken);

    /// <summary>Stages removal of runtime state and turn order while retaining configured settings.</summary>
    Task DeleteStateAsync(DraftRoomScope scope, CancellationToken cancellationToken);

    /// <summary>Validates proposed settings against the pool for those settings, not the previous expansions.</summary>
    Task<DraftValidationResult> ValidateSettingsAsync(
        DraftRoomScope scope,
        DraftSettingsDto settings,
        IDraftTypeModule module,
        CancellationToken cancellationToken);

    /// <summary>Stages validated type-specific settings in the active transaction.</summary>
    Task SaveSettingsAsync(DraftRoomScope scope, DraftSettingsDto settings, CancellationToken cancellationToken);
}

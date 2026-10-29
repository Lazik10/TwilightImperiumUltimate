using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

/// <summary>Reads immutable result snapshots and stages new snapshots in the caller's room transaction.</summary>
public interface IDraftResultRepository
{
    /// <summary>Stages a snapshot without saving independently of the finishing room operation.</summary>
    void Add(DraftRoomScope scope, DraftResult result);

    /// <summary>Finds a result using an exact case-sensitive share token.</summary>
    Task<DraftResult?> GetByTokenAsync(string token, CancellationToken cancellationToken);

    /// <summary>Finds a snapshot and its account-history associations.</summary>
    Task<DraftResult?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Gets a bounded, newest-first page of owned or participated snapshots.</summary>
    Task<IReadOnlyList<DraftResultHistoryItemDto>> GetHistoryAsync(string userId, int skip, int take, CancellationToken cancellationToken);

    /// <summary>Finds the source room if it still exists.</summary>
    Task<string?> GetSourceRoomCodeAsync(Guid? roomId, CancellationToken cancellationToken);

    /// <summary>Loads a tracked snapshot only inside its source room's operation.</summary>
    Task<DraftResult?> LoadForRoomAsync(DraftRoomScope scope, Guid id, CancellationToken cancellationToken);

    /// <summary>Deletes only an owning account's snapshot whose source room no longer exists.</summary>
    Task<bool> DeleteOwnedOrphanAsync(Guid id, string userId, CancellationToken cancellationToken);
}

using TwilightImperiumUltimate.Web.Models.DraftRooms;

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Provides browser-local persistence for guest identities associated with draft rooms.
/// </summary>
public interface IGuestIdentityStore
{
    /// <summary>
    /// Gets the saved guest identity for a room code, or <see langword="null"/> when none exists.
    /// </summary>
    Task<GuestIdentity?> GetAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves a guest identity, replacing any identity already stored for its room code.
    /// </summary>
    Task SaveAsync(GuestIdentity identity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes the saved guest identity associated with a room code.
    /// </summary>
    Task RemoveAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all saved guest identities.
    /// </summary>
    Task<IReadOnlyList<GuestIdentity>> ListAsync(CancellationToken cancellationToken = default);
}

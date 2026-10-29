namespace TwilightImperiumUltimate.DataAccess.UnitOfWork;

/// <summary>
/// Executes a draft room operation under a per-room lock and database transaction.
/// </summary>
public interface IDraftRoomUnitOfWork
{
    /// <summary>
    /// Loads and updates a room as one serialized, transactional operation.
    /// </summary>
    /// <typeparam name="TResult">The result returned by the operation.</typeparam>
    /// <param name="code">The room's public code.</param>
    /// <param name="work">The operation to execute with the tracked room scope.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <param name="changesState">
    /// Whether this operation changes room state and should increment the room version and publish a room-change event.
    /// </param>
    /// <returns>The result returned by <paramref name="work"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="code"/> is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="work"/> is null.</exception>
    /// <exception cref="DraftRoomNotFoundException">The room code does not identify a room.</exception>
    /// <exception cref="DraftRoomConflictException">The room changed concurrently.</exception>
    Task<TResult> ExecuteAsync<TResult>(
        string code,
        Func<DraftRoomScope, Task<TResult>> work,
        CancellationToken cancellationToken,
        bool changesState = true);
}

using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.DataAccess.UnitOfWork;

/// <summary>
/// Provides the tracked draft room state and persistence context for one room operation.
/// </summary>
public sealed class DraftRoomScope
{
    internal DraftRoomScope(DraftRoom room, TwilightImperiumDbContext context, DateTime now)
    {
        ArgumentNullException.ThrowIfNull(room);
        ArgumentNullException.ThrowIfNull(context);

        this.Room = room;
        this.Context = context;
        this.Now = now;
    }

    /// <summary>
    /// Gets the tracked draft room and its loaded participants and seats.
    /// </summary>
    public DraftRoom Room { get; }

    /// <summary>
    /// Gets the context used to persist this room operation.
    /// </summary>
    public TwilightImperiumDbContext Context { get; }

    /// <summary>
    /// Gets the UTC timestamp shared by the operation.
    /// </summary>
    public DateTime Now { get; }
}

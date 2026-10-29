using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

/// <summary>
/// Persists draft rooms and participant records.
/// </summary>
public sealed class DraftRoomRepository(IDbContextFactory<TwilightImperiumDbContext> context)
    : IDraftRoomRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<string>> GetInactiveRoomCodesAsync(DateTime cutoffUtc, int limit, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(limit, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 100);
        await using var dbContext = await this._context.CreateDbContextAsync(cancellationToken);
        return await dbContext.DraftRooms.AsNoTracking()
            .Where(room => room.LastActivityUtc < cutoffUtc)
            .OrderBy(room => room.LastActivityUtc).ThenBy(room => room.Id)
            .Take(limit).Select(room => room.Code).ToArrayAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        return await dbContext.DraftRooms
            .AsNoTracking()
            .AnyAsync(room => room.Code == code, cancellationToken);
    }

    /// <inheritdoc />
    public async Task CreateRoomWithHostAsync(
        DraftRoom room,
        DraftRoomParticipant hostParticipant,
        CancellationToken cancellationToken,
        Action<DraftRoomScope> appendLog)
    {
        ArgumentNullException.ThrowIfNull(room);
        ArgumentNullException.ThrowIfNull(hostParticipant);
        ArgumentNullException.ThrowIfNull(appendLog);

        hostParticipant.RoomId = room.Id;
        hostParticipant.Room = room;
        room.Participants.Add(hostParticipant);

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        dbContext.DraftRooms.Add(room);
        appendLog(new DraftRoomScope(room, dbContext, room.CreatedAtUtc));
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<DraftRoom?> GetRoomByCodeAsync(string code, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        return await dbContext.DraftRooms
            .AsNoTracking()
            .Include(room => room.Participants)
            .FirstOrDefaultAsync(room => room.Code == code, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<DraftRoom?> GetRoomForViewByCodeAsync(string code, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        return await dbContext.DraftRooms
            .AsNoTracking()
            .AsSingleQuery()
            .Include(room => room.Participants)
            .Include(room => room.Seats)
            .Include(room => room.FactionPickSettings)
            .Include(room => room.FactionPickState)
            .Include(room => room.FactionPickTurnOrder)
            .Include(room => room.FactionPickSelections)
            .FirstOrDefaultAsync(room => room.Code == code, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<DraftRoomChatMessage>> GetChatHistoryAsync(
        Guid roomId,
        long? afterId,
        int limit,
        CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(roomId, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);

        await using var dbContext = await this._context.CreateDbContextAsync(cancellationToken);
        var query = dbContext.DraftRoomChatMessages
            .AsNoTracking()
            .Where(message => message.RoomId == roomId);

        if (afterId.HasValue)
        {
            return await query
                .Where(message => message.Id > afterId.Value)
                .OrderBy(message => message.Id)
                .Take(limit)
                .ToListAsync(cancellationToken);
        }

        return await query
            .OrderByDescending(message => message.Id)
            .Take(limit)
            .OrderBy(message => message.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<DraftRoomLogEntry>> GetLogHistoryAsync(
        Guid roomId,
        long? afterId,
        int limit,
        CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(roomId, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);

        await using var dbContext = await this._context.CreateDbContextAsync(cancellationToken);
        var query = dbContext.DraftRoomLogEntries
            .AsNoTracking()
            .Where(entry => entry.RoomId == roomId);

        if (afterId.HasValue)
        {
            return await query
                .Where(entry => entry.Id > afterId.Value)
                .OrderBy(entry => entry.Id)
                .Take(limit)
                .ToListAsync(cancellationToken);
        }

        return await query
            .OrderByDescending(entry => entry.Id)
            .Take(limit)
            .OrderBy(entry => entry.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<DraftRoomParticipant>> GetActiveParticipantsForUserAsync(
        string userId,
        int limit,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);

        await using var dbContext = await this._context.CreateDbContextAsync(cancellationToken);

        return await dbContext.DraftRoomParticipants
            .AsNoTracking()
            .Where(participant =>
                participant.UserId == userId
                && participant.Status == DraftParticipantStatus.Active)
            .OrderByDescending(participant => participant.Room.LastActivityUtc)
            .ThenBy(participant => participant.Room.Code)
            .Take(limit)
            .Include(participant => participant.Room)
            .ThenInclude(room => room.Seats)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<DraftRoomParticipant?> FindParticipantByUserAsync(
        Guid roomId,
        string userId,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        return await dbContext.DraftRoomParticipants
            .AsNoTracking()
            .FirstOrDefaultAsync(
                participant => participant.RoomId == roomId && participant.UserId == userId,
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task<DraftRoomParticipant?> FindParticipantByIdAsync(
        Guid roomId,
        Guid participantId,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        return await dbContext.DraftRoomParticipants
            .AsNoTracking()
            .FirstOrDefaultAsync(
                participant => participant.RoomId == roomId && participant.Id == participantId,
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddParticipantAsync(
        DraftRoomParticipant participant,
        DateTime activityAtUtc,
        CancellationToken cancellationToken,
        Action<DraftRoomScope> appendLog)
    {
        ArgumentNullException.ThrowIfNull(participant);
        ArgumentNullException.ThrowIfNull(appendLog);

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var room = await dbContext.DraftRooms
            .SingleAsync(item => item.Id == participant.RoomId, cancellationToken);
        await IncrementRoomVersionAsync(
            dbContext,
            participant.RoomId,
            activityAtUtc,
            cancellationToken);
        dbContext.DraftRoomParticipants.Add(participant);
        appendLog(new DraftRoomScope(room, dbContext, activityAtUtc));
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ReactivateParticipantAsync(
        Guid roomId,
        Guid participantId,
        DateTime activityAtUtc,
        CancellationToken cancellationToken,
        Action<DraftRoomScope> appendLog)
    {
        ArgumentNullException.ThrowIfNull(appendLog);
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var room = await dbContext.DraftRooms
            .SingleAsync(item => item.Id == roomId, cancellationToken);
        var participant = await dbContext.DraftRoomParticipants
            .AsNoTracking()
            .SingleAsync(
                item => item.RoomId == roomId && item.Id == participantId,
                cancellationToken);
        if (participant.Status != DraftParticipantStatus.Left)
        {
            throw new DbUpdateConcurrencyException("The participant is no longer eligible for re-entry.");
        }

        await IncrementRoomVersionAsync(dbContext, roomId, activityAtUtc, cancellationToken);
        var displayNameIsTaken = await dbContext.DraftRoomParticipants
            .AsNoTracking()
            .AnyAsync(
                item => item.RoomId == roomId
                    && item.Id != participantId
                    && item.Status == DraftParticipantStatus.Active
                    && item.DisplayNameKey == participant.DisplayNameKey,
                cancellationToken);
        if (displayNameIsTaken)
        {
            await transaction.RollbackAsync(cancellationToken);

            return false;
        }

        var updatedParticipants = await dbContext.DraftRoomParticipants
            .Where(participant => participant.RoomId == roomId
                && participant.Id == participantId
                && participant.Status == DraftParticipantStatus.Left)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(participant => participant.Status, DraftParticipantStatus.Active)
                    .SetProperty(participant => participant.LastSeenUtc, activityAtUtc),
                cancellationToken);

        if (updatedParticipants != 1)
        {
            throw new DbUpdateConcurrencyException("The participant is no longer eligible for re-entry.");
        }

        appendLog(new DraftRoomScope(room, dbContext, activityAtUtc));
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return true;
    }

    /// <inheritdoc />
    public async Task<int> CountOpenRoomsForUserAsync(string userId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        return await dbContext.DraftRooms
            .AsNoTracking()
            .CountAsync(
                room => room.Status != DraftRoomStatus.Finished
                    && room.Participants.Any(
                        participant => participant.UserId == userId && participant.Role == DraftRoomRole.Host),
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountActiveParticipantsAsync(Guid roomId, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        return await dbContext.DraftRoomParticipants
            .AsNoTracking()
            .CountAsync(
                participant => participant.RoomId == roomId && participant.Status == DraftParticipantStatus.Active,
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> IsDisplayNameTakenAsync(
        Guid roomId,
        string displayNameKey,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(displayNameKey);

        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        return await dbContext.DraftRoomParticipants
            .AsNoTracking()
            .AnyAsync(
                participant => participant.RoomId == roomId
                    && participant.Status == DraftParticipantStatus.Active
                    && participant.DisplayNameKey == displayNameKey,
                cancellationToken);
    }

    private static async Task IncrementRoomVersionAsync(
        TwilightImperiumDbContext dbContext,
        Guid roomId,
        DateTime activityAtUtc,
        CancellationToken cancellationToken)
    {
        var currentVersion = await dbContext.DraftRooms
            .AsNoTracking()
            .Where(room => room.Id == roomId)
            .Select(room => room.Version)
            .SingleAsync(cancellationToken);
        var updatedRows = await dbContext.DraftRooms
            .Where(room => room.Id == roomId && room.Version == currentVersion)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(room => room.Version, room => room.Version + 1)
                    .SetProperty(room => room.LastActivityUtc, activityAtUtc),
                cancellationToken);

        if (updatedRows != 1)
        {
            throw new DbUpdateConcurrencyException("The draft room changed while processing a join.");
        }
    }
}

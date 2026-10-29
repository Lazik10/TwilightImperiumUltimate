using FluentResults;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

internal static class DraftRoomTypeChange
{
    internal static async Task<Result> ApplyAsync(
        DraftRoomScope scope,
        DraftRoomActor actor,
        DraftTypeId type,
        IDraftTypeRegistry registry,
        IDraftRoomDraftPersistence persistence,
        IDraftRoomLog log,
        CancellationToken cancellationToken)
    {
        if (!registry.All.Any(module => module.Id == type))
        {
            return Result.Fail(new DraftRoomError(DraftRoomErrorCode.InvalidDraftType));
        }

        var previous = scope.Room.DraftType;
        var selected = type.ToString();
        if (string.Equals(previous, selected, StringComparison.Ordinal))
        {
            return Result.Ok();
        }

        await persistence.DeleteAllAsync(scope, cancellationToken);
        persistence.InitializeDefaults(scope, type);
        scope.Room.DraftType = selected;
        log.Append(
            scope,
            DraftLogEntryType.DraftTypeChanged,
            actor.Participant.Id,
            new { PreviousType = previous, DraftType = selected });
        return Result.Ok();
    }
}

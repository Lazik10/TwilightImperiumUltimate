using System.Text.Json;
using FluentResults;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Returns only the public snapshot, never persistence ownership or room membership records.</summary>
public sealed class DraftResultQueryHandler(
    IDraftResultRepository results,
    IDraftRoomRepository rooms,
    IDraftRoomAuthorizer authorizer,
    IDraftTypeRegistry registry)
    : IRequestHandler<GetResultByTokenQuery, Result<DraftResultDto>>, IRequestHandler<GetRoomResultQuery, Result<DraftResultDto>>
{
    /// <inheritdoc />
    public async Task<Result<DraftResultDto>> Handle(GetResultByTokenQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Token is null || request.Token.Length != 22
            || request.Token.Any(character => !char.IsAsciiLetterOrDigit(character) && character is not ('-' or '_')))
        {
            return NotFound();
        }

        var stored = await results.GetByTokenAsync(request.Token, cancellationToken);
        return stored is null ? NotFound() : Result.Ok(this.ToPublicDto(stored));
    }

    /// <inheritdoc />
    public async Task<Result<DraftResultDto>> Handle(GetRoomResultQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var room = await rooms.GetRoomByCodeAsync(request.Code, cancellationToken);
        if (room is null || authorizer.Resolve(room, request.Caller) is null)
        {
            return Result.Fail<DraftResultDto>(new DraftRoomError(DraftRoomErrorCode.NotFoundOrWrongPassword));
        }

        if (room.Status != DraftRoomStatus.Finished || room.ResultId is not Guid id)
        {
            return NotFound();
        }

        var stored = await results.GetByIdAsync(id, cancellationToken);
        return stored is null ? NotFound() : Result.Ok(this.ToPublicDto(stored));
    }

    private static Result<DraftResultDto> NotFound() => Result.Fail<DraftResultDto>(new DraftRoomError(DraftRoomErrorCode.ResultNotFound));

    private DraftResultDto ToPublicDto(DraftResult stored)
    {
        if (stored.PayloadVersion != 1)
        {
            throw new InvalidOperationException("The stored result payload version is unsupported.");
        }

        using var document = JsonDocument.Parse(stored.PayloadJson);
        var payload = document.RootElement.Clone();
        var text = registry.Get(stored.DraftType).FormatResultAsText(payload);
        return new DraftResultDto(stored.Id, stored.ShareToken, stored.DraftType, stored.RoomName, stored.FinishedAtUtc, stored.PayloadVersion, payload, text);
    }
}

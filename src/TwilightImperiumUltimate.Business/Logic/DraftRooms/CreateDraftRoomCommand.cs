using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Creates a draft room and its host participant.
/// </summary>
/// <param name="request">The room creation details.</param>
/// <param name="caller">The account or guest making the request.</param>
public sealed record CreateDraftRoomCommand(
    CreateDraftRoomRequest Request,
    DraftRoomCaller Caller)
    : IRequest<Result<DraftRoomEntryDto>>;

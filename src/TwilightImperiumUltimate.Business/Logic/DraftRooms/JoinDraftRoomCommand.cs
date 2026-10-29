using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Joins a draft room or re-enters as a known participant.
/// </summary>
/// <param name="Request">The room code and new-participant credentials.</param>
/// <param name="Caller">The account or guest identity making the request.</param>
/// <param name="ClientIp">The connecting client's IP address for throttling.</param>
public sealed record JoinDraftRoomCommand(
    JoinDraftRoomRequest Request,
    DraftRoomCaller Caller,
    string ClientIp)
    : IRequest<Result<DraftRoomEntryDto>>;

using FluentResults;
using MediatR;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Reads a finished room's immutable result for a verified participant.</summary>
public sealed record GetRoomResultQuery(string Code, DraftRoomCaller Caller) : IRequest<Result<DraftResultDto>>;

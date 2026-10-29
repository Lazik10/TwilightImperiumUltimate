using FluentResults;
using MediatR;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Reads only the public, versioned result snapshot.</summary>
public sealed record GetResultByTokenQuery(string Token) : IRequest<Result<DraftResultDto>>;

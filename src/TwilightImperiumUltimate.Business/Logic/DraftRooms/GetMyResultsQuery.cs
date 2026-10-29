using FluentResults;
using MediatR;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Reads an account's bounded result history.</summary>
public sealed record GetMyResultsQuery(string UserId, int Page = 1) : IRequest<Result<DraftResultHistoryDto>>;

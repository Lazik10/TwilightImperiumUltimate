using FluentResults;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Reads a bounded page with one extra row to determine whether another page exists.</summary>
public sealed class GetMyResultsQueryHandler(IDraftResultRepository repository) : IRequestHandler<GetMyResultsQuery, Result<DraftResultHistoryDto>>
{
    private const int PageSize = 20;

    /// <inheritdoc />
    public async Task<Result<DraftResultHistoryDto>> Handle(GetMyResultsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            return Result.Fail<DraftResultHistoryDto>(new DraftRoomError(DraftRoomErrorCode.InvalidCaller));
        }

        if (request.Page is < 1 or > int.MaxValue / PageSize)
        {
            return Result.Fail<DraftResultHistoryDto>(new DraftRoomError(DraftRoomErrorCode.InvalidHistoryCursor));
        }

        var items = await repository.GetHistoryAsync(request.UserId, (request.Page - 1) * PageSize, PageSize + 1, cancellationToken);
        return Result.Ok(new DraftResultHistoryDto(items.Take(PageSize).ToArray(), request.Page, items.Count > PageSize));
    }
}

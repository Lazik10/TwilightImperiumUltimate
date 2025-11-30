using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class GetAllTiglUsersQueryHandler(IDbContextFactory<TwilightImperiumDbContext> contextFactory)
    : IRequestHandler<GetAllTiglUsersQuery, ItemListDto<TiglUserLiteDto>>
{
    public async Task<ItemListDto<TiglUserLiteDto>> Handle(GetAllTiglUsersQuery request, CancellationToken cancellationToken)
    {
        using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

        var items = await db.TiglUsers
            .AsNoTracking()
            .Select(u => new TiglUserLiteDto { Id = u.Id, DiscordUserId = u.DiscordId , TiglUserName = u.TiglUserName, DiscordUserName = u.DiscordTag })
            .OrderBy(u => u.TiglUserName)
            .ToListAsync(cancellationToken);

        return new ItemListDto<TiglUserLiteDto>(items);
    }
}

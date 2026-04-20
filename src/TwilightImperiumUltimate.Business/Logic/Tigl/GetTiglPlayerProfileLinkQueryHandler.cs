using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class GetTiglPlayerProfileLinkQueryHandler(IDbContextFactory<TwilightImperiumDbContext> dbFactory)
    : IRequestHandler<GetTiglPlayerProfileLinkQuery, TiglPlayerProfileLinkDto>
{
    private const string TiglPlayerProfilePage = "/community/tigl/player-profile";

    public async Task<TiglPlayerProfileLinkDto> Handle(GetTiglPlayerProfileLinkQuery request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var user = await db.TiglUsers
            .AsNoTracking()
            .Where(x => x.DiscordId == request.DiscordUserId)
            .Select(x => new { x.Id })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return new TiglPlayerProfileLinkDto
            {
                DiscordUserId = request.DiscordUserId,
                TiglUserId = -1,
                Url = string.Empty,
            };
        }

        return new TiglPlayerProfileLinkDto
        {
            DiscordUserId = request.DiscordUserId,
            TiglUserId = user.Id,
            Url = $"{TiglPlayerProfilePage}?playerId={user.Id}",
        };
    }
}

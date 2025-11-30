using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Prestige;

namespace TwilightImperiumUltimate.Business.Logic.Tigl.Prestige;

public class RemovePrestigeRankCommandHandler(IDbContextFactory<TwilightImperiumDbContext> dbFactory)
    : IRequestHandler<RemovePrestigeRankCommand, RemovePrestigeRankResponse>
{
    public async Task<RemovePrestigeRankResponse> Handle(RemovePrestigeRankCommand request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var r = request.Request;

        var prestigeRank = await db.PrestigeRanks
            .Where(p => p.Name == r.PrestigeRank && p.League == r.League && p.FactionName == r.Faction)
            .FirstOrDefaultAsync(cancellationToken);

        if (prestigeRank is null)
        {
            return new RemovePrestigeRankResponse
            {
                Success = false,
                ErrorTitle = "NotFound",
                ErrorMessage = "Prestige rank definition not found.",
                TiglUserId = r.TiglUserId,
                PrestigeRank = r.PrestigeRank,
                League = r.League,
                Faction = r.Faction,
                Level = r.Level,
            };
        }

        var userPrestige = await db.TiglUserPrestigeRanks
            .Where(x => x.TiglUserId == r.TiglUserId && x.PrestigeRankId == prestigeRank.Id && x.Rank == r.Level)
            .FirstOrDefaultAsync(cancellationToken);

        if (userPrestige is null)
        {
            return new RemovePrestigeRankResponse
            {
                Success = false,
                ErrorTitle = "NotFound",
                ErrorMessage = "User prestige rank entry not found.",
                TiglUserId = r.TiglUserId,
                PrestigeRank = r.PrestigeRank,
                League = r.League,
                Faction = r.Faction,
                Level = r.Level,
            };
        }

        db.TiglUserPrestigeRanks.Remove(userPrestige);
        await db.SaveChangesAsync(cancellationToken);

        return new RemovePrestigeRankResponse
        {
            Success = true,
            TiglUserId = r.TiglUserId,
            PrestigeRank = r.PrestigeRank,
            League = r.League,
            Faction = r.Faction,
            Level = r.Level,
        };
    }
}

using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Prestige;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ranks;
using TwilightImperiumUltimate.DataAccess.DbContexts;

namespace TwilightImperiumUltimate.Business.Logic.Tigl.Prestige;

public class AwardPrestigeRankCommandHandler(IDbContextFactory<TwilightImperiumDbContext> dbFactory)
    : IRequestHandler<AwardPrestigeRankCommand, AwardPrestigeRankResponse>
{
    public async Task<AwardPrestigeRankResponse> Handle(AwardPrestigeRankCommand request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var r = request.Request;

        var prestigeRank = await db.PrestigeRanks
            .Where(p => p.Name == r.PrestigeRank && p.League == r.League && p.FactionName == r.Faction)
            .FirstOrDefaultAsync(cancellationToken);
        if (prestigeRank is null)
        {
            prestigeRank = new PrestigeRank
            {
                Name = r.PrestigeRank,
                League = r.League,
                FactionName = r.Faction,
            };
            db.PrestigeRanks.Add(prestigeRank);
            await db.SaveChangesAsync(cancellationToken);
        }

        // Prevent duplicate same level entries
        var exists = await db.TiglUserPrestigeRanks.AnyAsync(x => x.TiglUserId == r.TiglUserId && x.PrestigeRankId == prestigeRank.Id && x.Rank == r.Level, cancellationToken);
        if (exists)
        {
            return new AwardPrestigeRankResponse
            {
                Success = false,
                ErrorTitle = "Duplicate",
                ErrorMessage = "Prestige rank already awarded at this level.",
                TiglUserId = r.TiglUserId,
                PrestigeRank = r.PrestigeRank,
                League = r.League,
                Faction = r.Faction,
                Level = r.Level,
            };
        }

        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var userPrestige = new TiglUserPrestigeRank
        {
            TiglUserId = r.TiglUserId,
            PrestigeRankId = prestigeRank.Id,
            Rank = r.Level,
            AchievedAt = now,
        };
        db.TiglUserPrestigeRanks.Add(userPrestige);
        await db.SaveChangesAsync(cancellationToken);

        return new AwardPrestigeRankResponse
        {
            Success = true,
            TiglUserId = r.TiglUserId,
            PrestigeRank = r.PrestigeRank,
            League = r.League,
            Faction = r.Faction,
            Level = r.Level,
            AchievedAt = now,
        };
    }
}

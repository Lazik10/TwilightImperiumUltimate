using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Prestige;
using TwilightImperiumUltimate.Contracts.Enums;
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

        var rankCandidates = await db.PrestigeRanks
            .Where(p => p.Name == r.PrestigeRank && p.League == r.League)
            .OrderBy(p => p.Id)
            .ToListAsync(cancellationToken);

        PrestigeRank? prestigeRank = null;

        if (r.Faction != TiglFactionName.None)
        {
            prestigeRank = rankCandidates.FirstOrDefault(p => p.FactionName == r.Faction);
        }
        else
        {
            if (rankCandidates.Count == 1)
            {
                prestigeRank = rankCandidates[0];
            }
            else
            {
                var nonDefaultFactionCandidates = rankCandidates.Where(p => p.FactionName != TiglFactionName.None).ToList();
                if (nonDefaultFactionCandidates.Count == 1)
                    prestigeRank = nonDefaultFactionCandidates[0];
            }
        }

        if (prestigeRank is null)
        {
            return new AwardPrestigeRankResponse
            {
                Success = false,
                ErrorTitle = "NotFound",
                ErrorMessage = rankCandidates.Count > 1
                    ? "Prestige rank definition is ambiguous for this league. Specify faction explicitly."
                    : "Prestige rank definition not found.",
                TiglUserId = r.TiglUserId,
                PrestigeRank = r.PrestigeRank,
                League = r.League,
                Faction = r.Faction,
                Level = r.Level,
            };
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

        var achievedAt = r.AchievedAt > 0 ? r.AchievedAt : DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var userPrestige = new TiglUserPrestigeRank
        {
            TiglUserId = r.TiglUserId,
            PrestigeRankId = prestigeRank.Id,
            Rank = r.Level,
            AchievedAt = achievedAt,
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
            AchievedAt = achievedAt,
        };
    }
}

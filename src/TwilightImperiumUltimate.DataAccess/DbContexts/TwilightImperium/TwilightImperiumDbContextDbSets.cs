using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TwilightImperiumUltimate.Core.Entities.Async;
using TwilightImperiumUltimate.Core.Entities.Logging;
using TwilightImperiumUltimate.Core.Entities.Statistics;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ranks;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContext : IdentityDbContext<TwilightImperiumUser>
{
    // Async
    public virtual DbSet<AsyncPlayerProfile> AsyncPlayerProfiles { get; set; }

    public virtual DbSet<AsyncPlayerProfileSettings> AsyncPlayerStatisticsSettings { get; set; }

    public virtual DbSet<GameStats> GameStats { get; set; }

    public virtual DbSet<PlayerStats> PlayerStats { get; set; }

    // Cards
    public virtual DbSet<ActionCard> ActionCards { get; set; }

    public virtual DbSet<AgendaCard> AgendaCards { get; set; }

    public virtual DbSet<ExplorationCard> ExplorationCards { get; set; }

    public virtual DbSet<FrontierCard> FrontierCards { get; set; }

    public virtual DbSet<ObjectiveCard> ObjectiveCards { get; set; }

    public virtual DbSet<RelicCard> RelicCards { get; set; }

    public virtual DbSet<StrategyCard> StrategyCards { get; set; }

    public virtual DbSet<PromissoryNoteCard> PromissoryNoteCards { get; set; }

    // Galaxy
    public virtual DbSet<Planet> Planets { get; set; }

    public virtual DbSet<SystemTile> SystemTiles { get; set; }

    public virtual DbSet<Wormhole> Wormholes { get; set; }

    public virtual DbSet<Map> Maps { get; set; }

    public virtual DbSet<SliceDraft> SliceDrafts { get; set; }

    public virtual DbSet<SliceDraftRating> SliceDraftRatings { get; set; }

    // Game Statistics
    public virtual DbSet<FactionRoundStatistics> FactionRoundStatistics { get; set; }

    public virtual DbSet<GameStatistics> GameStatistics { get; set; }

    public virtual DbSet<WebsiteStatistics> WebsiteStatistics { get; set; }

    // Player
    public virtual DbSet<Player> Players { get; set; }

    // Factions
    public virtual DbSet<Faction> Factions { get; set; }

    public virtual DbSet<FactionColorImportance> FactionColorImportances { get; set; }

    // Logs
    public virtual DbSet<RankUpLog> RankUpLogs { get; set; }

    public virtual DbSet<LeaderLog> LeaderLogHistory { get; set; }

    public virtual DbSet<PrestigeLog> PrestigeLogs { get; set; }

    public virtual DbSet<AchievementLog> AchievementLogs { get; set; }

    public virtual DbSet<GamePublishLog> GamePublishLogs { get; set; }

    // Relationships
    public virtual DbSet<FactionTechnology> FactionTechnology { get; set; }

    public virtual DbSet<FactionUnit> FactionUnit { get; set; }

    public virtual DbSet<MapRating> MapRatings { get; set; }

    public virtual DbSet<AsyncPlayerProfileGameStats> AsyncPlayerProfileGameStats { get; set; }

    public virtual DbSet<TiglUserPrestigeRank> TiglUserPrestigeRanks { get; set; }

    public virtual DbSet<TiglUserAchievement> TiglUserAchievements { get; set; }

    // Rules
    public virtual DbSet<Rule> Rules { get; set; }

    public virtual DbSet<Faq> Faqs { get; set; }

    // Technologies
    public virtual DbSet<Technology> Technologies { get; set; }

    // Tigl
    public virtual DbSet<TiglUser> TiglUsers { get; set; }

    public virtual DbSet<MatchReport> GameReports { get; set; }

    public virtual DbSet<AsyncStats> AsyncStats { get; set; }

    public virtual DbSet<GlickoStats> GlickoStats { get; set; }

    public virtual DbSet<TrueSkillStats> TrueSkillStats { get; set; }

    public virtual DbSet<AsyncRating> AsyncRatings { get; set; }

    public virtual DbSet<GlickoRating> GlickoRatings { get; set; }

    public virtual DbSet<TrueSkillRating> TrueSkillRatings { get; set; }

    public virtual DbSet<RatingDecay> RatingDecays { get; set; }

    public virtual DbSet<AsyncPlayerMatchStats> AsyncPlayerMatchStats { get; set; }

    public virtual DbSet<GlickoPlayerMatchStats> GlickoPlayerMatchStats { get; set; }

    public virtual DbSet<TrueSkillPlayerMatchStats> TrueSkillPlayerMatchStats { get; set; }

    public virtual DbSet<Season> Seasons { get; set; }

    public virtual DbSet<PlayerSeasonResult> SeasonLeaderboard { get; set; }

    public virtual DbSet<TiglParameter> TiglParameters { get; set; }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<TiglRank> Ranks { get; set; }

    public virtual DbSet<PrestigeRank> PrestigeRanks { get; set; }

    public virtual DbSet<Leader> Leaders { get; set; }

    // Units
    public virtual DbSet<Unit> Units { get; set; }

    // News
    public virtual DbSet<NewsArticle> NewsArticles { get; set; }

    // Websites
    public virtual DbSet<Website> Websites { get; set; }
}

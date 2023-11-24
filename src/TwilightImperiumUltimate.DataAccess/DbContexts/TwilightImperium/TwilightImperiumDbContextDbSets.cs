using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Entities.News;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Rules;
using TwilightImperiumUltimate.Core.Entities.Units;
using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.DataAccess.RelationshipEntities;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContext : DbContext
{
    // Cards
    public virtual DbSet<ActionCard> ActionCards { get; set; }

    public virtual DbSet<AgendaCard> AgendaCards { get; set; }

    public virtual DbSet<ExplorationCard> ExplorationCards { get; set; }

    public virtual DbSet<FrontierCard> FrontierCards { get; set; }

    public virtual DbSet<ObjectiveCard> ObjectivesCards { get; set; }

    public virtual DbSet<RelicCard> RelicCards { get; set; }

    public virtual DbSet<StrategyCard> StrategyCards { get; set; }

    // Galaxy
    public virtual DbSet<Planet> Planets { get; set; }

    public virtual DbSet<SystemTile> SystemTiles { get; set; }

    public virtual DbSet<Wormhole> Wormholes { get; set; }

    // Player
    public virtual DbSet<Player> Players { get; set; }

    // Factions
    public virtual DbSet<Faction> Factions { get; set; }

    public virtual DbSet<FactionColorImportance> FactionColorImportances { get; set; }

    // Relationships
    public virtual DbSet<FactionTechnology> RaceTechnology { get; set; }

    public virtual DbSet<FactionUnit> FactionUnit { get; set; }

    // Rules
    public virtual DbSet<Rule> Rules { get; set; }

    // Technologies
    public virtual DbSet<Technology> Technologies { get; set; }

    // Units
    public virtual DbSet<Unit> Units { get; set; }

    // Users
    public virtual DbSet<User> Users { get; set; }

    // News
    public virtual DbSet<NewsArticle> NewsArticles { get; set; }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContext : IdentityDbContext<TwilightImperiumUser>
{
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

    // Player
    public virtual DbSet<Player> Players { get; set; }

    // Factions
    public virtual DbSet<Faction> Factions { get; set; }

    public virtual DbSet<FactionColorImportance> FactionColorImportances { get; set; }

    // Relationships
    public virtual DbSet<FactionTechnology> FactionTechnology { get; set; }

    public virtual DbSet<FactionUnit> FactionUnit { get; set; }

    // Rules
    public virtual DbSet<Rule> Rules { get; set; }

    public virtual DbSet<Faq> Faqs { get; set; }

    // Technologies
    public virtual DbSet<Technology> Technologies { get; set; }

    // Units
    public virtual DbSet<Unit> Units { get; set; }

    // News
    public virtual DbSet<NewsArticle> NewsArticles { get; set; }

    // Websites
    public virtual DbSet<Website> Websites { get; set; }
}

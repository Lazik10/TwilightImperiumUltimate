using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Statistics;

public class WebsiteStatistics : IEntity
{
    public int Id { get; set; }

    public int Visitors { get; set; }

    public int MapsGenerated { get; set; }

    public int SlicesGenerated { get; set; }

    public int MapsArchived { get; set; }

    public int SlicesArchived { get; set; }

    public int GamesPlayed { get; set; }

    public int FactionsDrafted { get; set; }

    public int ColorsDrafted { get; set; }
}

using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;

public class AsyncRating : IEntity, IRating
{
    public int Id { get; set; }

    public double Rating { get; set; } = 1000.0;

    public double AussieScore { get; set; }

    public int AsyncStatsId { get; set; }

    public AsyncStats? AsyncStats { get; set; }
}

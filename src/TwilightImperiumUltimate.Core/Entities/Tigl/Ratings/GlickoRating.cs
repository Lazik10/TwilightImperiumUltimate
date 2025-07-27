using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;

public class GlickoRating : IEntity, IRating
{
    public int Id { get; set; }

    public double Rating { get; set; } = 1500;

    public double Rd { get; set; } = 350;

    public double Volatility { get; set; } = 0.06;

    public int GlickoStatsId { get; set; }

    public GlickoStats? GlickoStats { get; set; }
}

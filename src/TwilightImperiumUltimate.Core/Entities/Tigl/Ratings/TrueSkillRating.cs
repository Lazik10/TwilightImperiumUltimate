using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;

public class TrueSkillRating : IEntity, IRating
{
    public int Id { get; set; }

    public double Mu { get; set; } = 25.0;

    public double Sigma { get; set; } = 25.0 / 3;

    public double ConservativeRating => Mu - (3 * Sigma);

    public double Rating => Mu;

    public int TrueSkillStatsId { get; set; }

    public TrueSkillStats? TrueSkillStats { get; set; }
}

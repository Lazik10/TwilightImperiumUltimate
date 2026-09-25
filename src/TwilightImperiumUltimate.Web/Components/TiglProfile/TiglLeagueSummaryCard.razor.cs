using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Helpers.Numbers;

namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglLeagueSummaryCard
{
    [Parameter]
    [EditorRequired]
    public TiglLeagueProfileDto Profile { get; set; } = default!;

    [Parameter]
    public PrestigeRankHistoryDto? Prestige { get; set; }

    private TextColor PrestigeColor => Prestige?.PrestigeRank switch
    {
        TiglPrestigeRank.PaxMagnificaBellumGloriosum => TextColor.Pmbg,
        TiglPrestigeRank.GalacticThreat => TextColor.GalacticThreat,
        TiglPrestigeRank.Tyrant => TextColor.Tyrant,
        _ => TextColor.White,
    };

    private double WinRate => Profile.GamesPlayed == 0 ? 0 : Profile.AsyncMatchHistory.Count(item => item.RatingChange > 0) / (double)Profile.GamesPlayed * 100;

    private double MaxTrueSkill => Profile.TrueSkillMatchHistory.Count == 0 ? Profile.TrueSkillConservative : Profile.TrueSkillMatchHistory.Max(item => item.MuNew);

    private double MaxGlicko => Profile.GlickoMatchHistory.Count == 0 ? Profile.GlickoRating : Profile.GlickoMatchHistory.Max(item => item.RatingNew);

    private double MaxAsync => Profile.AsyncMatchHistory.Count == 0 ? Profile.AsyncRating : Profile.AsyncMatchHistory.Max(item => item.RatingNew);
}

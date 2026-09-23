using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

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
}

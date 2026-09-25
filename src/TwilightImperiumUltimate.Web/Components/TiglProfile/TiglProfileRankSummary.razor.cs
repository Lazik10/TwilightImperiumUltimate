using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Helpers.Numbers;
namespace TwilightImperiumUltimate.Web.Components.TiglProfile;
public partial class TiglProfileRankSummary
{
    [Parameter, EditorRequired] public TiglPlayerProfileDto Profile { get; set; } = default!;
    private IReadOnlyList<RankSummaryRow> Rows => [Create(TiglLeague.ThundersEdge, "Standard"), Create(TiglLeague.Fractured, "Fractured"), Create(TiglLeague.ProphecyOfKings, "Legacy")];
    private RankSummaryRow Create(TiglLeague league, string name)
    {
        var profile = Profile.LeagueProfiles.FirstOrDefault(item => item.League == league);
        var prestige = Profile.PrestigeRankHistory.Where(item => item.League == league).OrderByDescending(item => item.Level).ThenByDescending(item => item.AchievedAt).FirstOrDefault();
        return new(name, prestige is null ? "-" : FormatPrestigeRank(prestige), profile?.HighestRank.GetDisplayName() ?? "-", prestige is null ? TextColor.White : GetPrestigeColor(prestige.PrestigeRank), profile is null ? TextColor.White : profile.HighestRank.GetRankColor());
    }

    private static string FormatPrestigeRank(PrestigeRankHistoryDto prestige) => prestige.Level > 0
        ? $"{prestige.PrestigeRank.GetDisplayName()} {prestige.Level.ToRomanNumeral()}"
        : prestige.PrestigeRank.GetDisplayName();

    private static TextColor GetPrestigeColor(TiglPrestigeRank rank) => rank switch
    {
        TiglPrestigeRank.PaxMagnificaBellumGloriosum => TextColor.Pmbg,
        TiglPrestigeRank.GalacticThreat => TextColor.GalacticThreat,
        TiglPrestigeRank.Tyrant => TextColor.Tyrant,
        _ => TextColor.White,
    };

    private sealed record RankSummaryRow(string Name, string Prestige, string Rank, TextColor PrestigeColor, TextColor RankColor);
}

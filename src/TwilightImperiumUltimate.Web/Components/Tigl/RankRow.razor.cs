using System.Globalization;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Models.Rankings;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class RankRow
{
    [Parameter]
    public required TiglUserRanking Ranking { get; set; }

    [Parameter]
    public required string PrestigeRankText { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private TextColor RankColor => GetRankRowColor();

    private string GetRankText()
    {
        return Ranking.Rank.GetDisplayName() + Ranking.FactionPrestigeRankCount switch
        {
            > 0 => $" ({Ranking.FactionPrestigeRankCount})",
            _ => string.Empty,
        };
    }

    private string GetPrestigeRankText()
    {
        if (Strings.Rankings_PrestigeRankCategory_GalacticThreatOne == PrestigeRankText
            || Strings.Rankings_PrestigeRankCategory_GalacticThreatTwo == PrestigeRankText
            || Strings.Rankings_PrestigeRankCategory_GalacticThreatThree == PrestigeRankText
            || Strings.Rankings_PrestigeRankCategory_GalacticThreatFour == PrestigeRankText
            || Strings.Rankings_PrestigeRankCategory_GalacticThreatFive == PrestigeRankText
            || Strings.Rankings_PrestigeRankCategory_Pmbg == PrestigeRankText)
        {
            return PrestigeRankText;
        }

        if (Ranking.HasPrestigeRank && Ranking.Prestige == TiglPrestigeRank.Tyrant)
        {
            var addRankLevel = Ranking.PrestigeLevel > 0;
            return $"{TiglPrestigeRank.Tyrant.GetDisplayName()} {(addRankLevel ? ("#" + Ranking.PrestigeLevel.ToString(CultureInfo.InvariantCulture)) : string.Empty)}";
        }

        return string.Empty;
    }

    private string GetDurationTime()
    {
        if (Ranking.LastAchievedAt == 0)
            return "Unknown";

        var startDate = DateTimeOffset.FromUnixTimeMilliseconds(Ranking.LastAchievedAt);
        var endDate = DateTimeOffset.Now;

        var duration = endDate - startDate;

        if (duration.Days == 0)
            return "< 24 hours";
        if (duration.Days == 1)
            return "1 day";

        return $"{duration.Days:D2} days";
    }

    private TextColor GetPrestigeColor()
    {
        return Ranking.Prestige switch
        {
            TiglPrestigeRank.PaxMagnificaBellumGloriosum => TextColor.Pmbg,
            TiglPrestigeRank.GalacticThreat => TextColor.GalacticThreat,
            TiglPrestigeRank.Tyrant => TextColor.Tyrant,
            _ => TextColor.White,
        };
    }

    private TextColor GetRankRowColor()
    {
        return Ranking.HasPrestigeRank ? GetPrestigeColor() : Ranking.Rank.GetRankColor();
    }

    private void NavigateToProfile(int tiglUserId)
    {
        var returnUrl = Uri.EscapeDataString(Pages.Pages.TiglRankings);
        NavigationManager.NavigateTo($"{Pages.Pages.TiglPlayerProfile}?playerId={tiglUserId}&returnUrl={returnUrl}");
    }
}
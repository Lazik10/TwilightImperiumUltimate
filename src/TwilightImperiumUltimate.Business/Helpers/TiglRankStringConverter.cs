using System.Text;

namespace TwilightImperiumUltimate.Business.Helpers;

public static class TiglRankStringConverter
{
    public static string ToDisplayString(TiglRankName rank)
    {
        return rank switch
        {
            TiglRankName.Unranked => "Unranked",
            TiglRankName.Minister => "Minister",
            TiglRankName.Agent => "Agent",
            TiglRankName.Commander => "Commander",
            TiglRankName.Hero => "Hero",
            TiglRankName.Thrall => "Thrall",
            TiglRankName.Acolyte => "Acolyte",
            TiglRankName.Legionnaire => "Legionnaire",
            TiglRankName.Starlancer => "Starlancer",
            TiglRankName.GeneSorcerer => "Gene-Sorcerer",
            TiglRankName.IxthLord => "Ixth-Lord",
            TiglRankName.Archon => "Archon",
            _ => rank.ToString(),
        };
    }

    public static string ToDisplayString(TiglPrestigeRank prestigeRank)
    {
        return prestigeRank switch
        {
            TiglPrestigeRank.GalacticThreat => "Galactic Threat",
            TiglPrestigeRank.PaxMagnificaBellumGloriosum => "Pax Magnifica Bellum Gloriosum",
            TiglPrestigeRank.Tyrant => "Tyrant",
            _ => prestigeRank.ToString(),
        };
    }

    public static string ComposeCurrentRank(TiglRankName baseRank, TiglPrestigeRank? prestigeRank, int? prestigeLevel)
    {
        var baseRankText = ToDisplayString(baseRank);
        if (prestigeRank is null)
            return baseRankText;

        var prestigeText = ToDisplayString(prestigeRank.Value);
        if (prestigeLevel.GetValueOrDefault() > 0)
        {
            var level = ToRoman(prestigeLevel!.Value);
            return $"{prestigeText} {level} ({baseRankText})";
        }

        return $"{prestigeText} ({baseRankText})";
    }

    private static string ToRoman(int value)
    {
        return value switch
        {
            1 => "I",
            2 => "II",
            3 => "III",
            4 => "IV",
            5 => "V",
            _ => string.Empty,
        };
    }
}

namespace TwilightImperiumUltimate.Business.Helpers;

public static class TiglCurrentRankFormatter
{
    public static string FormatLegacyOrStandard(TiglRankName baseRank, TiglPrestigeRank? prestigeRank, int? prestigeLevel)
    {
        var allowedPrestige = prestigeRank is TiglPrestigeRank.GalacticThreat or TiglPrestigeRank.PaxMagnificaBellumGloriosum
            ? prestigeRank
            : null;

        return TiglRankStringConverter.ComposeCurrentRank(baseRank, allowedPrestige, prestigeLevel);
    }

    public static string FormatFractured(TiglRankName baseRank, TiglPrestigeRank? prestigeRank, int? prestigeLevel)
    {
        var allowedPrestige = prestigeRank == TiglPrestigeRank.Tyrant
            ? prestigeRank
            : null;

        return TiglRankStringConverter.ComposeCurrentRank(baseRank, allowedPrestige, prestigeLevel);
    }
}

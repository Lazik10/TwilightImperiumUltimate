namespace TwilightImperiumUltimate.Web.Services.Rules;

public sealed record ApprovedFaqSnapshot(
    IReadOnlyDictionary<string, IReadOnlyList<FaqModel>> FaqByKey,
    IReadOnlyDictionary<string, string> SearchTextByKey)
{
    public static ApprovedFaqSnapshot Empty { get; } = new(
        new Dictionary<string, IReadOnlyList<FaqModel>>(StringComparer.OrdinalIgnoreCase),
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
}

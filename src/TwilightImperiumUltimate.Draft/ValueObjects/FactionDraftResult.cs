namespace TwilightImperiumUltimate.Draft.ValueObjects;

public record FactionDraftResult(IReadOnlyDictionary<int, List<FactionName>> PlayerFactions);

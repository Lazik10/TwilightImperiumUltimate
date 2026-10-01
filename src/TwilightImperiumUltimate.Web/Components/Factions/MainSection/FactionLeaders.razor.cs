namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionLeaders : FactionInfoComponentBase
{
    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private IReadOnlyList<FactionLeaderSection> LeaderSections => FactionName switch
    {
        FactionName.TheFirmamentTheObsidian => CreateFirmamentObsidianSections(),
        FactionName.TheNomad => CreateNomadSections(),
        FactionName.TheCouncilKeleres => CreateKeleresSections(),
        FactionName.TheGheminaRaiders => CreateGheminaSections(),
        _ => [new(string.Empty, CreateDefaultLeaders())],
    };

    private string CommanderRequirement => FactionName.GetFactionUIText(FactionResourceType.CommanderRequirement);

    private static FactionLeader CreateLeader(string title, string unlockRequirement, ComponentType componentType) =>
        new(title, unlockRequirement, componentType);

    private IReadOnlyList<FactionLeader> CreateDefaultLeaders() =>
    [
        CreateLeader(Strings.Faction_Agent, Strings.Faction_AgentRequirement, ComponentType.Agent),
        CreateLeader(Strings.Faction_Commander, CommanderRequirement, ComponentType.Commander),
        CreateLeader(Strings.Faction_Hero, Strings.Faction_HeroRequirement, ComponentType.Hero),
    ];

    private IReadOnlyList<FactionLeaderSection> CreateFirmamentObsidianSections() =>
    [
        new("The Firmament", CreateDefaultLeaders()),
        new("The Obsidian", CreateObsidianLeaders()),
    ];

    private IReadOnlyList<FactionLeaderSection> CreateNomadSections() =>
    [
        new(string.Empty, CreateNomadAgents()),
        new(string.Empty, CreateCommanderAndHeroLeaders()),
    ];

    private IReadOnlyList<FactionLeaderSection> CreateKeleresSections() =>
    [
        new(string.Empty, CreateAgentAndCommanderLeaders()),
        new(string.Empty, CreateKeleresHeroes()),
    ];

    private IReadOnlyList<FactionLeaderSection> CreateGheminaSections() =>
    [
        new(string.Empty, CreateAgentAndCommanderLeaders()),
        new(string.Empty, CreateGheminaHeroes()),
    ];

    private IReadOnlyList<FactionLeader> CreateObsidianLeaders() =>
    [
        CreateLeader(Strings.Faction_Agent, Strings.Faction_AgentRequirement, ComponentType.Agent),
        CreateLeader(Strings.Faction_Commander, CommanderRequirement, ComponentType.Commander),
        CreateLeader(Strings.Faction_Hero, Strings.Faction_HeroRequirement, ComponentType.Hero),
    ];

    private IReadOnlyList<FactionLeader> CreateNomadAgents() =>
    [
        CreateLeader(Strings.Faction_Agent, Strings.Faction_AgentRequirement, ComponentType.AgentOne),
        CreateLeader(Strings.Faction_Agent, Strings.Faction_AgentRequirement, ComponentType.AgentTwo),
        CreateLeader(Strings.Faction_Agent, Strings.Faction_AgentRequirement, ComponentType.AgentThree),
    ];

    private IReadOnlyList<FactionLeader> CreateCommanderAndHeroLeaders() =>
    [
        CreateLeader(Strings.Faction_Commander, CommanderRequirement, ComponentType.Commander),
        CreateLeader(Strings.Faction_Hero, Strings.Faction_HeroRequirement, ComponentType.Hero),
    ];

    private IReadOnlyList<FactionLeader> CreateAgentAndCommanderLeaders() =>
    [
        CreateLeader(Strings.Faction_Agent, Strings.Faction_AgentRequirement, ComponentType.Agent),
        CreateLeader(Strings.Faction_Commander, CommanderRequirement, ComponentType.Commander),
    ];

    private IReadOnlyList<FactionLeader> CreateKeleresHeroes() =>
    [
        CreateLeader(Strings.Faction_Hero, Strings.Faction_HeroRequirement, ComponentType.HeroOne),
        CreateLeader(Strings.Faction_Hero, Strings.Faction_HeroRequirement, ComponentType.HeroTwo),
        CreateLeader(Strings.Faction_Hero, Strings.Faction_HeroRequirement, ComponentType.HeroThree),
    ];

    private IReadOnlyList<FactionLeader> CreateGheminaHeroes() =>
    [
        CreateLeader(Strings.Faction_Hero, Strings.Faction_HeroRequirement, ComponentType.HeroOne),
        CreateLeader(Strings.Faction_Hero, Strings.Faction_HeroRequirement, ComponentType.HeroTwo),
    ];

    private string GetLeaderImagePath(ComponentType componentType) =>
        PathProvider.GetFactionComponenetTypeImagePath(FactionName.ToString(), componentType);

    private string GetLeaderImagePath(ComponentType componenetType, string sectionName)
    {
        var factionName = sectionName switch
        {
            "The Firmament" => "TheFirmament",
            "The Obsidian" => "TheObsidian",
            _ => FactionName.ToString(),
        };

        return PathProvider.GetFactionComponenetTypeImagePath(factionName, componenetType);
    }

    private sealed record FactionLeaderSection(string Title, IReadOnlyList<FactionLeader> Leaders)
    {
        public int DesktopColumns => Leaders.Count;
    }

    private sealed record FactionLeader(string Title, string UnlockRequirement, ComponentType ComponentType);
}

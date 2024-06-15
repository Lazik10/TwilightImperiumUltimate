using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Models.Technologies;

public class Technology
{
    public int Id { get; set; }

    public TechnologyName TechnologyName { get; set; }

    public TechnologyType Type { get; set; }

    public TechnologyLevel Level { get; set; }

    public bool IsFactionTechnology { get; set; }

    public FactionName FactionName { get; set; }

    public GameVersion GameVersion { get; set; }
}

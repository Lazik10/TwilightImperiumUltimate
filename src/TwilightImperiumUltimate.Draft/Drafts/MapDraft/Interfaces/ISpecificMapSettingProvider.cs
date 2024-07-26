using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

public interface ISpecificMapSettingProvider
{
    Task<IMapSettings> GetMapSettingsForSpecificTemplate(MapTemplate mapTemplate);
}

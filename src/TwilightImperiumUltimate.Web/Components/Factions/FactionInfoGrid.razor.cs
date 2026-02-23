using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionInfoGrid
{
    private FactionModel _selectedFaction = default!;

    private FactionInfoType _selectedFactionInfoType = FactionInfoType.Ability;

    private bool showBigImage;

    private string currentBigImageSrc = string.Empty;

    private RenderFragment DynamicComponent => builder =>
    {
        builder.OpenComponent(0, CreateInfoType());
        builder.AddAttribute(1, "Faction", _selectedFaction);
        builder.CloseComponent();
    };

    public void UpdateSelectedFaction(FactionModel faction)
    {
        _selectedFaction = faction;
        StateHasChanged();
    }

    public void SetFactionInfo(string factionInfoType)
    {
        if (Enum.TryParse<FactionInfoType>(factionInfoType, out var infoType))
        {
            SetFactionInfoType(infoType);
        }

        StateHasChanged();
    }

    private Type CreateInfoType()
    {
        var infoType = _selectedFactionInfoType switch
        {
            FactionInfoType.Ability => typeof(FactionAbilities),
            FactionInfoType.Setup => typeof(FactionSetup),
            FactionInfoType.Components => typeof(FactionComponents),
            FactionInfoType.Leaders => typeof(FactionLeaders),
            FactionInfoType.History => typeof(FactionLore),
            FactionInfoType.Faq => typeof(FactionFaq),
            _ => throw new NotImplementedException(),
        };

        return infoType;
    }

    private void ShowBigImage(bool front, bool isObsidian)
    {
        if (_selectedFaction is not null)
        {
            var factionName = _selectedFaction.FactionName.ToString();
            if (_selectedFaction.FactionName == FactionName.TheFirmamentTheObsidian)
            {
                if (isObsidian)
                    factionName = "TheObsidian";
                else
                    factionName = "TheFirmament";
            }

            currentBigImageSrc = PathProvider.GetFactionSheetPath(factionName, front);
            showBigImage = true;
        }
    }

    private void HideBigImage()
    {
        showBigImage = false;
    }

    private void SetFactionInfoType(FactionInfoType factionInfoType)
    {
        _selectedFactionInfoType = factionInfoType;
    }
}

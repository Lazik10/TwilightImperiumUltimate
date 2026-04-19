using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class Technologies
{
    private TechnologyType _selectedTechnologyType = TechnologyType.Biotic;

    private void TechnologyTypeChange(TechnologyType technologyType) => _selectedTechnologyType = technologyType;
}
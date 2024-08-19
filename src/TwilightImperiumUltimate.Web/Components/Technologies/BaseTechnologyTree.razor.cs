using System.Globalization;

namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class BaseTechnologyTree : TwilightImperiumBaseComponenet
{
    private bool _showBigImage;

    private string _currentBigImageSrc = string.Empty;

    private string _currentBigImageCulture = string.Empty;

    [Parameter]
    public TechnologyType SelectedTechnologyType { get; set; } = TechnologyType.None;

    [Parameter]
    public IReadOnlyCollection<TechnologyModel> Technologies { get; set; } = Array.Empty<TechnologyModel>();

    private void ShowBigImage(TechnologyModel technology)
    {
        _currentBigImageSrc = PathProvider.GetTechnologyImagePath(technology.TechnologyName);
        _currentBigImageCulture = CultureInfo.CurrentCulture.Name;
        _showBigImage = true;
    }

    private void HideBigImage()
    {
        _showBigImage = false;
    }

    private string GetCultureIconPath(string culture)
    {
        return PathProvider.GetCultureIconPath(culture);
    }

    private void SetBigImageAddress(string culture)
    {
        _currentBigImageSrc = _currentBigImageSrc.Replace(_currentBigImageCulture, culture);
        _currentBigImageCulture = culture;
        StateHasChanged();
    }

    private int GetCorrectNumberOFColumns() => SelectedTechnologyType == TechnologyType.Faction ? 4 : 3;
}

using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Text.RegularExpressions;
using TwilightImperiumUltimate.Web.Models.Technologies;

namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class BaseTechnologyTree
{
    private bool showBigImage;

    private string currentBigImageSrc = string.Empty;

    private string currentBigImageCulture = string.Empty;

    [Parameter]
    public IReadOnlyCollection<Technology> Technologies { get; set; } = Array.Empty<Technology>();

    private void ShowBigImage(Technology technology)
    {
        currentBigImageSrc = PathProvider.GetTechnologyImagePath(technology.TechnologyName.ToString());
        currentBigImageCulture = CultureInfo.CurrentCulture.Name;
        showBigImage = true;
    }

    private void HideBigImage()
    {
        showBigImage = false;
    }

    private string GetCultureIconPath(string culture)
    {
        return PathProvider.GetCultureIconPath(culture);
    }

    private void SetBigImageAddress(string culture)
    {
        currentBigImageSrc = currentBigImageSrc.Replace(currentBigImageCulture, culture);
        currentBigImageCulture = culture;
        StateHasChanged();
    }
}

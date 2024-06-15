using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Cards;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Components.Cards;

public partial class CardsGrid
{
    private List<BaseCardModel> _listOfCards = default!;

    private bool showBigImage;

    private string currentBigImageSrc = string.Empty;

    private string currentBigImageCulture = string.Empty;

    [Parameter]
    public string TypeOfCard { get; set; } = string.Empty;

    protected override async Task OnParametersSetAsync()
    {
        _listOfCards = await HttpCLient.GetAsync<List<BaseCardModel>>(string.Concat(Paths.ApiPath_Cards, TypeOfCard));

        StateHasChanged();
    }

    private void ShowBigImage(BaseCardModel card, string culture)
    {
        currentBigImageSrc = card.ImagePath;
        currentBigImageCulture = culture;
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

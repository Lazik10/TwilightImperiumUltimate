namespace TwilightImperiumUltimate.Web.Components.Websites;

public partial class WebsiteCard : TwilightImperiumBaseComponenet
{
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public string Description { get; set; } = string.Empty;

    [Parameter]
    public string WebsitePath { get; set; } = string.Empty;

    private string GetWebsiteIconPath()
    {
        return PathProvider.GetWebsitePreviewImagePath(Title);
    }
}
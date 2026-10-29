using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using TwilightImperiumUltimate.Web.Components.Shared.Seo;

namespace TwilightImperiumUltimate.ComponentTests.Components.Shared.Seo;

public sealed class SeoMetadataTests : BunitContext
{
    public SeoMetadataTests()
    {
        JSInterop.Setup<string>("Blazor._internal.PageTitle.getAndRemoveExistingTitle").SetResult(string.Empty);
    }

    [Theory]
    [InlineData("/tools/draft-rooms/ABC234")]
    [InlineData("/tools/draft-rooms/results/AbCdEf0123456789_-abcdE")]
    public void SeoMetadata_WhenDraftRoomOrResultRoute_ShouldRenderNoIndexMetadata(string path)
    {
        Services.GetRequiredService<NavigationManager>().NavigateTo(path);
        var headOutlet = Render<HeadOutlet>();

        Render<SeoMetadata>();

        headOutlet.Find("meta[name='robots']").GetAttribute("content").Should().Be("noindex, follow");
    }

    [Fact]
    public void SeoMetadata_WhenDraftRoomsLandingRoute_ShouldRemainIndexable()
    {
        Services.GetRequiredService<NavigationManager>().NavigateTo("/tools/draft-rooms");
        var headOutlet = Render<HeadOutlet>();

        Render<SeoMetadata>();

        headOutlet.FindAll("meta[name='robots']").Should().BeEmpty();
    }
}

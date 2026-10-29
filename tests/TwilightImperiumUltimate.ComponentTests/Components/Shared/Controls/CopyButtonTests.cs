using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using TwilightImperiumUltimate.Web.Components.Shared.Controls;
using TwilightImperiumUltimate.Web.Services.HttpClients;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.ComponentTests.Components.Shared.Controls;

public sealed class CopyButtonTests : BunitContext
{
    public CopyButtonTests()
    {
        Services.AddSingleton(Mock.Of<IPathProvider>());
        Services.AddSingleton(Mock.Of<ITwilightImperiumApiHttpClient>());
        Services.AddSingleton(Mock.Of<ILocalStorageService>());
        Services.AddSingleton(Mock.Of<IMapper>());
    }

    [Fact]
    public void CopyButton_WhenClipboardIsPending_ShouldNotAnnounceSuccessBeforeBrowserCompletes()
    {
        var module = JSInterop.SetupModule("./Components/Shared/Controls/CopyButton.razor.js");
        var pending = module.SetupVoid("copyToClipboard", invocation => invocation.Arguments[0]?.ToString() == "public result");
        var button = Render<CopyButton>(parameters => parameters.Add(component => component.TextToCopy, "public result"));

        button.Find("button").Click();

        button.Find("[role=status]").TextContent.Should().BeEmpty();
        button.Find("button").GetAttribute("aria-busy").Should().Be("true");
        button.Find("button").HasAttribute("disabled").Should().BeFalse();
        pending.SetVoidResult();
        button.WaitForAssertion(() => button.Find("[role=status]").TextContent.Should().Contain("Copied"));
    }

    [Fact]
    public void CopyButton_WhenBrowserRejectsCopy_ShouldAnnounceFailureAndAllowRetryWithoutSuccess()
    {
        var module = JSInterop.SetupModule("./Components/Shared/Controls/CopyButton.razor.js");
        module.SetupVoid("copyToClipboard", _ => true).SetException(new JSException("Permission denied"));
        var button = Render<CopyButton>(parameters => parameters.Add(component => component.TextToCopy, "public result"));

        button.Find("button").Click();

        button.Find("[role=alert]").TextContent.Should().Contain("not permitted");
        button.Find("[role=status]").TextContent.Should().BeEmpty();
        button.Find("button").GetAttribute("aria-busy").Should().Be("false");
    }
}

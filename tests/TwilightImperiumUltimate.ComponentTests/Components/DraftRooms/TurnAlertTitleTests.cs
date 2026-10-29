using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using TwilightImperiumUltimate.Web.Components.DraftRooms;
using TwilightImperiumUltimate.Web.Components.Shared.Controls;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.ComponentTests.Components.DraftRooms;

public sealed class TurnAlertTitleTests : BunitContext
{
    private readonly Mock<IPageVisibility> _visibility = new();
    private readonly Mock<ILocalStorageService> _storage = new();
    private readonly ManualClock _clock = new();

    public TurnAlertTitleTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        _visibility.Setup(service => service.InitializeAsync()).Returns(Task.CompletedTask);
        _visibility.SetupGet(service => service.IsHidden).Returns(true);
        Services.AddSingleton(_visibility.Object);
        Services.AddSingleton(_storage.Object);
        Services.AddSingleton<TimeProvider>(_clock);
    }

    [Fact]
    public async Task TurnAlert_WhenHiddenAndActionRequired_ShouldAlternateEachSecondAndStopImmediatelyWhenVisible()
    {
        var head = Render<HeadOutlet>();
        var alert = Render<TurnAlertTitle>(parameters => parameters
            .Add(component => component.NormalTitle, "Snake draft")
            .Add(component => component.MyActionRequired, true));

        head.Find("title").TextContent.Should().Be("Snake draft");
        await alert.InvokeAsync(_clock.Tick);
        head.WaitForAssertion(() => head.Find("title").TextContent.Should().Be("Your turn - Snake draft"));
        await alert.InvokeAsync(_clock.Tick);
        head.WaitForAssertion(() => head.Find("title").TextContent.Should().Be("Snake draft"));
        await alert.InvokeAsync(_clock.Tick);
        head.WaitForAssertion(() => head.Find("title").TextContent.Should().Be("Your turn - Snake draft"));

        _visibility.SetupGet(service => service.IsHidden).Returns(false);
        _visibility.Raise(service => service.Changed += null);
        head.WaitForAssertion(() => head.Find("title").TextContent.Should().Be("Snake draft"));
        await alert.InvokeAsync(_clock.Tick);
        head.Find("title").TextContent.Should().Be("Snake draft");
        alert.Find("[role=status]").TextContent.Should().Contain("Your turn");
    }

    [Fact]
    public async Task TurnAlert_WhenTurnEndsOrSwitchIsOff_ShouldRestoreTitleAndPersistPreferenceWithoutHidingPrompt()
    {
        var head = Render<HeadOutlet>();
        var alert = Render<TurnAlertTitle>(parameters => parameters
            .Add(component => component.NormalTitle, "Room")
            .Add(component => component.MyActionRequired, true));
        await alert.InvokeAsync(_clock.Tick);
        head.WaitForAssertion(() => head.Find("title").TextContent.Should().Be("Your turn - Room"));

        await alert.InvokeAsync(() => alert.FindComponent<ResponsiveSwitch>().Instance.IsToggledChanged.InvokeAsync(false));
        head.WaitForAssertion(() => head.Find("title").TextContent.Should().Be("Room"));
        _storage.Verify(service => service.SetItemAsync("draftRooms.turnAlert.v1", false, It.IsAny<CancellationToken>()), Times.Once);
        alert.FindAll("[role=status]").Should().ContainSingle();
        await alert.InvokeAsync(_clock.Tick);
        head.Find("title").TextContent.Should().Be("Room");

        await alert.InvokeAsync(() => alert.FindComponent<ResponsiveSwitch>().Instance.IsToggledChanged.InvokeAsync(true));
        await alert.InvokeAsync(_clock.Tick);
        head.WaitForAssertion(() => head.Find("title").TextContent.Should().Be("Your turn - Room"));
        alert.Render(parameters => parameters.Add(component => component.MyActionRequired, false));
        head.Find("title").TextContent.Should().Be("Room");
        alert.FindAll("[role=status]").Should().BeEmpty();
    }

    [Fact]
    public async Task TurnAlert_WhenPreferenceDisabled_ShouldRestoreSavedSettingAndDisposeTimer()
    {
        _storage.Setup(service => service.GetItemAsync<bool?>("draftRooms.turnAlert.v1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var head = Render<HeadOutlet>();
        var alert = Render<TurnAlertTitle>(parameters => parameters
            .Add(component => component.NormalTitle, "Room")
            .Add(component => component.MyActionRequired, true));
        await alert.InvokeAsync(_clock.Tick);
        head.Find("title").TextContent.Should().Be("Room");
        alert.FindComponent<ResponsiveSwitch>().Instance.IsToggled.Should().BeFalse();
        await alert.Instance.DisposeAsync();
        _visibility.Verify(service => service.DisposeAsync(), Times.Once);
        _clock.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void TurnAlert_WhenBrowserInteropFails_ShouldShowExplicitFailureAndKeepVisibleTurnPrompt()
    {
        _visibility.Setup(service => service.InitializeAsync()).ThrowsAsync(new JSException("Browser unavailable"));

        var alert = Render<TurnAlertTitle>(parameters => parameters
            .Add(component => component.NormalTitle, "Room")
            .Add(component => component.MyActionRequired, true));

        alert.Find("[role=alert]").TextContent.Should().Contain("unavailable");
        alert.Find("[role=status]").TextContent.Should().Contain("Your turn");
        alert.FindComponent<ResponsiveSwitch>().Instance.Disabled.Should().BeTrue();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA1001:Types that own disposable fields should be disposable",
        Justification = "The component owns the returned timer; the clock retains it only to drive and verify its disposal.")]
    private sealed class ManualClock : TimeProvider
    {
        private ManualTimer? _timer;

        public bool IsDisposed => _timer?.IsDisposed == true;

        public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
        {
            dueTime.Should().Be(TimeSpan.FromSeconds(1));
            period.Should().Be(TimeSpan.FromSeconds(1));
            _timer = new ManualTimer(callback, state);
            return _timer;
        }

        public void Tick() => _timer!.Tick();

        private sealed class ManualTimer(TimerCallback callback, object? state) : ITimer
        {
            public bool IsDisposed { get; private set; }

            public void Tick()
            {
                if (!IsDisposed)
                {
                    callback(state);
                }
            }

            public bool Change(TimeSpan dueTime, TimeSpan period) => throw new NotSupportedException();

            public void Dispose() => IsDisposed = true;

            public ValueTask DisposeAsync()
            {
                this.Dispose();
                return ValueTask.CompletedTask;
            }
        }
    }
}

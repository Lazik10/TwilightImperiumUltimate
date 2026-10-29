using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using TwilightImperiumUltimate.E2ETests.Hosting;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.Playwright.Assertions;

namespace TwilightImperiumUltimate.E2ETests;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomJourneyTests(E2EHostFixture host, ITestOutputHelper output) : IClassFixture<E2EHostFixture>
{
    private readonly ConcurrentQueue<string> browserErrors = new();

    [Fact]
    public async Task Landing_WhenOpenedInRealBrowser_ShouldOfferCreateAndJoinWithoutStartupExceptions()
    {
        await using var context = await CreateContextAsync();
        var page = await context.NewPageAsync();
        var errors = new List<string>();
        page.PageError += (_, error) => errors.Add(error);

        await page.GotoAsync("/tools/draft-rooms");

        await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Create Draft", Exact = true })).ToBeVisibleAsync();
        await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Join Draft", Exact = true })).ToBeVisibleAsync();
        Assert.Empty(errors);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Room_WhenGuestsCreateJoinClaimChatAndReload_ShouldSynchronizeAndKeepIdentity(int run)
    {
        await using var hostContext = await CreateContextAsync();
        await using var guestContext = await CreateContextAsync();
        var roomName = $"Browser room {run}";
        var owner = await CreateRoomAsync(hostContext, roomName);
        var code = new Uri(owner.Url).Segments[^1];
        var guest = await JoinRoomAsync(guestContext, code, "Browser guest");
        if (run == 3)
        {
            await guest.SetViewportSizeAsync(390, 844);
        }

        await Seat(guest, 2).GetByRole(AriaRole.Button, new() { Name = "Claim", Exact = true }).ClickAsync();
        await Expect(Seat(owner, 2)).ToContainTextAsync("Browser guest");
        if (run == 3)
        {
            await guest.Locator("#draft-room-mobile-chat-tab").FocusAsync();
            await guest.Locator("#draft-room-mobile-chat-tab").PressAsync("Enter");
            await Expect(guest.Locator("#draft-room-mobile-chat-tab")).ToHaveAttributeAsync("aria-expanded", "true");
            await Expect(guest.Locator("#draft-room-chat-panel")).ToBeVisibleAsync();
            await Expect(guest.GetByRole(AriaRole.Tabpanel)).ToHaveCountAsync(0);
            Assert.True(await guest.Locator(".draft-room-lobby__mobile-tabs").EvaluateAsync<bool>(
                "nav => nav.scrollWidth <= nav.clientWidth && Array.from(nav.querySelectorAll('button')).every(button => button.getBoundingClientRect().right <= nav.getBoundingClientRect().right + 0.5)"));
        }

        await owner.Locator("#draft-room-chat-input").FillAsync($"Live browser message {run}");
        await owner.Locator("#draft-room-chat-input").PressAsync("Tab");
        await owner.GetByRole(AriaRole.Button, new() { Name = "Send", Exact = true }).ClickAsync();
        await Expect(guest.GetByRole(AriaRole.Log, new() { Name = "Draft room chat messages" }))
            .ToContainTextAsync($"Live browser message {run}");

        await guest.ReloadAsync();

        await Expect(guest.GetByRole(AriaRole.Heading, new() { Name = roomName, Exact = true })).ToBeVisibleAsync(new() { Timeout = 60_000 });
        await Expect(Seat(guest, 2)).ToContainTextAsync("Browser guest");
        await Expect(Seat(guest, 2).GetByRole(AriaRole.Button, new() { Name = "Leave seat", Exact = true })).ToBeVisibleAsync();
        await Expect(guest.Locator(".site-header, .site-shell")).ToHaveCountAsync(0);
        await Expect(guest.Locator(".draft-room-connection-status--connected")).ToBeVisibleAsync();
        Assert.Empty(this.browserErrors);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Draft_WhenThreePlayersBanAndSnakePick_ShouldKeepPendingChoicesPrivateAndPublishCopiedResults(int run)
    {
        await using var ownerContext = await CreateContextAsync();
        await using var secondContext = await CreateContextAsync();
        await using var thirdContext = await CreateContextAsync();
        await using var cleanContext = await CreateContextAsync();
        var owner = await CreateRoomAsync(ownerContext, $"Browser draft {run}");
        var code = new Uri(owner.Url).Segments[^1];
        for (var count = 6; count > 3; count--)
        {
            await Seat(owner, count).GetByRole(AriaRole.Button, new() { Name = "Remove seat", Exact = true }).ClickAsync();
            await Expect(owner.Locator(".draft-seat-list").GetByRole(AriaRole.Article)).ToHaveCountAsync(count - 1);
        }

        var second = await JoinRoomAsync(secondContext, code, "Second drafter");
        var third = await JoinRoomAsync(thirdContext, code, "Third drafter");
        IPage[] players = [owner, second, third];
        await Expect(Seat(owner, 1)).ToContainTextAsync("Browser owner");
        for (var index = 1; index < players.Length; index++)
        {
            await Seat(players[index], index + 1).GetByRole(AriaRole.Button, new() { Name = "Claim", Exact = true }).ClickAsync();
            await Expect(Seat(owner, index + 1)).ToContainTextAsync(index == 1 ? "Second drafter" : "Third drafter");
        }

        await owner.Locator("#draft-picks-per-player").FillAsync("2");
        await owner.Locator("#draft-picks-per-player").PressAsync("Tab");
        await owner.Locator("#draft-pick-order").SelectOptionAsync("HostDefined");
        await owner.GetByLabel("Bans mode", new() { Exact = true }).SelectOptionAsync("SimultaneousPlayerBans");
        var saved = await owner.RunAndWaitForResponseAsync(
            () => owner.GetByRole(AriaRole.Button, new() { Name = "Save draft settings", Exact = true }).ClickAsync(),
            response => response.Url.EndsWith("/draft/settings", StringComparison.Ordinal) && response.Request.Method == "PUT");
        Assert.Equal(204, saved.Status);
        await owner.GetByRole(AriaRole.Button, new() { Name = "Start draft", Exact = true }).ClickAsync();
        foreach (var player in players)
        {
            await Expect(player.Locator(".faction-pick-board")).ToBeVisibleAsync();
        }

        await owner.Locator(".faction-pick-board__pool button").Nth(0).ClickAsync();
        Assert.True(await owner.EvaluateAsync<bool>(
            "() => Array.from(document.querySelectorAll('.faction-pick-board__pool button')).every(button => button.getBoundingClientRect().width <= button.parentElement.getBoundingClientRect().width + 0.5)"));
        await Expect(owner.Locator(".faction-pick-board__pool button[aria-pressed='true']")).ToHaveCountAsync(1);
        var otherView = await ReadRoomAsync(second, code);
        Assert.Empty(otherView.GetProperty("draft").GetProperty("myBans").EnumerateArray());
        Assert.Empty(otherView.GetProperty("draft").GetProperty("publicBans").EnumerateArray());
        await Expect(second.Locator(".faction-pick-board__pool button[aria-pressed='true']")).ToHaveCountAsync(0);
        var unauthorized = await cleanContext.APIRequest.GetAsync($"{host.ApiUrl}api/draft-rooms/{code}");
        Assert.Equal(404, unauthorized.Status);
        Assert.False((await unauthorized.JsonAsync())!.Value.TryGetProperty("draft", out _));

        for (var index = 1; index < players.Length; index++)
        {
            await players[index].Locator(".faction-pick-board__pool button").Nth(index).ClickAsync();
            await Expect(players[index].Locator(".faction-pick-board__pool button[aria-pressed='true']")).ToHaveCountAsync(1);
        }

        await Task.WhenAll(players.Select(player => player.GetByRole(AriaRole.Button, new() { Name = "Lock in bans", Exact = true }).ClickAsync()));
        await Expect(owner.GetByRole(AriaRole.Button, new() { Name = "Lock in pick", Exact = true })).ToBeVisibleAsync();
        var revealed = await ReadRoomAsync(owner, code);
        Assert.Equal(3, revealed.GetProperty("draft").GetProperty("publicBans").GetArrayLength());
        Assert.True(revealed.GetProperty("settings").GetProperty("snake").GetBoolean());

        await SelectPickAsync(owner);
        var privatePick = await ReadRoomAsync(third, code);
        Assert.Equal(JsonValueKind.Null, privatePick.GetProperty("draft").GetProperty("myPendingPick").ValueKind);
        await Expect(third.Locator(".faction-pick-board__pool button[aria-pressed='true']")).ToHaveCountAsync(0);
        await owner.GetByRole(AriaRole.Button, new() { Name = "Lock in pick", Exact = true }).ClickAsync();
        await Expect(owner.Locator(".faction-pick-board li[aria-current='step']")).ToContainTextAsync("Seat 2");
        await owner.GetByRole(AriaRole.Button, new() { Name = "Undo last pick", Exact = true }).ClickAsync();
        await owner.GetByRole(AriaRole.Alertdialog).GetByRole(AriaRole.Button, new() { Name = "Undo last pick", Exact = true }).ClickAsync();
        await Expect(owner.Locator(".faction-pick-board li[aria-current='step']")).ToContainTextAsync("Seat 1");
        var undone = await ReadRoomAsync(owner, code);
        Assert.Empty(undone.GetProperty("draft").GetProperty("lockedPicks").EnumerateArray());

        int[] snakeSeats = [1, 2, 3, 3, 2, 1];
        foreach (var seat in snakeSeats)
        {
            var player = players[seat - 1];
            await Expect(player.Locator(".faction-pick-board li[aria-current='step']")).ToContainTextAsync($"Seat {seat}");
            await SelectPickAsync(player);
            await player.GetByRole(AriaRole.Button, new() { Name = "Lock in pick", Exact = true }).ClickAsync();
            await Expect(player.Locator(".faction-pick-board__pool button[aria-pressed='true']")).ToHaveCountAsync(0);
        }

        foreach (var player in players)
        {
            await Expect(player.GetByRole(AriaRole.Heading, new() { Name = "Draft results", Exact = true })).ToBeVisibleAsync();
        }

        var shareUrl = await owner.GetByRole(AriaRole.Link, new() { Name = "Open public results (new tab)", Exact = true }).GetAttributeAsync("href");
        Assert.NotNull(shareUrl);
        var publicPage = await cleanContext.NewPageAsync();
        await publicPage.GotoAsync(shareUrl);
        await Expect(publicPage.GetByRole(AriaRole.Heading, new() { Name = "Draft results", Exact = true })).ToBeVisibleAsync();
        await Expect(publicPage.Locator("meta[name='robots']")).ToHaveAttributeAsync("content", new Regex("\\bnoindex\\b"));
        var token = new Uri(shareUrl).Segments[^1];
        var result = await cleanContext.APIRequest.GetAsync($"{host.ApiUrl}api/draft-rooms/results/{token}");
        Assert.True(result.Ok);
        var expectedText = (await result.JsonAsync())!.Value.GetProperty("text").GetString();
        await owner.BringToFrontAsync();
        await owner.GetByRole(AriaRole.Button, new() { Name = "Copy results as text", Exact = true }).ClickAsync();
        await Expect(owner.GetByRole(AriaRole.Button, new() { Name = "Copied!", Exact = true })).ToBeVisibleAsync();
        Assert.Equal(expectedText, await owner.EvaluateAsync<string>("navigator.clipboard.readText()"));
        Assert.Empty(this.browserErrors);
    }

    private static ILocator Seat(IPage page, int index) =>
        page.GetByRole(AriaRole.Article, new() { Name = $"Seat {index}", Exact = true });

    private static async Task SelectPickAsync(IPage player)
    {
        await player.BringToFrontAsync();
        await player.Locator(".faction-pick-board__pool button:not([disabled])").First.ClickAsync();
        await Expect(player.Locator(".faction-pick-board__pool button[aria-pressed='true']")).ToHaveCountAsync(1);
    }

    private static async Task<IPage> CreateRoomAsync(IBrowserContext context, string roomName)
    {
        var landing = await context.NewPageAsync();
        await landing.GotoAsync("/tools/draft-rooms");
        await landing.GetByRole(AriaRole.Button, new() { Name = "Create Draft", Exact = true }).ClickAsync();
        await landing.Locator("#draft-room-name").FillAsync(roomName);
        await landing.Locator("#draft-room-password").FillAsync("browser123");
        await landing.Locator("#draft-room-display-name").FillAsync("Browser owner");
        var room = await context.RunAndWaitForPageAsync(() =>
            landing.GetByRole(AriaRole.Button, new() { Name = "Create room", Exact = true }).ClickAsync());
        await room.BringToFrontAsync();
        await Expect(room.GetByRole(AriaRole.Heading, new() { Name = roomName, Exact = true })).ToBeVisibleAsync(new() { Timeout = 60_000 });
        await Expect(room.Locator(".draft-room-connection-status--connected")).ToBeVisibleAsync();
        await landing.CloseAsync();
        return room;
    }

    private static async Task<IPage> JoinRoomAsync(IBrowserContext context, string code, string displayName)
    {
        var landing = await context.NewPageAsync();
        await landing.GotoAsync("/tools/draft-rooms");
        await landing.GetByRole(AriaRole.Button, new() { Name = "Join Draft", Exact = true }).ClickAsync();
        await landing.Locator("#draft-room-code").FillAsync(code);
        await landing.Locator("#draft-room-code").PressAsync("Tab");
        await landing.Locator("#draft-room-join-password").FillAsync("browser123");
        await landing.Locator("#draft-room-join-display-name").FillAsync(displayName);
        var room = await context.RunAndWaitForPageAsync(() =>
            landing.GetByRole(AriaRole.Button, new() { Name = "Join room", Exact = true }).ClickAsync());
        await room.BringToFrontAsync();
        await Expect(room.Locator(".draft-room-connection-status--connected")).ToBeVisibleAsync(new() { Timeout = 60_000 });
        await landing.CloseAsync();
        return room;
    }

    private async Task<JsonElement> ReadRoomAsync(IPage page, string code)
    {
        await page.BringToFrontAsync();
        var requests = new ConcurrentDictionary<IRequest, byte>();
        var navigationCommitted = false;
        void HandleNavigation(object? sender, IFrame frame)
        {
            if (frame == page.MainFrame)
            {
                navigationCommitted = true;
            }
        }

        void HandleRequest(object? sender, IRequest request)
        {
            // Pre-reload live refreshes lose their response bodies when the old document unloads.
            if (navigationCommitted && request.Url == $"{host.ApiUrl}api/draft-rooms/{code}" && request.Method == "GET")
            {
                requests.TryAdd(request, 0);
            }
        }

        page.FrameNavigated += HandleNavigation;
        page.Request += HandleRequest;
        try
        {
            var response = await page.RunAndWaitForResponseAsync(
                async () => { await page.ReloadAsync(); },
                candidate => requests.ContainsKey(candidate.Request),
                new() { Timeout = 60_000 });
            Assert.True(response.Ok);
            var view = (await response.JsonAsync())!.Value.Clone();
            await Expect(page.Locator(".draft-room-connection-status--connected")).ToBeVisibleAsync();
            return view;
        }
        catch (TimeoutException)
        {
            output.WriteLine($"Room reload timed out at {new Uri(page.Url).AbsolutePath}. Rendered content: {await page.Locator("body").InnerTextAsync()}");
            throw;
        }
        finally
        {
            page.FrameNavigated -= HandleNavigation;
            page.Request -= HandleRequest;
        }
    }

    private async Task<IBrowserContext> CreateContextAsync()
    {
        var context = await host.CreateContextAsync();
        context.Page += (_, page) =>
        {
            page.PageError += (_, error) => this.browserErrors.Enqueue(error);
            page.Console += (_, message) =>
            {
                if (message.Type == "error")
                {
                    output.WriteLine($"Browser error: {message.Text}");
                }
            };
            page.Response += (_, response) =>
            {
                if (response.Url.StartsWith(host.ApiUrl, StringComparison.Ordinal) || response.Status >= 400)
                {
                    output.WriteLine($"Response {response.Request.Method} {new Uri(response.Url).AbsolutePath}: {response.Status}");
                }
            };
            page.RequestFailed += (_, request) => output.WriteLine($"Request failed {new Uri(request.Url).AbsolutePath}: {request.Failure}");
        };
        return context;
    }
}

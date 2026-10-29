using System.Collections.Concurrent;
using System.Data.Common;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.IntegrationTests.Hosting;
using TwilightImperiumUltimate.IntegrationTests.Infrastructure;
using Xunit.Abstractions;

namespace TwilightImperiumUltimate.IntegrationTests.Load;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1001:Types that own disposable fields should be disposable",
    Justification = "xUnit owns cleanup through IAsyncLifetime.")]
public sealed class DraftRoomLoadTests(ITestOutputHelper output) : IAsyncLifetime
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    private readonly CommitProbe commits = new();
    private readonly List<HttpClient> clients = [];
    private readonly List<HubConnection> connections = [];
    private readonly ConcurrentBag<double> latencies = [];
    private readonly ConcurrentBag<string> missingCommits = [];
    private readonly ConcurrentDictionary<(Guid Room, int Client), int> observedVersions = [];
    private TestDatabase database = default!;
    private TestApiFactory factory = default!;

    public async Task InitializeAsync()
    {
        this.database = await TestDatabase.CreateAsync(false, this.commits);
        this.factory = new TestApiFactory(this.database, TimeProvider.System);
    }

    public async Task DisposeAsync()
    {
        try
        {
            await Task.WhenAll(this.connections.Select(connection => connection.DisposeAsync().AsTask()));
        }
        finally
        {
            foreach (var client in this.clients)
            {
                client.Dispose();
            }

            try
            {
                await this.factory.DisposeAsync();
            }
            finally
            {
                await this.database.DisposeAsync();
            }
        }
    }

    [Fact]
    [Trait("Category", "Load")]
    public async Task Rooms_WhenTwentyRoomsHaveEightConnectedClientsForOneMinute_ShouldDeliverCommittedChangesWithinLimits()
    {
        using var deadline = new CancellationTokenSource(TimeSpan.FromMinutes(10));
        var rooms = new List<RoomTraffic>();
        for (var index = 0; index < 20; index++)
        {
            rooms.Add(await this.CreateRoomTrafficAsync(index, deadline.Token));
        }

        this.connections.Should().HaveCount(160).And.OnlyContain(connection => connection.State == HubConnectionState.Connected);
        using var process = Process.GetCurrentProcess();
        var baselineMemory = process.PrivateMemorySize64;
        var peakMemory = baselineMemory;
        var baselineCpu = process.TotalProcessorTime;
        var watch = Stopwatch.StartNew();
        var round = 0;
        while (watch.Elapsed < TimeSpan.FromMinutes(1))
        {
            await Task.WhenAll(rooms.Select(async room =>
            {
                var operation = round % 2 == 0 ? "unclaim" : "claim";
                using var changed = await room.Host.PostAsync(
                    new Uri($"/api/draft-rooms/{room.Code}/seats/{room.SeatId}/{operation}", UriKind.Relative), null, deadline.Token);
                changed.StatusCode.Should().Be(HttpStatusCode.NoContent);
                room.ExpectedVersion++;
                if (round % 10 == 0)
                {
                    await Task.WhenAll(room.Clients.Select(client => SendBurstAsync(client, room.Code, deadline.Token)));
                }
            }));
            process.Refresh();
            peakMemory = Math.Max(peakMemory, process.PrivateMemorySize64);
            round++;
            await Task.Delay(TimeSpan.FromSeconds(1), deadline.Token);
        }

        var duration = watch.Elapsed;
        var cpu = process.TotalProcessorTime - baselineCpu;
        var deliveryDeadline = Stopwatch.StartNew();
        while (!rooms.All(room => Enumerable.Range(0, 8).All(client =>
                   this.observedVersions.GetValueOrDefault((room.Id, client)) >= room.ExpectedVersion)))
        {
            if (deliveryDeadline.Elapsed > TimeSpan.FromSeconds(15))
            {
                throw new TimeoutException("Not all 160 connected clients observed their room's final committed version.");
            }

            await Task.Delay(50, deadline.Token);
        }

        this.missingCommits.Should().BeEmpty("latency must start at actual SQL commit, not notifier entry");
        var samples = this.latencies.Order().ToArray();
        samples.Should().NotBeEmpty().And.OnlyContain(milliseconds => milliseconds >= 0);
        await using var context = await this.database.ContextFactory.CreateDbContextAsync(deadline.Token);
        var counts = await context.DraftRooms.Select(room => new
        {
            Seats = room.Seats.Count,
            Participants = room.Participants.Count(participant => participant.Status == DraftParticipantStatus.Active),
        }).ToListAsync(deadline.Token);
        counts.Should().HaveCount(20).And.OnlyContain(room =>
            room.Seats == DraftRoomLimits.MaxSeatCount && room.Participants == 8 && room.Participants <= DraftRoomLimits.MaxActiveParticipants);
        output.WriteLine(
            "20 rooms, 160 clients, {0:F2}s measured, {1} state changes, {2} latency samples; private memory baseline/peak {3:F1}/{4:F1} MiB; CPU {5:F2}s ({6:F1}% of one core); commit-to-RoomChanged p50/p95/max {7:F2}/{8:F2}/{9:F2} ms. TestServer long polling; API and clients share this process, SQL Server excluded.",
            duration.TotalSeconds,
            round * 20,
            samples.Length,
            baselineMemory / 1048576d,
            peakMemory / 1048576d,
            cpu.TotalSeconds,
            cpu.TotalSeconds / duration.TotalSeconds * 100,
            samples[samples.Length / 2],
            samples[(int)Math.Ceiling(samples.Length * .95) - 1],
            samples[^1]);
    }

    private static async Task SendBurstAsync(HttpClient client, string code, CancellationToken cancellationToken)
    {
        for (var message = 0; message < 2; message++)
        {
            using var sent = await client.PostAsJsonAsync(
                $"/api/draft-rooms/{code}/chat",
                new SendDraftRoomChatMessageRequest { Text = $"Load message {message}" },
                cancellationToken);
            sent.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }

    private HttpClient GuestClient(DraftRoomEntryDto entry)
    {
        var client = this.factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Draft-Participant-Id", entry.ParticipantId.ToString());
        client.DefaultRequestHeaders.Add("X-Draft-Participant-Secret", entry.GuestSecret);
        this.clients.Add(client);
        return client;
    }

    private async Task<RoomTraffic> CreateRoomTrafficAsync(int index, CancellationToken cancellationToken)
    {
        // Seed through the real business command: setup must not consume the HTTP limit of ten creations/IP/hour.
        using var scope = this.factory.Services.CreateScope();
        var created = await scope.ServiceProvider.GetRequiredService<IMediator>().Send(
            new CreateDraftRoomCommand(new CreateDraftRoomRequest($"Load room {index}", "load-password", "Load host"), new DraftRoomCaller(null, null, null, null)),
            cancellationToken);
        created.IsSuccess.Should().BeTrue();
        var entry = created.Value;
        var host = this.GuestClient(entry);
        for (var seat = DraftRoomLimits.DefaultSeatCount; seat < DraftRoomLimits.MaxSeatCount; seat++)
        {
            using var added = await host.PostAsync(new Uri($"/api/draft-rooms/{entry.Code}/seats", UriKind.Relative), null, cancellationToken);
            added.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        var room = (await host.GetFromJsonAsync<DraftRoomViewDto>($"/api/draft-rooms/{entry.Code}", JsonOptions, cancellationToken))!;
        var entries = new List<DraftRoomEntryDto> { entry };
        var roomClients = new List<HttpClient> { host };
        using var joinClient = this.factory.CreateClient();
        for (var client = 1; client < 8; client++)
        {
            using var response = await joinClient.PostAsJsonAsync(
                "/api/draft-rooms/join", new JoinDraftRoomRequest(entry.Code, "load-password", $"Load guest {client}"), cancellationToken);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var guest = (await response.Content.ReadFromJsonAsync<DraftRoomEntryDto>(JsonOptions, cancellationToken))!;
            entries.Add(guest);
            var guestClient = this.GuestClient(guest);
            roomClients.Add(guestClient);
            using var claimed = await guestClient.PostAsync(
                new Uri($"/api/draft-rooms/{entry.Code}/seats/{room.Seats[client].Id}/claim", UriKind.Relative), null, cancellationToken);
            claimed.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        room = (await host.GetFromJsonAsync<DraftRoomViewDto>($"/api/draft-rooms/{entry.Code}", JsonOptions, cancellationToken))!;
        for (var client = 0; client < entries.Count; client++)
        {
            var clientIndex = client;
            var connection = new HubConnectionBuilder().WithUrl(
                new Uri(this.factory.Server.BaseAddress, "/hubs/draft-rooms"),
                options =>
                {
                    options.Transports = HttpTransportType.LongPolling;
                    options.HttpMessageHandlerFactory = _ => this.factory.Server.CreateHandler();
                }).Build();
            this.connections.Add(connection);
            connection.On<int>("RoomChanged", version =>
            {
                var receivedAt = Stopwatch.GetTimestamp();
                this.observedVersions.AddOrUpdate((entry.RoomId, clientIndex), version, (_, previous) => Math.Max(previous, version));
                if (version > room.Version)
                {
                    if (this.commits.Timestamps.TryGetValue((entry.RoomId, version), out var committedAt))
                    {
                        this.latencies.Add(Stopwatch.GetElapsedTime(committedAt, receivedAt).TotalMilliseconds);
                    }
                    else
                    {
                        this.missingCommits.Add($"{index}:{version}");
                    }
                }
            });
            await connection.StartAsync(cancellationToken);
            await connection.InvokeAsync<int>(
                "Subscribe",
                entry.Code,
                new DraftRoomCredentials
                {
                    GuestParticipantId = entries[client].ParticipantId,
                    GuestSecret = entries[client].GuestSecret,
                },
                cancellationToken);
        }

        return new RoomTraffic(entry.RoomId, entry.Code, room.Seats[0].Id, host, roomClients, room.Version);
    }

    private sealed class RoomTraffic(Guid id, string code, Guid seatId, HttpClient host, List<HttpClient> clients, int version)
    {
        public Guid Id { get; } = id;

        public string Code { get; } = code;

        public Guid SeatId { get; } = seatId;

        public HttpClient Host { get; } = host;

        public List<HttpClient> Clients { get; } = clients;

        public int ExpectedVersion { get; set; } = version;
    }

    private sealed class CommitProbe : DbTransactionInterceptor
    {
        public ConcurrentDictionary<(Guid Room, int Version), long> Timestamps { get; } = new();

        public override Task TransactionCommittedAsync(
            DbTransaction transaction, TransactionEndEventData eventData, CancellationToken cancellationToken = default)
        {
            var timestamp = Stopwatch.GetTimestamp();
            if (eventData.Context is { } context)
            {
                foreach (var entry in context.ChangeTracker.Entries<DraftRoom>())
                {
                    this.Timestamps.TryAdd((entry.Entity.Id, entry.Entity.Version), timestamp);
                }
            }

            return Task.CompletedTask;
        }
    }
}

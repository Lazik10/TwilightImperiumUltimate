using System.Data.Common;
using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwilightImperiumUltimate.Business.Services.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.Core.Interfaces.DraftRooms;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;
using TwilightImperiumUltimate.DataAccess.UnitOfWork;

namespace TwilightImperiumUltimate.API.Services.DraftRooms;

/// <summary>
/// Persists room chat messages for participant presence transitions.
/// </summary>
public sealed class PresenceChatPublisher(
    IPresenceTracker presenceTracker,
    IServiceScopeFactory serviceScopeFactory,
    IRoomChangeNotifier roomChangeNotifier,
    ILogger<PresenceChatPublisher> logger)
    : BackgroundService
{
    private readonly IPresenceTracker _presenceTracker = presenceTracker
        ?? throw new ArgumentNullException(nameof(presenceTracker));

    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory
        ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

    private readonly IRoomChangeNotifier _roomChangeNotifier = roomChangeNotifier
        ?? throw new ArgumentNullException(nameof(roomChangeNotifier));

    private readonly ILogger<PresenceChatPublisher> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    private readonly Channel<PresenceChatTransition> _transitions =
        Channel.CreateUnbounded<PresenceChatTransition>();

    /// <inheritdoc />
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        this._presenceTracker.ParticipantOnline += this.HandleParticipantOnline;
        this._presenceTracker.ParticipantOffline += this.HandleParticipantOffline;

        return base.StartAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        this._presenceTracker.ParticipantOnline -= this.HandleParticipantOnline;
        this._presenceTracker.ParticipantOffline -= this.HandleParticipantOffline;
        this._transitions.Writer.TryComplete();

        await base.StopAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var transition in this._transitions.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await this.PublishAsync(transition, stoppingToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception) when (
                exception is DraftRoomNotFoundException
                    or DraftRoomConflictException
                    or DbUpdateException
                    or DbException)
            {
                this._logger.LogError(
                    exception,
                    "Failed to persist presence chat message for draft room {RoomId}, participant {ParticipantId}.",
                    transition.Change.RoomId,
                    transition.Change.ParticipantId);
            }
        }
    }

    private void HandleParticipantOnline(DraftRoomPresenceChange change)
    {
        this.Enqueue(change, ChatMessageKind.Join);
    }

    private void HandleParticipantOffline(DraftRoomPresenceChange change)
    {
        this.Enqueue(change, ChatMessageKind.Leave);
    }

    private void Enqueue(DraftRoomPresenceChange change, ChatMessageKind kind)
    {
        if (!this._transitions.Writer.TryWrite(new PresenceChatTransition(change, kind)))
        {
            this._logger.LogWarning(
                "Presence chat transition could not be queued for draft room {RoomId}, participant {ParticipantId}.",
                change.RoomId,
                change.ParticipantId);
        }
    }

    private async Task PublishAsync(PresenceChatTransition transition, CancellationToken cancellationToken)
    {
        await using var serviceScope = this._serviceScopeFactory.CreateAsyncScope();
        var contextFactory = serviceScope.ServiceProvider
            .GetRequiredService<IDbContextFactory<TwilightImperiumDbContext>>();
        await using var lookupContext = await contextFactory.CreateDbContextAsync(cancellationToken);

        var participantSnapshot = await lookupContext.DraftRooms
            .AsNoTracking()
            .Where(room => room.Id == transition.Change.RoomId)
            .SelectMany(
                room => room.Participants
                    .Where(participant => participant.Id == transition.Change.ParticipantId)
                    .Select(participant => new
                    {
                        room.Code,
                        participant.Status,
                    }))
            .SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
        if (participantSnapshot?.Status != DraftParticipantStatus.Active)
        {
            return;
        }

        string displayName = string.Empty;
        var message = await serviceScope.ServiceProvider
            .GetRequiredService<IDraftRoomUnitOfWork>()
            .ExecuteAsync(
                participantSnapshot.Code,
                scope =>
                {
                    var participant = scope.Room.Participants
                        .SingleOrDefault(item => item.Id == transition.Change.ParticipantId);
                    if (participant?.Status != DraftParticipantStatus.Active)
                    {
                        return Task.FromResult<DraftRoomChatMessage?>(null);
                    }

                    displayName = participant.DisplayName;
                    var chatMessage = new DraftRoomChatMessage
                    {
                        RoomId = scope.Room.Id,
                        ParticipantId = participant.Id,
                        Kind = transition.Kind,
                        Text = $"{participant.DisplayName} {(transition.Kind == ChatMessageKind.Join ? "joined" : "left")}",
                        CreatedAtUtc = scope.Now,
                    };
                    scope.Context.DraftRoomChatMessages.Add(chatMessage);

                    return Task.FromResult<DraftRoomChatMessage?>(chatMessage);
                },
                cancellationToken,
                changesState: false)
            .ConfigureAwait(false);
        if (message is null)
        {
            return;
        }

        await this._roomChangeNotifier
            .NotifyChatMessageReceivedAsync(
                transition.Change.RoomId,
                new ChatMessageDto
                {
                    Id = message.Id,
                    ParticipantId = message.ParticipantId,
                    DisplayName = displayName,
                    Kind = message.Kind,
                    Text = message.Text,
                    CreatedAtUtc = message.CreatedAtUtc,
                })
            .ConfigureAwait(false);
    }

    private sealed record PresenceChatTransition(
        DraftRoomPresenceChange Change,
        ChatMessageKind Kind);
}

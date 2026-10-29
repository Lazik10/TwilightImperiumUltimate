using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class DraftRoomLogPanel : IAsyncDisposable
{
    private readonly CancellationTokenSource _lifetimeCancellation = new();
    private readonly List<DraftLogEntryDto> _entries = [];
    private readonly Dictionary<long, string> _descriptions = [];
    private bool _isLoading = true;
    private bool _historyError;
    private bool _historyReadyForAnnouncements;
    private bool _disposed;

    [CascadingParameter]
    private IDraftRoomSession DraftRoomSession { get; set; } = default!;

    [Inject]
    private ILogger<DraftRoomLogPanel> Logger { get; set; } = default!;

    public async ValueTask DisposeAsync()
    {
        if (this._disposed)
        {
            return;
        }

        this._disposed = true;
        this.DraftRoomSession.LogEntryAdded -= this.HandleLogEntryAddedAsync;
        await this._lifetimeCancellation.CancelAsync();
        this._lifetimeCancellation.Dispose();
        GC.SuppressFinalize(this);
    }

    protected override async Task OnInitializedAsync()
    {
        this.DraftRoomSession.LogEntryAdded += this.HandleLogEntryAddedAsync;

        try
        {
            var result = await this.DraftRoomSession
                .GetLogHistoryAsync(afterId: null, DraftRoomLimits.DefaultHistoryPageSize, this._lifetimeCancellation.Token);

            if (result.IsSuccess && result.Data is not null)
            {
                this.MergeEntries(result.Data);
            }
            else
            {
                this._historyError = true;
            }
        }
        catch (OperationCanceledException) when (this._lifetimeCancellation.IsCancellationRequested)
        {
            return;
        }
        finally
        {
            this._isLoading = false;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!this._historyReadyForAnnouncements && !this._isLoading)
        {
            this._historyReadyForAnnouncements = true;
            await this.InvokeAsync(this.StateHasChanged);
        }
    }

    private static string Format(string resourceName, params object[] values)
    {
        return string.Format(CultureInfo.CurrentCulture, GetString(resourceName), values);
    }

    private static string GetString(string resourceName)
    {
        return Strings.ResourceManager.GetString(resourceName, CultureInfo.CurrentUICulture)
            ?? throw new InvalidOperationException($"Missing draft-room log resource '{resourceName}'.");
    }

    private static string GetTimestamp(DraftLogEntryDto entry)
    {
        return entry.CreatedAtUtc.ToLocalTime().ToString("t", CultureInfo.CurrentCulture);
    }

    private static string GetTimestampIso(DraftLogEntryDto entry)
    {
        return entry.CreatedAtUtc.ToString("O", CultureInfo.InvariantCulture);
    }

    private static string? GetPropertyText(JsonElement data, string propertyName)
    {
        if (data.ValueKind != JsonValueKind.Object
            || !data.TryGetProperty(propertyName, out var property)
            || property.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return null;
        }

        return property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : property.ToString();
    }

    private static string GetRoleName(string? role)
    {
        return role switch
        {
            "Host" => Strings.DraftRooms_RoleHost,
            "CoHost" => Strings.DraftRooms_RoleCoHost,
            "Member" => Strings.DraftRooms_RoleMember,
            _ => role ?? GetString("DraftRooms_LogUnknownRole"),
        };
    }

    private static string GetFactionNames(JsonElement data)
    {
        if (!data.TryGetProperty("Factions", out var values) || values.ValueKind != JsonValueKind.Array)
        {
            throw new JsonException("An activity entry has no faction array.");
        }

        var factions = values.EnumerateArray().Select(GetFactionName).ToArray();
        return factions.Length == 0 ? GetString("DraftRooms_NoFactions") : string.Join(", ", factions);
    }

    private static string GetFactionName(JsonElement value)
    {
        var text = value.ValueKind == JsonValueKind.String ? value.GetString() : value.GetRawText();
        if (!Enum.TryParse<FactionName>(text, out var faction) || !Enum.IsDefined(faction))
        {
            throw new JsonException("An activity entry contains an invalid faction.");
        }

        return faction.GetFactionUIText(FactionResourceType.Title);
    }

    private Task HandleLogEntryAddedAsync(DraftLogEntryDto entry)
    {
        if (this._disposed)
        {
            return Task.CompletedTask;
        }

        return this.InvokeAsync(() =>
        {
            if (this._entries.Any(item => item.Id == entry.Id))
            {
                return;
            }

            this.MergeEntries([entry]);
            this.StateHasChanged();
        });
    }

    private void MergeEntries(IEnumerable<DraftLogEntryDto> entries)
    {
        var knownIds = this._entries.Select(item => item.Id).ToHashSet();
        foreach (var entry in entries)
        {
            if (!knownIds.Add(entry.Id))
            {
                continue;
            }

            this._entries.Add(entry);
            this._descriptions.Add(entry.Id, this.CreateDescription(entry));
        }

        this._entries.Sort(static (left, right) => left.Id.CompareTo(right.Id));
        if (this._entries.Count > DraftRoomLimits.MaxHistoryPageSize)
        {
            var removedEntries = this._entries.Take(this._entries.Count - DraftRoomLimits.MaxHistoryPageSize).ToArray();
            foreach (var entry in removedEntries)
            {
                this._descriptions.Remove(entry.Id);
            }

            this._entries.RemoveRange(0, removedEntries.Length);
        }
    }

    private string CreateDescription(DraftLogEntryDto entry)
    {
        try
        {
            using var document = JsonDocument.Parse(entry.DataJson);
            var data = document.RootElement;
            var participantName = GetPropertyText(data, "DisplayName");
            var actorName = this.ResolveActorName(entry.ActorParticipantId, GetPropertyText(data, "ActorDisplayName") ?? participantName);

            return entry.Type switch
            {
                DraftLogEntryType.RoomCreated => Format("DraftRooms_LogRoomCreated", this.ResolveActorName(entry.ActorParticipantId, GetPropertyText(data, "HostDisplayName"))),
                DraftLogEntryType.ParticipantJoined => Format("DraftRooms_LogParticipantJoined", this.ResolveActorName(entry.ActorParticipantId, participantName)),
                DraftLogEntryType.ParticipantLeft => Format("DraftRooms_LogParticipantLeft", this.ResolveActorName(entry.ActorParticipantId, participantName)),
                DraftLogEntryType.ParticipantKicked => Format("DraftRooms_LogParticipantKicked", participantName ?? GetString("DraftRooms_LogUnknownParticipant"), actorName),
                DraftLogEntryType.ParticipantLinked => Format(
                    "DraftRooms_LogParticipantLinked",
                    GetPropertyText(data, "PreviousDisplayName") ?? GetString("DraftRooms_LogUnknownParticipant"),
                    GetPropertyText(data, "NewDisplayName") ?? GetString("DraftRooms_LogUnknownParticipant")),
                DraftLogEntryType.SeatAdded => Format("DraftRooms_LogSeatAdded", actorName, GetPropertyText(data, "Index") ?? GetString("DraftRooms_LogUnknownSeat")),
                DraftLogEntryType.SeatRemoved => Format("DraftRooms_LogSeatRemoved", actorName, GetPropertyText(data, "Index") ?? GetString("DraftRooms_LogUnknownSeat")),
                DraftLogEntryType.SeatClaimed => Format("DraftRooms_LogSeatClaimed", this.ResolveActorName(entry.ActorParticipantId, participantName), GetPropertyText(data, "Index") ?? GetString("DraftRooms_LogUnknownSeat")),
                DraftLogEntryType.SeatUnclaimed => Format("DraftRooms_LogSeatUnclaimed", participantName ?? GetString("DraftRooms_LogUnknownParticipant"), actorName),
                DraftLogEntryType.RoleChanged => Format("DraftRooms_LogRoleChanged", actorName, participantName ?? GetString("DraftRooms_LogUnknownParticipant"), GetRoleName(GetPropertyText(data, "NewRole"))),
                DraftLogEntryType.RoomRenamed => Format("DraftRooms_LogRoomRenamed", actorName, GetPropertyText(data, "NewName") ?? GetString("DraftRooms_LogUnknownRoomName")),
                DraftLogEntryType.RoomLocked => Format("DraftRooms_LogRoomLocked", actorName),
                DraftLogEntryType.RoomUnlocked => Format("DraftRooms_LogRoomUnlocked", actorName),
                DraftLogEntryType.PasswordChanged => Format("DraftRooms_LogPasswordChanged", actorName),
                DraftLogEntryType.DraftTypeChanged => Format("DraftRooms_LogDraftTypeChanged", actorName),
                DraftLogEntryType.DraftRandomized => Format("DraftRooms_LogDraftRandomized", this.GetOrderDescription(data)),
                DraftLogEntryType.DraftOrderConfigured => Format("DraftRooms_LogDraftOrderConfigured", this.GetOrderDescription(data)),
                DraftLogEntryType.DraftStarted => Format("DraftRooms_LogDraftStarted", actorName),
                DraftLogEntryType.DraftReset => Format("DraftRooms_LogDraftReset", actorName),
                DraftLogEntryType.DraftSettingsChanged => Format("DraftRooms_LogDraftSettingsChanged", actorName),
                DraftLogEntryType.HostBansChanged => Format("DraftRooms_LogHostBansChanged", actorName, GetFactionNames(data)),
                DraftLogEntryType.SeatBansLocked => Format("DraftRooms_LogSeatBansLocked", GetPropertyText(data, "SeatIndex") ?? GetString("DraftRooms_LogUnknownSeat")),
                DraftLogEntryType.BansRevealed => Format("DraftRooms_LogBansRevealed", GetFactionNames(data)),
                DraftLogEntryType.HostDecidedForSeat => Format(
                    "DraftRooms_LogHostDecidedForSeat",
                    actorName,
                    GetPropertyText(data, "SeatIndex") ?? GetString("DraftRooms_LogUnknownSeat"),
                    GetFactionNames(data)),
                DraftLogEntryType.SeatChoicesDiscarded => Format("DraftRooms_LogSeatChoicesDiscarded", GetPropertyText(data, "Index") ?? GetString("DraftRooms_LogUnknownSeat")),
                DraftLogEntryType.FactionPicked => Format(
                    "DraftRooms_LogFactionPicked",
                    GetPropertyText(data, "SeatIndex") ?? GetString("DraftRooms_LogUnknownSeat"),
                    GetFactionName(data.GetProperty("Faction")),
                    data.GetProperty("DecidedByHost").GetBoolean() ? GetString("DraftRooms_DecidedByHost") : string.Empty),
                DraftLogEntryType.PickUndone => Format("DraftRooms_LogPickUndone", actorName, GetPropertyText(data, "SeatIndex") ?? GetString("DraftRooms_LogUnknownSeat")),
                DraftLogEntryType.DraftFinished => GetString("DraftRooms_LogDraftFinished"),
                DraftLogEntryType.DraftResultDeleted => Format("DraftRooms_LogDraftResultDeleted", actorName),
                _ => GetString("DraftRooms_LogUnknownActivity"),
            };
        }
        catch (JsonException exception)
        {
            this.Logger.LogWarning(exception, "Draft room activity entry {EntryId} contained invalid public data.", entry.Id);
            return GetString("DraftRooms_LogUnknownActivity");
        }
    }

    private string GetOrderDescription(JsonElement data)
    {
        if (data.TryGetProperty("Data", out var engineData))
        {
            data = engineData;
        }

        if (!data.TryGetProperty("SeatIds", out var seats) || seats.ValueKind != JsonValueKind.Array)
        {
            this.Logger.LogWarning("Draft room randomized order entry did not contain a seat array.");
            return GetString("DraftRooms_LogUnknownSeat");
        }

        return string.Join(", ", seats.EnumerateArray().Select(element =>
        {
            var seat = this.DraftRoomSession.View?.Seats.FirstOrDefault(item => item.Id == element.GetGuid());
            return seat is null ? GetString("DraftRooms_LogUnknownSeat") : Format("DraftRooms_SeatNumber", seat.Index);
        }));
    }

    private string ResolveActorName(Guid? participantId, string? fallbackName)
    {
        if (participantId is Guid id)
        {
            var participant = this.DraftRoomSession.View?.Participants
                .FirstOrDefault(item => item.Id == id);
            if (participant is not null)
            {
                return participant.DisplayName;
            }
        }

        return fallbackName ?? GetString("DraftRooms_LogUnknownParticipant");
    }
}

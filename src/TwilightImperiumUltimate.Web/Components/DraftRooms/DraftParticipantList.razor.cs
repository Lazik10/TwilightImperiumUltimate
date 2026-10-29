using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class DraftParticipantList : IDisposable
{
    private ElementReference _participantListRef;
    private DraftRoomViewParticipantDto? _participantToKick;
    private bool _isKickConfirmationOpen;
    private bool _isProcessing;
    private bool _showActionError;
    private bool _disposed;

    [CascadingParameter]
    private IDraftRoomSession DraftRoomSession { get; set; } = default!;

    [Inject]
    private IDraftRoomApiService DraftRoomApiService { get; set; } = default!;

    private DraftRoomViewDto? Room => DraftRoomSession.View;

    private string KickConfirmationMessage =>
        string.Format(
            CultureInfo.CurrentCulture,
            Strings.DraftRooms_ConfirmKickMessage,
            _participantToKick?.DisplayName ?? string.Empty);

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            DraftRoomSession.Changed -= this.HandleSessionChanged;
        }

        _disposed = true;
    }

    protected override void OnInitialized()
    {
        DraftRoomSession.Changed += this.HandleSessionChanged;
    }

    private static string GetRoleLabel(DraftRoomRole role) => role switch
    {
        DraftRoomRole.Host => Strings.DraftRooms_RoleHost,
        DraftRoomRole.CoHost => Strings.DraftRooms_RoleCoHost,
        _ => Strings.DraftRooms_RoleMember,
    };

    private static string GetSeatLabel(DraftRoomViewDto room, DraftRoomViewParticipantDto participant)
    {
        if (participant.SeatId is not Guid seatId)
        {
            return Strings.DraftRooms_NoSeat;
        }

        var seat = room.Seats.FirstOrDefault(item => item.Id == seatId);
        return seat is null
            ? Strings.DraftRooms_SeatUnavailable
            : string.Format(CultureInfo.CurrentCulture, Strings.DraftRooms_SeatNumber, seat.Index);
    }

    private static bool CanChangeParticipantRole(
        DraftRoomViewDto room,
        DraftRoomViewParticipantDto participant) =>
        room.Me.CanChangeRoles
        && participant.Id != room.Me.ParticipantId
        && (participant.Role is DraftRoomRole.Member or DraftRoomRole.CoHost);

    private static bool CanUnclaimSeat(
        DraftRoomViewDto room,
        DraftRoomViewParticipantDto participant) =>
        (room.Me.Role is DraftRoomRole.Host or DraftRoomRole.CoHost)
        && participant.SeatId is not null;

    private static bool CanKickParticipant(
        DraftRoomViewDto room,
        DraftRoomViewParticipantDto participant) =>
        room.Me.CanKick
        && participant.Id != room.Me.ParticipantId
        && participant.Role != DraftRoomRole.Host
        && (room.Me.Role == DraftRoomRole.Host || participant.Role == DraftRoomRole.Member);

    private static bool HasActions(DraftRoomViewDto room, DraftRoomViewParticipantDto participant) =>
        CanChangeParticipantRole(room, participant)
        || CanUnclaimSeat(room, participant)
        || CanKickParticipant(room, participant);

    private void HandleSessionChanged()
    {
        _ = this.InvokeAsync(this.StateHasChanged);
    }

    private void RequestKick(DraftRoomViewParticipantDto participant)
    {
        _participantToKick = participant;
        _isKickConfirmationOpen = true;
    }

    private async Task ConfirmKickAsync()
    {
        if (Room is not { } room || _participantToKick is not { } participant)
        {
            _isKickConfirmationOpen = false;
            _participantToKick = null;
            await _participantListRef.FocusAsync();
            return;
        }

        await this.ExecuteCommandAsync(() => DraftRoomApiService.KickAsync(room.Code, participant.Id));
        _isKickConfirmationOpen = false;
        _participantToKick = null;
        await _participantListRef.FocusAsync();
    }

    private async Task CancelKickAsync()
    {
        _isKickConfirmationOpen = false;
        _participantToKick = null;
        await _participantListRef.FocusAsync();
    }

    private async Task ChangeRoleAsync(Guid participantId, DraftRoomRole role)
    {
        if (Room is not { } room)
        {
            return;
        }

        await this.ExecuteCommandAsync(
            () => DraftRoomApiService.SetRoleAsync(room.Code, participantId, role));
        await _participantListRef.FocusAsync();
    }

    private async Task UnclaimSeatAsync(Guid seatId)
    {
        if (Room is not { } room)
        {
            return;
        }

        await this.ExecuteCommandAsync(() => DraftRoomApiService.UnclaimSeatAsync(room.Code, seatId));
        await _participantListRef.FocusAsync();
    }

    private Task UnclaimParticipantSeatAsync(DraftRoomViewParticipantDto participant) =>
        participant.SeatId is Guid seatId
            ? this.UnclaimSeatAsync(seatId)
            : Task.CompletedTask;

    private async Task ExecuteCommandAsync(Func<Task<DraftRoomApiCommandResult>> command)
    {
        _isProcessing = true;
        _showActionError = false;
        try
        {
            var result = await DraftRoomSession.ExecuteAsync(command);
            _showActionError = !result.IsSuccess;
        }
        catch (JSException)
        {
            _showActionError = true;
        }
        finally
        {
            _isProcessing = false;
        }
    }
}

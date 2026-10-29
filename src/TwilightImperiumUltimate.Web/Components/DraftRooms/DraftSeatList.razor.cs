using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class DraftSeatList : IDisposable
{
    private bool _isProcessing;
    private bool _showActionError;
    private bool _disposed;

    private static string AssignPrompt => DraftRoomText.Get("AssignParticipant");

    [CascadingParameter]
    private IDraftRoomSession DraftRoomSession { get; set; } = default!;

    [Inject]
    private IDraftRoomApiService DraftRoomApiService { get; set; } = default!;

    private DraftRoomViewDto? Room => DraftRoomSession.View;

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

    private static string AssignLabel(int index) =>
        string.Format(CultureInfo.CurrentCulture, DraftRoomText.Get("AssignSeat"), index);

    private void HandleSessionChanged()
    {
        _ = this.InvokeAsync(this.StateHasChanged);
    }

    private Task AssignAsync(Guid seatId, ChangeEventArgs args)
    {
        if (Room is not { } room || !Guid.TryParse(args.Value?.ToString(), out var participantId))
        {
            return Task.CompletedTask;
        }

        return this.ExecuteCommandAsync(() => DraftRoomApiService.AssignSeatAsync(room.Code, seatId, participantId));
    }

    private async Task AddSeatAsync()
    {
        if (Room is not { } room)
        {
            return;
        }

        await this.ExecuteCommandAsync(() => DraftRoomApiService.AddSeatAsync(room.Code));
    }

    private Task ClaimSeatAsync(Guid seatId) =>
        this.ExecuteSeatCommandAsync(seatId, DraftRoomApiService.ClaimSeatAsync);

    private Task LeaveSeatAsync(Guid seatId) =>
        this.ExecuteSeatCommandAsync(seatId, DraftRoomApiService.UnclaimSeatAsync);

    private Task UnclaimSeatAsync(Guid seatId) =>
        this.ExecuteSeatCommandAsync(seatId, DraftRoomApiService.UnclaimSeatAsync);

    private Task RemoveSeatAsync(Guid seatId) =>
        this.ExecuteSeatCommandAsync(seatId, DraftRoomApiService.RemoveSeatAsync);

    private async Task ExecuteSeatCommandAsync(
        Guid seatId,
        Func<string, Guid, CancellationToken, Task<DraftRoomApiCommandResult>> command)
    {
        if (Room is not { } room)
        {
            return;
        }

        await this.ExecuteCommandAsync(() => command(room.Code, seatId, CancellationToken.None));
    }

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

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.DraftRooms;

namespace TwilightImperiumUltimate.Web.Components.DraftRooms;

public partial class DraftRoomChatPanel : IAsyncDisposable
{
    private readonly CancellationTokenSource _lifetimeCancellation = new();
    private readonly List<ChatMessageDto> _messages = [];
    private ElementReference _messagesElement;
    private IJSObjectReference? _jsModule;
    private string _messageText = string.Empty;
    private string? _statusMessage;
    private bool _statusIsError;
    private bool _statusIsPolite;
    private bool _isLoading = true;
    private bool _isSending;
    private bool _historyError;
    private bool _historyReadyForAnnouncements;
    private bool _shouldScrollToBottom;
    private bool _disposed;

    [CascadingParameter]
    private IDraftRoomSession DraftRoomSession { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private ILogger<DraftRoomChatPanel> Logger { get; set; } = default!;

    public async ValueTask DisposeAsync()
    {
        if (this._disposed)
        {
            return;
        }

        this._disposed = true;
        this.DraftRoomSession.ChatMessageReceived -= this.HandleChatMessageReceivedAsync;
        await this._lifetimeCancellation.CancelAsync();
        this._lifetimeCancellation.Dispose();

        if (this._jsModule is not null)
        {
            try
            {
                await this._jsModule.DisposeAsync();
            }
            catch (JSDisconnectedException exception)
            {
                this.Logger.LogDebug(exception, "Draft room chat JavaScript module disconnected before disposal.");
            }
        }

        GC.SuppressFinalize(this);
    }

    protected override async Task OnInitializedAsync()
    {
        this.DraftRoomSession.ChatMessageReceived += this.HandleChatMessageReceivedAsync;

        try
        {
            var result = await this.DraftRoomSession
                .GetChatHistoryAsync(afterId: null, DraftRoomLimits.DefaultHistoryPageSize, this._lifetimeCancellation.Token);

            if (result.IsSuccess && result.Data is not null)
            {
                this.MergeMessages(result.Data);
                this._shouldScrollToBottom = true;
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

        if (!this._shouldScrollToBottom || this._disposed)
        {
            return;
        }

        this._shouldScrollToBottom = false;
        try
        {
            await this.EnsureJsModuleAsync();
            await this._jsModule!.InvokeVoidAsync("scrollToBottom", this._messagesElement);
        }
        catch (JSException exception)
        {
            this.Logger.LogWarning(exception, "Unable to scroll the draft room chat to its latest message.");
        }
    }

    private static string GetString(string resourceName)
    {
        return Strings.ResourceManager.GetString(resourceName, CultureInfo.CurrentUICulture)
            ?? throw new InvalidOperationException($"Missing draft-room chat resource '{resourceName}'.");
    }

    private static string GetMessageClass(ChatMessageDto message)
    {
        return message.Kind == ChatMessageKind.User
            ? "draft-room-chat-panel__message"
            : "draft-room-chat-panel__message draft-room-chat-panel__message--system";
    }

    private static string GetTimestamp(ChatMessageDto message)
    {
        return message.CreatedAtUtc.ToLocalTime().ToString("t", CultureInfo.CurrentCulture);
    }

    private static string GetTimestampIso(ChatMessageDto message)
    {
        return message.CreatedAtUtc.ToString("O", CultureInfo.InvariantCulture);
    }

    private void HandleMessageTextChanged(string value)
    {
        this._messageText = value;
    }

    private async Task HandleSubmitAsync()
    {
        var text = this._messageText.Trim();
        if (text.Length == 0 || this._isSending)
        {
            return;
        }

        this._isSending = true;
        this._statusMessage = null;
        this._statusIsError = false;
        this._statusIsPolite = false;

        try
        {
            var result = await this.DraftRoomSession
                .SendChatAsync(text, this._lifetimeCancellation.Token);

            if (result.IsSuccess && result.Data is not null)
            {
                this._messageText = string.Empty;
                await this.AddMessageAsync(result.Data);
                return;
            }

            this._statusMessage = result.Kind switch
            {
                DraftRoomApiResultKind.Throttled => GetString("DraftRooms_ChatThrottled"),
                DraftRoomApiResultKind.ValidationFailed => GetString("DraftRooms_ChatValidationFailed"),
                _ => GetString("DraftRooms_ChatSendFailed"),
            };
            this._statusIsError = true;
            this._statusIsPolite = result.Kind == DraftRoomApiResultKind.Throttled;
        }
        catch (OperationCanceledException) when (this._lifetimeCancellation.IsCancellationRequested)
        {
            return;
        }
        finally
        {
            this._isSending = false;
        }
    }

    private Task HandleChatMessageReceivedAsync(ChatMessageDto message)
    {
        if (this._disposed)
        {
            return Task.CompletedTask;
        }

        return this.InvokeAsync(() => this.AddMessageAsync(message));
    }

    private async Task AddMessageAsync(ChatMessageDto message)
    {
        if (this._disposed || this._messages.Any(item => item.Id == message.Id))
        {
            return;
        }

        var wasAtBottom = this._messages.Count == 0;
        if (!this._isLoading && this._jsModule is not null)
        {
            try
            {
                wasAtBottom = await this._jsModule.InvokeAsync<bool>("isAtBottom", this._messagesElement);
            }
            catch (JSException exception)
            {
                this.Logger.LogWarning(exception, "Unable to determine the draft room chat scroll position.");
            }
        }

        this.MergeMessages([message]);
        this._shouldScrollToBottom = wasAtBottom;
        this.StateHasChanged();
    }

    private void MergeMessages(IEnumerable<ChatMessageDto> messages)
    {
        var knownIds = this._messages.Select(item => item.Id).ToHashSet();
        foreach (var message in messages)
        {
            if (knownIds.Add(message.Id))
            {
                this._messages.Add(message);
            }
        }

        this._messages.Sort(static (left, right) => left.Id.CompareTo(right.Id));
        if (this._messages.Count > DraftRoomLimits.MaxHistoryPageSize)
        {
            this._messages.RemoveRange(0, this._messages.Count - DraftRoomLimits.MaxHistoryPageSize);
        }
    }

    private async Task EnsureJsModuleAsync()
    {
        this._jsModule ??= await this.JSRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            "./Components/DraftRooms/DraftRoomChatPanel.razor.js");
    }
}

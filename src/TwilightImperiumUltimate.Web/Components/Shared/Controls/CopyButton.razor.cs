using Microsoft.JSInterop;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class CopyButton : IAsyncDisposable
{
    private readonly CancellationTokenSource _lifetime = new();
    private IJSObjectReference? _module;
    private Task? _copyTask;
    private bool _showCopiedMessage;
    private bool _copying;
    private bool _failed;
    private bool _disposed;

    [Parameter]
    [EditorRequired]
    public string TextToCopy { get; set; } = string.Empty;

    [Parameter]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public string AriaLabel { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    private static string FailureText => Strings.ResourceManager.GetString("CopyButton_Failed", System.Globalization.CultureInfo.CurrentUICulture)
        ?? throw new InvalidOperationException("The clipboard error resource is missing.");

    [Inject]
    private ILogger<CopyButton> Logger { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private string ButtonLabel => _showCopiedMessage ? Strings.ButtonText_Copied : DisplayLabel;

    private string DisplayLabel => string.IsNullOrWhiteSpace(Label) ? Strings.ButtonText_Copy : Label;

    private Dictionary<string, object> ButtonAttributes => string.IsNullOrWhiteSpace(AriaLabel)
        ? []
        : new Dictionary<string, object> { ["aria-label"] = AriaLabel };

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        await _lifetime.CancelAsync();
        if (_copyTask is not null)
        {
            await _copyTask;
        }

        if (_module is not null)
        {
            await _module.DisposeAsync();
        }

        _lifetime.Dispose();
        GC.SuppressFinalize(this);
    }

    private Task CopyAsync()
    {
        if (_copying || _disposed)
        {
            return Task.CompletedTask;
        }

        _copyTask = this.CopyCoreAsync();
        return _copyTask;
    }

    private async Task CopyCoreAsync()
    {
        _copying = true;
        _failed = false;
        _showCopiedMessage = false;
        try
        {
            _module ??= await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import", _lifetime.Token, "./Components/Shared/Controls/CopyButton.razor.js");
            await _module.InvokeVoidAsync("copyToClipboard", _lifetime.Token, TextToCopy);
            _showCopiedMessage = true;
            this.StateHasChanged();
            await Task.Delay(1500, _lifetime.Token);
            _showCopiedMessage = false;
        }
        catch (OperationCanceledException exception) when (_disposed)
        {
            Logger.LogDebug(exception, "Clipboard copy was cancelled during disposal");
        }
        catch (JSException exception)
        {
            Logger.LogWarning(exception, "Clipboard copy was not completed");
            _failed = true;
        }
        finally
        {
            _copying = false;
            if (!_disposed)
            {
                this.StateHasChanged();
            }
        }
    }
}

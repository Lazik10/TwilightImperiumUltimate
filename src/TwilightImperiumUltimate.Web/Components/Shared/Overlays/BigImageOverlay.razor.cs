using Microsoft.AspNetCore.Components.Web;

namespace TwilightImperiumUltimate.Web.Components.Shared.Overlays;

/// <summary>
/// Reusable full-screen "big image" lightbox overlay. The host page/component owns the
/// open/closed state and current image source, and routes any element's click handler into
/// setting that state to show this overlay. Clicking the backdrop/image, or pressing Escape,
/// raises <see cref="OnClose"/> so the host can hide it again.
/// </summary>
public partial class BigImageOverlay
{
    private ElementReference _overlayElement;
    private bool _wasOpen;

    /// <summary>
    /// Gets or sets whether the overlay is currently shown.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Gets or sets the image source to display. When null, the overlay renders no built-in
    /// image and the caller is expected to render its own image/layout via <see cref="ChildContent"/>.
    /// </summary>
    [Parameter]
    public string? ImageSrc { get; set; }

    /// <summary>
    /// Gets or sets the accessible alt text for the displayed image.
    /// </summary>
    [Parameter]
    public string ImageAlt { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the accessible name announced for the overlay/dialog itself.
    /// </summary>
    [Parameter]
    public string AriaLabel { get; set; } = "Image preview";

    /// <summary>
    /// Gets or sets the callback invoked when the overlay should close (backdrop/image click or
    /// pressing Escape). The host is responsible for actually setting its "is open" state to false.
    /// </summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <summary>
    /// Gets or sets additional content rendered inside the overlay below the image (for example a
    /// language switcher or extra details/FAQ content).
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets additional CSS classes applied to the overlay's root element.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsOpen && !_wasOpen)
            await _overlayElement.FocusAsync();

        _wasOpen = IsOpen;
    }

    private Task HandleClose() => OnClose.InvokeAsync();

    private Task HandleKeyDown(KeyboardEventArgs e) =>
        e.Key == "Escape" ? HandleClose() : Task.CompletedTask;
}

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>Observes browser visibility without exposing room data to JavaScript.</summary>
public interface IPageVisibility : IAsyncDisposable
{
    /// <summary>Occurs when document visibility changes.</summary>
    event Action? Changed;

    /// <summary>Gets whether the browser document is hidden.</summary>
    bool IsHidden { get; }

    /// <summary>Starts observing and reads the current visibility.</summary>
    Task InitializeAsync();
}

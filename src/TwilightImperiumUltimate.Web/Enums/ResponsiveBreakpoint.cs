namespace TwilightImperiumUltimate.Web.Enums;

/// <summary>
/// Viewport breakpoint, matching the breakpoints used by <c>ResponsiveGridContainer</c>
/// (tablet: max-width 1024px, mobile: max-width 768px).
/// </summary>
public enum ResponsiveBreakpoint
{
    /// <summary>
    /// Default/widest layout, applied when no narrower breakpoint matches.
    /// </summary>
    Desktop,

    /// <summary>
    /// Layout applied at tablet widths (max-width 1024px).
    /// </summary>
    Tablet,

    /// <summary>
    /// Layout applied at mobile widths (max-width 768px).
    /// </summary>
    Mobile,
}

namespace TwilightImperiumUltimate.Web.Enums;

/// <summary>
/// Container type for ResponsiveContainer.
/// </summary>
public enum ContainerType
{
    /// <summary>
    /// Fluid container (default); full width within parent constraints.
    /// </summary>
    Fluid,

    /// <summary>
    /// Centered container; fixed max-width, centered within viewport.
    /// </summary>
    Centered,

    /// <summary>
    /// Narrow container; narrower than fluid.
    /// </summary>
    Narrow,

    /// <summary>
    /// Wide container; wider than fluid but not full-width.
    /// </summary>
    Wide,

    /// <summary>
    /// Flex container; uses display: flex for flex layout of children.
    /// </summary>
    Flex,
}

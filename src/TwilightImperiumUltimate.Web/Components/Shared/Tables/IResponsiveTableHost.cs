namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Implemented by the table/grid host component (currently <c>ResponsiveTable{TItem}</c>)
/// so cascaded <c>ResponsiveTableColumn{TItem}</c> children can
/// register themselves without either side needing to know the other's concrete type.
/// </summary>
/// <typeparam name="TItem">The row model type.</typeparam>
internal interface IResponsiveTableHost<TItem>
{
    /// <summary>
    /// Registers a column with the host. Called once by each <c>ResponsiveTableColumn</c> when it
    /// initializes.
    /// </summary>
    void AddColumn(ResponsiveTableColumn<TItem> column);

    /// <summary>
    /// Unregisters a column from the host. Called when a conditionally-rendered column is removed.
    /// </summary>
    void RemoveColumn(ResponsiveTableColumn<TItem> column);
}

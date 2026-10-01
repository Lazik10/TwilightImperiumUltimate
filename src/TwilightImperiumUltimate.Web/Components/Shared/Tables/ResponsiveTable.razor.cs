namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// A fully responsive, filterable HTML table bound to a model list (<see cref="ResponsiveDataViewBase{TItem}.Items"/>).
/// Declare columns with nested <see cref="ResponsiveTableColumn{TItem}"/> elements.
///
/// Supports: text/value/boolean/enum column filters (rendered in a dedicated filter row below the
/// column headers), optional horizontal scrolling with sticky columns, a sticky header row,
/// zebra-striped rows, vertical column headers, and responsive font sizing (desktop/tablet/mobile).
///
/// Does not sort. For sortable columns, row selection, and Radzen-based grids, see
/// <see cref="TwilightImperiumUltimate.Web.Components.Grid.ResponsiveRadzenDataGrid{TItem}"/> instead.
/// </summary>
/// <typeparam name="TItem">The row model type.</typeparam>
[CascadingTypeParameter(nameof(TItem))]
public partial class ResponsiveTable<TItem>
{
}

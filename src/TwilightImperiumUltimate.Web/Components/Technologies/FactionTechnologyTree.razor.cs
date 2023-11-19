using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Technologies;

namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class FactionTechnologyTree
{
    [Parameter]
    public IReadOnlyCollection<Technology> Technologies { get; set; } = Array.Empty<Technology>();
}

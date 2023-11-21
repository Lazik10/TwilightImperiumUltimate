using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionSystemInfo
{
    [Parameter]
    public FactionName FactionName { get; set; } = default!;
}

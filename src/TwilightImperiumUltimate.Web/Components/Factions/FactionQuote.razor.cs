using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionQuote
{
    [Parameter]
    public FactionName FactionName { get; set; } = default!;
}

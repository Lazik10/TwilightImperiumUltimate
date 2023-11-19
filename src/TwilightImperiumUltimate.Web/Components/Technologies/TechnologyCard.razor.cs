using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.Technologies;

public partial class TechnologyCard
{
    [Parameter]
    public string ImagePath { get; set; } = string.Empty;
}

using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Parameters;

public sealed class UpdateTiglParameterRequest
{
    public TiglParameterName Name { get; set; }

    public bool Enabled { get; set; }
}

using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class UpdateRoleRemovalSettingsCommand(
    UpdateRoleRemovalSettingsRequest request)
    : IRequest<NewTiglUserResponse>
{
    public long DiscordId { get; set; } = request.DiscordId;

    public bool RemoveOldStandardRole { get; set; } = request.RemoveOldStandardRole;

    public bool RemoveOldFracturedRole { get; set; } = request.RemoveOldFracturedRole;
}

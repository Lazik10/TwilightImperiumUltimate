namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;

public class UpdateRoleRemovalSettingsRequest
{
    public long DiscordId { get; set; }

    public bool RemoveOldStandardRole { get; set; }

    public bool RemoveOldFracturedRole { get; set; }
}

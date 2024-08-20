namespace TwilightImperiumUltimate.Contracts.ApiContracts.User;

public class AddClaimRequest
{
    public required string UserId { get; set; }

    public required string ClaimType { get; set; }

    public required string ClaimValue { get; set; }
}
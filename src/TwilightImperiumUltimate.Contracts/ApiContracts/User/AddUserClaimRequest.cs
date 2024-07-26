namespace TwilightImperiumUltimate.Contracts.ApiContracts.User;

public class AddUserClaimRequest
{
    required public string UserId { get; set; }

    required public string ClaimType { get; set; }

    required public string ClaimValue { get; set; }
}
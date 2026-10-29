using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class UpdateRoleRemovalSettingsCommandHandler(
    ITiglUserRepository tiglUserRepository)
    : IRequestHandler<UpdateRoleRemovalSettingsCommand, NewTiglUserResponse>
{
    public async Task<NewTiglUserResponse> Handle(UpdateRoleRemovalSettingsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var result = await tiglUserRepository.UpdateRoleRemovalSettings(
            request.DiscordId,
            request.RemoveOldStandardRole,
            request.RemoveOldFracturedRole,
            cancellationToken);

        if (result.IsFailed)
        {
            return new NewTiglUserResponse()
            {
                Success = false,
                ErrorTitle = "Failed to update role removal settings",
                ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Message)),
            };
        }

        return new NewTiglUserResponse { Success = true };
    }
}

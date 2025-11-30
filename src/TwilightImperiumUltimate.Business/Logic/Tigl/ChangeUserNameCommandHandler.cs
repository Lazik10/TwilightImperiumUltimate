using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class ChangeUserNameCommandHandler(
    ITiglUserRepository tiglUserRepository)
    : IRequestHandler<ChangeUserNameCommand, NewTiglUserResponse>
{
    private readonly int minimumUserNameLength = 3;
    private readonly int minimumDaysForUserNameChange = 30;

    public async Task<NewTiglUserResponse> Handle(ChangeUserNameCommand request, CancellationToken cancellationToken)
    {
        var tiglUser = await tiglUserRepository.GetTiglUserByDiscordId(request.DiscordId, cancellationToken);
        if (tiglUser is null)
        {
            return new NewTiglUserResponse() { Success = false, ErrorTitle = "User not found", ErrorMessage = "The specified user does not exist in the TIGL system." };
        }

        if (string.IsNullOrWhiteSpace(request.NewTiglUserName) || request.NewTiglUserName.Length < minimumUserNameLength)
        {
            return new NewTiglUserResponse() { Success = false, ErrorTitle = "Invalid User Name", ErrorMessage = "The new TIGL user name cannot be empty or have less than 3 characters." };
        }

        if (!tiglUser.TiglUserNameChanged || tiglUser.LastUserNameChange.AddDays(minimumDaysForUserNameChange).ToDateTime(TimeOnly.MinValue) <= DateTime.UtcNow)
        {
            var result = await tiglUserRepository.ChangeTiglUserName(
                request.DiscordId,
                request.NewTiglUserName,
                cancellationToken);

            if (result.IsFailed)
            {
                return new NewTiglUserResponse() { Success = false, ErrorTitle = "Change Failed", ErrorMessage = "Username is already taken." };
            }
        }
        else
        {
            return new NewTiglUserResponse() { Success = false, ErrorTitle = "Username was changed during previous 30 days.", ErrorMessage = $"Last change: {tiglUser.LastUserNameChange}" };
        }

        return new NewTiglUserResponse { Success = true };
    }
}

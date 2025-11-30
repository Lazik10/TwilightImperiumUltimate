namespace TwilightImperiumUltimate.Contracts.Enums;

public enum DiscordRoleChangeStatus
{
    Success = 0,
    UserNotFound = 1,
    UpdatedUserNotFound = 2,
    RoleNotFound = 3,
    FailedToAddRole = 4,
    FailedToRemoveRole = 5,
    AlreadyHasRole = 6,
    DoesNotHaveRole = 7,
    GuildNotFound = 8,
    UnknownError = 9,
}

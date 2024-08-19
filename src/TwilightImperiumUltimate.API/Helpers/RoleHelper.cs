namespace TwilightImperiumUltimate.API.Helpers;

public static class RoleHelper
{
    public static IReadOnlyCollection<UserRole> GetValidUserRoles(IReadOnlyCollection<string> roles)
    {
        var userRoles = new List<UserRole>();

        foreach (var role in roles)
        {
            if (Enum.TryParse<UserRole>(role, false, out var userRole))
            {
                userRoles.Add(userRole);
            }
        }

        return userRoles;
    }
}
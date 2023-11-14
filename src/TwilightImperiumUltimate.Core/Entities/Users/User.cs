using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Users;

public class User : IEntity
{
    public int Id { get; set; }

    public string Nickname { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public int? Age { get; set; }

    public string Email { get; set; } = string.Empty;
}

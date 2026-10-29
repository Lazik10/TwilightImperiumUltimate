using System.Text;
using TwilightImperiumUltimate.Contracts.DraftRooms;

namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Applies the display-name rules used by draft-room guests.
/// </summary>
public sealed class DraftRoomDisplayNameValidator(
    IUserRepository userRepository)
    : IDraftRoomDisplayNameValidator
{
    private readonly IUserRepository _userRepository = userRepository
        ?? throw new ArgumentNullException(nameof(userRepository));

    /// <inheritdoc />
    public string Normalize(string displayName)
    {
        ArgumentNullException.ThrowIfNull(displayName);

        var trimmedDisplayName = displayName.Trim(' ');

        return string.Join(' ', trimmedDisplayName.Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    /// <inheritdoc />
    public string MakeKey(string displayName)
    {
        return this.Normalize(displayName).ToUpperInvariant();
    }

    /// <inheritdoc />
    public async Task<bool> ValidateGuestName(string displayName)
    {
        ArgumentNullException.ThrowIfNull(displayName);

        if (displayName.Any(char.IsControl))
        {
            return false;
        }

        var normalizedDisplayName = this.Normalize(displayName);
        var displayNameLength = normalizedDisplayName.Length;

        if (displayNameLength < DraftRoomLimits.MinDisplayNameLength
            || displayNameLength > DraftRoomLimits.MaxDisplayNameLength
            || !IsAllowedDisplayName(normalizedDisplayName))
        {
            return false;
        }

        var registeredUser = await this._userRepository
            .GetUserByUserName(normalizedDisplayName)
            .ConfigureAwait(false);

        return registeredUser is null
            || !string.Equals(
                registeredUser.UserName,
                normalizedDisplayName,
                StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsAllowedDisplayName(string displayName)
    {
        foreach (var rune in displayName.EnumerateRunes())
        {
            if (Rune.IsLetterOrDigit(rune)
                || rune.Value is ' ' or '_' or '-' or '.' or '\'')
            {
                continue;
            }

            return false;
        }

        return true;
    }
}

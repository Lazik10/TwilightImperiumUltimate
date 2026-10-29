using System.Security.Claims;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;

namespace TwilightImperiumUltimate.API.Helpers;

/// <summary>
/// Resolves account or guest identity information from an API request.
/// </summary>
public static class DraftRoomCallerResolver
{
    private const string ParticipantIdHeader = "X-Draft-Participant-Id";
    private const string ParticipantSecretHeader = "X-Draft-Participant-Secret";

    /// <summary>
    /// Resolves the draft-room caller from authenticated claims or guest headers.
    /// </summary>
    /// <param name="user">The request's claims principal.</param>
    /// <param name="headers">The request headers.</param>
    /// <returns>The resolved caller, or <see langword="null"/> if identity data is incomplete or invalid.</returns>
    public static DraftRoomCaller? Resolve(ClaimsPrincipal user, IHeaderDictionary headers)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(headers);

        if (HasGuestIdentityHeaders(headers))
        {
            return ResolveGuest(headers);
        }

        return ResolveAccount(user) ?? new DraftRoomCaller(null, null, null, null);
    }

    /// <summary>
    /// Resolves an account-link request, requiring both an authenticated account and valid guest headers.
    /// </summary>
    /// <param name="user">The request's claims principal.</param>
    /// <param name="headers">The request headers.</param>
    /// <returns>A caller containing both identities, or <see langword="null"/> if either proof is missing or invalid.</returns>
    public static DraftRoomCaller? ResolveForAccountLink(ClaimsPrincipal user, IHeaderDictionary headers)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(headers);

        var account = ResolveAccount(user);
        var guest = ResolveGuest(headers);
        if (account is null || guest is null)
        {
            return null;
        }

        return account with
        {
            GuestParticipantId = guest.GuestParticipantId,
            GuestSecret = guest.GuestSecret,
        };
    }

    private static bool HasGuestIdentityHeaders(IHeaderDictionary headers)
    {
        return headers.ContainsKey(ParticipantIdHeader)
            || headers.ContainsKey(ParticipantSecretHeader);
    }

    private static DraftRoomCaller? ResolveAccount(ClaimsPrincipal user)
    {
        if (user.Identity?.IsAuthenticated is not true)
        {
            return null;
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = user.FindFirst(ClaimTypes.Name)?.Value;

        return string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(userName)
            ? null
            : new DraftRoomCaller(userId, userName, null, null);
    }

    private static DraftRoomCaller? ResolveGuest(IHeaderDictionary headers)
    {
        if (!headers.TryGetValue(ParticipantIdHeader, out var participantIdValues)
            || !headers.TryGetValue(ParticipantSecretHeader, out var participantSecretValues)
            || participantIdValues.Count != 1
            || participantSecretValues.Count != 1
            || !Guid.TryParse(participantIdValues[0], out var participantId)
            || participantId == Guid.Empty
            || string.IsNullOrWhiteSpace(participantSecretValues[0]))
        {
            return null;
        }

        return new DraftRoomCaller(null, null, participantId, participantSecretValues[0]);
    }
}

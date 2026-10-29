using System.Text.Json;
using Blazored.LocalStorage;
using TwilightImperiumUltimate.Web.Models.DraftRooms;

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Persists guest room identities in browser local storage.
/// </summary>
public sealed class GuestIdentityStore(
    ILocalStorageService localStorage,
    ILogger<GuestIdentityStore> logger)
    : IGuestIdentityStore
{
    private const string StorageKey = "draftRooms.guests.v1";

    public async Task<GuestIdentity?> GetAsync(string code, CancellationToken cancellationToken = default)
    {
        var identities = await ReadAsync(cancellationToken).ConfigureAwait(false);
        return identities.GetValueOrDefault(NormalizeCode(code));
    }

    public async Task SaveAsync(GuestIdentity identity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identity);

        var code = NormalizeCode(identity.Code);
        var identities = await ReadAsync(cancellationToken).ConfigureAwait(false);
        identities[code] = identity with { Code = code };

        await localStorage.SetItemAsync(StorageKey, identities, cancellationToken).ConfigureAwait(false);
    }

    public async Task RemoveAsync(string code, CancellationToken cancellationToken = default)
    {
        var identities = await ReadAsync(cancellationToken).ConfigureAwait(false);
        if (identities.Remove(NormalizeCode(code)))
        {
            await localStorage.SetItemAsync(StorageKey, identities, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task<IReadOnlyList<GuestIdentity>> ListAsync(CancellationToken cancellationToken = default)
    {
        var identities = await ReadAsync(cancellationToken).ConfigureAwait(false);
        return identities.Values.ToArray();
    }

    private static string NormalizeCode(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        return code.Trim().ToUpperInvariant();
    }

    private async Task<Dictionary<string, GuestIdentity>> ReadAsync(CancellationToken cancellationToken)
    {
        Dictionary<string, GuestIdentity>? storedIdentities;
        try
        {
            storedIdentities = await localStorage
                .GetItemAsync<Dictionary<string, GuestIdentity>>(StorageKey, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (JsonException exception)
        {
            logger.LogWarning(exception, "Malformed draft-room guest identities were discarded from browser storage.");
            return new Dictionary<string, GuestIdentity>(StringComparer.OrdinalIgnoreCase);
        }

        var identities = new Dictionary<string, GuestIdentity>(StringComparer.OrdinalIgnoreCase);
        if (storedIdentities is null)
        {
            return identities;
        }

        foreach (var (storedCode, identity) in storedIdentities)
        {
            if (identity is not null && !string.IsNullOrWhiteSpace(storedCode))
            {
                var code = NormalizeCode(storedCode);
                identities[code] = identity with { Code = NormalizeCode(identity.Code) };
            }
        }

        return identities;
    }
}

using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using TwilightImperiumUltimate.API.Helpers;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftRoomCallerResolverTests
{
    [Fact]
    public void Resolve_WhenUserIsAuthenticated_ShouldUseAccountClaims()
    {
        var principal = CreateAuthenticatedPrincipal("account-id", "AccountName");

        var caller = DraftRoomCallerResolver.Resolve(principal, new HeaderDictionary());

        caller.Should().Be(new DraftRoomCaller("account-id", "AccountName", null, null));
    }

    [Fact]
    public void Resolve_WhenAuthenticatedUserAlsoHasGuestHeaders_ShouldPreferGuestIdentity()
    {
        var participantId = Guid.NewGuid();
        var headers = CreateGuestHeaders(participantId);

        var caller = DraftRoomCallerResolver.Resolve(
            CreateAuthenticatedPrincipal("account-id", "AccountName"),
            headers);

        caller.Should().Be(new DraftRoomCaller(null, null, participantId, "guest-secret"));
    }

    [Fact]
    public void ResolveForAccountLink_WhenBothIdentitiesArePresent_ShouldReturnCombinedCaller()
    {
        var participantId = Guid.NewGuid();
        var headers = CreateGuestHeaders(participantId);

        var caller = DraftRoomCallerResolver.ResolveForAccountLink(
            CreateAuthenticatedPrincipal("account-id", "AccountName"),
            headers);

        caller.Should().Be(new DraftRoomCaller(
            "account-id",
            "AccountName",
            participantId,
            "guest-secret"));
    }

    [Fact]
    public void ResolveForAccountLink_WhenOnlyGuestIdentityIsPresent_ShouldReturnNull()
    {
        var caller = DraftRoomCallerResolver.ResolveForAccountLink(
            new ClaimsPrincipal(),
            CreateGuestHeaders(Guid.NewGuid()));

        caller.Should().BeNull();
    }

    [Fact]
    public void ResolveForAccountLink_WhenOnlyAccountIdentityIsPresent_ShouldReturnNull()
    {
        var caller = DraftRoomCallerResolver.ResolveForAccountLink(
            CreateAuthenticatedPrincipal("account-id", "AccountName"),
            new HeaderDictionary());

        caller.Should().BeNull();
    }

    [Fact]
    public void Resolve_WhenGuestHeadersAreValid_ShouldUseGuestIdentity()
    {
        var participantId = Guid.NewGuid();
        var headers = new HeaderDictionary
        {
            ["X-Draft-Participant-Id"] = participantId.ToString(),
            ["X-Draft-Participant-Secret"] = "guest-secret",
        };

        var caller = DraftRoomCallerResolver.Resolve(new ClaimsPrincipal(), headers);

        caller.Should().Be(new DraftRoomCaller(null, null, participantId, "guest-secret"));
    }

    [Fact]
    public void Resolve_WhenNoIdentityHeadersArePresent_ShouldReturnNewGuestCaller()
    {
        var caller = DraftRoomCallerResolver.Resolve(new ClaimsPrincipal(), new HeaderDictionary());

        caller.Should().Be(new DraftRoomCaller(null, null, null, null));
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void Resolve_WhenOnlyOneGuestHeaderIsPresent_ShouldReturnNull(
        bool includeParticipantId,
        bool includeParticipantSecret)
    {
        var headers = new HeaderDictionary();
        if (includeParticipantId)
        {
            headers["X-Draft-Participant-Id"] = Guid.NewGuid().ToString();
        }

        if (includeParticipantSecret)
        {
            headers["X-Draft-Participant-Secret"] = "guest-secret";
        }

        var caller = DraftRoomCallerResolver.Resolve(new ClaimsPrincipal(), headers);

        caller.Should().BeNull();
    }

    [Fact]
    public void Resolve_WhenAuthenticatedUserClaimsAreIncompleteAndGuestProofIsValid_ShouldUseGuest()
    {
        var principal = CreateAuthenticatedPrincipal("account-id", null);
        var participantId = Guid.NewGuid();
        var headers = new HeaderDictionary
        {
            ["X-Draft-Participant-Id"] = participantId.ToString(),
            ["X-Draft-Participant-Secret"] = "guest-secret",
        };

        var caller = DraftRoomCallerResolver.Resolve(principal, headers);

        caller.Should().Be(new DraftRoomCaller(null, null, participantId, "guest-secret"));
    }

    private static ClaimsPrincipal CreateAuthenticatedPrincipal(string userId, string? userName)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
        };
        if (userName is not null)
        {
            claims.Add(new Claim(ClaimTypes.Name, userName));
        }

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
    }

    private static HeaderDictionary CreateGuestHeaders(Guid participantId)
    {
        return new HeaderDictionary
        {
            ["X-Draft-Participant-Id"] = participantId.ToString(),
            ["X-Draft-Participant-Secret"] = "guest-secret",
        };
    }
}

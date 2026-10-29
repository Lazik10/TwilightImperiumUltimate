using FluentAssertions;
using Moq;
using TwilightImperiumUltimate.Business.Logic.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.Repositories;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftResultHistoryTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    public async Task History_WhenPageIsInvalid_ShouldRefuseBeforePersistence(int page)
    {
        var repository = new Mock<IDraftResultRepository>(MockBehavior.Strict);

        var response = await new GetMyResultsQueryHandler(repository.Object).Handle(new GetMyResultsQuery("account", page), CancellationToken.None);

        response.Errors.OfType<DraftRoomError>().Single().Code.Should().Be(DraftRoomErrorCode.InvalidHistoryCursor);
        repository.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(20, false)]
    [InlineData(21, true)]
    public async Task History_WhenPageHasExtraRow_ShouldReturnTwentyItemsAndMoreFlag(int count, bool hasMore)
    {
        var items = Enumerable.Range(1, count).Select(index => new DraftResultHistoryItemDto(
            Guid.NewGuid(), $"token-{index}", DraftTypeId.FactionPick, $"Room {index}", DateTime.UnixEpoch, false)).ToArray();
        var repository = new Mock<IDraftResultRepository>(MockBehavior.Strict);
        repository.Setup(service => service.GetHistoryAsync("account", 20, 21, It.IsAny<CancellationToken>())).ReturnsAsync(items);

        var response = await new GetMyResultsQueryHandler(repository.Object).Handle(new GetMyResultsQuery("account", 2), CancellationToken.None);

        response.Value.Items.Should().Equal(items.Take(20));
        response.Value.Page.Should().Be(2);
        response.Value.HasMore.Should().Be(hasMore);
        repository.VerifyAll();
    }

    [Theory]
    [InlineData(typeof(DraftResultDto))]
    [InlineData(typeof(DraftResultHistoryDto))]
    [InlineData(typeof(DraftResultHistoryItemDto))]
    [InlineData(typeof(FactionPickResultDto))]
    [InlineData(typeof(FactionPickResultSeatDto))]
    [InlineData(typeof(FactionPickResultPickDto))]
    [InlineData(typeof(FactionPickResultBanDto))]
    public void PublicContracts_WhenInspected_ShouldNotExposePrivateIdentityOrCredentialMembers(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        var forbidden = new[] { "UserId", "ParticipantId", "SourceRoomId", "Secret", "Password", "Chat", "Log", "Pending" };

        type.GetProperties().Select(property => property.Name).Should()
            .NotContain(name => forbidden.Any(value => name.Contains(value, StringComparison.OrdinalIgnoreCase)));
    }
}

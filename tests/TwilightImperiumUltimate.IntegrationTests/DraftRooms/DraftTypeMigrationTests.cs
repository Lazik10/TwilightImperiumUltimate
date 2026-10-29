using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.IntegrationTests.Infrastructure;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftTypeMigrationTests
{
    [Fact]
    public async Task Migrate_WhenLegacyRoomHasNoType_ShouldBackfillDefaultsAndSupportRollbackReapply()
    {
        await using var database = await TestDatabase.CreateAsync(useMigrations: true);
        await using var context = await database.ContextFactory.CreateDbContextAsync();
        var migrator = context.GetService<IMigrator>();
        await migrator.MigrateAsync("20261017052423_AddFactionPickSettingsAndState");
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = "LEGACY01",
            Name = "Existing legacy room",
            PasswordHash = "test-hash",
            DraftType = string.Empty,
            CreatedAtUtc = DateTime.UtcNow,
            LastActivityUtc = DateTime.UtcNow,
        };
        context.DraftRooms.Add(room);
        await context.SaveChangesAsync();
        var customizedRoom = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = "CUSTOM01",
            Name = "Existing customized room",
            PasswordHash = "test-hash",
            CreatedAtUtc = room.CreatedAtUtc,
            LastActivityUtc = room.LastActivityUtc,
        };
        context.DraftRooms.Add(customizedRoom);
        await context.SaveChangesAsync();
        await context.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO [Draft].[FactionPickSettings]
                ([RoomId], [GameVersions], [BansMode], [BansPerPlayer], [HostBanCount], [PickOrderMode], [PicksPerPlayer], [Snake])
            VALUES ({customizedRoom.Id}, '[0,7]', 'None', 1, 0, 'Random', 4, 0)
            """);

        await migrator.MigrateAsync();
        context.ChangeTracker.Clear();
        var settings = await context.FactionPickSettings.SingleAsync(item => item.RoomId == room.Id);
        settings.GameVersions.Should().Equal(new FactionPickSettingsDto().GameVersions);
        settings.PicksPerPlayer.Should().Be(1);
        var customized = await context.FactionPickSettings.SingleAsync(item => item.RoomId == customizedRoom.Id);
        customized.GameVersions.Should().Equal(GameVersion.BaseGame, GameVersion.ThundersEdge);
        customized.PicksPerPlayer.Should().Be(4);
        customized.Snake.Should().BeFalse();
        (await context.DraftRooms.SingleAsync(item => item.Id == room.Id)).DraftType.Should().Be(nameof(DraftTypeId.FactionPick));

        settings.PicksPerPlayer = 3;
        await context.SaveChangesAsync();
        await migrator.MigrateAsync("20261017052423_AddFactionPickSettingsAndState");
        await migrator.MigrateAsync();
        context.ChangeTracker.Clear();
        (await context.FactionPickSettings.SingleAsync(item => item.RoomId == room.Id)).PicksPerPlayer.Should().Be(3);
        (await context.FactionPickSettings.CountAsync(item => item.RoomId == room.Id)).Should().Be(1);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TwilightImperiumUltimate.Core.Entities.DraftRooms;
using TwilightImperiumUltimate.IntegrationTests.Infrastructure;

namespace TwilightImperiumUltimate.IntegrationTests.DraftRooms;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class FactionPickMigrationTests
{
    [Fact]
    public async Task Migrate_WhenConfiguredOrderIsRolledBackAndReapplied_ShouldBackfillEmptyOrderAndPreserveOtherRules()
    {
        await using var database = await TestDatabase.CreateAsync(useMigrations: true);
        await using var context = await database.ContextFactory.CreateDbContextAsync();
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = "MIGRATE3",
            Name = "Configured order migration",
            PasswordHash = "test-hash",
            CreatedAtUtc = DateTime.UtcNow,
            LastActivityUtc = DateTime.UtcNow,
        };
        context.DraftRooms.Add(room);
        context.FactionPickSettings.Add(new FactionPickSettings { RoomId = room.Id, HostBanCount = 2, SeatOrder = [Guid.NewGuid()] });
        await context.SaveChangesAsync();
        var migrator = context.GetService<IMigrator>();

        await migrator.MigrateAsync("20261017064715_AddFactionPickSelections");
        await migrator.MigrateAsync();
        context.ChangeTracker.Clear();

        var settings = await context.FactionPickSettings.SingleAsync(item => item.RoomId == room.Id);
        settings.SeatOrder.Should().BeEmpty();
        settings.HostBanCount.Should().Be(2);
        var order = new[] { Guid.NewGuid(), Guid.NewGuid() };
        settings.SeatOrder = order.ToList();
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
        (await context.FactionPickSettings.SingleAsync(item => item.RoomId == room.Id)).SeatOrder.Should().Equal(order);
    }

    [Fact]
    public async Task Migrate_WhenSelectionsMigrationIsRolledBackAndReapplied_ShouldKeepRulesAndRecreateChoices()
    {
        await using var database = await TestDatabase.CreateAsync(useMigrations: true);
        await using var context = await database.ContextFactory.CreateDbContextAsync();
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = "MIGRATE2",
            Name = "Selections migration",
            PasswordHash = "test-hash",
            CreatedAtUtc = DateTime.UtcNow,
            LastActivityUtc = DateTime.UtcNow,
        };
        context.DraftRooms.Add(room);
        context.FactionPickSettings.Add(new FactionPickSettings { RoomId = room.Id, HostBanCount = 2 });
        await context.SaveChangesAsync();
        var migrator = context.GetService<IMigrator>();

        await migrator.MigrateAsync("20261017061907_AddDraftSeatReclaimHistory");
        await migrator.MigrateAsync();
        context.ChangeTracker.Clear();
        context.FactionPickSelections.Add(new FactionPickSelection
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            CreatedAtUtc = DateTime.UtcNow,
        });
        await context.SaveChangesAsync();

        (await context.FactionPickSettings.SingleAsync(item => item.RoomId == room.Id)).HostBanCount.Should().Be(2);
        (await context.FactionPickSelections.CountAsync(item => item.RoomId == room.Id)).Should().Be(1);
    }

    [Fact]
    public async Task Migrate_WhenFactionPickMigrationIsRolledBackAndReapplied_ShouldPreserveExistingRoom()
    {
        await using var database = await TestDatabase.CreateAsync(useMigrations: true);
        await using var context = await database.ContextFactory.CreateDbContextAsync();
        var room = new DraftRoom
        {
            Id = Guid.NewGuid(),
            Code = "MIGRATE1",
            Name = "Migration test",
            PasswordHash = "test-hash",
            CreatedAtUtc = DateTime.UtcNow,
            LastActivityUtc = DateTime.UtcNow,
        };
        context.DraftRooms.Add(room);
        context.FactionPickSettings.Add(new FactionPickSettings { RoomId = room.Id });
        await context.SaveChangesAsync();

        var migrator = context.GetService<IMigrator>();
        await migrator.MigrateAsync("20261016135916_AddDraftRoomChatAndLog");
        var rolledBack = await context.Database.GetAppliedMigrationsAsync();
        rolledBack.Should().NotContain("20261017052423_AddFactionPickSettingsAndState");

        await migrator.MigrateAsync("20261017052423_AddFactionPickSettingsAndState");
        context.ChangeTracker.Clear();
        (await context.FactionPickSettings.AnyAsync()).Should().BeFalse();
        await migrator.MigrateAsync();
        context.ChangeTracker.Clear();
        (await context.DraftRooms.AnyAsync(item => item.Id == room.Id)).Should().BeTrue();
        (await context.FactionPickSettings.CountAsync()).Should().Be(1);
    }
}

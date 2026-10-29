using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Draft;
using TwilightImperiumUltimate.Draft.DraftRooms.Engine;
using Xunit;

namespace TwilightImperiumUltimate.Tests.DraftRooms.Engine;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test names follow the repository's required behavior naming convention.")]
public sealed class DraftTypeRegistryTests
{
    [Fact]
    public void Get_WhenTypeIsRegistered_ShouldReturnTheSameModuleAsAll()
    {
        var module = CreateModule();
        var registry = new DraftTypeRegistry([module]);

        registry.Get(DraftTypeId.FactionPick).Should().BeSameAs(module);
        registry.All.Should().ContainSingle().Which.Should().BeSameAs(module);
    }

    [Fact]
    public void Get_WhenTypeIsUnknown_ShouldFailClearly()
    {
        var registry = new DraftTypeRegistry([CreateModule()]);

        var resolve = () => registry.Get((DraftTypeId)999);

        resolve.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("id");
    }

    [Fact]
    public void Get_WhenKnownTypeHasNoModule_ShouldFailRatherThanReturnFallback()
    {
        var registry = new DraftTypeRegistry([]);

        var resolve = () => registry.Get(DraftTypeId.FactionPick);

        resolve.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("id");
    }

    [Fact]
    public void Constructor_WhenModuleIsRegisteredTwice_ShouldRejectAmbiguousConfiguration()
    {
        var create = () => new DraftTypeRegistry([CreateModule(), CreateModule()]);

        create.Should().Throw<ArgumentException>().WithParameterName("modules");
    }

    [Fact]
    public void RegisterDraftServices_WhenModulesAreProvided_ShouldResolveRegistryAndRandomness()
    {
        var services = new ServiceCollection();
        services.RegisterDraftServices();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var registry = scope.ServiceProvider.GetRequiredService<IDraftTypeRegistry>();
        var randomness = scope.ServiceProvider.GetRequiredService<IRandomnessSource>();

        registry.Get(DraftTypeId.FactionPick).Should().NotBeNull();
        randomness.Should().BeOfType<CryptoRandomnessSource>();
    }

    private static DraftTypeModule<FactionPickSettingsDto, FakeDraftState> CreateModule() =>
        new DraftTypeModule<FactionPickSettingsDto, FakeDraftState>(new FakeDraftType());
}

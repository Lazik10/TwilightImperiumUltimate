using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Builds a fixed registry of the configured engine modules and refuses duplicate registrations.
/// </summary>
public sealed class DraftTypeRegistry : IDraftTypeRegistry
{
    private readonly Dictionary<DraftTypeId, IDraftTypeModule> _modules;

    /// <summary>
    /// Initializes a new instance of the <see cref="DraftTypeRegistry"/> class.
    /// </summary>
    public DraftTypeRegistry(IEnumerable<IDraftTypeModule> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);
        var registry = new Dictionary<DraftTypeId, IDraftTypeModule>();
        foreach (var module in modules)
        {
            ArgumentNullException.ThrowIfNull(module);
            if (!Enum.IsDefined(module.Id))
            {
                throw new ArgumentException("A draft module has an unknown type identifier.", nameof(modules));
            }

            if (!registry.TryAdd(module.Id, module))
            {
                throw new ArgumentException($"The draft type '{module.Id}' is registered more than once.", nameof(modules));
            }
        }

        _modules = registry;
        All = Array.AsReadOnly(registry.Values.OrderBy(module => module.Id).ToArray());
    }

    /// <inheritdoc />
    public IReadOnlyList<IDraftTypeModule> All { get; }

    /// <inheritdoc />
    public IDraftTypeModule Get(DraftTypeId id)
    {
        return _modules.TryGetValue(id, out var module)
            ? module
            : throw new ArgumentOutOfRangeException(nameof(id), id, "The draft type is not registered.");
    }
}

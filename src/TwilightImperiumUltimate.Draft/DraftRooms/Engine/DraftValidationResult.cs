namespace TwilightImperiumUltimate.Draft.DraftRooms.Engine;

/// <summary>
/// Contains stable rule codes for application-level error mapping and localized UI messages.
/// </summary>
public sealed record DraftValidationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DraftValidationResult"/> class.
    /// </summary>
    public DraftValidationResult(IEnumerable<string> errorCodes)
    {
        ArgumentNullException.ThrowIfNull(errorCodes);
        ErrorCodes = Array.AsReadOnly(errorCodes.Distinct(StringComparer.Ordinal).ToArray());
    }

    /// <summary>
    /// Gets the successful validation result.
    /// </summary>
    public static DraftValidationResult Valid { get; } = new([]);

    /// <summary>
    /// Gets the refusal codes, empty on success.
    /// </summary>
    public IReadOnlyList<string> ErrorCodes { get; }

    /// <summary>
    /// Gets a value indicating whether all rules passed.
    /// </summary>
    public bool IsValid => ErrorCodes.Count == 0;
}

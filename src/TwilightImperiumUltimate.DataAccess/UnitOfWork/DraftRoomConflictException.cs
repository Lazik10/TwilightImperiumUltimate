namespace TwilightImperiumUltimate.DataAccess.UnitOfWork;

/// <summary>
/// Indicates that another request changed the draft room before the operation could be saved.
/// </summary>
public sealed class DraftRoomConflictException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DraftRoomConflictException"/> class.
    /// </summary>
    public DraftRoomConflictException()
        : base("The draft room was modified by another request.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DraftRoomConflictException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DraftRoomConflictException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DraftRoomConflictException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that caused this conflict.</param>
    public DraftRoomConflictException(string message, Exception innerException)
        : base(message, innerException)
    {
        ArgumentNullException.ThrowIfNull(innerException);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DraftRoomConflictException"/> class.
    /// </summary>
    /// <param name="innerException">The concurrency exception that caused this conflict.</param>
    public DraftRoomConflictException(Exception innerException)
        : base("The draft room was modified by another request.", innerException)
    {
        ArgumentNullException.ThrowIfNull(innerException);
    }
}

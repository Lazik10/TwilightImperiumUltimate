namespace TwilightImperiumUltimate.DataAccess.UnitOfWork;

/// <summary>
/// Indicates that the requested draft room does not exist.
/// </summary>
public sealed class DraftRoomNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DraftRoomNotFoundException"/> class.
    /// </summary>
    public DraftRoomNotFoundException()
        : base("The draft room was not found.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DraftRoomNotFoundException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DraftRoomNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DraftRoomNotFoundException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that caused this error.</param>
    public DraftRoomNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
        ArgumentNullException.ThrowIfNull(innerException);
    }
}

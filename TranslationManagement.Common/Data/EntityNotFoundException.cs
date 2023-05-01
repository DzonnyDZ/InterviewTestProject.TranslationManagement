using System.Runtime.Serialization;

namespace TranslationManagement.Data;

/// <summary>An exception thrown when entity is not found in database and it's needed for further processing</summary>
[Serializable]
public class EntityNotFoundException : Exception
{
    /// <summary>Initializes a new instance of the <see cref="EntityNotFoundException"/> class with default error message.</summary>
    public EntityNotFoundException() : this("Entity not found") { }

    /// <summary>Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error message.</summary>
    /// <param name="message">The message that describes the error.</param>
    public EntityNotFoundException(string? message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with
    /// a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException"> The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
    public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>Initializes a new instance of the <see cref="EntityNotFoundException"/> class with standard message.</summary>
    /// <param name="type">Type of entity which has no been found</param>
    /// <param name="id">ID of entity which has not been found</param>
    public EntityNotFoundException(Type type, int id) : this($"{(type ?? throw new ArgumentNullException(nameof(type))).Name} id {id} not found.") { }

    /// <summary>Initializes a new instance of the <see cref="EntityNotFoundException"/> class with serialized data.</summary>
    /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
    protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

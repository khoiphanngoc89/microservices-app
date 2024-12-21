using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Ordering.Domain.Exceptions;

public class DomainException : Exception
{
    private const string DefaultMessage = "Domain Exception: \"{0}\" throws from Domain Layer";
    public DomainException()
    {
    }
    public DomainException(string message)
        : base(string.Format(DefaultMessage, message))
    {
    }
    public DomainException(string message, Exception innerException)
        : base(string.Format(DefaultMessage, message), innerException)
    {
    }

    public static void ThrowIfNullOrEmpty([NotNull] Guid? value, [CallerArgumentExpression(nameof(value))] string paramName = default!)
    {
        if (value is null || value == Guid.Empty)
        {
            throw new DomainException($"The value of {paramName} can not be null or empty");
        }
    }

    public static void ThrowIfNullOrWhitespace([NotNull] string? value, [CallerArgumentExpression(nameof(value))] string paramName = default!)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"The value of {paramName} can not be null or white space");
        }
    }

    public static void ThrowIfNotEqual([NotNull] int valueLength, int expectedLength, [CallerArgumentExpression(nameof(valueLength))] string paramName = default!)
    {
        if (valueLength != expectedLength)
        {
            string message = paramName is null
                ? $"The value is not equal to {expectedLength}"
                : $"The value of {paramName} is not equal to {expectedLength}";
            throw new DomainException(message);
        }
    }
}

namespace BuildingBlocks.Domains;

public sealed class InternalServerException : Exception
{
    public InternalServerException(string message)
        : base(message)
    {
    }

    public InternalServerException(string message, string details)
        : base(message)
    {
        Details = details;
    }

    public InternalServerException(string message, string details, Exception innerException)
        : base(message, innerException)
    {
        Details = details;
    }

    public string? Details { get; }
}

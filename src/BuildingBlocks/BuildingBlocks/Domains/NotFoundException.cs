namespace BuildingBlocks.Domains;

public class NotFoundException : Exception
{
    private const string DefaultMessage = "Entity \"{0}\" ({1}) are not found";
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string name, object key)
        : base(string.Format(DefaultMessage, name, key))
    {
    }

    public NotFoundException(string name, object key, Exception? innerException)
        : base(string.Format(DefaultMessage, name, key), innerException)
    {
    }
}

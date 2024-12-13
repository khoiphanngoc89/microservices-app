namespace BuildingBlocks.Domains;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" {key}\" are not found")
    {
    }
}

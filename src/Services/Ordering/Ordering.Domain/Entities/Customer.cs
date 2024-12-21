namespace Ordering.Domain.Entities;

public class Customer : EntityBase<CustomerId>
{
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;

    public static Customer Create(CustomerId id, string name, string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(email);

        return new Customer
        {
            Id = id,
            Name = name,
            Email = email
        };
    }
}

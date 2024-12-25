namespace Ordering.Domain.ValueObjects;

public sealed record Address
{
    public string FirstName { get; } = default!;
    public string LastName { get; } = default!;
    public string? EmailAddress { get; } = default!;
    public string AddressLine { get; } = default!;
    public string PhoneNumber { get; } = default!;
    public string City { get; } = default!;
    public string? State { get; } = default!;
    public string Country { get; } = default!;
    public string ZipCode { get; } = default!;
#pragma warning disable CS0628 // New protected member declared in sealed type
    protected Address()
#pragma warning restore CS0628 // New protected member declared in sealed type
    {
    }

    private Address(string firstName,
                    string lastName,
                    string? emailAddress,
                    string addressLine,
                    string phoneNumber,
                    string city,
                    string? state,
                    string country,
                    string zipCode)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        AddressLine = addressLine;
        PhoneNumber = phoneNumber;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;
    }

    public static Address Of(string firstName,
                                  string lastName,
                                  string? emailAddress,
                                  string addressLine,
                                  string phoneNumber,
                                  string city,
                                  string? state,
                                  string country,
                                  string zipCode)
    {
        DomainException.ThrowIfNullOrWhitespace(emailAddress);
        DomainException.ThrowIfNullOrWhitespace(addressLine);
        return new Address(firstName, lastName, emailAddress, addressLine, phoneNumber, city, state, country, zipCode);
    }

    public static Address Of(string firstName,
                                  string lastName,
                                  string? emailAddress,
                                  string addressLine,
                                  string phoneNumber,
                                  string city,
                                  string country,
                                  string zipCode)
    {
        DomainException.ThrowIfNullOrWhitespace(emailAddress);
        DomainException.ThrowIfNullOrWhitespace(addressLine);
        return new Address(firstName, lastName, emailAddress, addressLine, phoneNumber, city, default, country, zipCode);
    }
}
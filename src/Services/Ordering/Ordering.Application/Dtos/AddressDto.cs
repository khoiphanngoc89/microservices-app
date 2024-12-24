namespace Ordering.Application.Dtos;

public sealed record AddressDto(
    string FirstName,
    string LastName,
    string? EmailAddress,
    string AddressLine,
    string PhoneNumber,
    string City,
    string? State,
    string Country,
    string ZipCode
    );

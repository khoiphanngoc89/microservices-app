namespace Ordering.Application.Dtos;

public sealed record PaymentDto(
    string CardNumber,
    string CardHolderName,
    string Expiration,
    string Cvv,
    int PaymentMethod
    );

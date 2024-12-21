namespace Ordering.Domain.ValueObjects;

public sealed record Payment
{
    private const int DefaultLength = 3;
    public string? CardName { get; } = default!;
    public string CardNumber { get; } = default!;
    public string Expiration { get; } = default!;
    public string CVV { get; } = default!;
    public int PaymentMethod { get; } = default!;
    protected Payment() { }
    private Payment(string cardName,
                    string cardNumber,
                    string expiration,
                    string cvv,
                    int paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethod = paymentMethod;
    }

    public static Payment Create(string cardName,
                                  string cardNumber,
                                  string expiration,
                                  string cvv,
                                  int paymentMethod)
    {
        DomainException.ThrowIfNullOrWhitespace(cardName);
        DomainException.ThrowIfNullOrWhitespace(cardNumber);
        DomainException.ThrowIfNullOrWhitespace(expiration);
        DomainException.ThrowIfNullOrWhitespace(cvv);
        DomainException.ThrowIfNotEqual(cvv.Length, DefaultLength);

        return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
    }
}

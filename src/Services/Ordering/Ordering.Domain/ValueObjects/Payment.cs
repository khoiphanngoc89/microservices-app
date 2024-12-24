namespace Ordering.Domain.ValueObjects;

public sealed record Payment
{
    private const int DefaultLength = 3;
    public string CardHolderName { get; } = default!;
    public string CardNumber { get; } = default!;
    public string Expiration { get; } = default!;
    public string CVV { get; } = default!;
    public int PaymentMethod { get; } = default!;
#pragma warning disable CS0628 // New protected member declared in sealed type
    protected Payment() { }
#pragma warning restore CS0628 // New protected member declared in sealed type
    private Payment(string cardName,
                    string cardNumber,
                    string expiration,
                    string cvv,
                    int paymentMethod)
    {
        CardHolderName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethod = paymentMethod;
    }

    public static Payment Of(string CardHolderName,
                                  string cardNumber,
                                  string expiration,
                                  string cvv,
                                  int paymentMethod)
    {
        DomainException.ThrowIfNullOrWhitespace(CardHolderName);
        DomainException.ThrowIfNullOrWhitespace(cardNumber);
        DomainException.ThrowIfNullOrWhitespace(expiration);
        DomainException.ThrowIfNullOrWhitespace(cvv);
        DomainException.ThrowIfNotEqual(cvv.Length, DefaultLength);

        return new Payment(CardHolderName, cardNumber, expiration, cvv, paymentMethod);
    }
}

using System.Drawing;

namespace BuildingBlocks.Messaging.Events;

public sealed record BasketCheckoutEvent : IntegrationEvent
{
    public string UserName { get; set; } = default!;
    public Guid CustomerId { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;

    // Shipping and billing
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string AddressLine { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string? State { get; set; } = default!;
    public string City { get; set; } = default!;
    public string ZipCode { get; set; } = default!;

    // Payment
    public string CardHolderName { get; set; } = default!;
    public string CardNumber { get; set; } = default!;
    public string Expiration { get; set; } = default!;
    public string CVV { get; set; } = default!;
    public int PaymentMethod { get; set; } = default!;
    public List<BasketItemCheckout> Items { get; set; } = new();

}

public sealed class BasketItemCheckout
{
    public Guid ProductId { get; set; } = default!;
    public int Quantity { get; set; } = default!;
    public decimal Price { get; set; } = default!;
}

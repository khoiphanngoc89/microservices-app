namespace Ordering.Application.Dtos;

public sealed class OrderDtoListTransformer
{
    private IEnumerable<Order>? _orders;
    public OrderDtoListTransformer DataSource(IEnumerable<Order> orders)
    {
        _orders = orders;
        return this;
    }

    public static OrderDtoListTransformer Init() => new();

    private IEnumerable<OrderDto> ToOrderDto()
    {
        if (_orders is null)
        {
            throw new ArgumentNullException(nameof(_orders), "Data source is not set");
        }

        return _orders.Select(order => new OrderDto(
            Id: order.Id.Value,
            CustomerId: order.CustomerId.Value,
            OrderName: order.OrderName.Value,
            ShippingAddress: new AddressDto(
                order.ShippingAddress.FirstName,
                order.ShippingAddress.LastName,
                order.ShippingAddress.EmailAddress,
                order.ShippingAddress.AddressLine,
                order.ShippingAddress.PhoneNumber,
                order.ShippingAddress.City,
                order.ShippingAddress.State,
                order.ShippingAddress.Country,
                order.ShippingAddress.ZipCode),
            BillingAddress: new AddressDto(
                order.BillingAddress.FirstName,
                order.BillingAddress.LastName,
                order.BillingAddress.EmailAddress,
                order.BillingAddress.AddressLine,
                order.BillingAddress.PhoneNumber,
                order.BillingAddress.City,
                order.BillingAddress.State,
                order.BillingAddress.Country,
                order.BillingAddress.ZipCode),
            Payment: new PaymentDto(
                order.Payment.CardNumber,
                order.Payment.CardHolderName,
                order.Payment.Expiration,
                order.Payment.CVV,
                order.Payment.PaymentMethod),
            Status: order.Status,
            OrderItems: order.OrderItems.Select(orderItem =>
                new OrderItemDto(orderItem.OrderId.Value,
                orderItem.ProductId.Value,
                orderItem.Quantity,
                orderItem.Price
            )).ToList())
        );
    }

    public IEnumerable<OrderDto> Transform()
        => ToOrderDto();
}

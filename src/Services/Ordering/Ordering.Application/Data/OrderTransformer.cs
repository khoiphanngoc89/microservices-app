namespace Ordering.Application.Data;

public sealed class OrderTransformer
{
    private Address? _shippingAddress;
    private Address? _billingAddress;
    private Payment? _payment;
    private Order? _order;
    private bool _createNew = true;

    private OrderTransformer()
    {
    }

    public static OrderTransformer Init() => new();


    public OrderTransformer From(Order order)
    {
        _createNew = false;
        _order = order;
        return this;
    }

    public OrderTransformer WithShippingAddress(AddressDto shippingAddress)
    {
        _shippingAddress = ToAddress(shippingAddress);
        return this;
    }

    public OrderTransformer WithBillingAddress(AddressDto billingAddress)
    {
        _billingAddress = ToAddress(billingAddress);
        return this;
    }

    public OrderTransformer WithPayment(PaymentDto payment)
    {
        _payment = Payment.Of(
            payment.CardNumber,
            payment.CardHolderName,
            payment.Expiration,
            payment.Cvv,
            payment.PaymentMethod);
        return this;
    }

    public Order Transform(OrderDto orderDto)
    {
        return _createNew ? this.Create(orderDto) : this.Update(orderDto);
    }

    private static Address ToAddress(AddressDto addressDto)
        => Address.Of(
            addressDto.FirstName,
            addressDto.LastName,
            addressDto.EmailAddress,
            addressDto.AddressLine,
            addressDto.PhoneNumber,
            addressDto.City,
            addressDto.State,
            addressDto.Country,
            addressDto.ZipCode);

    private Order Create(OrderDto orderDto)
    {
        var result = Order.Create(
           id: OrderId.Of(Guid.NewGuid()),
           customerId: CustomerId.Of(orderDto.CustomerId),
           orderName: OrderName.Of(orderDto.OrderName),
           shippingAddress: _shippingAddress!,
           billingAddress: _billingAddress!,
           payment: _payment!);

        foreach (var item in orderDto.OrderItems)
        {
            result.Add(ProductId.Of(item.ProductId), item.Quantity, item.Price);
        }

        return result;
    }

    private Order Update(OrderDto orderDto)
    {
        _order!.Update(
            orderName: OrderName.Of(orderDto.OrderName),
            shippingAddress: _shippingAddress!,
            billingAddress: _billingAddress!,
            payment: _payment!,
            status: orderDto.Status);
        return _order;
    }
}

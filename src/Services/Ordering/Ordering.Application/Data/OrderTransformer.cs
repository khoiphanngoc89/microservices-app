namespace Ordering.Application.Data;

public sealed class OrderTransformer
{
    private Address? _shippingAddress;
    private Address? _billingAddress;
    private Payment? _payment;
    private Order? _originalOrder;
    private OrderDto? _orderDto;
    private bool _createNew = true;

    private OrderTransformer()
    {
    }

    public static OrderTransformer Init() => new();

    public OrderTransformer From(Order order)
    {
        _originalOrder = order;
        _createNew = false;
        return this;
    }

    public OrderTransformer DataSource(OrderDto orderDto)
    {
        _orderDto = orderDto;
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
            payment.CardHolderName,
            payment.CardNumber,
            payment.Expiration,
            payment.Cvv,
            payment.PaymentMethod);
        return this;
    }

    public Order Transform()
    {
        ArgumentNullException.ThrowIfNull(_orderDto);
        return _createNew ? this.Create(_orderDto) : this.Update(_orderDto);
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
        ArgumentNullException.ThrowIfNull(_originalOrder);
        _originalOrder!.Update(
            orderName: OrderName.Of(orderDto.OrderName),
            shippingAddress: _shippingAddress!,
            billingAddress: _billingAddress!,
            payment: _payment!,
            status: orderDto.Status);
        return _originalOrder;
    }
}

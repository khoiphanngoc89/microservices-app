using BuildingBlocks.Messaging.Events;
using Ordering.Domain.Enums;

namespace Ordering.Application.Dtos;

public sealed class OrderDtoTransformer
{
    private AddressDto _shippingAddress;
    private AddressDto _billingAddress;
    private PaymentDto _payment;
    private static Guid _orderId;
    private List<OrderItemDto> _orderItems;
    private Guid _customerId;
    private string _orderName;
    private OrderStatus _orderStatus = OrderStatus.Pending;



#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private OrderDtoTransformer()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    }

    public static OrderDtoTransformer Init() => new();


    public OrderDtoTransformer WithOrderId(Guid orderId)
    {
        _orderId = orderId;
        return this;
    }

    public OrderDtoTransformer WithAutoGenerateOrderId()
    {
        _orderId = Guid.NewGuid();
        return this;
    }


    public OrderDtoTransformer WithShippingAddress(string firstName,
                                                   string lastName,
                                                   string emailAddress,
                                                   string addressLine,
                                                   string phoneNumber,
                                                   string city,
                                                   string? state,
                                                   string country,
                                                   string zipCode)
    {
        _shippingAddress = new AddressDto(
            firstName,
            lastName,
            emailAddress,
            addressLine,
            phoneNumber,
            city,
            state,
            country,
            zipCode
            );
        return this;
    }

    public OrderDtoTransformer WithBillingAddress(string firstName,
                                                  string lastName,
                                                  string emailAddress,
                                                  string addressLine,
                                                  string phoneNumber,
                                                  string city,
                                                  string? state,
                                                  string country,
                                                  string zipCode)
    {
        _billingAddress = new AddressDto(
            firstName,
            lastName,
            emailAddress,
            addressLine,
            phoneNumber,
            city,
            state,
            country,
            zipCode
            );
        return this;
    }

    public OrderDtoTransformer WithPayment(string cardNumber,
                                           string cardHolderName,
                                           string expiration,
                                           string cvv,
                                           int paymentMethod)
    {
        _payment = new PaymentDto(
            cardNumber,
            cardHolderName,
            expiration,
            cvv,
            paymentMethod
            );
        return this;
    }

    public OrderDtoTransformer WithOrderItems(IEnumerable<BasketItemCheckout> items)
    {
        _orderItems = items.Select(i => new OrderItemDto(
            _orderId,
            i.ProductId,
            i.Quantity,
            i.Price
            )).ToList();
        return this;
    }

    public OrderDtoTransformer WithOrderItems(IEnumerable<OrderItem> items)
    {
        _orderItems = items.Select(i => new OrderItemDto(
            i.OrderId.Value,
            i.ProductId.Value,
            i.Quantity,
            i.Price
            )).ToList();
        return this;
    }

    public OrderDtoTransformer WithCustomerId(Guid customerId)
    {
        _customerId = customerId;
        return this;
    }

    public OrderDtoTransformer WithOrderName(string orderName)
    {
        _orderName = orderName;
        return this;
    }

    public OrderDtoTransformer WithOrderStatus(OrderStatus status)
    {
        _orderStatus = status;
        return this;
    }

    public OrderDto Transform()
    {
        var result = new OrderDto(
            Id: _orderId,
            CustomerId: _customerId,
            OrderName: _orderName,
            ShippingAddress: _shippingAddress,
            BillingAddress: _billingAddress,
            Payment: _payment,
            Status: _orderStatus,
            OrderItems: _orderItems
            );
        return result;
    }
}

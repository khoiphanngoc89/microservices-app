using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers.Domain;

/// <summary>
/// OrderCreatedEventHandler
/// </summary>
/// <param name="publisher">The publisher</param>
/// <param name="logger">The logger</param>
/// <remarks>
/// When a order is created, the OrderCreatedDomainEvent is publish.
/// The OrderCreatedDomainEvent would lead to create OrderCreatedIntegrationEvent to
/// make another microservices know in system the order is created.
/// These subscriber must continue to process when the order is create in system.
/// i.e. Shipment, Payment, Notification micoservices.
/// OrderCreatedDomainEvent to OrderCreatedIntegrationEvent for another microservices
/// Subscriber to OrderCreatedDomainEvent:
/// - Shipment
/// - Payment
/// - Notification
/// ---------------------------------------------------------------------------------------------------------------------------
/// --|Order|--> |OrderCreatedDomainEvent| --> | Hander |--> |OrderCreatedIntegrationEvent| --> |Subscriber-Bounded-Contexts|--
/// ----|-----------------------|---------------------------------------------------------------|---------------------------|--
/// ----|-----------------------V---------------------------------------------------------------|----------Shipment---------|--
/// -|OrderItem|------------|Handler|-----------------------------------------------------------|----------Payment----------|--
/// ----------------------------^---------------------------------------------------------------|--------Notification-------|--
/// ----------------------------|---------------------------------------------------------------|---------------------------|--
/// -------------|BasketCheckoutIntegrationEvent|<---------------|Shopping-Cart|-----------------------------------------------
/// ---------------------------------------------------------------------------------------------------------------------------
/// </remarks>
public sealed class OrderCreatedEventHandler
    (IPublishEndpoint publisher,
    IFeatureManager featureManager,
    ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    private const string OrderFullfilment = nameof(OrderFullfilment);
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        if (!await featureManager.IsEnabledAsync(OrderFullfilment) )
        {
            return;
        }

        var orderCreatedIntegrationEvent = OrderDtoTransformer.Init()
            .WithOrderId(domainEvent.Order.Id.Value)
            .WithCustomerId(domainEvent.Order.CustomerId.Value)
            .WithOrderName(domainEvent.Order.OrderName.Value)
            .WithBillingAddress(domainEvent.Order.BillingAddress.FirstName,
                                domainEvent.Order.BillingAddress.LastName,
                                domainEvent.Order.BillingAddress.EmailAddress!,
                                domainEvent.Order.BillingAddress.AddressLine,
                                domainEvent.Order.BillingAddress.PhoneNumber,
                                domainEvent.Order.BillingAddress.City,
                                domainEvent.Order.BillingAddress.State,
                                domainEvent.Order.BillingAddress.Country,
                                domainEvent.Order.BillingAddress.ZipCode)
            .WithShippingAddress(domainEvent.Order.ShippingAddress.FirstName,
                                domainEvent.Order.ShippingAddress.LastName,
                                domainEvent.Order.ShippingAddress.EmailAddress!,
                                domainEvent.Order.ShippingAddress.AddressLine,
                                domainEvent.Order.ShippingAddress.PhoneNumber,
                                domainEvent.Order.ShippingAddress.City,
                                domainEvent.Order.ShippingAddress.State,
                                domainEvent.Order.ShippingAddress.Country,
                                domainEvent.Order.ShippingAddress.ZipCode)
            .WithPayment(domainEvent.Order.Payment.CardHolderName,
                        domainEvent.Order.Payment.CardNumber,
                        domainEvent.Order.Payment.Expiration,
                        domainEvent.Order.Payment.CVV,
                        domainEvent.Order.Payment.PaymentMethod)
            .WithOrderStatus(domainEvent.Order.Status)
            .WithOrderItems(domainEvent.Order.OrderItems)
            .Transform();


        await publisher.Publish(orderCreatedIntegrationEvent, cancellationToken);
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);
        return;
    }
}

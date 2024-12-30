using BuildingBlocks.Messaging.Events;
using MassTransit;
using Ordering.Application.Orders.Commands;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public sealed class BasketCheckoutEventHandler
    (ISender sender, ILogger<BasketCheckoutEventHandler> logger)
    : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        // TODO: Create new order and start order fullfillment process
        var order = OrderDtoTransformer.Init()
            .WithAutoGenerateOrderId()
            .WithOrderName(context.Message.UserName)
            .WithCustomerId(context.Message.CustomerId)
            .WithBillingAddress(context.Message.FirstName,
                                context.Message.LastName,
                                context.Message.EmailAddress,
                                context.Message.AddressLine,
                                context.Message.PhoneNumber,
                                context.Message.Country,
                                context.Message.State,
                                context.Message.City,
                                context.Message.ZipCode)
            .WithShippingAddress(context.Message.FirstName,
                                 context.Message.LastName,
                                 context.Message.EmailAddress,
                                 context.Message.AddressLine,
                                 context.Message.PhoneNumber,
                                 context.Message.Country,
                                 context.Message.State,
                                 context.Message.City,
                                 context.Message.ZipCode)
            .WithPayment(context.Message.CardNumber,
                         context.Message.CardHolderName,
                         context.Message.Expiration,
                         context.Message.CVV,
                         context.Message.PaymentMethod)
            .WithOrderItems(context.Message.Items)
            .Transform();
        await sender.Send(new CreateOrderCommand(order));
        logger.LogInformation("Integration event handled: {IntegrationEvent}", context.Message);
    }
}

// SAGA pattern for Distributed Transaction
//
// 1. Saga design pattern is provide to manage data consistency across
// microservices in distributed transaction cases.
//
// 2. Saga offers to create a set of transactions that update
// microservices sequentially, and publisg events to trigger the next
// transaction for the next microservices
//
// 3. If one of the step is failed, than saga patterns trigger to rollback
// transactions, do reverse operations with publishing rollback events to
// previous.
//
// 4. Publish/subscribe pattern with brokers or API composition
//
// 5. Saga pattern manage long-running transactions that involve multiple
// multiple microservices which is a series of local transactions that
// work together to achieve end-to-end use cases.
// The saga pattern is a way to manage long running transactions that involve multiple microservices.
// It is based on the idea of the saga, which is a series of local transactions that work together to
// achieve end to end use cases.
// 
// 6. Useful in distributed systems, where multiple microservices need to coordinate their actions.
//
// 7. Ensure that the overall transaction is either completed sucessfully or rolled back to its initial state.
// (compensating transactions).
//
// The SAGA pattern can help to ensure that overall transaction is either completed
// successfully or rolled back to its initial state if any individual transaction encounters any errors.
//
// In order to use SAGA patterns in a distributed transaction, we can use a compensating transaction to undo the changes made by each microservices
// in the event of an error.
//
// By this way, we can ensure that the overall transaction is either completed successfully or rolled back to its initial state.
//
// 8. The SAGA pattern provides transaction management with using a sequence of local transactions of microservices. And grouping these local
// transactions and sequentially invoking one by one.
//
// Every microservices has its own database, and it can able to manage local transactions in atomic way with strong consistency.
//
// So SAGA pattern grouping these local transactions and sequentially invoking one by one each local transaction updates the database
// and publishes an event to the trigger the next local transaction.
//
// If one of the step is failed, then SAGA pattern trigger to rollback transactions that are a set of compensating transaction that rollback
// the changes on previous  microservices and restore the data consistency.


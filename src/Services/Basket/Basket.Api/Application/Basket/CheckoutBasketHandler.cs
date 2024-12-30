using Basket.Api.Application.Transformers;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.Api.Application.Basket;

public sealed record CheckoutBasketCommand(BasketCheckoutDto Basket)
    : ICommand<CheckoutBasketResult>;
public sealed record CheckoutBasketResult(bool IsSuccess);
public sealed class CheckoutBasketCommandValidator
    : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.Basket).NotNull().WithMessage("Basket cannot be null");
        RuleFor(x => x.Basket.UserName).NotNull().NotEmpty().WithMessage("UserName cannot be empty");
    }
}
public sealed class CheckoutBasketCommandHandler
    (IBasketRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        // get existing basket with total price
        // set total price on basket checkout event message
        // send basket checkout to rabbitmq using masstransit
        // remove basket
        var basket = await repository.GetBasketAsync(command.Basket.UserName, cancellationToken);
        if (basket is null)
        {
            return new CheckoutBasketResult(false);
        }

        
        var source = command.Basket.Adapt<BasketCheckoutEvent>();
        var eventMessage = BasketCheckoutTransformer.Init()
            .DataSource(source)
            .WithTotalPrice(basket.TotalPrice)
            .WithItems(basket.Items)
            .Transform();
        // 1--publish event
        await publishEndpoint.Publish(eventMessage, cancellationToken);

        
        // 2--delete from database
        await repository.DeleteBasketAsnyc(command.Basket.UserName, cancellationToken);

        // From 2 (delete from db) and 1 (publish event), this will be leading dual write problems
        // these dual write problems can be solved by using outbox pattern
        //
        // dual write problems happen when application need to change data
        // in different systems. i.e database and message queue, if one of them fails
        // the other one will be inconsistent.
        //
        // happens when you use a local transaction with each of the external systems
        // operations, i.e. application need to persist data in the database and send
        // a message to Kafka for notifying other system.
        //
        // if one of these two operations fails, the system will be inconsistent.
        //
        // Problem:
        // 1. Data loss or corruption.
        // 2. Difficult to resolve without proper error handling and recovery mechanism.
        // 3. Dual write can be hard to detect and fix.
        return new CheckoutBasketResult(true);

        // Solution: Transactional Outbox Pattern
        // The outbox pattern provides to publish event reliably.
        // 1. The idea is to have an "Outbox" table in the microservice's database.
        // It provides to publish events reliably.
        // 2. Dual write problem happens when application need to change data in different
        // systems.
        // 3. Instead of sending the data to two separate locations, send a single transaction that will
        // store 2 separate copies of the data on the database
        // 4. One copy is stored in relevant database table, and the other copy is stored
        // in an outbox table that will publish to event bus.
        // 5. When API publishes event messages, it does not directly send them,
        // insread, the messages are persisted in a database table.
        // 6. After that, a job publish events  to message broker system in predefined time intervals.
        // 7. Events are not written to a event bú, it is writeen to a table in the "Outbox" role of the service.
        // 8. Transaction performed before the event and the event written to the outbox table are part of the same
        // transaction.
        // 9. When a new order is added to the system, the process of adding the order
        // and writing the ORDER_CREATED event to the OUTBOX TABLE is done in the same transaction
        // to ensure the event is saved to the database.
        // 10. If one of the process is fail, this will rollback the whole operations with following ACID principles.
        // 11. The second step is receive these events written to the Outbox table by an independent service
        // and write them to the Event bus. An service listen and polls the outbox table records and publish
        // events.

        // ACID
        // What is a transaction?
        //
        //
        // ACID properties: Atomicity, Consistency, Isolation, & Durability
        //
        // ACID is acronym that refers to the set of 4 key properties of a transaction:Atomicity, Consistency, Isolation, & Durability.
        // If a database operation has these ACID properties, it can be called an ACID transaction, and data storage system
        // that apply these operations can called transactional systems. ACID transactions guarantee that each read, write, or modification
        // of a table has the following properties:
        //
        // Atomicity - each statement in a transaction is treated as a single unit. Either the entire statement is executed,
        // or none of it is executed. This property prevents data los and corruption frin occuring if, for example,
        // if your streaming data source fails mid-stream.
        //
        // Consistency - ensures that transactions only make changesto tables in predefined, predictable ways.
        // Transactional consistency ensures that coruption or erors in your data do not create unintended consequences
        // for the integrity of your table.
        //
        // Isolation - when multiple users are reading and writing from the same table all at once, isolation of their transactions
        // ensures that concurrent transactions don't interfere with or afect one another. Each request can occur as though they
        // were occurring one by one, even though they 'rre actually occurring simultaneously.
        //
        // Durability - ensures that changes yo your data made by successfully executed transactions will be saved, even in the event of system failure.

    }
}

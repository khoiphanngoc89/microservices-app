using System.Reflection;
using BuildingBlocks.Application.MediatR.Behaviors;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        // register feature management
        services.AddFeatureManagement();

        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        return services;
    }

    // Domain vs Integration Events

    // Domain events:
    //
    // 1. Published and consumed within a single domain. Strictly within the boundary of the domain. context/microservice.
    // 2. Indicated something that happened within the aggregate. 
    // 3. In-pocess and synchronously, sent using an in-memory message bus. i.e. OrderCreatedEvent
    //
    // Integration events:
    // 1. Used to communicate state changes or events bounded contexts or microservices.
    // 2. Overall system's reaction to certain domain events.
    // 3. Asynchronous, sent wth a message broker over a queue.
    // i.e. After handling Created, an OrderCreatedIngrationEvent published to a mesage broker like
    // RabbitMQ, Kafka, etc, then consumes by other microservices.

}

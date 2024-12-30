using System.Net.Http.Headers;
using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMessageBroker
        (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = default!)
    {
        var hostAddress = configuration["MessageBroker:Host"];
        var userName = configuration["MessageBroker:Username"];
        var password = configuration["MessageBroker:Password"];

        ArgumentNullException.ThrowIfNullOrWhiteSpace(hostAddress);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(userName);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(password);

        services.AddMassTransit(config =>
        {
            // set naming convention for endpoints
            // kebab case is common naming convention used in the Url,
            // which is prefferred by the readability 
            config.SetKebabCaseEndpointNameFormatter();

            // add consumers from the assembly
            // We are adding the consumer because thiss method can using from both basket and ordering service
            // When we are using for the ordering microservices, this wwill be as a consumers
            // that wwe can trigger for the consumer operation.
            // So if the assembly parameter is provided, this line will scan the assembly and automatically
            // register and discover the consumers.

            // This is crucial for the microservices acting as a subscriber or
            // consumer of the messages.
            if (assembly is not null)
            {
                config.AddConsumers(assembly);
            }

            // We are using the Rabbit MQ configure method, which is configures the bus
            // to use RabbitMQ as transport.
            config.UsingRabbitMq((context, cfg) =>
            {
                // We are setting the host address, username and password
                // from the configuration file.
                cfg.Host(new Uri(hostAddress), host =>
                {
                    host.Username(userName);
                    host.Password(password);
                });

                // This ensures that Masstransit automatically configures the endpoints
                // for the consumers that we have added in the previous line.
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}

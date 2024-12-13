﻿
namespace Basket.Api.Presentation.Extensions;

public static partial class HostExtensions
{
    internal static WebApplication AddServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddCarter(new DependencyContextAssemblyBasketCustom());

        var assembly = typeof(Program).Assembly;
        builder.Services.AddMediatR(config =>
        {
            // The MediatR will auto scan running assembly to register the Request and Command services are located
            config.RegisterServicesFromAssembly(assembly);
            // add the PipleBehavior into MediatR
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        // Add Fluent Validation in the service
        builder.Services.AddValidatorsFromAssembly(assembly);

        // register database
        var connectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionStrings);
        builder.Services.AddDatabase(connectionStrings);

        // register services
        builder.Services.AddScoped<IBasketRepository, BasketRepository>();

        // Register global exception handler
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // create a IHost base on the services and essential configurations
        // not yet listen on http/https
        return builder.Build();
    }

    internal static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddMarten(opts =>
        {
            opts.Connection(connectionString);
            opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
        }).UseLightweightSessions();

        return services;
    }
}

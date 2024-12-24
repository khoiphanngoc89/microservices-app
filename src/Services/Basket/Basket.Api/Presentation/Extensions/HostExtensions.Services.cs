using BuildingBlocks.Application.Endpoints;
using BuildingBlocks.Presentation;
using Discount.Grpc;

namespace Basket.Api.Presentation.Extensions;

public static partial class HostExtensions
{
    internal static WebApplication AddServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddEndpoints(typeof(Program).Assembly);

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
        var connectionStrings = builder.Configuration.GetConnectionString(Constants.DefaultConnection);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionStrings);
        builder.Services.AddDatabase(connectionStrings);

        // register services
        builder.Services.AddScoped<IBasketRepository, BasketRepository>();
        builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

        var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
        ArgumentNullException.ThrowIfNullOrWhiteSpace(redisConnectionString);
        builder.Services.AddStackExchangeRedisCache(opts =>
        {
            opts.Configuration = redisConnectionString;
        });

        // register the decorate with add scope
        //builder.Services.AddScoped<IBasketRepository>(provider =>
        //{
        //    var repository = provider.GetRequiredService<BasketRepository>();
        //    return new CachedBasketRepository(repository, provider.GetRequiredService<IDistributedCache>());
        //});

        // register grpc
        var grpcAddress = builder.Configuration["GrpcSettings:DiscountUrl"];
        ArgumentNullException.ThrowIfNullOrWhiteSpace(grpcAddress);
        builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opts =>
        {
            opts.Address = new Uri(grpcAddress);
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            // https://github.com/grpc/grpc-dotnet/issues/792
            // For debug only
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            return handler;
        });

        // Register global exception handler
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        // register for health check
        builder.Services.AddHealthChecks()
            .AddNpgSql(connectionStrings)
            .AddRedis(redisConnectionString);

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

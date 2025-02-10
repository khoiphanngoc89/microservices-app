using Basket.Api.Database;
using Basket.Api.Entities;
using BuildingBlocks.Common.Behaviors;
using BuildingBlocks.Common.Core;
using BuildingBlocks.Common.Core.Exceptions.Handler;
using Carter;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpLogging;

namespace Basket.Api;

public static class DependencyInjection
{
    internal static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var assembly = typeof(Program).Assembly;

        // Application services
        // register for carter
        builder.Services.AddCarter(new ContextAssemblyCatalog(assembly));
        // register for mediatr
        builder.Services.AddMediatR(opts =>
        {
            opts.RegisterServicesFromAssembly(assembly);
            opts.AddOpenBehavior(typeof(LoggingBehavior<,>));
            opts.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Data service
        var connectionString = builder.Configuration.GetConnectionString("Database");
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionString);
        builder.Services.AddMarten(opts =>
        {
            opts.Connection(connectionString);
            // setup username as identity
            opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
        }).UseLightweightSessions();

        builder.Services.AddScoped<IBasketRepository, BasketRepository>();
        builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

        var redis = builder.Configuration.GetConnectionString("Redis");
        ArgumentNullException.ThrowIfNullOrWhiteSpace(redis);
        builder.Services.AddStackExchangeRedisCache(opts =>
        {
            opts.Configuration = redis;
            //options.InstanceName = "Basket";
        });

        // Grpc Service
        var discountUrl = builder.Configuration["GrpcSettings:DiscountUrl"];
        ArgumentNullException.ThrowIfNullOrWhiteSpace(discountUrl);
        builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opts =>
        {
            opts.Address = new Uri(discountUrl);
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        }); ;

        // cutting-cross concerns
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        // register health checks
        builder.Services.AddHealthChecks()
            .AddNpgSql(connectionString)
            .AddRedis(redis);

        // register htp logging the request and response data
        // https://stackoverflow.com/questions/78673062/how-to-set-up-serilog-to-log-request-and-response-bodies-together
        builder.Services.AddHttpLogging(opts =>
        {
            opts.LoggingFields =
                HttpLoggingFields.RequestPath |
                HttpLoggingFields.RequestBody |
                HttpLoggingFields.ResponseStatusCode |
                HttpLoggingFields.ResponseBody |
                HttpLoggingFields.Duration;

            opts.MediaTypeOptions.AddText("application/json");
            opts.MediaTypeOptions.AddText("text/plain");
            opts.RequestBodyLogLimit = 4096;
            opts.ResponseBodyLogLimit = 4096;
            opts.CombineLogs = true;
        });

        // register for swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opts =>
        {
            opts.EnableAnnotations();
        });

        return builder.Build();
    }

    internal static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // use swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapCarter();

        // use http logging
        app.UseHttpLogging();

        // using cutting-cross concerns
        app.UseExceptionHandler(opts => { });
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}

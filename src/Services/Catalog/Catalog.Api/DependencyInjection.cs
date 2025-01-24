using BuildingBlocks.Common.Behaviors;
using BuildingBlocks.Common.Core.Exceptions.Handler;
using Catalog.Api.Database;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpLogging;

namespace Catalog.Api;

public static class DependencyInjection
{
    internal static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var assembly = typeof(Program).Assembly;

        // register for carter
        builder.Services.AddCarter(new ContextAssemblyCatalog(assembly));

        // register swagger
        // https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-8.0&tabs=visual-studio
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opts =>
        {
            opts.EnableAnnotations();
        });

        // register for database
        var connectionString = builder.Configuration.GetConnectionString("Database");
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        builder.Services.AddMarten(opts =>
        {
            opts.Connection(connectionString);
        }).UseLightweightSessions();

        // register mediatr and its behaviors
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            // behaviors of mediatr like middleware
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        // initialize  data
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.InitializeMartenWith<CatalogInitialData>();
        }

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

       

        // cross-cutting concerns
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        // register health checks
        builder.Services.AddHealthChecks().AddNpgSql(connectionString);

        var app = builder.Build();
        return app;
    }

    internal static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // use swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // use carter
        app.MapCarter();

        // use http logging
        app.UseHttpLogging();

        // use exception handler
        app.UseExceptionHandler(options => { });

        // use health checks
        app.UseHealthChecks("/health",
        new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}

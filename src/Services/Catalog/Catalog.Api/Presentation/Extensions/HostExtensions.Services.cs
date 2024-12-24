namespace Catalog.Api.Presentation.Extensions;

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

        var connectionStrings = builder.Configuration.GetConnectionString(Constants.DefaultConnection);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionStrings);

        // register database and its migration
        builder.Services.AddDatabase(connectionStrings);

        // Register global exception handler
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // Add health check for this service
        builder.Services.AddHealthChecks()
            .AddNpgSql(connectionStrings);

        // create a IHost base on the services and essential configurations
        // not yet listen on http/https
        return builder.Build();
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, string connectionStrings)
    {
        services.AddMarten(opts =>
        {
            opts.Connection(connectionStrings);
        }).UseLightweightSessions();

        // If in dev env, we will migrate the database
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        IWebHostEnvironment? env = serviceProvider.GetService<IWebHostEnvironment>();
        if (env?.IsDevelopment() == true)
        {
            services.InitializeMartenWith<CatalogInitialData>();
        }

        return services;
    }
}

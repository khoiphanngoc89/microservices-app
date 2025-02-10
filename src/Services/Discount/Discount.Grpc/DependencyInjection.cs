using Discount.Grpc.Database;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Discount.Grpc;

public static class DependencyInjection
{
    internal static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc();
        
        builder.Services.AddDbContext<DiscountDbContext>(opts =>
        {
            var connectionString = builder.Configuration.GetConnectionString("Database");
            ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionString);
            opts.UseSqlite(connectionString);
        });

        var app = builder.Build();
        return app;
    }

    internal static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        app.MapGrpcService<DiscountService>();
        app.UseMigration();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
        return app;
    }
}

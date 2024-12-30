using BuildingBlocks.Domains;
using Microsoft.Extensions.Configuration;
using Ordering.Application;
using Ordering.Infrastructure;

namespace Ordering.Api.Extensions;

public static partial class HostExtensions
{
    public static WebApplication AddServies(this WebApplicationBuilder builder)
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        var connectionStrings = builder.Configuration.GetConnectionString(AppConstants.Database);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionStrings);

        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddInfrastructureServices(connectionStrings);
        builder.Services.AddApiServices(connectionStrings);

        return builder.Build();
    }
}

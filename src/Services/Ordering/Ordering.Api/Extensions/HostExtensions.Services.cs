namespace Ordering.Api.Extensions;

public static partial class HostExtensions
{
    public static WebApplication AddServies(this WebApplicationBuilder builder)
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        //builder.Services.AddApplicationServices();
        //builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApiServices();

        return builder.Build();
    }
}

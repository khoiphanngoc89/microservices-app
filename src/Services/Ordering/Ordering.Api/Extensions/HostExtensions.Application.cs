using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.Api.Extensions;

public static partial class HostExtensions
{
    public static WebApplication UsePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.MapEndpoints();

        app.UseHttpsRedirection();

        // Use exception handler in Host
        // need to add option, if not, the exception would be throw
        // when application run
        app.UseExceptionHandler(options => { });

        // use health check
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            // must add this configuration for display in detail all related
            // services
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}

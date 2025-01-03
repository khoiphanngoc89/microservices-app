﻿namespace Catalog.Api.Presentation.Extensions;

public static partial class HostExtensions
{
    internal static WebApplication UsePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        // Configure  the HTTP request pipeline
        app.MapEndpoints();

        // Use exception handler in Host
        // need to add option, if not, the exception would be throw
        // when application run
        app.UseExceptionHandler(options => { });

        // use health check in IHost
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}

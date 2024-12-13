﻿using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Basket.Api.Presentation.Extensions;

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
        app.MapCarter();

        // Use exception handler in Host
        // need to add option, if not, the exception would be throw
        // when application run
        app.UseExceptionHandler(options => { });

        return app;
    }
}
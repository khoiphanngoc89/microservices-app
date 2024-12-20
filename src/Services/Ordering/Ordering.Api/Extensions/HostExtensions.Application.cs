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

        app.UseHttpsRedirection();

        return app;
    }
}

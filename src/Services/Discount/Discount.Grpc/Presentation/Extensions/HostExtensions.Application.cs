namespace Discount.Grpc.Presentation.Extensions;

public static partial class HostExtensions
{
    internal static WebApplication UsePipeline(this WebApplication app)
    {
        app.UseMigration();

        // Configure the HTTP request pipeline.
        // configure to call discount protobruf service
        app.MapGrpcService<DiscountService>();

        return app;
    }
}

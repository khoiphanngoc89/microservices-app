using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Database;

public static class Extensions
{
    internal static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DiscountDbContext>();
        context.Database.MigrateAsync();
        return app;
    }
}

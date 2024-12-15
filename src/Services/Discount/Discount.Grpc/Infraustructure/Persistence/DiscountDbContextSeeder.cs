namespace Discount.Grpc.Infraustructure.Persistence;

public static class DiscountDbContextSeeder
{
    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetService<DiscountDbContext>();
        dbContext!.Database.MigrateAsync();
        return app;
    }
}

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        //services.AddDbContext<OrderContext>(options =>
        //{
        //    options.UseSqlServer("name=ConnectionStrings:OrderingContext");
        //});
        //services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }
}

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // services.AddMediatR(cfg =>
        // {
        //      Assembly.RegisterServicesFromAssembly(GetExecutingAssembly());
        // });
        return services;
    }
}

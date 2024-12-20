namespace Discount.Grpc.Presentation.Extensions;
public static partial class HostExtensions
{
    internal static WebApplication AddServices(this WebApplicationBuilder builder)
    {

        // Add services to the container.
        builder.Services.AddGrpc();


        var connectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionStrings);
        builder.Services.AddDbContext<DiscountDbContext>(opts =>
        {
            opts.UseSqlite(connectionStrings);
        });

        return builder.Build();
    }
}

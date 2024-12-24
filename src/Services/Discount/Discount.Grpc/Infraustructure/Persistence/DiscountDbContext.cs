namespace Discount.Grpc.Infraustructure.Persistence;

public sealed class DiscountDbContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; }
    public DiscountDbContext(DbContextOptions<DiscountDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon { Id = 1, ProductName = "IPhone X", Description = "IPhone Discount", Amount = 10 },
            new Coupon { Id = 2, ProductName = "Samsung 10", Description = "Samsung Discount", Amount = 20 });
        base.OnModelCreating(modelBuilder);
    }
}

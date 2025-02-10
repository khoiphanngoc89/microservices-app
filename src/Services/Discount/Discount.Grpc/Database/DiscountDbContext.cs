using Discount.Grpc.Entities;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Database;

public sealed class DiscountDbContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; } = default!;
    public DiscountDbContext(DbContextOptions<DiscountDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscountDbContext).Assembly);
        OnMigration(modelBuilder);
    }

    private void OnMigration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon { Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"), ProductName = "IPhone X", Description = "IPhone Discount", Amount = 150 },
            new Coupon { Id = new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914") , ProductName = "Samsung 10", Description = "Samsung Discount", Amount = 100 }
            );
    }
}

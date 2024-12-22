using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(io => io.Id);
        builder.Property(io => io.Id).HasConversion(
            orderItemId => orderItemId.Value,
            dbId => OrderItemId.Of(dbId));

        // product presents in one or many order items
        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(io => io.ProductId);

        builder.Property(io => io.Quantity).IsRequired();
        builder.Property(io => io.Price).IsRequired();

    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;

namespace Ordering.Infrastructure.Data.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasConversion(
            orderId => orderId.Value,
            dbId => OrderId.Of(dbId));

        // one customer has one or more orders
        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .IsRequired();

        // one order has one or more order items
        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);

        builder.ComplexProperty(
            o => o.OrderName, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName(nameof(OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
            });

        builder.ComplexProperty(
            o => o.ShippingAddress, addressBuilder => ConfigureAddress(addressBuilder));


        builder.ComplexProperty(
           o => o.BillingAddress, addressBuilder => ConfigureAddress(addressBuilder));

        builder.ComplexProperty(
            o => o.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(p => p.CardHolderName)
                    .HasMaxLength(100)
                    .IsRequired();

                paymentBuilder.Property(p => p.CardNumber)
                    .HasMaxLength(50)
                    .IsRequired();

                paymentBuilder.Property(p => p.Expiration)
                    .HasMaxLength(10)
                    .IsRequired();

                paymentBuilder.Property(p => p.CVV)
                    .HasMaxLength(3)
                    .IsRequired();

                paymentBuilder.Property(p => p.PaymentMethod);
            });

        builder.Property(o => o.Status)
            .HasDefaultValue(OrderStatus.Draft)
            .HasConversion(
                status => status.ToString(),
                dbStatus => Enum.Parse<OrderStatus>(dbStatus));

        //builder.Property(o => o.TotalPrice);
        builder.Ignore(o => o.TotalPrice);
    }

    private static void ConfigureAddress(ComplexPropertyBuilder<Address> addressBuilder)
    {
        addressBuilder.Property(a => a.FirstName)
                   .HasMaxLength(50)
                   .IsRequired();

        addressBuilder.Property(a => a.LastName)
            .HasMaxLength(50)
            .IsRequired();

        addressBuilder.Property(a => a.EmailAddress)
           .HasMaxLength(50);

        addressBuilder.Property(a => a.AddressLine)
            .HasMaxLength(180)
            .IsRequired();

        addressBuilder.Property(a => a.PhoneNumber)
           .HasMaxLength(20);

        addressBuilder.Property(a => a.City)
            .HasMaxLength(50)
            .IsRequired();

        addressBuilder.Property(a => a.State)
            .HasMaxLength(50);

        addressBuilder.Property(a => a.Country)
            .HasMaxLength(50)
            .IsRequired();

        addressBuilder.Property(a => a.ZipCode)
            .HasMaxLength(6)
            .IsRequired();
    }
}

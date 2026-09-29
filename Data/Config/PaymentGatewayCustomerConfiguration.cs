using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentGateway.Models.Customers;
using PaymentGateway.Models.Info;

namespace PaymentGateway.Data.Config;

public class PaymentGatewayCustomerConfiguration : IEntityTypeConfiguration<PaymentGatewayCustomer>
{

    public void Configure(EntityTypeBuilder<PaymentGatewayCustomer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.UserId);

        builder.Property(c => c.GatewayId)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(c => c.Document)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(c => c.Gender)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.Document).IsUnique();
        builder.HasIndex(c => c.Email).IsUnique();

        builder.HasOne(c => c.Address)
            .WithOne()
            .HasForeignKey<Address>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Phones)
            .WithOne()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Metadata)
            .WithOne()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
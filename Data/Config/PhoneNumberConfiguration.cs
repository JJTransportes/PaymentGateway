using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentGateway.Enums;
using PaymentGateway.Models.Info;

namespace PaymentGateway.Data.Config;

public class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumber>
{
    public void Configure(EntityTypeBuilder<PhoneNumber> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.Property(p => p.Number)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.AreaCode)
            .IsRequired()
            .HasMaxLength(5);

        builder.Property(p => p.CountryCode)
            .IsRequired()
            .HasMaxLength(5);

        builder.Property(p => p.Type)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion<string>()
            .HasDefaultValue(PhoneType.Mobile);
    }
}

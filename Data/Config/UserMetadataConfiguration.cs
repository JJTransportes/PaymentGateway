using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentGateway.Models.Info;

namespace PaymentGateway.Data.Config;

public class UserMetadataConfiguration : IEntityTypeConfiguration<UserMetadata>
{
    public void Configure(EntityTypeBuilder<UserMetadata> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Value)
            .HasMaxLength(500);
    }
}
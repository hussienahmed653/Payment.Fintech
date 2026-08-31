namespace Merchant.Infrastructure.Merchants.Persistence;

internal class MerchantConfiguration : IEntityTypeConfiguration<Domain.Entities.Merchant>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Merchant> builder)
    {
        builder.Property(m => m.ContactFirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.ContactLastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(m => m.Email)
            .IsUnique();

        builder.Property(m => m.Phone)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(m => m.BusinessName)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(m => m.BusinessType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(m => m.TaxId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.Status)
            .HasConversion<string>()
            .IsRequired();

    }
}

namespace Customer.Infrastructure.Customers.Persistence;

internal class CustomerConfiguration : IEntityTypeConfiguration<Domain.Entities.Customer>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Customer> builder)
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



using System.Security.Cryptography;

namespace PaymentRequest.Infrastructure.Merchants.Persistence;

internal class PaymentRequestConfiguration : IEntityTypeConfiguration<Domain.Entities.PaymentRequest>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.PaymentRequest> builder)
    {
        builder.Property(x => x.Reference)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Reference)
            .IsUnique();

        builder.Property(x => x.Currency)
            .HasConversion<string>()
            .IsRequired()
            .HasDefaultValue(PaymentCurrency.EGP);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasDefaultValue(PaymentStatus.Processing);
    }
}

namespace PaymentRequest.Infrastructure.PaymentTransactions.Persistence;

internal class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.HasIndex(p => p.IdemPotency)
            .IsUnique();

        builder.Property(p => p.IdemPotency)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(200);

        builder.Property(p => p.ExternalTransactionId)
            .IsRequired(false)
            .HasMaxLength(128);

        builder.Property(p => p.FailureResponse)
            .IsRequired(false)
            .HasMaxLength(255);

        builder.Property(p => p.ResponsePayload)
            .IsRequired(false);
    }
}

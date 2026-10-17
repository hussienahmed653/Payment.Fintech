using ResultPattern.Abstraction;

namespace Shared.Messaging;

public record UpdateCustomerMessage(PaymentEnvelope Envelope);

public record InitiateDepositMessage(PaymentEnvelope Envelope);

public record CreatePaymentMessage(PaymentEnvelope Envelope);

public record MessagingCompleted(string? PayloadJson = null);

public record MessagingFaulted(Error Error);

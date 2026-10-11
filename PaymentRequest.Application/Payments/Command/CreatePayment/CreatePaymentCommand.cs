using MediatR;
using Shared;

namespace PaymentRequest.Application.Payments.Command.CreatePayment;

public record CreatePaymentCommand(PaymentEnvelope Request) : IRequest<Result<PaymentResponse>>;

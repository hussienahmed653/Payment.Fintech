using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentRequest.Application.Payments.Command.PayPayment;

public record PayPaymentCommand(string reference, string idempotencyId) : IRequest<Result<PayPaymentResponse>>;
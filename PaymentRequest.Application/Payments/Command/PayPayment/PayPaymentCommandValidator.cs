using PaymentRequest.Application.Payments.Command.PayPayment;

namespace PaymentRequest.Application.Payments.Command.PayPayment;

public class PayPaymentCommandValidator : AbstractValidator<PayPaymentCommand>
{
    public PayPaymentCommandValidator()
    {
        RuleFor(x => x.reference)
            .NotEmpty()
            .WithMessage("Reference is required.")
            .MaximumLength(100)
            .WithMessage("Reference cannot exceed 100 characters.");
    }
}

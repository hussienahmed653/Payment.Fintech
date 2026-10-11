using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace PaymentRequest.Application.Payments.Command.CreatePayment;

public class PayPaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public PayPaymentCommandValidator()
    {
        RuleFor(m => m.Request.payment.MerchantId)
           .NotEmpty();

        RuleFor(m => m.Request.payment.MerchantGuid)
           .NotEmpty();

        RuleFor(m => m.Request.payment.Amount)
            .NotEmpty();

        //RuleFor(m => m.Request.Currency)
        //    .IsInEnum();
    }
}

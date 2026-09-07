using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PaymentRequest.Application.Merchant.Command.CreateMerchant;

namespace PaymentRequest.Application.Payments.Command.CreatePayment;

public class PayPaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public PayPaymentCommandValidator()
    {
        RuleFor(m => m.Request.MerchantId)
           .NotEmpty();

        RuleFor(m => m.Request.MerchantGuid)
           .NotEmpty();

        RuleFor(m => m.Request.CustomerId)
           .NotEmpty();

        RuleFor(m => m.Request.CustomerGuid)
           .NotEmpty();

        RuleFor(m => m.Request.Amount)
            .NotEmpty();

        RuleFor(m => m.Request.Currency)
            .IsInEnum();
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace PaymentRequest.Application.Merchant.Command.CreateMerchant;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
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

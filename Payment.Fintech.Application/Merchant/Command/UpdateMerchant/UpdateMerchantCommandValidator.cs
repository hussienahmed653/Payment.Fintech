using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Payment.Fintech.Domain.Entities;

namespace Payment.Fintech.Application.Merchant.Command.UpdateMerchant;

public class UpdateMerchantCommandValidator : AbstractValidator<UpdateMerchantCommand>
{
    public UpdateMerchantCommandValidator()
    {
        RuleFor(m => m.Request.ContactFirstName)
           .Length(3, 100)
           .WithMessage("{PropertyName} must be between 3 and 100 characters long.");

        RuleFor(m => m.Request.ContactLastName)
           .Length(3, 100)
           .WithMessage("'Contact Last Name' must be between 3 and 100 characters long.");

        RuleFor(m => m.Request.Email)
            .EmailAddress()
            .WithMessage("'Email' is not valid.")
            .MaximumLength(100)
            .WithMessage("'Email' must be between 3 and 100 characters long.");

        RuleFor(m => m.Request.Phone)
            .Length(10, 30)
            .WithMessage("'Phone' number must be between 10 and 30 characters long.");


        RuleFor(m => m.Request.BusinessName)
            .Length(5, 1000)
            .WithMessage("'Business Name' must be between 5 and 1000 characters long.");

        RuleFor(m => m.Request.BusinessType)
            .Must(value => Enum.IsDefined(typeof(BusinessType), value))
            .WithMessage("'Business Type' is not valid.")
            .When(m => m.Request.BusinessType.HasValue);
    }
}

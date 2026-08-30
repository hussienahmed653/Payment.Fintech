using System.Net.NetworkInformation;

namespace Payment.Fintech.Application.Merchant.Command.CreateMerchant;

public class CreateMerchantCommandValidator : AbstractValidator<CreateMerchantCommand>
{
    public CreateMerchantCommandValidator()
    {
        RuleFor(m => m.Request.ContactFirstName)
           .NotEmpty()
           .WithMessage("'Contact First Name' is required.")
           .Length(3,100)
           .WithMessage("'Contact First Name' must be between 3 and 100 characters long.");

        RuleFor(m => m.Request.ContactLastName)
           .NotEmpty()
           .WithMessage("'Contact Last Name' is required.")
           .Length(3, 100)
           .WithMessage("'Contact Last Name' must be between 3 and 100 characters long.");

        RuleFor(m => m.Request.Email)
            .NotEmpty()
            .WithMessage("'Email' is required.")
            .EmailAddress()
            .WithMessage("'Email' is not valid.")
            .MaximumLength(100)
            .WithMessage("'Email' must be between 3 and 100 characters long.");

        RuleFor(m => m.Request.Phone)
            .NotEmpty()
            .WithMessage("'Phone' number is required.")
            .Length(10,30)
            .WithMessage("'Phone' number must be between 10 and 30 characters long.");


        RuleFor(m => m.Request.BusinessName)
            .NotEmpty()
            .WithMessage("'Business Name' is required.")
            .Length(5,1000)
            .WithMessage("'Business Name' must be between 5 and 1000 characters long.");

        RuleFor(m => m.Request.BusinessType)
            .Must(value => Enum.IsDefined(typeof(BusinessType), value))
            .WithMessage("'Business Type' is not valid.");

        RuleFor(m => m.Request.TaxId)
            .NotEmpty()
            .WithMessage("'Tax ID' is required.")
            .MaximumLength(50)
            .WithMessage("'Tax ID' must be between 1 and 50 characters long.");

        RuleFor(m => m.Request.Status)
            .Must(value => Enum.IsDefined(typeof(MerchantStatus), value))
            .WithMessage("'Status' is not valid.");
    }
}

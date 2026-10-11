namespace Customer.Application.Customer.Command.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        When(x => x.Request != null && x.Request.customer != null, () =>
        {
            RuleFor(m => m.Request.customer.ContactFirstName)
           .Length(3, 100)
           .WithMessage("{PropertyName} must be between 3 and 100 characters long.");

            RuleFor(m => m.Request.customer.ContactLastName)
               .Length(3, 100)
               .WithMessage("'Contact Last Name' must be between 3 and 100 characters long.");

            RuleFor(m => m.Request.customer.Email)
                .EmailAddress()
                .WithMessage("'Email' is not valid.")
                .MaximumLength(100)
                .WithMessage("'Email' must be between 3 and 100 characters long.");

            RuleFor(m => m.Request.customer.Phone)
                .Length(10, 30)
                .WithMessage("'Phone' number must be between 10 and 30 characters long.");


            RuleFor(m => m.Request.customer.BusinessName)
                .Length(5, 1000)
                .WithMessage("'Business Name' must be between 5 and 1000 characters long.");

            //RuleFor(m => m.Request.customer.BusinessType)
            //    .Must(value => Enum.IsDefined(typeof(BusinessType), value))
            //    .WithMessage("'Business Type' is not valid.")
            //    .When(m => m.Request.customer.BusinessType);
        });
        
    }
}

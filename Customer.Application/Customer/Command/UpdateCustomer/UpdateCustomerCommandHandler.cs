using Customer.Domain.Entities;
using Shared.Sections;

namespace Customer.Application.Customer.Command.UpdateCustomer;

public class UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCustomerCommand, Result<CustomerResponse>>
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<CustomerResponse>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if (await _customerRepository.GetCustomerByGuidAsync((Guid)request.Request.payment.CustomerGuid, cancellationToken) is not { } customer)
                return Result.Failure<CustomerResponse>(CustomerErrors.CustomerNotFound);

            if (IsEmailChanged(request.Request.customer.Email, customer.Email))
            {
                if (await _customerRepository.EmailIsExistsAsync(request.Request.customer.Email, cancellationToken))
                    return Result.Failure<CustomerResponse>(CustomerErrors.EmailDublicated);
            }

            UpdatedProperty(customer, request.Request.customer);

            await _customerRepository.UpdateCustomerAsync(customer);

            var TotalChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (TotalChanges == 0)
                return Result.Failure<CustomerResponse>(CustomerErrors.ZeroRowsAffected);
            if (TotalChanges > 1)
                return Result.Failure<CustomerResponse>(CustomerErrors.MultibleRowsAffected);

            //await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success(customer.Adapt<CustomerResponse>());
        }
        catch (Exception ex)
        {
            //await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<CustomerResponse>(new Error(ex.Message, ex.StackTrace, StatusCodes.Status400BadRequest));
        }
    }

    private bool IsEmailChanged(string requestEmail, string EntityEmail)
        => !string.IsNullOrWhiteSpace(requestEmail) &&
                !string.Equals(requestEmail, EntityEmail, StringComparison.OrdinalIgnoreCase);

    private void UpdatedProperty(Domain.Entities.Customer customer, CustomerRequestSection request)
    {
        if (!string.IsNullOrWhiteSpace(request.ContactFirstName))
            customer.ContactFirstName = request.ContactFirstName;

        if (!string.IsNullOrWhiteSpace(request.ContactLastName))
            customer.ContactLastName = request.ContactLastName;

        if (!string.IsNullOrWhiteSpace(request.Email))
            customer.Email = request.Email;

        if (!string.IsNullOrWhiteSpace(request.Phone))
            customer.Phone = request.Phone;

        if (!string.IsNullOrWhiteSpace(request.BusinessName))
            customer.BusinessName = request.BusinessName;

        if (!string.IsNullOrWhiteSpace(request.BusinessType.ToString()))
            customer.BusinessType = (Domain.Enums.BusinessType)request.BusinessType!;
    }
}

using Customer.Domain.Entities;
using Shared.Sections;

namespace Customer.Application.Customer.Command.UpdateCustomer;

public class UpdateCustomerCommandHandler(ICustomerRepository merchantRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCustomerCommand, Result<CustomerResponse>>
{
    private readonly ICustomerRepository _merchantRepository = merchantRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<CustomerResponse>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if (await _merchantRepository.GetCustomerByGuidAsync(request.Request.payment.CustomerGuid, cancellationToken) is not { } merchant)
                return Result.Failure<CustomerResponse>(CustomerErrors.CustomerNotFound);

            if (IsEmailChanged(request.Request.customer.Email, merchant.Email))
            {
                if (await _merchantRepository.EmailIsExistsAsync(request.Request.customer.Email, cancellationToken))
                    return Result.Failure<CustomerResponse>(CustomerErrors.EmailDublicated);
            }

            UpdatedProperty(merchant, request.Request.customer);

            await _merchantRepository.UpdateCustomerAsync(merchant);

            var TotalChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (TotalChanges == 0)
                return Result.Failure<CustomerResponse>(CustomerErrors.ZeroRowsAffected);
            if (TotalChanges > 1)
                return Result.Failure<CustomerResponse>(CustomerErrors.MultibleRowsAffected);

            //await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success(merchant.Adapt<CustomerResponse>());
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

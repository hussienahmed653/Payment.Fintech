using Azure.Core;

namespace Customer.Application.Customer.Command.UpdateCustomer;

public class UpdateCustomerCommandHandler(ICustomerRepository merchantRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCustomerCommand, Result<CustomerResponse>>
{
    private readonly ICustomerRepository _merchantRepository = merchantRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<CustomerResponse>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if (await _merchantRepository.GetCustomerByGuidAsync(request.Request.Guid, cancellationToken) is not { } merchant)
                return Result.Failure<CustomerResponse>(CustomerErrors.CustomerNotFound);

            if (EmailIsNotValid(request.Request.Email, merchant.Email))
            {
                if (await _merchantRepository.EmailIsExistsAsync(request.Request.Email, cancellationToken))
                    return Result.Failure<CustomerResponse>(CustomerErrors.EmailDublicated);
            }

            UpdatedProperty(merchant, request.Request);

            await _merchantRepository.UpdateCustomerAsync(merchant);

            var TotalChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (TotalChanges == 0)
                return Result.Failure<CustomerResponse>(CustomerErrors.ZeroRowsAffected);
            if (TotalChanges > 1)
                return Result.Failure<CustomerResponse>(CustomerErrors.MultibleRowsAffected);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success(merchant.Adapt<CustomerResponse>());
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<CustomerResponse>(new Error(ex.Message, ex.StackTrace, StatusCodes.Status400BadRequest));
        }
    }

    private bool EmailIsNotValid(string requestEmail, string EntityEmail)
        => !string.IsNullOrWhiteSpace(requestEmail) &&
                !string.Equals(requestEmail, EntityEmail, StringComparison.OrdinalIgnoreCase);

    private void UpdatedProperty(Domain.Entities.Customer merchant, UpdateCustomerRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.ContactFirstName))
            merchant.ContactFirstName = request.ContactFirstName;

        if (!string.IsNullOrWhiteSpace(request.ContactLastName))
            merchant.ContactLastName = request.ContactLastName;

        if (!string.IsNullOrWhiteSpace(request.Email))
            merchant.Email = request.Email;

        if (!string.IsNullOrWhiteSpace(request.Phone))
            merchant.Phone = request.Phone;

        if (!string.IsNullOrWhiteSpace(request.BusinessName))
            merchant.BusinessName = request.BusinessName;

        if (!string.IsNullOrWhiteSpace(request.BusinessType.ToString()))
            merchant.BusinessType = (BusinessType)request.BusinessType!;
    }
}

using Azure.Core;

namespace Merchant.Application.Merchant.Command.UpdateMerchant;

public class UpdateMerchantCommandHandler(IMerchantRepository merchantRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateMerchantCommand, Result<MerchantResponse>>
{
    private readonly IMerchantRepository _merchantRepository = merchantRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<MerchantResponse>> Handle(UpdateMerchantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if (await _merchantRepository.GetMerchantByGuidAsync(request.Request.Guid, cancellationToken) is not { } merchant)
                return Result.Failure<MerchantResponse>(MerchantErrors.MerchantNotFound);

            if (EmailIsNotValid(request.Request.Email, merchant.Email))
            {
                if (await _merchantRepository.EmailIsExistsAsync(request.Request.Email, cancellationToken))
                    return Result.Failure<MerchantResponse>(MerchantErrors.EmailDublicated);
            }

            UpdatedProperty(merchant, request.Request);

            await _merchantRepository.UpdateMerchantAsync(merchant);

            var TotalChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (TotalChanges == 0)
                return Result.Failure<MerchantResponse>(MerchantErrors.ZeroRowsAffected);
            if (TotalChanges > 1)
                return Result.Failure<MerchantResponse>(MerchantErrors.MultibleRowsAffected);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success(merchant.Adapt<MerchantResponse>());
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<MerchantResponse>(new Error(ex.Message, ex.StackTrace, StatusCodes.Status400BadRequest));
        }
    }

    private bool EmailIsNotValid(string requestEmail, string EntityEmail)
        => !string.IsNullOrWhiteSpace(requestEmail) &&
                !string.Equals(requestEmail, EntityEmail, StringComparison.OrdinalIgnoreCase);

    private void UpdatedProperty(Domain.Entities.Merchant merchant, UpdateMerchantRequest request)
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

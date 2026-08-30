namespace Payment.Fintech.Application.Merchant.Command.UpdateMerchant;

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

            if(!string.IsNullOrWhiteSpace(request.Request.Email) &&
                !string.Equals(request.Request.Email, merchant.Email, StringComparison.OrdinalIgnoreCase))
            {
                if (await _merchantRepository.EmailIsExistsAsync(request.Request.Email, cancellationToken))
                    return Result.Failure<MerchantResponse>(MerchantErrors.EmailDublicated);
            }

            merchant.UpdateProfile(request.Request.ContactFirstName,
                                    request.Request.ContactLastName,
                                    request.Request.Email,
                                    request.Request.Phone,
                                    request.Request.BusinessName,
                                    request.Request.BusinessType);

            await _merchantRepository.UpdateMerchantAsync(merchant);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success(merchant.Adapt<MerchantResponse>());
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<MerchantResponse>(new Error(ex.Message, ex.StackTrace, StatusCodes.Status400BadRequest));
        }
    }
}

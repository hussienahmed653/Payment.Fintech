namespace Payment.Fintech.Application.Merchant.Query.GetMerchantByGuid;

public class GetMerchantByGuidQueryHandler(IMerchantRepository merchantRepository,
                                           IUnitOfWork unitOfWork) : IRequestHandler<GetMerchantByGuidQuery, Result<MerchantResponse>>
{
    private readonly IMerchantRepository _merchantRepository = merchantRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<MerchantResponse>> Handle(GetMerchantByGuidQuery request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if (await _merchantRepository.GetMerchantByGuidAsync(request.Guid.Value, cancellationToken) is not { } merchant)
                return Result.Failure<MerchantResponse>(MerchantErrors.MerchantNotFound);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success(merchant.Adapt<MerchantResponse>());
        }
        catch(Exception ex) 
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<MerchantResponse>(new Error(ex.Message, ex.StackTrace, StatusCodes.Status400BadRequest));
        }
    }
}

namespace Payment.Fintech.Application.Merchant.Query.GetMerchantByBusinessType;

public class GetMerchantByBusinessTypeQueryHandler(IMerchantRepository merchantRepository,
                                                     IUnitOfWork unitOfWork) : IRequestHandler<GetMerchantByBusinessTypeQuery, Result<IEnumerable<MerchantResponse>>>
{
    private readonly IMerchantRepository _merchantRepository = merchantRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IEnumerable<MerchantResponse>>> Handle(GetMerchantByBusinessTypeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            var merchants = await _merchantRepository.GetMerchantByBusinessTypeAsync(request.BusinessType, cancellationToken);
            if(merchants.Count() == 0)
                return Result.Failure<IEnumerable<MerchantResponse>>(MerchantErrors.BusinessTypeNotFound);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success(merchants);
        }
        catch(Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<IEnumerable<MerchantResponse>>(new Error(ex.Message, ex.StackTrace, StatusCodes.Status400BadRequest));
        }
    }
}
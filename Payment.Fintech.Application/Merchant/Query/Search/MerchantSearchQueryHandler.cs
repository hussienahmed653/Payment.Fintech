namespace Payment.Fintech.Application.Merchant.Query.Search;

public class MerchantSearchQueryHandler(IUnitOfWork unitOfWork,
                                        IMerchantRepository merchantRepository) : IRequestHandler<MerchantSearchQuery, Result<IEnumerable<MerchantResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMerchantRepository _merchantRepository = merchantRepository;

    public async Task<Result<IEnumerable<MerchantResponse>>> Handle(MerchantSearchQuery request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if(!string.IsNullOrWhiteSpace(request.Search))
            {
                var merchants = await _merchantRepository.SearchAsync(request.Search);
                if (!merchants.Any())
                    return Result.Failure<IEnumerable<MerchantResponse>>(MerchantErrors.SearchNotFound);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return Result.Success(merchants.Adapt<IEnumerable<MerchantResponse>>());
            }
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<IEnumerable<MerchantResponse>>(MerchantErrors.SearchKeyWordNotFound);
        }
        catch(Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<IEnumerable<MerchantResponse>>(new Error(ex.Message, ex.StackTrace, StatusCodes.Status400BadRequest));
        }
    }
}

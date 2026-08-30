namespace Payment.Fintech.Application.Merchant.Query.Filter;

public class MerchantFilterQueryHandler(IUnitOfWork unitOfWork, IMerchantRepository merchantRepository) : IRequestHandler<MerchantFilterQuery, Result<IEnumerable<MerchantResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMerchantRepository _merchantRepository = merchantRepository;

    public async Task<Result<IEnumerable<MerchantResponse>>> Handle(MerchantFilterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var filter = new MerchantFilterSpecification(request.Request);
            var merchantsResult = await _merchantRepository.FilterAsync(filter, cancellationToken);

            if (!merchantsResult.Any())
                return Result.Failure<IEnumerable<MerchantResponse>>(MerchantErrors.Filter);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success(merchantsResult.Adapt<IEnumerable<MerchantResponse>>());
        }
        catch(Exception ex) 
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<IEnumerable<MerchantResponse>>(new Error(ex.Message, ex.StackTrace, StatusCodes.Status400BadRequest));
        }
    }
}
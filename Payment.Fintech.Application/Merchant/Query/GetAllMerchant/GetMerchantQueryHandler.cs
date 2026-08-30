namespace Payment.Fintech.Application.Merchant.Query.GetAllMerchant;

public class GetMerchantQueryHandler(IUnitOfWork unitOfWork, IMerchantRepository merchantRepository) : IRequestHandler<GetMerchantQuery, Result<IEnumerable<MerchantResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMerchantRepository _merchantRepository = merchantRepository;

    public async Task<Result<IEnumerable<MerchantResponse>>> Handle(GetMerchantQuery request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            
            var merchants = await _merchantRepository.GetMerchantAsync(cancellationToken);
            if (merchants.Count() == 0)
                return Result.Failure<IEnumerable<MerchantResponse>>(MerchantErrors.MerchantNotFound);
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

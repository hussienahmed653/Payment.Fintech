namespace Merchant.Application.Merchant.Command.CreateMerchant;

public class CreateMerchantCommandHandler(IUnitOfWork unitOfWork, IMerchantRepository merchantRepository) : IRequestHandler<CreateMerchantCommand, Result<MerchantResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMerchantRepository _merchantRepository = merchantRepository;

    public async Task<Result<MerchantResponse>> Handle(CreateMerchantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if (await _merchantRepository.EmailIsExistsAsync(request.Request.Email, cancellationToken))
                return Result.Failure<MerchantResponse>(MerchantErrors.EmailDublicated);
            var merchant = await _merchantRepository.CreateMerchantAsync(request.Request, cancellationToken);
            var TotalChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (TotalChanges == 0)
                return Result.Failure<MerchantResponse>(MerchantErrors.ZeroRowsAffected);
            if (TotalChanges > 1)
                return Result.Failure<MerchantResponse>(MerchantErrors.MultibleRowsAffected);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success(merchant);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<MerchantResponse>(new Error("CreateMerchantFailed", ex.Message, StatusCodes.Status500InternalServerError));
        }
    }
}

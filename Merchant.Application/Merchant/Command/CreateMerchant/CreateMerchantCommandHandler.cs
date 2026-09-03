namespace Merchant.Application.Merchant.Command.CreateMerchant;

public class CreateMerchantCommandHandler(IUnitOfWork unitOfWork, IMerchantRepository merchantRepository) : IRequestHandler<CreateMerchantCommand, Result<MerchantResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMerchantRepository _merchantRepository = merchantRepository;

    public async Task<Result<MerchantResponse>> Handle(CreateMerchantCommand request, CancellationToken cancellationToken)
    {
        if (await _merchantRepository.EmailIsExistsAsync(request.Request.Email, cancellationToken))
            return Result.Failure<MerchantResponse>(MerchantErrors.EmailDublicated);

        var merchant = await _merchantRepository.CreateMerchantAsync(request.Request, cancellationToken);

        var TotalChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (TotalChanges == 0)
            return Result.Failure<MerchantResponse>(MerchantErrors.ZeroRowsAffected);
        if (TotalChanges > 1)
            return Result.Failure<MerchantResponse>(MerchantErrors.MultibleRowsAffected);

        return Result.Success(merchant);
    }
}

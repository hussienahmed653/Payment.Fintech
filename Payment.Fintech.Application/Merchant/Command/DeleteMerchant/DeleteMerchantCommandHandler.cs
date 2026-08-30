namespace Payment.Fintech.Application.Merchant.Command.DeleteMerchant;

public class DeleteMerchantCommandHandler(IUnitOfWork unitOfWork,
                                          IMerchantRepository merchantRepository) : IRequestHandler<DeleteMerchantCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMerchantRepository _merchantRepository = merchantRepository;

    public async Task<Result> Handle(DeleteMerchantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if (await _merchantRepository.GetMerchantByGuidAsync(request.Guid, cancellationToken) is not { } merchant)
                return Result.Failure(MerchantErrors.MerchantNotFound);

            await _merchantRepository.DeleteMerchantAsync(merchant, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success();
        }
        catch(Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure(new Error(ex.Message, ex.StackTrace, StatusCodes.Status400BadRequest));
        }
    }
}

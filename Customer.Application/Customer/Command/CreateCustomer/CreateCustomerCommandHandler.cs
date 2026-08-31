namespace Customer.Application.Customer.Command.CreateCustomer;

public class CreateCustomerCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository merchantRepository) : IRequestHandler<CreateCustomerCommand, Result<CustomerResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICustomerRepository _merchantRepository = merchantRepository;

    public async Task<Result<CustomerResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if (await _merchantRepository.EmailIsExistsAsync(request.Request.Email, cancellationToken))
                return Result.Failure<CustomerResponse>(CustomerErrors.EmailDublicated);
            var merchant = await _merchantRepository.CreateCustomerAsync(request.Request, cancellationToken);
            var TotalChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (TotalChanges == 0)
                return Result.Failure<CustomerResponse>(CustomerErrors.ZeroRowsAffected);
            if (TotalChanges > 1)
                return Result.Failure<CustomerResponse>(CustomerErrors.MultibleRowsAffected);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success(merchant);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<CustomerResponse>(new Error("CreateMerchantFailed", ex.Message, StatusCodes.Status500InternalServerError));
        }
    }
}

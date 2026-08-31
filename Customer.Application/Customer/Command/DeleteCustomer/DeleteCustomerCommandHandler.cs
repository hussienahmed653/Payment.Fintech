namespace Customer.Application.Customer.Command.DeleteCustomer;

public class DeleteCustomerCommandHandler(IUnitOfWork unitOfWork,
                                          ICustomerRepository merchantRepository) : IRequestHandler<DeleteCustomerCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICustomerRepository _merchantRepository = merchantRepository;

    public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            if (await _merchantRepository.GetCustomerByGuidAsync(request.Guid, cancellationToken) is not { } merchant)
                return Result.Failure(CustomerErrors.CustomerNotFound);

            await _merchantRepository.DeleteCustomerAsync(merchant, cancellationToken);
            var TotalChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (TotalChanges == 0)
                return Result.Failure<CustomerResponse>(CustomerErrors.ZeroRowsAffected);
            if (TotalChanges > 1)
                return Result.Failure<CustomerResponse>(CustomerErrors.MultibleRowsAffected);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure(new Error(ex.Message, ex.StackTrace, StatusCodes.Status400BadRequest));
        }
    }
}

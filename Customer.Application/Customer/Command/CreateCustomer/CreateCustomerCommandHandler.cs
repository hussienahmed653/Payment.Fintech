namespace Customer.Application.Customer.Command.CreateCustomer;

public class CreateCustomerCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository) : IRequestHandler<CreateCustomerCommand, Result<CustomerResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<Result<CustomerResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        if (await _customerRepository.EmailIsExistsAsync(request.Request.Email, cancellationToken))
            return Result.Failure<CustomerResponse>(CustomerErrors.EmailDublicated);

        var customer = await _customerRepository.CreateCustomerAsync(request.Request, cancellationToken);

        var TotalChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (TotalChanges == 0)
            return Result.Failure<CustomerResponse>(CustomerErrors.ZeroRowsAffected);
        if (TotalChanges > 1)
            return Result.Failure<CustomerResponse>(CustomerErrors.MultibleRowsAffected);

        return Result.Success(customer);
    }
}

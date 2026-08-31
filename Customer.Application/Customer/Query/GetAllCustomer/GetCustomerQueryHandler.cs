namespace Customer.Application.Customer.Query.GetAllCustomer;

public class GetCustomerQueryHandler(ICustomerRepository merchantRepository) : IRequestHandler<GetCustomerQuery, Result<IEnumerable<CustomerResponse>>>
{
    private readonly ICustomerRepository _merchantRepository = merchantRepository;

    public async Task<Result<IEnumerable<CustomerResponse>>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var merchants = await _merchantRepository.GetCustomerAsync(cancellationToken);
        if (merchants.Count() == 0)
            return Result.Failure<IEnumerable<CustomerResponse>>(CustomerErrors.CustomerNotFound);
        return Result.Success(merchants);
    }
}

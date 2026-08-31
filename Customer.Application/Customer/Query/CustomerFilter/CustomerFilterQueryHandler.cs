namespace Customer.Application.Customer.Query.CustomerFilter;

public class CustomerFilterQueryHandler(ICustomerRepository merchantRepository) : IRequestHandler<CustomerFilterQuery, Result<IEnumerable<CustomerResponse>>>
{
    private readonly ICustomerRepository _merchantRepository = merchantRepository;

    public async Task<Result<IEnumerable<CustomerResponse>>> Handle(CustomerFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = new CustomerFilterSpecification(request.Request);
        var merchantsResult = await _merchantRepository.FilterAsync(filter, cancellationToken);

        if (!merchantsResult.Any())
            return Result.Failure<IEnumerable<CustomerResponse>>(CustomerErrors.Filter);

        return Result.Success(merchantsResult.Adapt<IEnumerable<CustomerResponse>>());
    }
}
namespace Customer.Application.Customer.Query.CustomerSearch;

public class CustomerSearchQueryHandler(ICustomerRepository merchantRepository) : IRequestHandler<CustomerSearchQuery, Result<IEnumerable<CustomerResponse>>>
{
    private readonly ICustomerRepository _merchantRepository = merchantRepository;

    public async Task<Result<IEnumerable<CustomerResponse>>> Handle(CustomerSearchQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Search))
            return Result.Failure<IEnumerable<CustomerResponse>>(CustomerErrors.SearchKeyWordNotFound);

        var merchants = await _merchantRepository.SearchAsync(request.Search);
        if (!merchants.Any())
            return Result.Failure<IEnumerable<CustomerResponse>>(CustomerErrors.SearchNotFound);

        return Result.Success(merchants.Adapt<IEnumerable<CustomerResponse>>());
    }
}

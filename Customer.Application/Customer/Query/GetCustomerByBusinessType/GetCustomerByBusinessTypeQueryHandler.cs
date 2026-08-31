namespace Customer.Application.Customer.Query.GetCustomerByBusinessType;

public class GetCustomerByBusinessTypeQueryHandler(ICustomerRepository merchantRepository) : IRequestHandler<GetCustomerByBusinessTypeQuery, Result<IEnumerable<CustomerResponse>>>
{
    private readonly ICustomerRepository _merchantRepository = merchantRepository;

    public async Task<Result<IEnumerable<CustomerResponse>>> Handle(GetCustomerByBusinessTypeQuery request, CancellationToken cancellationToken)
    {
        var merchants = await _merchantRepository.GetCustomerByBusinessTypeAsync(request.BusinessType, cancellationToken);
        if (merchants.Count() == 0)
            return Result.Failure<IEnumerable<CustomerResponse>>(CustomerErrors.BusinessTypeNotFound);

        return Result.Success(merchants);
    }
}
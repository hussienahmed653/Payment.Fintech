namespace Customer.Application.Customer.Query.GetCustomerByGuid;

public class GetCustomerByGuidQueryHandler(ICustomerRepository merchantRepository) : IRequestHandler<GetCustomerByGuidQuery, Result<CustomerResponse>>
{
    private readonly ICustomerRepository _merchantRepository = merchantRepository;
    public async Task<Result<CustomerResponse>> Handle(GetCustomerByGuidQuery request, CancellationToken cancellationToken)
    {
        if (await _merchantRepository.GetCustomerByGuidAsync(request.Guid.Value, cancellationToken) is not { } merchant)
            return Result.Failure<CustomerResponse>(CustomerErrors.CustomerNotFound);

        return Result.Success(merchant.Adapt<CustomerResponse>());
    }
}

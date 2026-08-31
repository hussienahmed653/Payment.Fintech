
namespace Customer.Application.Customer.Query.GetCustomerByGuid;

public record GetCustomerByGuidQuery(Guid? Guid) : IRequest<Result<CustomerResponse>>;

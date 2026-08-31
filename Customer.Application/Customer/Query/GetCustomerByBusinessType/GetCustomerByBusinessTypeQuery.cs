namespace Customer.Application.Customer.Query.GetCustomerByBusinessType;

public record GetCustomerByBusinessTypeQuery(string BusinessType) : IRequest<Result<IEnumerable<CustomerResponse>>>;

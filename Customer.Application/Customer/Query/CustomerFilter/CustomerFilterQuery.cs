namespace Customer.Application.Customer.Query.CustomerFilter;

public record CustomerFilterQuery(CustomerFilterRequest Request) : IRequest<Result<IEnumerable<CustomerResponse>>>;

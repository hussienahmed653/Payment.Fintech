namespace Customer.Application.Customer.Query.CustomerSearch;

public record CustomerSearchQuery(string Search) : IRequest<Result<IEnumerable<CustomerResponse>>>;

using MediatR;

namespace Customer.Application.Customer.Query.GetAllCustomer;

public record GetCustomerQuery() : IRequest<Result<IEnumerable<CustomerResponse>>>;

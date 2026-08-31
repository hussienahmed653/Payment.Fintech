using MediatR;

namespace Customer.Application.Customer.Command.CreateCustomer;

public record CreateCustomerCommand(CustomerRequest Request) : IRequest<Result<CustomerResponse>>;

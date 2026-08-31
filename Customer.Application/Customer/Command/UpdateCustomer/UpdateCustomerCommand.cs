namespace Customer.Application.Customer.Command.UpdateCustomer;

public record UpdateCustomerCommand(UpdateCustomerRequest Request) : IRequest<Result<CustomerResponse>>;

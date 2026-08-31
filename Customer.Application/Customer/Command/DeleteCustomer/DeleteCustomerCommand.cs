namespace Customer.Application.Customer.Command.DeleteCustomer;

public record DeleteCustomerCommand(Guid Guid) : IRequest<Result>;

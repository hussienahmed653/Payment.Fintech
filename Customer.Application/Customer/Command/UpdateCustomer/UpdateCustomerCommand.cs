using Shared;

namespace Customer.Application.Customer.Command.UpdateCustomer;

public record UpdateCustomerCommand(PaymentEnvelope? Request) : IRequest<Result<CustomerResponse>>;

namespace Customer.Application.Customer.Command.ProcessCustomerPayment;

public record ProcessCustomerPaymentCommand(CustomerPaymentRequest Request) : IRequest<Result<object>>;

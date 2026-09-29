using System.Net.Http.Json;

namespace Customer.Application.Customer.Command.ProcessCustomerPayment;

public class ProcessCustomerPaymentCommandHandler(IHttpClientFactory httpClientFactory) : IRequestHandler<ProcessCustomerPaymentCommand, Result<object>>
{
    //private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<Result<object>> Handle(ProcessCustomerPaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var client = httpClientFactory.CreateClient("PaymentGateway");
            var response = await client.PostAsJsonAsync("api/payments", request, cancellationToken);
            if (response == null)
            {
                return Result.Failure<object>(new Error("Failed to process customer payment", "No response received from the payment gateway", StatusCodes.Status500InternalServerError));
            }
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
                return Result.Failure<object>(new Error("Failed to process customer payment", errorMessage, 500));
            }
            return Result.Success(response.Content.ReadFromJsonAsync<object>(cancellationToken).Result!);
        }
        catch
        {
            return Result.Failure<object>(new Error("An error occurred while processing the customer payment", "An unexpected error occurred", 500));
        }
    }
}
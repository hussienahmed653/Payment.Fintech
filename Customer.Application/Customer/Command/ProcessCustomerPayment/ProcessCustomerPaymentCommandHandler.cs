using PaymentRequest.Application.Payments.Command.PayPayment;
using System.Net.Http.Json;
using System.Text.Json;

namespace Customer.Application.Customer.Command.ProcessCustomerPayment;

public class ProcessCustomerPaymentCommandHandler(IHttpClientFactory httpClientFactory) : IRequestHandler<ProcessCustomerPaymentCommand, Result<object>>
{
    public async Task<Result<object>> Handle(ProcessCustomerPaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //any business logic
            // go to payment gateway and then call customer for any update instead of call customerfrom postman
            //
            var client = httpClientFactory.CreateClient("PaymentGateway");//ensure this key map to actuall url in Program.cs
            var response = await client.PostAsJsonAsync("api/payments", request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var error = await HttpErrorReader.ReadErrorAsync(response, cancellationToken);
                return Result.Failure<object>(error);
            }

            var result = await response.Content
                .ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken);
            return Result.Success<object>(result);
        }
        catch
        {
            return Result.Failure<object>(new Error("An error occurred while processing the customer payment", "An unexpected error occurred", 500));
        }
    }
}
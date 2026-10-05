using Mapster;
using PaymentProvider.Application.Common.Contracts.Deposit;
using PaymentProvider.Application.Common.Interfaces.Processor;
using ResultPattern.Abstraction;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PaymentProvider.Integration.MMO;

public class MMOProcessor(HttpClient httpClient) : IPaymentProcessor
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() }

    };

    public async Task<Result<object>> MakeRequest(InitiateDepositRequest request)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new
                System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJraWQiOiIxIiwiYWxnIjoiRVMyNTYifQ.eyJ0dCI6IkFBVCIsInN1YiI6IjI0ODQ0IiwibWF2IjoiMSIsImV4cCI6MjA5OTQ4NDA2MSwiaWF0IjoxNzgzODY0ODYxLCJwbSI6IkRBRixQQUYiLCJqdGkiOiJiNjhmOTcwMS1jYWI4LTQ5YzktYjhiYS1mNGI2YzRlYmNjNzMifQ.OvoidNagCkHYax3ksCBmVftHLOh5pbeji7CF2_LnL-KvHv7Ja9_1oVX-w-3D04d49RIHC2pdYOEOsdehG3eswA");
        var response = await _httpClient.PostAsJsonAsync("https://api.sandbox.pawapay.io/v2/deposits", request, _options);
        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            InitiateDepositResponse? errorResponse = null;
            try
            {
                errorResponse = JsonSerializer.Deserialize<InitiateDepositResponse>(body, _options);
            }
            catch (JsonException)
            {
            }
                return Result.Failure<object>(new Error(
                errorResponse?.FailureReason?.FailureCode.ToString() ?? FailureCode.DEPOSITS_NOT_ALLOWED.ToString(),
                errorResponse?.FailureReason?.FailureMessage ?? body,
                (int)response.StatusCode));
        }

        response.EnsureSuccessStatusCode();

        return Result.Success(JsonSerializer.Deserialize<InitiateDepositResponse>(body));

    }
}

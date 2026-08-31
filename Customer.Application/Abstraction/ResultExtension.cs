namespace Customer.Application.Abstraction;

public static class ResultExtension
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert a successful result to a problem.");

        var statusCode = result.Error.StatusCode ?? StatusCodes.Status400BadRequest;

        var errorsGrouped = result.Errors.Any()
        ? result.Errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Description).ToArray()
            )
        : new Dictionary<string, string[]>
        {
            {
                result.Error.Code, new[] { result.Error.Description }
            }
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = "An error occurred while processing your request.",
            Extensions = new Dictionary<string, object?>
            {
                {
                    "errors", errorsGrouped
                }
            }
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }
}

namespace Merchant.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next(cancellationToken);

        var errors = _validators
            .Select(validation => validation.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new Error
            (
                Code: failure.PropertyName.Split('.')[1],
                Description: failure.ErrorMessage,
                StatusCode: StatusCodes.Status400BadRequest
            ))
            .Distinct()
            .ToArray();
        if (errors.Any())
        {
            return CreateValidationErrorResponse<TResponse>(errors);
        }
        return await next(cancellationToken);
    }
    private static TResponse CreateValidationErrorResponse<TResponse>(Error[] errors)
    {
        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(errors);
        }
        return Activator.CreateInstance(typeof(TResponse), false, errors, default!) is TResponse response
        ? response
        : throw new InvalidOperationException("Unsupported response type for validation error response.");
    }
}


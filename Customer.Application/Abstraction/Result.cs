public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
            throw new InvalidOperationException("Invalid error state for Result.");

        IsSuccess = isSuccess;
        Error = error;
        Errors = error == Error.None ? Array.Empty<Error>() : new[] { error };
    }

    public Result(bool isSuccess, Error[] errors)
    {
        if ((isSuccess && errors.Length > 0) || (!isSuccess && (errors == null || errors.Length == 0)))
            throw new InvalidOperationException("Invalid errors state for Result.");

        IsSuccess = isSuccess;
        Errors = errors;
        Error = errors.FirstOrDefault() ?? Error.None;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    public Error[] Errors { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result Failure(Error[] errors) => new(false, errors);

    public static Result<T> Success<T>(T value) => new(true, Error.None, value);
    public static Result<T> Failure<T>(Error error) => new(false, error, default);
    public static Result<T> Failure<T>(Error[] errors) => new(false, errors, default);
}

public class Result<T> : Result
{
    private readonly T? _value;

    public Result(bool isSuccess, Error error, T? value)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public Result(bool isSuccess, Error[] errors, T? value)
        : base(isSuccess, errors)
    {
        _value = value;
    }

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    public static implicit operator Result<T>(T value) => Success(value);
}
namespace PaymentRequest.Application.Common.Interfaces.Redis;

public interface ICacheService
{
    Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task SetAsync(string key, TimeSpan expiry, CancellationToken cancellationToken = default);
    Task RemoveAsync<T>(string key, CancellationToken cancellationToken = default);
    Task<bool> AcquireLockAsync(string key, string value, TimeSpan expiry, CancellationToken cancellationToken = default);
    Task ReleaseLockAsync(string key, string value, CancellationToken cancellationToken = default);
}

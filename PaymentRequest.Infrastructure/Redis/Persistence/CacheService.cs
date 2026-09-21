
using StackExchange.Redis;
using System.Text.Json;

namespace PaymentRequest.Infrastructure.Redis.Persistence;

internal class CacheService(StackExchange.Redis.IDatabase database,
                                        IConnectionMultiplexer redis
                                        ) : ICacheService
{
    private readonly StackExchange.Redis.IDatabase _database = database;
    private readonly IConnectionMultiplexer _redis = redis;
    private JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false

        };


    public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if(_database.StringGetAsync(key) is not { } cachedValue)
        {
            return default!;
        }
        return JsonSerializer.Deserialize<T>(json: (await cachedValue)!, options: _jsonOptions)!;
    }


    public async Task RemoveAsync<T>(string key, CancellationToken cancellationToken = default) =>
        await _database.KeyDeleteAsync(key);

    public async Task SetAsync(string key, TimeSpan expiry, CancellationToken cancellationToken = default)
    {
        await _database.StringSetAsync(key, "placeholder", expiry);
    }
    public async Task<bool> AcquireLockAsync(string key, string value, TimeSpan expiry, CancellationToken cancellationToken = default) =>
        await _database.StringSetAsync(key, value, expiry, When.NotExists);
    public async Task ReleaseLockAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        var script = @"
            if redis.call('get', KEYS[1]) == ARGV[1] then
                return redis.call('del', KEYS[1])
            else
                return 0
            end";
        await _database.ScriptEvaluateAsync(script, [key], [value]);
    }
}

using Bank.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System.Text.Json;

namespace Bank.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cacheDb;

        public CacheService()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDb = redis.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
                
        }

        public object RemoveData(string key)
        {
            var exist = _cacheDb.KeyExists(key);
            if (exist)
            {
                return _cacheDb.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            try
            {
                var isSet = _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
                Console.WriteLine($"Set operation success: {isSet}");
                return isSet;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Redis set operation: {ex.Message}");
                return false;
            }
        }
    }
}

// in memory cache method
//public async Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> fetchFunction, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
//{
//    if(!_cache.TryGetValue(cacheKey, out T cacheEntry))
//    {
//        cacheEntry = await fetchFunction();
//        var cacheEntryOptions = new MemoryCacheEntryOptions();

//        if (absoluteExpireTime.HasValue)
//        {
//            cacheEntryOptions.SetAbsoluteExpiration(absoluteExpireTime.Value);
//        }
//        if (unusedExpireTime.HasValue)
//        {
//            cacheEntryOptions.SetSlidingExpiration(unusedExpireTime.Value);
//        }

//        _cache.Set(cacheKey, cacheEntry, cacheEntryOptions);
//    }
//    return cacheEntry;
//}

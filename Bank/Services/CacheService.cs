using Bank.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Bank.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public async Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> fetchFunction, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
        {
            if(!_cache.TryGetValue(cacheKey, out T cacheEntry))
            {
                cacheEntry = await fetchFunction();
                var cacheEntryOptions = new MemoryCacheEntryOptions();

                if (absoluteExpireTime.HasValue)
                {
                    cacheEntryOptions.SetAbsoluteExpiration(absoluteExpireTime.Value);
                }
                if (unusedExpireTime.HasValue)
                {
                    cacheEntryOptions.SetSlidingExpiration(unusedExpireTime.Value);
                }

                _cache.Set(cacheKey, cacheEntry, cacheEntryOptions);
            }
            return cacheEntry;
        }
    }
}

namespace Bank.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> fetchFunction, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null);

    }
}

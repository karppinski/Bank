namespace Bank.Interfaces
{
    public interface ICacheService
    {
        T GetData<T>(string key);
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
        object RemoveData(string key);
    }
}


//        Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> fetchFunction, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null);//


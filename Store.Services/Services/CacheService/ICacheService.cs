namespace Store.Services.Services.CacheService
{
    public interface ICacheService
    {
        Task SetCacheAsyn(string key, object response, TimeSpan timeToLive);
        Task<string> GetCacheAsyn(string key);

    }
}

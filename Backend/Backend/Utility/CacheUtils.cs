using Microsoft.Extensions.Caching.Memory;

namespace Backend.Utility;

public class CacheUtils
{
    private static readonly MemoryCache ObjCache = new MemoryCache(new MemoryCacheOptions());

    public static object GetCache(string cacheKey)
    {
        if (ObjCache.TryGetValue(cacheKey, out var result))
        {
            return result;
        }

        return null;
    }

    public static void SetCache(string cachekey, object objObject)
    {
        MemoryCacheEntryOptions options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1.0));
        ObjCache.Set(cachekey, objObject, options);
    }

    public static void SetCache(string cachekey, object objObject, int durationInDays)
    {
        MemoryCacheEntryOptions options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(durationInDays));
        ObjCache.Set(cachekey, objObject, options);
    }

    public static void SetCache(string cachekey, object objObject, TimeSpan absoluteExpiration)
    {
        MemoryCacheEntryOptions options = new MemoryCacheEntryOptions().SetSlidingExpiration(absoluteExpiration);
        ObjCache.Set(cachekey, objObject, options);
    }

    public static void RemoveCache(string cacheKey)
    {
        if (ObjCache.Get(cacheKey) != null)
        {
            ObjCache.Remove(cacheKey);
        }
    }
}

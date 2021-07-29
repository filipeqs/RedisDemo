using System;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.Extensions.Caching.Distributed;

namespace RedisDemoMVC.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
            string recordId,
            T data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null)
        {
            // Options so be set on this cache
            var options = new DistributedCacheEntryOptions();

            // Set When the cache will be cleared
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            // Set an unesed exprite time, if this is set to null, unused expire time will not be set
            options.SlidingExpiration = unusedExpireTime;

            // Convert data into JSON
            var jsonData = Json.Encode(data);

            // Save to cache
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetReacordAsync<T>(this IDistributedCache cache, string recordId)
        {
            try
            {
                // Get data from cache
                var jsonData = await cache.GetStringAsync(recordId);

                if (jsonData is null)
                    return default(T);

                // Deserialize data into the given model
                return Json.Decode<T>(jsonData);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
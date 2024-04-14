using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Strateq.Core.Service
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> GetCache<T>(string key)
        {
            var cache = await _distributedCache.GetAsync(key);
            if (cache != null)
            {
                var serializedCacheList = Encoding.UTF8.GetString(cache);
                var cacheList = JsonSerializer.Deserialize<T>(serializedCacheList)!;
                return cacheList;
            }
            return default;
        }

        public async Task SetCache<T>(string key, T list)
        {
            var serializedAccountList = JsonSerializer.Serialize(list);
            var cacheList = Encoding.UTF8.GetBytes(serializedAccountList);
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddDays(1)).SetSlidingExpiration(TimeSpan.FromMinutes(20));
            await _distributedCache.SetAsync(key, cacheList, options);
        }

        public async Task ClearCache(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }
    }
}

using Strateq.Core.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strateq.Core.Service
{
    public interface IRedisCacheService : IBaseService
    {
        Task<T> GetCache<T>(string key);

        Task SetCache<T>(string key, T list);

        Task ClearCache(string key);
    }
}

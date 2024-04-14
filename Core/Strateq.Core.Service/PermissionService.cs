using Microsoft.AspNetCore.Http;
using Strateq.Core.Model;
using Strateq.Core.Service;
using System;
using System.Security.Claims;


namespace Strateq.Core.Service
{
    public class PermissionService : IPermissionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRedisCacheService _redisCacheService;
        public PermissionService(IHttpContextAccessor httpContextAccessor,
            IRedisCacheService redisCacheService)
        {
            _httpContextAccessor = httpContextAccessor;
            _redisCacheService = redisCacheService;
        }
        public int[] GetCurrentUserPermissionIdToArray()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                throw new Exception("Unfound HttpContext");
            }

            var currentUser = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUser == null)
            {
                throw new Exception("Invalid User.");
            }

            var redisCache = _redisCacheService.GetCache<AccountSessionModel>("User-" + currentUser.Value).Result;
            if (redisCache == null)
            {
                throw new Exception("No Cache Found for current user.");
            }

            return redisCache.PermissionId;
        }

        public bool IsAuthorized(int permissionId)
        {
            var cachePermission = GetCurrentUserPermissionIdToArray();
            foreach (var permission in cachePermission)
            {
                if (permissionId == permission)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

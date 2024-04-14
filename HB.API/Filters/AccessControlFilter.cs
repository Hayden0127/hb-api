using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Strateq.Core.Service;

namespace HB.API.Filter
{
    public class AccessFilter : TypeFilterAttribute
    {
        public AccessFilter(int permission) : base(typeof(AccessFilterImplementation))
        {
            Arguments = new object[] { permission };
        }

        private class AccessFilterImplementation : IAuthorizationFilter
        {
            private readonly IPermissionService _permissionService;
            public static ActionResult? OnAccessDenied { get; set; }
            public int _permissionId;

            public AccessFilterImplementation(IPermissionService permissionService, int permissionId)
            {
                _permissionId = permissionId;
                _permissionService = permissionService;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var identity = context.HttpContext.User.Identity;

                if (identity != null && identity.IsAuthenticated)
                {
                    if (!_permissionService.IsAuthorized(_permissionId))
                    {
                        context.Result = new UnauthorizedObjectResult("Role permission not allowed");
                    }

                    return;
                }
            }
        }
    }
}

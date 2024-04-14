using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Strateq.Core.API.Controllers
{
    public class STQControllerBase : ControllerBase
    {
        public abstract class BaseController : Controller
        {
            protected int GetUserId()
            {
                return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
        }
    }
}

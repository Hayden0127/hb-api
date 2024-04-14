using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Strateq.Core.Utilities;
using Strateq.Core.Model;

namespace Strateq.Core.API.Filter
{
    public class ModelStateValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var messages = string.Join(";", context.ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
                ErrorResponseModel response = new() { Code = SystemDataCore.ErrorCode.Validation, Message = messages };
                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}

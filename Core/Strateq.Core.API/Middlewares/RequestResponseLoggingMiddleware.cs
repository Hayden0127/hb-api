using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using System.IO;
using Strateq.Core.Service;
using Strateq.Core.Database.DbModel;

namespace Strateq.Core.API.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IRequestLogService _requestLogService)
        {
            try
            {
                // get name of controller & action
                var controllerActionDescriptor = context
                    .GetEndpoint();

                var metadata = controllerActionDescriptor?.Metadata ?? null;
                var detail = metadata?.GetMetadata<ControllerActionDescriptor>() ?? null;

                var controllerName = detail?.ControllerName ?? string.Empty;
                var actionName = detail?.ActionName ?? string.Empty;

                // read query string & request body from HttpContext
                context.Request.EnableBuffering();
                StringBuilder requestLogger = new StringBuilder();
                requestLogger.AppendLine($"Request QueryString: {context.Request.QueryString}");

                MemoryStream requestBody = new MemoryStream();
                var body = context.Request.BodyReader.AsStream(true);
                using var streamReader = new StreamReader(body);
                {
                    var requestBodyText = await streamReader.ReadToEndAsync();
                    requestLogger.AppendLine($"Request Body: {requestBodyText}");
                }
                context.Request.Body.Position = 0;

                // add request log to store request
                RequestLog addRequest = new()
                {
                    Controller = controllerName,
                    Action = actionName,
                    Request = requestLogger.ToString()
                };

                var log = await _requestLogService.AddAsync(addRequest);

                context.Items["RequestLogId"] = log.Id;

                // store original stream
                var originalBodyStream = context.Response.Body;
                MemoryStream responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                await _next.Invoke(context);

                // read response body from HttpContext
                responseBody.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();

                //update the response to existing request log
                log.Response = responseBodyText;
                log.Status = context.Response.StatusCode.ToString();

                await _requestLogService.UpdateAsync(log);

                // replace original stream
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                //Add system erro logging here
                _logger.LogError("RequestLog Middleware Error", ex);
            }
            finally
            {

            }
        }
    }
}

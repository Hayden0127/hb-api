using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Strateq.Core.Database.DbModel;
using Strateq.Core.Database.Repositories;
using Strateq.Core.Service.Base;
using Strateq.Core.Utilities;

namespace Strateq.Core.Service
{
    public class SystemLogService : ISystemLogService, IBaseService
    {
        private readonly ISystemLogRepository _systemLogRepository;
        private readonly IHttpContextAccessor _httpAccessor;

        private string controller = string.Empty;
        private string action = string.Empty;
        private string requestId = "0";

        public SystemLogService(ISystemLogRepository systemLogRepository, IHttpContextAccessor httpAccessor)
        {
            _systemLogRepository = systemLogRepository;
            _httpAccessor = httpAccessor;
            var context = _httpAccessor.HttpContext;

            if (context != null)
            {
                controller = context.Request.Query["controller"].ToString();
                action = context.Request.Query["action"].ToString();

                if (context.Items["RequestLogId"] != null)
                {
                    requestId = _httpAccessor.HttpContext.Items["RequestLogId"].ToString();
                }
                else
                {
                    requestId = "0";
                }
            }
        }

        public async Task LogInformation(string message)
        {
            await SaveLogAsync(message, SystemDataCore.Logs.Information);
        }

        public async Task LogWarning(string message)
        {
            await SaveLogAsync(message, SystemDataCore.Logs.Warning);
        }

        public async Task LogError(string message, Exception ex = null)
        {
            try
            {
                if (ex != null)
                {
                    message += Environment.NewLine +
                        "Exception type " + ex.GetType() + Environment.NewLine +
                        "Exception message: " + ex.Message + Environment.NewLine +
                        "Stack trace: " + ex.StackTrace + Environment.NewLine;
                    if (ex.InnerException != null)
                    {
                        message += "---BEGIN InnerException--- " + Environment.NewLine +
                                   "Exception type " + ex.InnerException.GetType() + Environment.NewLine +
                                   "Exception message: " + ex.InnerException.Message + Environment.NewLine +
                                   "Stack trace: " + ex.InnerException.StackTrace + Environment.NewLine +
                                   "---END Inner Exception";
                    }


                }

                await SaveLogAsync(message, SystemDataCore.Logs.Error);
            }
            catch
            {
                return;
            }
            finally
            {

            }
        }

        public async Task LogDebug(string message)
        {
            await SaveLogAsync(message, SystemDataCore.Logs.Debug);
        }

        private async Task SaveLogAsync(string message, string type)
        {
            SystemLog log = new()
            {
                RequestLogId = Convert.ToInt64(requestId),
                Controller = controller,
                Action = action,
                Detail = message,
                Type = type
            };
            await _systemLogRepository.AddAndSaveChangesAsync(log);
        }
    }
}

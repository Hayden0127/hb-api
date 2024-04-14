using Microsoft.AspNetCore.Http;
using Strateq.Core.Utilities;
using System;
using System.Threading.Tasks;
using Strateq.Core.Service.Base;
using Strateq.Core.Database.Repositories;
using Strateq.Core.Database.DbModel;

namespace Strateq.Core.Service
{
    public class RequestLogService : IRequestLogService, IBaseService
    {
        private readonly IRequestLogRepository _requestLogRepository;
        //private readonly IHttpContextAccessor _httpAccessor;

        //private string controller;
        //private string action;
        //private string requestId;

        public RequestLogService(IRequestLogRepository requestLogRepository)
        {
            _requestLogRepository = requestLogRepository;
        }

        public async Task<RequestLog> AddAsync(RequestLog log)
        {
            var obj = await _requestLogRepository.AddAndSaveChangesAsync(log);
            return obj;
        }

        public async Task UpdateAsync(RequestLog log)
        {
            try
            {
                await _requestLogRepository.UpdateAndSaveChangesAsync(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

            }
        }
    }
}

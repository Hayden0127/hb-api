using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Strateq.Core.Database.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Strateq.Core.Database.DbModel;

namespace Strateq.Core.Database.Repositories
{
    public interface IRequestLogRepository
    {
        Task<RequestLog> AddAndSaveChangesAsync(RequestLog entity);
        Task<RequestLog> UpdateAndSaveChangesAsync(RequestLog entity);
    }
}

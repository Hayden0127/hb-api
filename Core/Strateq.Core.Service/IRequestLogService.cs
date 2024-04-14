using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Strateq.Core.Database.DbModel;

namespace Strateq.Core.Service
{
    public interface IRequestLogService
    {
        Task<RequestLog> AddAsync(RequestLog log);

        Task UpdateAsync(RequestLog log);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strateq.Core.Service
{
    public interface ISystemLogService
    {
        public Task LogInformation(string message);
        public Task LogError(string message, Exception exception = null);
        public Task LogWarning(string message);
        public Task LogDebug(string message);
    }
}

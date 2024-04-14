using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strateq.Core.Service
{
    public interface IPermissionService
    {
        int[] GetCurrentUserPermissionIdToArray();
        bool IsAuthorized(int permissionId);
    }
}

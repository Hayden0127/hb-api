using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strateq.Core.Model
{
    public class UserPermission
    {
        public bool AllowUpdate { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowCreate { get; set; }
        public bool AllowView { get; set; }
        public bool AllowExport { get; set; }
    }
}

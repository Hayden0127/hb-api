using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strateq.Core.Model
{
    public class DetailViewBaseModel
    {
        public UserPermission UserPermission { get; set; } = new();
    }
}

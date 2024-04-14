using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strateq.Core.Database.DbModel.Base
{
    public abstract class DbModelBase
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}

using Strateq.Core.Database.DbModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Database.DbModels
{
    public class RunningSequenceNumber : DbModelBase
    {
        [Column(TypeName = "nvarchar(30)")]
        public string Type { get; set; }
        public int SequenceNumber { get; set; }
    }
}

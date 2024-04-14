using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Strateq.Core.Database.DbModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Database.DbModels
{
    public class CPBreakdownDurationDetails : DbModelBase
    {
        [Column(TypeName = "int")]
        public int CPBreakdownErrorId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime StartTime { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime EndTime { get; set; }
    }
}

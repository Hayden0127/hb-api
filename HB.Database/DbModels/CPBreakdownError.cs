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
    public class CPBreakdownError: DbModelBase
    {
        [Column(TypeName = "int")]
        public int CPDetailsId { get; set; }
        [Column(TypeName = "int")]
        public int ConnectorId { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string ErrorCode { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string ErrorDescription { get; set; }
        [Column(TypeName = "int")]
        public int Status { get; set; }
        [Column(TypeName = "int")]
        public int Severity { get; set; }
        public DateTime TimeStamp { get; set; }
        [Column(TypeName = "int")]
        public int IncidentId { get; set; }
        [Column(TypeName = "nvarchar(40)")]
        public string IncidentNo { get; set; }
    }
}

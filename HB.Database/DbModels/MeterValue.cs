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
    public class MeterValue : DbModelBase
    {
        [Column(TypeName = "int")]
        public int CPDetailsId { get; set; }
        [Column(TypeName = "int")]
        public int ConnectorId { get; set; }
        [Column(TypeName = "int")]
        public int? TransactionId { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string UniqueId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime TimeStamp { get; set; }
        [Column(TypeName = "int")]
        public int CurrentMeterValue { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Context { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string Measurand { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Unit { get; set; }
    }
}

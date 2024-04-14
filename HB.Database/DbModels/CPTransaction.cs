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
    public class CPTransaction : DbModelBase
    {
        [Column(TypeName = "int")]
        public int CPDetailsId { get; set; }

        [Column(TypeName = "int")]
        public int ConnectorId { get; set; }

        [Column(TypeName = "int")]
        public int TransactionId { get; set; }

        [ForeignKey("Id")]
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Country { get; set; }

        [Column(TypeName = "decimal(10,5)")]
        public decimal MeterStartValue { get; set; }

        [Column(TypeName = "decimal(10,5)")]
        public decimal MeterStopValue { get; set; }

        [Column(TypeName = "decimal(10,5)")]
        public decimal TotalMeterValue { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalHoursTaken { get; set; }

        [Column(TypeName = "decimal(10,5)")]
        public decimal TotalAmount { get; set; }

        public DateTime PaymentDate { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Status { get; set; }
    }
}

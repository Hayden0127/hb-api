using Microsoft.EntityFrameworkCore;
using Strateq.Core.Database.DbModel.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HB.Database.DbModels
{
    public class CPSiteDetails : DbModelBase
    {
        [ForeignKey("Id")]
        public int UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string PersonInCharge { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string SiteName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string OfficeNo { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string MobileNo { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string MaintenanceProgram { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18, 10)")]
        public decimal Longitude { get; set; }

        [Column(TypeName = "decimal(18, 10)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Country { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        public string Address { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string City { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string State { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string OperationalStatus { get; set; }
        public DateTime? UpdatedOn { get; set; }
        [Column(TypeName = "int")]
        public int? LoadLimit { get; set; }
        [Column(TypeName = "int")]
        public int SmartSDSiteId { get; set; }
    }
}

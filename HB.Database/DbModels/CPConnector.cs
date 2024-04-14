using Microsoft.EntityFrameworkCore;
using Strateq.Core.Database.DbModel.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HB.Database.DbModels
{
    public class CPConnector : DbModelBase
    {
        [ForeignKey("Id")]
        public int CPDetailsId { get; set; }
        public CPDetails CPDetails { get; set; }

        [Column(TypeName = "int")]
        public int ConnectorId { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string Name { get; set; }

        [ForeignKey("Id")]
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedOn { get; set; }
        [Column(TypeName = "int")]
        public int PowerOutput { get; set; }
        public bool IsLock { get; set; }
    }
}
